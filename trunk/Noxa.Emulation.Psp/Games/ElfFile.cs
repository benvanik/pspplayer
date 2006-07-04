using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Bios;

namespace Noxa.Emulation.Psp.Games
{
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
		protected Dictionary<string, ElfSection> _sectionLookup = new Dictionary<string, ElfSection>();

		List<ElfSymbol> _symbols = new List<ElfSymbol>();
		List<ElfRelocation> _relocations = new List<ElfRelocation>();

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
			public uint EntryLength;

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
			public ElfSection Section;
			public uint BaseAddress;
			public ElfRelocationType RelocationType;
			public uint Symbol;
			public uint Offset;
			public uint NewAddress;
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
			if( LoadElf( new BinaryReader( source ) ) == false )
			{
				// Failed
				throw new Exception( "Elf load failed" );
			}
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
					_needsRelocation = true;
				}

				if( ( section.Name != null ) &&
					( section.Name.Length > 0 ) )
					_sectionLookup.Add( section.Name, section );
			}

			_initAddress = _sectionLookup[ ".init" ].Address;

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
				}
			}

			foreach( ElfSection section in _sections )
			{
				if( section.SectionType != ElfSectionType.Relocation )
					continue;
				for( int n = 0; n < section.Length / 8; n++ )
				{
					reader.BaseStream.Seek( section.ElfOffset + ( 8 * n ), SeekOrigin.Begin );
					NativeElfRel rel = new NativeElfRel( reader );

					ElfRelocation relocation = new ElfRelocation();
					relocation.Section = section;
					relocation.Offset = rel.Offset;
					relocation.Symbol = rel.Info >> 8;
					relocation.RelocationType = ( ElfRelocationType )( byte )( rel.Info & 0xFF );
					
					_relocations.Add( relocation );
				}
			}

			return true;
		}

		protected ElfSymbol FindSymbol( ElfSection section, uint symbolIndex )
		{
			return _symbols[ ( int )symbolIndex ];
		}

		protected void FixRelocation( IMemory memory, ElfRelocation relocation, uint baseAddress )
		{
			uint target = baseAddress + relocation.Section.ElfOffset + relocation.Offset;

			if( relocation.Symbol == ElfAbsoluteSymbol )
				return;
			if( relocation.Symbol == ElfUnknownSymbol )
				return;

			ElfSymbol symbol = this.FindSymbol( relocation.Section.Reference, relocation.Symbol );
			if( symbol == null )
				return;

			uint symbolValue = symbol.Value + relocation.Section.ElfOffset;

			uint pointer = ( uint )memory.ReadWord( ( int )target );
			switch( relocation.RelocationType )
			{
				case ElfRelocationType.Mips16:
					pointer = pointer + symbolValue;
					break;
				case ElfRelocationType.Mips32:
					pointer = pointer + symbolValue - ( uint )target;
					break;
				default:
					// Unsupported
					break;
			}
		}

		public void Load( Stream stream, IEmulationInstance emulator, uint baseAddress )
		{
			// Relocate only if we need to
			if( _needsRelocation == false )
				baseAddress = 0;

			IMemory memory = emulator.Cpu.Memory;

			foreach( ElfSection section in _allocSections )
			{
				uint address = baseAddress + section.Address;

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

			if( _needsRelocation == true )
			{
				foreach( ElfRelocation relocation in _relocations )
				{
					this.FixRelocation( memory, relocation, baseAddress );
				}
			}

			BinaryReader reader = new BinaryReader( stream );
			this.FixupStubs( reader, emulator.Cpu, memory, emulator.Bios );
		}

		protected void FixupStubs( BinaryReader reader, ICpu cpu, IMemory memory, IBios bios )
		{
			ElfSection sceModuleInfo = _sectionLookup[ ".rodata.sceModuleInfo" ];
			reader.BaseStream.Seek( sceModuleInfo.ElfOffset, SeekOrigin.Begin );
			ModuleInfo moduleInfo = new ModuleInfo( reader );

			_globalPointer = moduleInfo.Gp;

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

				int address = ( int )sm.StringAddress;
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

				address = ( int )sm.NidAddress;
				for( int m = 0; m < sm.StubSize; m++ )
				{
					uint nid = ( uint )memory.ReadWord( address );

					BiosFunction function = bios.FindFunction( nid );
					if( function != null )
					{
						int syscall = cpu.RegisterSyscall( nid );
						uint instruction = ( uint )( ( syscall << 6 ) | 0xC );

						int syscallAddress = ( int )sm.SyscallAddress + m * 8;
						memory.WriteWord( syscallAddress + 4, 4, ( int )instruction );

						Debug.WriteLine( string.Format( "FixupStubs: 0x{1:X8} found and patched at 0x{2:X8} -> 0x{3:X8} {0}::{4} {5}", moduleName, nid, syscallAddress, syscall, function.Name, ( function.IsImplemented == true ) ? "" : "(NI)" ) );
					}
					else
					{
						Debug.WriteLine( string.Format( "FixupStubs: {0} 0x{1:X8} not found", moduleName, nid ) );
					}

					address += 4;
				}
			}
		}

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
