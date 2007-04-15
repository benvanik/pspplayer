// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Bios;

namespace Noxa.Emulation.Psp.Debugging.DebugData
{
	// Unfortunately when it comes to relocating elf files, there is very little info besides the data structures
	// Had to go to the source to figure things out :(
	// Also a thing to keep in mind is that the prx files (what games are) are different than normal elfs (what
	// pspsdk usually spits out) - basically the same, but different types, sections and such.

	// Helpful links:
	// http://www.sco.com/developers/devspecs/mipsabi.pdf
	// http://glide.stanford.edu/lxr/source/arch/mips/kernel/module-elf32.c?v=linux-2.6.10#L189

	class ElfFile
	{
		protected ElfType _programType;
		protected uint _address;
		protected uint _entryAddress;
		protected uint _initAddress;
		protected uint _upperAddress;
		protected uint _globalPointer;
		protected bool _needsRelocation;

		protected List<ElfSection> _sections = new List<ElfSection>();
		protected List<ElfSection> _allocSections = new List<ElfSection>();
		protected List<ElfSection> _relocSections = new List<ElfSection>();
		protected Dictionary<string, ElfSection> _sectionLookup = new Dictionary<string, ElfSection>();

		List<ElfSymbol> _symbols = new List<ElfSymbol>();
		//List<ElfRelocation> _relocations = new List<ElfRelocation>();

		#region Types/enums

		public const ushort ElfMachineMips = 0x0008;
		public const string ElfShStrTab = ".shstrtab";

		public const uint ElfUnknownSymbol = 0x0;
		public const uint ElfAbsoluteSymbol = 0xFFF1;

		public enum ElfType : uint
		{
			Executable = 0x0002,
			Prx = 0xFFA0
		}

		public enum ElfSectionType : uint
		{
			Null = 0,
			ProgramBits = 1,
			SymbolTable = 2,
			StringTable = 3,
			Rela = 4,
			Hash = 5,
			Dynamic = 6,
			Note = 7,
			NoBits = 8,
			Relocation = 9,
			ShLib = 10,
			DynamicSymbols = 11,
			LoProc = 0x70000000,
			HiProc = 0x7FFFFFFF,
			LoUser = 0x80000000,
			HiUser = 0xFFFFFFFF,

			PrxReloc = LoProc | 0xA0
		}

		public enum ElfRelocationType : uint
		{
			None = 0,
			Mips16 = 1,
			Mips32 = 2,
			Rel32 = 3,
			Mips26 = 4,
			Hi16 = 5,
			Lo16 = 6,
			GpRel16 = 7,
			Literal = 8,
			Got16 = 9,
			Pc16 = 10,
			Call16 = 11,
			GpRel32 = 12
		}

		[Flags]
		public enum ElfSectionFlags : uint
		{
			Write = 1,
			Alloc = 2,
			ExecuteInstruction = 4
		}

		public enum Pt : uint
		{
			Null = 0,
			Load = 1,
			Dynamic = 2,
			Interp = 3,
			Note = 4,
			ShLib = 5,
			Phdr = 6,
			LoProc = 0x70000000,
			HiProc = 0x7FFFFFFF
		}

		public enum ElfSymbolBinding
		{
			Local = 0,
			Global = 1,
			Weak = 2,
			LoProc = 13,
			HiProc = 15
		}

		public enum ElfSymbolType
		{
			NoType = 0,
			Object = 1,
			Function = 2,
			Section = 3,
			File = 4,
			LoProc = 13,
			HiProc = 15
		}

		public class ElfSection
		{
			public string Name;
			public uint NameIndex;
			public ElfSectionType SectionType;
			public ElfSectionFlags Flags;
			public uint Address;
			public uint ElfOffset;
			public uint Length;
			public uint LinkInfo;
			public uint AddressAlignment;
			//public uint EntryLength;

			// For relocation
			public ElfSection Reference;

			public override string ToString()
			{
				return string.Format( "{0} {1} @ 0x{2:X8} in file at 0x{3:X8}",
					Name, SectionType, Address, ElfOffset );
			}
		}

		public class ElfRelocation
		{
			//public ElfSection Section;
			public uint BaseAddress;
			public ElfRelocationType RelocationType;
			public uint Symbol;
			public uint Offset;
			//public uint NewAddress;
		}

		public class ElfSymbol
		{
			public string Name;
			public uint Value;
			public uint Size;
			public ElfSymbolType SymbolType;
			public ElfSymbolBinding Binding;
			public uint Index;
		}

		public struct NativeElfEhdr
		{
			public const uint ElfMagic = 0x464C457F;

			public uint Magic;
			public byte Class;
			public byte Data;
			public byte Idver;
			public byte[] Pad; // 9
			public ushort Type;
			public ushort Machine;
			public uint Version;
			public uint Entry;
			public uint Phoff;
			public uint Shoff;
			public uint Flags;
			public ushort Ehsize;
			public ushort Phentsize;
			public ushort Phnum;
			public ushort Shentsize;
			public ushort Shnum;
			public ushort Shstrndx;

			public NativeElfEhdr( BinaryReader reader )
			{
				Magic = reader.ReadUInt32();
				Class = reader.ReadByte();
				Data = reader.ReadByte();
				Idver = reader.ReadByte();
				Pad = reader.ReadBytes( 9 );
				Type = reader.ReadUInt16();
				Machine = reader.ReadUInt16();
				Version = reader.ReadUInt32();
				Entry = reader.ReadUInt32();
				Phoff = reader.ReadUInt32();
				Shoff = reader.ReadUInt32();
				Flags = reader.ReadUInt32();
				Ehsize = reader.ReadUInt16();
				Phentsize = reader.ReadUInt16();
				Phnum = reader.ReadUInt16();
				Shentsize = reader.ReadUInt16();
				Shnum = reader.ReadUInt16();
				Shstrndx = reader.ReadUInt16();
			}
		}

		public struct NativeElfShdr
		{
			public uint Name;
			public uint Type;
			public uint Flags;
			public uint Address;
			public uint Offset;
			public uint Size;
			public uint Link;
			public uint Info;
			public uint AddressAlignment;
			public uint EntSize;

			public NativeElfShdr( BinaryReader reader )
			{
				Name = reader.ReadUInt32();
				Type = reader.ReadUInt32();
				Flags = reader.ReadUInt32();
				Address = reader.ReadUInt32();
				Offset = reader.ReadUInt32();
				Size = reader.ReadUInt32();
				Link = reader.ReadUInt32();
				Info = reader.ReadUInt32();
				AddressAlignment = reader.ReadUInt32();
				EntSize = reader.ReadUInt32();
			}
		}

		public struct NativeElfPhdr
		{
			public uint Type;
			public uint Offset;
			public uint Vaddr;
			public uint Paddr;
			public uint FileSize;
			public uint MemorySize;
			public uint Flags;
			public uint Alignment;

			public NativeElfPhdr( BinaryReader reader )
			{
				Type = reader.ReadUInt32();
				Offset = reader.ReadUInt32();
				Vaddr = reader.ReadUInt32();
				Paddr = reader.ReadUInt32();
				FileSize = reader.ReadUInt32();
				MemorySize = reader.ReadUInt32();
				Flags = reader.ReadUInt32();
				Alignment = reader.ReadUInt32();
			}
		}

		public struct NativeElfRel
		{
			public uint Offset;
			public uint Info;

			public NativeElfRel( BinaryReader reader )
			{
				Offset = reader.ReadUInt32();
				Info = reader.ReadUInt32();
			}
		}

		public struct NativeElfSym
		{
			public uint Name;
			public uint Value;
			public uint Size;
			public byte Info;
			public byte Other;
			public ushort SectionIndex;

			public NativeElfSym( BinaryReader reader )
			{
				Name = reader.ReadUInt32();
				Value = reader.ReadUInt32();
				Size = reader.ReadUInt32();
				Info = reader.ReadByte();
				Other = reader.ReadByte();
				SectionIndex = reader.ReadUInt16();
			}
		}

		private struct LibraryEntry
		{
			public uint StringAddress; // or 0
			public ushort Version; // bcd
			public ushort Attributes;
			public byte ExportSize;
			public byte VariableCount;
			public ushort FunctionCount;
			public uint TableAddress; // __entrytable in .rodata.sceResident

			public LibraryEntry( BinaryReader reader )
			{
				StringAddress = reader.ReadUInt32();
				Version = reader.ReadUInt16();
				Attributes = reader.ReadUInt16();
				ExportSize = reader.ReadByte();
				VariableCount = reader.ReadByte();
				FunctionCount = reader.ReadUInt16();
				TableAddress = reader.ReadUInt32();
			}
		}

		private struct StubMetadata
		{
			public uint StringAddress; // __stub_modulestr in .rodata.sceResident
			public ushort ImportFlags;
			public ushort LibraryVersion;
			public ushort StubCount;
			public ushort StubSize; // in words
			public uint NidAddress; // __stub_nidtable in .rodata.sceNid
			public uint SyscallAddress; // sceXXX in .sceStub.text

			public StubMetadata( BinaryReader reader )
			{
				StringAddress = reader.ReadUInt32();
				ImportFlags = reader.ReadUInt16();
				LibraryVersion = reader.ReadUInt16();
				StubCount = reader.ReadUInt16();
				StubSize = reader.ReadUInt16();
				NidAddress = reader.ReadUInt32();
				SyscallAddress = reader.ReadUInt32();
			}
		}

		private enum ModuleMode : ushort
		{
			User = 0x0000,
			Kernel = 0x1000
		}

		private struct ModuleInfo
		{
			public ModuleMode Mode;
			public ushort Version; // 2 chars
			public string Name; // 28 bytes, 0 terminated
			public uint Gp;
			public uint LibEnt;
			public uint LibEntBtm;
			public uint LibStub;
			public uint LibStubBtm;

			public ModuleInfo( BinaryReader reader )
			{
				Mode = ( ModuleMode )reader.ReadUInt16();
				Version = reader.ReadUInt16();

				byte[] bytes = reader.ReadBytes( 28 );
				StringBuilder sb = new StringBuilder( 28 );
				foreach( byte b in bytes )
				{
					if( b == 0 )
						break;
					sb.Append( ( char )b );
				}
				Name = sb.ToString();

				Gp = reader.ReadUInt32();
				LibEnt = reader.ReadUInt32();
				LibEntBtm = reader.ReadUInt32();
				LibStub = reader.ReadUInt32();
				LibStubBtm = reader.ReadUInt32();
			}
		}

		#endregion

		public ElfFile( Stream source )
		{
			_address = 0;

			//long position = source.Position;
			source.Position = 0;
			if( LoadElf( new BinaryReader( source ) ) == false )
			{
				// Failed
				throw new Exception( "Elf load failed" );
			}
			//source.Position = position;
		}

		public ElfType ProgramType
		{
			get
			{
				return _programType;
			}
		}

		public uint Address
		{
			get
			{
				return _address;
			}
		}

		public uint EntryAddress
		{
			get
			{
				return _entryAddress;
			}
		}

		public uint InitAddress
		{
			get
			{
				return _initAddress;
			}
		}

		public uint UpperAddress
		{
			get
			{
				return _upperAddress;
			}
		}

		public uint GlobalPointer
		{
			get
			{
				return _globalPointer;
			}
		}

		public List<ElfSymbol> Symbols
		{
			get
			{
				return _symbols;
			}
		}

		protected bool LoadElf( BinaryReader reader )
		{
			NativeElfEhdr ehdr = new NativeElfEhdr( reader );
			if( ehdr.Magic != NativeElfEhdr.ElfMagic )
			{
				Debug.WriteLine( "ElfFile: elf magic number invalid" );
				return false;
			}
			if( ehdr.Machine != ElfMachineMips )
			{
				Debug.WriteLine( "ElfFile: machine version invalid" );
				return false;
			}

			_programType = ( ElfType )ehdr.Type;
			_entryAddress = ehdr.Entry;

			// Easy test for relocation:
			if( _entryAddress < 0x08000000 )
				_needsRelocation = true;
			
			reader.BaseStream.Seek( ehdr.Phoff, SeekOrigin.Begin );
			List<NativeElfPhdr> phdrs = new List<NativeElfPhdr>();
			for( int n = 0; n < ehdr.Phnum; n++ )
			{
				NativeElfPhdr phdr = new NativeElfPhdr( reader );
				phdrs.Add( phdr );
			}

			for( int n = 0; n < ehdr.Shnum; n++ )
			{
				reader.BaseStream.Seek( ehdr.Shoff + ( n * ehdr.Shentsize ), SeekOrigin.Begin );
				NativeElfShdr shdr = new NativeElfShdr( reader );
				
				ElfSection section = new ElfSection();
				section.NameIndex = shdr.Name;
				section.ElfOffset = shdr.Offset;
				section.Flags = ( ElfSectionFlags )shdr.Flags;
				section.LinkInfo = shdr.Info;
				section.SectionType = ( ElfSectionType )shdr.Type;
				section.Address = shdr.Address;
				section.AddressAlignment = shdr.AddressAlignment;
				section.Length = shdr.Size;

				_sections.Add( section );
				if( ( section.Flags & ElfSectionFlags.Alloc ) == ElfSectionFlags.Alloc )
					_allocSections.Add( section );

				if( ( ( _programType == ElfType.Executable ) &&
						( section.SectionType == ElfSectionType.Relocation ) ) ||
						( ( _programType == ElfType.Prx ) &&
						( section.SectionType == ElfSectionType.PrxReloc ) ) )
					_relocSections.Add( section );
			}

			uint nameBase = _sections[ ehdr.Shstrndx ].ElfOffset;
			foreach( ElfSection section in _sections )
			{
				reader.BaseStream.Seek( nameBase + section.NameIndex, SeekOrigin.Begin );
				section.Name = ReadString( reader );

				if( ( ( section.SectionType == ElfSectionType.Relocation ) ||
					( section.SectionType == ElfSectionType.PrxReloc ) ) &&
					( ( _sections[ ( int )section.LinkInfo ].Flags & ElfSectionFlags.Alloc ) != 0 ) )
				{
					section.Reference = _sections[ ( int )section.LinkInfo ];
					//_needsRelocation = true;
				}

				if( ( section.Name != null ) &&
					( section.Name.Length > 0 ) )
					_sectionLookup.Add( section.Name, section );
			}

			// Not sure if this is important
			if( _sectionLookup.ContainsKey( ".init" ) == true )
				_initAddress = _sectionLookup[ ".init" ].Address;
			else
				_initAddress = 0x0;
			
			if( _sectionLookup.ContainsKey( ".symtab" ) == true )
			{
				ElfSection symtab = _sectionLookup[ ".symtab" ];
				ElfSection strtab = _sectionLookup[ ".strtab" ];
				for( int n = 0; n < symtab.Length / 16; n++ )
				{
					reader.BaseStream.Seek( symtab.ElfOffset + ( 16 * n ), SeekOrigin.Begin );
					NativeElfSym sym = new NativeElfSym( reader );

					ElfSymbol symbol = new ElfSymbol();
					symbol.Index = sym.SectionIndex; // May be ElfAbsoluteSymbol
					symbol.Size = sym.Size;
					symbol.Value = sym.Value;
					symbol.Binding = ( ElfSymbolBinding )( sym.Info >> 4 );
					symbol.SymbolType = ( ElfSymbolType )( sym.Info & 0xF );

					reader.BaseStream.Seek( strtab.ElfOffset + sym.Name, SeekOrigin.Begin );
					symbol.Name = ReadString( reader );

					_symbols.Add( symbol );

					if( symbol.Index != ElfAbsoluteSymbol )
					{
						ElfSection symbolParent = _sections[ ( int )symbol.Index ];
						List<ElfSymbol> syms;
						if( _symbolLookup.ContainsKey( symbolParent ) == true )
							syms = _symbolLookup[ symbolParent ];
						else
						{
							syms = new List<ElfSymbol>();
							_symbolLookup.Add( symbolParent, syms );
						}
						syms.Add( symbol );
					}
				}
			}

			//foreach( ElfSection section in _sections )
			//{
			//	Debugger.Break();
			//    if( ( _programType == ElfType.Executable ) &&
			//        ( section.SectionType != ElfSectionType.Relocation ) )
			//        continue;
			//    if( ( _programType == ElfType.Prx ) &&
			//        ( section.SectionType != ElfSectionType.PrxReloc ) )
			//        continue;
			//    for( int n = 0; n < section.Length / 8; n++ )
			//    {
			//        reader.BaseStream.Seek( section.ElfOffset + ( 8 * n ), SeekOrigin.Begin );
			//        NativeElfRel rel = new NativeElfRel( reader );

			//        ElfRelocation relocation = new ElfRelocation();
			//        relocation.Section = section;
			//        relocation.Offset = rel.Offset;
			//        relocation.BaseAddress = ( rel.Info >> 16 ) & 0xFF;
			//        relocation.Symbol = rel.Info >> 8;
			//        relocation.RelocationType = ( ElfRelocationType )( byte )( rel.Info & 0xFF );
					
			//        _relocations.Add( relocation );
			//    }
			//}

			return true;
		}

		protected Dictionary<ElfSection, List<ElfSymbol>> _symbolLookup = new Dictionary<ElfSection, List<ElfSymbol>>();

		protected ElfSymbol FindSymbol( ElfSection section, uint symbolIndex )
		{
			return _symbols[ ( int )symbolIndex ];
			//return _symbolLookup[ section ][ ( int )symbolIndex ];
		}

//        protected void FixRelocation( BinaryReader reader, IMemory memory, ElfRelocation relocation, uint baseAddress )
//        {
//            uint target;
//            ElfSection realSection = _sections[ ( int )relocation.BaseAddress ];
//            if( realSection.Address != 0 )
//            {
//                target = realSection.Address + relocation.Offset;
//            }
//            else
//            {
//                target = relocation.Section.Reference.Address + relocation.Offset;
//            }
//            //uint target = baseAddress + relocation.Section.ElfOffset + relocation.Offset;

//            /*
//216                 sym = (Elf32_Sym *)sechdrs[symindex].sh_addr
//217                         + ELF32_R_SYM(r_info);
//218                 if (!sym->st_value) {
//219                         printk(KERN_WARNING "%s: Unknown symbol %s\n",
//220                                me->name, strtab + sym->st_name);
//221                         return -ENOENT;
//222                 }
//223 
//224                 v = sym->st_value;
//*/

//            ElfSymbol symbol = this.FindSymbol( relocation.Section, relocation.Symbol );
//            if( symbol == null )
//                return;

//            uint symbolValue = symbol.Value;
//            if( symbol.Index != ElfUnknownSymbol )
//            {
//                ElfSection symbolRef = _sections[ ( int )symbol.Index ];
//                uint symbolAddress = symbolRef.Address + relocation.Symbol;
//                // Hacky!
//                byte[] buffer = memory.ReadBytes( ( int )symbolAddress, 24 );
//                using( BinaryReader myReader = new BinaryReader( new MemoryStream( buffer, false ) ) )
//                {
//                    NativeElfSym nes = new NativeElfSym( reader );
//                    symbolValue += symbolRef.Address;
//                }
//            }
//            else
//            {
//                symbolValue = relocation.Section.Reference.Address;
//            }

//            // = relocation.Section.Reference.Address + symbol.Value;

//            Debug.WriteLine( string.Format( "Relocating 0x{0:X8} referencing symbol {1} ({2})", target, symbol.Name, symbol.SymbolType.ToString() ) );

//            if( symbol.SymbolType == ElfSymbolType.Function )
//                Debugger.Break();

//            //if( relocation.Symbol == ElfAbsoluteSymbol )
//            //	return;
//            //if( relocation.Symbol == ElfUnknownSymbol )
//            //	return;

//            // AHL = (AHI << 16) + (short)ALO
//            // S = symbol value OR if local original sh_addr - final sh_addr
//            // GP = final gp value
//            // P = relocation offset
//            uint pointer = ( uint )memory.ReadWord( ( int )target );
//            uint temp;
//            switch( relocation.RelocationType )
//            {
//                case ElfRelocationType.Mips32:
//                    pointer = symbolValue;
//                    break;
//                case ElfRelocationType.Mips26:
//                    // ((A << 2) | S) >> 2
//                    temp = pointer & 0x03FFFFFF;
//                    temp = ( temp << 2 ) | symbolValue;
//                    pointer = ( pointer & 0xFC000000 ) | ( temp >> 2 ); // Should this be + instead of |? Docs say |, but I don't trust them...
//                    break;
//                case ElfRelocationType.Hi16:
//                    {
//                        HiReloc hiReloc = new HiReloc();
//                        hiReloc.Address = target;
//                        hiReloc.Value = symbolValue;
//                        _hiRelocs.Add( hiReloc );
//                    }
//                    return;
//                case ElfRelocationType.Lo16:
//                    // AHL + S
//                    uint vallo = ( ( pointer & 0x0000FFFF ) ^ 0x00008000 ) - 0x00008000;
//                    if( _hiRelocs.Count > 0 )
//                    {
//                        HiReloc hiReloc = _hiRelocs[ _hiRelocs.Count - 1 ];
//                        _hiRelocs.RemoveAt( _hiRelocs.Count - 1 );

//                        Debug.Assert( hiReloc.Value == symbolValue );

//                        uint pointer2 = ( uint )memory.ReadWord( ( int )hiReloc.Address );
//                        temp = ( ( pointer2 & 0x0000FFFF ) << 16 ) + vallo;
//                        temp += symbolValue;

//                        temp = ( ( temp >> 16 ) + ( ( ( temp & 0x00008000 ) != 0 ) ? ( uint )1 : ( uint )0 ) ) & 0x0000FFFF;

//                        uint originalPointer2 = pointer2;
//                        pointer2 = ( pointer2 & 0xFFFF0000 ) | temp;
//                        Debug.WriteLine( string.Format( "patching hi 0x{0:X8} from {1:X8} to {2:X8} (temp: {3:X8})", hiReloc.Address, originalPointer2, pointer2, temp ) );
//                        memory.WriteWord( ( int )hiReloc.Address, 4, ( int )pointer2 );
//                    }
//                    temp = symbolValue + vallo;
//                    uint originalPointer = pointer;
//                    pointer = ( pointer & 0xFFFF0000 ) | ( temp & 0x0000FFFF );
//                    Debug.WriteLine( string.Format( "patching lo 0x{0:X8} from {1:X8} to {2:X8} (temp: {3:X8})", target, originalPointer, pointer, temp ) );
//                    break;
//                default:
//                    // Unsupported
//                    break;
//            }
//            memory.WriteWord( ( int )target, 4, ( int )pointer );
//        }

		protected struct HiReloc
		{
			public uint Address;
			public uint Value;
		}

		protected enum RelocationOffset
		{
			Text = 0,
			Data = 1
		}

		protected const uint RelocationRelativeData = 0x100;
		
		protected void ApplyRelocations( BinaryReader reader, IMemory memory, ElfSection section, uint baseAddress )
		{
			List<HiReloc> hiRelocs = new List<HiReloc>();

			ElfSection linked = section.Reference;
			if( ( linked.Flags & ElfSectionFlags.Alloc ) != ElfSectionFlags.Alloc )
				return;

			Debug.WriteLine( string.Format( "Relocating section {0} using {1}, {2} relocations total", linked.Name, section.Name, section.Length / 8 ) );

			ElfSection textSection = _sectionLookup[ ".text" ];
			ElfSection dataSection = _sectionLookup[ ".data" ];
			Debug.Assert( textSection != null );
			Debug.Assert( dataSection != null );

			for( int n = 0; n < section.Length / 8; n++ )
			{
				reader.BaseStream.Seek( section.ElfOffset + ( 8 * n ), SeekOrigin.Begin );
				NativeElfRel rel = new NativeElfRel( reader );

				ElfRelocation relocation = new ElfRelocation();
				//relocation.Section = section;
				relocation.Offset = rel.Offset;
				relocation.BaseAddress = ( rel.Info >> 16 ) & 0xFF;
				relocation.Symbol = ( rel.Info >> 8 ) & 0xFF;
				relocation.RelocationType = ( ElfRelocationType )( byte )( rel.Info & 0xFF );

				uint pointer = relocation.Offset;

				//ElfSection offsetBase = null;
				//ElfSection addressBase = null;
				if( ( relocation.Symbol & ( uint )RelocationOffset.Data ) == ( uint )RelocationOffset.Data )
				{
					//offsetBase = dataSection;
					pointer += dataSection.Address;
				}
				//else if( relocation.BaseAddress == 0x1 )
				//{
				//    offsetBase = textSection;
				//    pointer += textSection.Address;
				//}
				else
				{
					//offsetBase = textSection;
					pointer += baseAddress;
				}
				//ElfSection offsetBase = _sections[ ( int )relocation.Symbol ];
				//ElfSection addressBase = _sections[ ( int )relocation.BaseAddress ];

				// Happens sometime
				if( _symbols.Count == 0 )
				{
					Debug.WriteLine( "Unable to relocate symbol; symbol table is empty - possibly no .symtab section?" );
					return;
				}

				ElfSymbol symbol = _symbols[ ( int )relocation.Symbol ];
				uint symbolValue = symbol.Value;

				// !!! This could be bogus
				if( ( ( rel.Info >> 8 ) & RelocationRelativeData ) == RelocationRelativeData )
					symbolValue += dataSection.Address;
				else
					symbolValue += baseAddress;
				//if( addressBase.Address == 0 )
				//	symbolValue += baseAddress;
				//else
				//	symbolValue += addressBase.Address;

				uint value = ( uint )memory.ReadWord( ( int )pointer );

				//Debug.WriteLine( string.Format( " Relocation pointer 0x{0:X8} (elf {1:X8}), value {2:X8}, type {3}, existing memory value {4:X8}",
				//	pointer, relocation.Offset, symbolValue, relocation.RelocationType, value ) );

				bool writeMemory = false;
				switch( relocation.RelocationType )
				{
					case ElfRelocationType.None:
						break;
					case ElfRelocationType.Mips32:
						value += symbolValue;
						writeMemory = true;
						break;
					case ElfRelocationType.Mips26:
						Debug.Assert( symbolValue % 4 == 0 );
						value = ( uint )( ( value & ~0x03FFFFFF ) | ( ( value + ( symbolValue >> 2 ) ) & 0x03FFFFFF ) );
						writeMemory = true;
						break;
					case ElfRelocationType.Hi16:
						{
							HiReloc hiReloc = new HiReloc();
							hiReloc.Address = pointer;
							hiReloc.Value = symbolValue;
							hiRelocs.Add( hiReloc );
						}
						break;
					case ElfRelocationType.Lo16:
						{
							uint vallo = ( ( value & 0x0000FFFF ) ^ 0x00008000 ) - 0x00008000;
							while( hiRelocs.Count > 0 )
							{
								HiReloc hiReloc = hiRelocs[ hiRelocs.Count - 1 ];
								hiRelocs.RemoveAt( hiRelocs.Count - 1 );

								Debug.Assert( hiReloc.Value == symbolValue );

								uint value2 = ( uint )memory.ReadWord( ( int )hiReloc.Address );
								uint temp = ( ( value2 & 0x0000FFFF ) << 16 ) + vallo;
								temp += symbolValue;

								temp = ( ( temp >> 16 ) + ( ( ( temp & 0x00008000 ) != 0 ) ? ( uint )1 : ( uint )0 ) ) & 0x0000FFFF;

								value2 = ( uint )( ( value2 & ~0x0000FFFF ) | temp );

								//Debug.WriteLine( string.Format( "   Updating memory at 0x{0:X8} to {1:X8} (from previous HI16)", hiReloc.Address, value2 ) );
								memory.WriteWord( ( int )hiReloc.Address, 4, ( int )value2 );
							}
							value = ( uint )( ( value & ~0x0000FFFF ) | ( ( symbolValue + vallo ) & 0x0000FFFF ) );
						}
						writeMemory = true;
						break;
					default:
						// Unsupported type
						Debugger.Break();
						break;
				}
				if( writeMemory == true )
				{
					//Debug.WriteLine( string.Format( "   Updating memory at 0x{0:X8} to {1:X8}", pointer, value ) );
					memory.WriteWord( ( int )pointer, 4, ( int )value );
				}
			}
		}

		public bool Load( Stream stream, IEmulationInstance emulator, uint baseAddress )
		{
			// Relocate only if we need to
			if( _needsRelocation == false )
				baseAddress = 0;

			_entryAddress += baseAddress;
			_initAddress += baseAddress;

			IMemory memory = emulator.Cpu.Memory;

			foreach( ElfSection section in _allocSections )
			{
				// Sanity check for broken ELFs
				if( ( baseAddress != 0 ) && ( baseAddress == section.Address ) )
				{
					// Fuckers lied - no relocation needed!
					_needsRelocation = false;
					baseAddress = 0;
				}

				uint address = baseAddress + section.Address;

				section.Address = address;
				if( section.SectionType == ElfSectionType.NoBits )
				{
					// Write zeros?
				}
				else if( section.SectionType == ElfSectionType.ProgramBits )
				{
					stream.Seek( section.ElfOffset, SeekOrigin.Begin );

					byte[] bytes = new byte[ section.Length ];
					stream.Read( bytes, 0, ( int )section.Length );

					memory.WriteBytes( ( int )address, bytes );

					_upperAddress = Math.Max( _upperAddress, address + ( uint )bytes.Length );
				}
			}

			BinaryReader reader = new BinaryReader( stream );

			ElfSection sceModuleInfo = _sectionLookup[ ".rodata.sceModuleInfo" ];
			reader.BaseStream.Seek( sceModuleInfo.ElfOffset, SeekOrigin.Begin );
			ModuleInfo moduleInfo = new ModuleInfo( reader );

			_globalPointer = baseAddress + moduleInfo.Gp;

			// Not sure if this is needed - the header seems to give an ok address
			//if( _programType == ElfType.Prx )
			//{
			//    ElfSection sceResident = _sectionLookup[ ".rodata.sceResident" ];
			//    reader.BaseStream.Seek( sceResident.ElfOffset, SeekOrigin.Begin );
			//    // 2 magic words
			//    reader.BaseStream.Seek( 8, SeekOrigin.Current );
			//    _entryAddress = reader.ReadUInt32();
			//}

			if( _needsRelocation == true )
			{
				//foreach( ElfRelocation relocation in _relocations )
				//{
				//    this.FixRelocation( reader, memory, relocation, baseAddress );
				//}
				foreach( ElfSection section in _relocSections )
				{
					this.ApplyRelocations( reader, memory, section, baseAddress );
				}
			}

			//result.Stubs = this.FixupStubs( reader, emulator.Cpu, memory, emulator.Bios, baseAddress );

			return true;
		}

		/*protected List<StubReference> FixupStubs( BinaryReader reader, ICpu cpu, IMemory memory, IBios bios, uint baseAddress )
		{
			List<StubReference> stubs = new List<StubReference>();
			int nidGoodCount = 0;
			int nidNotFoundCount = 0;
			int nidNotImplementedCount = 0;
			int moduleNotFoundCount = 0;
			List<string> missingModules = new List<string>();

			ElfSection libEnt = _sectionLookup[ ".lib.ent.top" ];
			ElfSection libEntBottom = _sectionLookup[ ".lib.ent.btm" ];
			int entCount = ( int )( libEntBottom.ElfOffset - libEnt.ElfOffset ) / 12;
			reader.BaseStream.Seek( libEnt.ElfOffset + 4, SeekOrigin.Begin );
			for( int n = 0; n < entCount; n++ )
			{
				LibraryEntry ent = new LibraryEntry( reader );
			}

			ElfSection libStub = _sectionLookup[ ".lib.stub.top" ];
			ElfSection libStubBottom = _sectionLookup[ ".lib.stub.btm" ];
			int stubCount = ( int )( libStubBottom.ElfOffset - libStub.ElfOffset ) / 20;
			for( int n = 0; n < stubCount; n++ )
			{
				reader.BaseStream.Seek( libStub.ElfOffset + 4 + ( n * 20 ), SeekOrigin.Begin );
				StubMetadata sm = new StubMetadata( reader );

				int address = ( int )( baseAddress + sm.StringAddress );
				StringBuilder sb = new StringBuilder();
				while( true )
				{
					byte c = ( byte )( memory.ReadWord( address ) & 0xFF );
					if( c == 0 )
						break;
					sb.Append( ( char )c );
					address++;
				}
				string moduleName = sb.ToString();
				bool moduleFound = ( bios.FindModule( moduleName ) != null );

				address = ( int )( baseAddress + sm.NidAddress );
				for( int m = 0; m < sm.StubSize; m++ )
				{
					uint nid = ( uint )memory.ReadWord( address );

					if( moduleFound == true )
					{
						BiosFunction function = bios.FindFunction( nid );
						if( function != null )
						{
							int syscall = cpu.RegisterSyscall( nid );
							uint instruction = ( uint )( ( syscall << 6 ) | 0xC );

							int syscallAddress = ( int )( baseAddress + sm.SyscallAddress ) + m * 8;
							memory.WriteWord( syscallAddress + 4, 4, ( int )instruction );

							Debug.WriteLine( string.Format( "FixupStubs: 0x{1:X8} found and patched at 0x{2:X8} -> 0x{3:X8} {0}::{4} {5}", moduleName, nid, syscallAddress, syscall, function.Name, ( function.IsImplemented == true ) ? "" : "(NI)" ) );
							if( function.IsImplemented == false )
							{
								stubs.Add( StubReference.NidNotImplemented( moduleName, nid, function ) );
								nidNotImplementedCount++;
							}
							else
							{
								stubs.Add( StubReference.Success( moduleName, nid, function ) );
								nidGoodCount++;
							}
						}
						else
						{
							Debug.WriteLine( string.Format( "FixupStubs: {0} 0x{1:X8} not found (nid not present)", moduleName, nid ) );
							stubs.Add( StubReference.NidNotFound( moduleName, nid ) );
							nidNotFoundCount++;
						}
					}
					else
					{
						Debug.WriteLine( string.Format( "FixupStubs: {0} 0x{1:X8} not found (module not present)", moduleName, nid ) );
						stubs.Add( StubReference.ModuleNotFound( moduleName, nid ) );
						if( missingModules.Contains( moduleName ) == false )
							missingModules.Add( moduleName );
						moduleNotFoundCount++;
					}

					address += 4;
				}
			}

			Debug.WriteLine( string.Format( "FixupStubs: {0}/{1} ({2}%) NIDs good; {3} not implemented, {4} not found, {5} in missing modules = {6} total", nidGoodCount, stubs.Count - moduleNotFoundCount, ( nidGoodCount / ( float )( stubs.Count - moduleNotFoundCount ) ) * 100.0f, nidNotImplementedCount, nidNotFoundCount, moduleNotFoundCount, stubs.Count ) );
			if( missingModules.Count > 0 )
			{
				Debug.WriteLine( string.Format( "FixupStubs: {0} missing modules (contain {1} NIDs ({2}% of total)):", missingModules.Count, moduleNotFoundCount, ( moduleNotFoundCount / ( float )stubs.Count ) * 100.0f ) );
				foreach( string moduleName in missingModules )
					Debug.WriteLine( string.Format( "\t\t{0}", moduleName ) );
			}

			return stubs;
		}*/

		protected static string ReadString( BinaryReader reader )
		{
			StringBuilder sb = new StringBuilder();
			while( true )
			{
				char c = reader.ReadChar();
				if( c == 0 )
					break;
				sb.Append( c );
			}
			return sb.ToString();
		}
	}
}
