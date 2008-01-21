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
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	unsafe partial class Loader : ILoader
	{
		private Bios _bios;

		public IBios Bios
		{
			get
			{
				return _bios;
			}
		}

		public Loader( Bios bios )
		{
			Debug.Assert( bios != null );
			_bios = bios;
		}

		#region ELF Types

		private enum ElfType : ushort
		{
			Executable = 0x0002,
			Prx = 0xFFA0,
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct Elf32_Ehdr
		{
			public const uint Magic = 0x464C457F;
			public const ushort MachineMips = 0x0008;

			public uint e_magic;
			public byte e_class;
			public byte e_data;
			public byte e_idver;
			public uint e_pad0; // 9 bytes of pad total
			public uint e_pad1;
			public byte e_pad2;
			public ElfType e_type;
			public ushort e_machine;
			public uint e_version;
			public uint e_entry;
			public uint e_phoff;
			public uint e_shoff;
			public uint e_flags;
			public ushort e_ehsize;
			public ushort e_phentsize;
			public ushort e_phnum;
			public ushort e_shentsize;
			public ushort e_shnum;
			public ushort e_shstrndx;
		}

		private static class ShType
		{
			public const uint SHT_NULL = 0;
			public const uint SHT_PROGBITS = 1;
			public const uint SHT_SYMTAB = 2;
			public const uint SHT_STRTAB = 3;
			public const uint SHT_RELA = 4;
			public const uint SHT_HASH = 5;
			public const uint SHT_DYNAMIC = 6;
			public const uint SHT_NOTE = 7;
			public const uint SHT_NOBITS = 8;
			public const uint SHT_REL = 9;
			public const uint SHT_SHLIB = 10;
			public const uint SHT_DYNSYM = 11;

			public const uint SHT_LOPROC = 0x70000000;
			public const uint SHT_HIPROC = 0x7FFFFFFF;
			public const uint SHT_LOUSER = 0x80000000;
			public const uint SHT_HIUSER = 0xFFFFFFFF;

			public const uint SHT_PRXRELOC = ( SHT_LOPROC | 0xA0 );
		}

		[Flags]
		private enum ShFlags : uint
		{
			None = 0,
			Write = 1,
			Allocate = 2,
			Execute = 4,
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct Elf32_Shdr
		{
			public uint sh_name;
			public uint sh_type;	// Use ShType
			public ShFlags sh_flags;
			public uint sh_addr;
			public uint sh_offset;
			public uint sh_size;
			public uint sh_link;
			public uint sh_info;
			public uint sh_addralign;
			public uint sh_entsize;
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct Elf32_Phdr
		{
			public uint p_type;
			public uint p_offset;
			public uint p_vaddr;
			public uint p_paddr;
			public uint p_filesz;
			public uint p_memsz;
			public uint p_flags;
			public uint p_align;
		}

		private enum RelType : byte
		{
			None = 0,
			Mips16 = 1,
			Mips32 = 2,
			MipsRel32 = 3,
			Mips26 = 4,
			MipsHi16 = 5,
			MipsLo16 = 6,
			MipsGpRel16 = 7,
			MipsLiteral = 8,
			MipsGot16 = 9,
			MipsPc16 = 10,
			MipsCall16 = 11,
			MipsGpRel32 = 12,
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct Elf32_Rel
		{
			public uint r_offset;
			public uint r_info;
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct Elf32_Sym
		{
			public uint st_name;
			public uint st_value;
			public uint st_size;
			public byte st_info;
			public byte st_other;
			public ushort st_shndx;
		}

		#endregion

		#region PRX Types

		private static class ModuleNids
		{
			public const uint MODULE_INFO = 0xF01D73A7;
			public const uint MODULE_BOOTSTART = 0xD3744BE0;
			public const uint MODULE_REBOOT_BEFORE = 0x2F064FA6;
			public const uint MODULE_START = 0xD632ACDB;
			public const uint MODULE_START_THREAD_PARAMETER = 0x0F7C276C;
			public const uint MODULE_STOP = 0xCEE8593C;
			public const uint MODULE_STOP_THREAD_PARAMETER = 0xCF0CC697;
		}

		private enum PspModuleFlags : ushort
		{
			User = 0x0000,
			Kernel = 0x1000,
		}

		private enum PspLibFlags : ushort
		{
			SysLib = 0x8000,
			DirectJump = 0x0001,
			Syscall = 0x4000,
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct PspModuleExport
		{
			public uint name;
			public ushort version;
			public ushort flags;
			public byte entry_size;
			public byte var_count;
			public ushort func_count;
			public uint exports;
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct PspModuleImport
		{
			public uint name;
			public ushort version;
			public ushort flags;
			public byte entry_size;
			public byte var_count;
			public ushort func_count;
			public uint nids;
			public uint funcs;
		}

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		private struct PspModuleInfo
		{
			public uint flags;

			public sbyte name;	// name is 28 bytes - this takes up the first 1
			public uint name1;	// following is the rest of the 27 bytes
			public uint name2;
			public uint name3;
			public uint name4;
			public uint name5;
			public uint name6;
			public ushort name7;
			public byte name8;

			public uint gp;
			public uint exports;
			public uint exp_end;
			public uint imports;
			public uint imp_end;
		}

		#endregion

		#region Helpers

		private Elf32_Shdr* FindSection( byte* buffer, string name )
		{
			Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;

			Elf32_Shdr* strhdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * header->e_shstrndx ) );
			byte* pstrtab = buffer + strhdr->sh_offset;

			for( int n = 0; n < header->e_shnum; n++ )
			{
				Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );
				byte* pname = ( byte* )( pstrtab + shdr->sh_name );
				string sname = new string( ( sbyte* )pname );
				if( sname == name )
					return shdr;
			}

			return null;
		}

		private Elf32_Shdr* FindSection( byte* buffer, uint index )
		{
			Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;
			Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * index ) );
			return shdr;
		}

		private byte* FindSectionAddress( byte* buffer, uint index )
		{
			Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;
			Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * index ) );
			return buffer + shdr->sh_offset;
		}

		private byte* GetStrTab( byte* buffer )
		{
			Elf32_Shdr* strtab = FindSection( buffer, ".strtab" );
			Debug.Assert( strtab != null );
			if( strtab == null )
				return null;

			byte* pstrtab = buffer + strtab->sh_offset;

			return pstrtab;
		}

		private string GetName( byte* pstrtab, int index )
		{
			byte* pname = ( byte* )( pstrtab + index );
			return new string( ( sbyte* )pname );
		}

		private struct HiReloc
		{
			public uint Value;
			public uint* CodePointer;
		}

		#endregion

		public LoadResults LoadModule( ModuleType type, Stream moduleStream, LoadParameters parameters )
		{
			Elf32_Shdr*[] allocSections = new Elf32_Shdr*[ 128 ];
			Elf32_Shdr*[] relocSections = new Elf32_Shdr*[ 128 ];
			int allocSectionsCount = 0;
			int relocSectionsCount = 0;

			LoadResults results = new LoadResults();
			results.Successful = false;
			results.Imports = new List<StubImport>();
			results.Exports = new List<StubExport>();
			results.ExportNames = new List<string>();
			results.MissingImports = new FastLinkedList<DelayedImport>();

			Debug.Assert( moduleStream != null );
			Debug.Assert( moduleStream.CanRead == true );
			if( ( moduleStream == null ) ||
				( moduleStream.CanRead == false ) )
				return results;

			Kernel kernel = _bios._kernel;
			ICpu cpu = kernel.Cpu;
			MemorySystem memory = kernel.MemorySystem;

			IntPtr pbuffer = IntPtr.Zero;
			try
			{
				// Load the entire module in to memory - nasty, but it works
				int length = ( int )moduleStream.Length;
				byte[] mbuffer = new byte[ length ];
				moduleStream.Read( mbuffer, 0, length );
				pbuffer = Marshal.AllocHGlobal( length );
				Debug.Assert( pbuffer != IntPtr.Zero );
				Marshal.Copy( mbuffer, 0, pbuffer, length );
				mbuffer = null;

				byte* buffer = ( byte* )pbuffer.ToPointer();

				// Get header
				Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;
				//Debug.Assert( header->e_magic == Elf32_Ehdr.Magic );
				if( header->e_magic != Elf32_Ehdr.Magic )
				{
					Log.WriteLine( Verbosity.Critical, Feature.Loader, "Module header does not match: {0:X8} != {1:X8}", header->e_magic, Elf32_Ehdr.Magic );
					return results;
				}
				Debug.Assert( header->e_machine == Elf32_Ehdr.MachineMips );
				if( header->e_machine != Elf32_Ehdr.MachineMips )
					return results;

				/*switch( type )
				{
				case ModuleType.Boot:
					//Debug.Assert( header->e_type == ELF_EXEC_TYPE );
					break;
				case ModuleType.Prx:
					//Debug.Assert( header->e_type == ELF_PRX_TYPE );
					break;
				}*/

				results.EntryAddress = header->e_entry;
				bool needsRelocation = (
					( header->e_entry < 0x08000000 ) ||
					( header->e_type == ElfType.Prx ) );

				// p-hdrs
				//for( int n = 0; n < header->e_phnum; n++ )
				//{
				//    Elf32_Phdr* phdr = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * n ) );
				//}

				// 0x08900000
				//uint defaultLoad = 0x08880000;
				uint defaultLoad = 0x08000000;

				// s-hdrs
				uint lextents = defaultLoad;
				uint extents = 0;
				for( int n = 0; n < header->e_shnum; n++ )
				{
					Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );

					if( ( shdr->sh_flags & ShFlags.Allocate ) == ShFlags.Allocate )
					{
						allocSections[ allocSectionsCount++ ] = shdr;
						if( shdr->sh_addr < lextents )
							lextents = shdr->sh_addr;
						uint upperBound = shdr->sh_addr + shdr->sh_size;
						if( upperBound > extents )
							extents = upperBound;
					}
					if( ( shdr->sh_type == ShType.SHT_REL ) ||
						( shdr->sh_type == ShType.SHT_PRXRELOC ) )
						relocSections[ relocSectionsCount++ ] = shdr;
				}

				// Module info
				Elf32_Shdr* moduleInfoShdr = FindSection( buffer, ".rodata.sceModuleInfo" );
				Debug.Assert( moduleInfoShdr != null );
				PspModuleInfo* moduleInfo = ( PspModuleInfo* )( buffer + moduleInfoShdr->sh_offset );
				results.GlobalPointer = moduleInfo->gp;
				results.Name = new string( &moduleInfo->name );

				// See if this module is already implemented
				BiosModule existing = _bios.FindModule( results.Name );
				if( existing != null )
				{
					Log.WriteLine( Verbosity.Normal, Feature.Loader, "attempting to load module {0} with BIOS implementation; ignoring", results.Name );
					results.Successful = true;
					results.Ignored = true;
					return results;
				}
				else
					Log.WriteLine( Verbosity.Normal, Feature.Loader, "adding new module {0}", results.Name );

				uint baseAddress = 0;
				KMemoryBlock moduleBlock = null;
				if( needsRelocation == true )
				{
					if( type == ModuleType.Boot )
						baseAddress = defaultLoad;
					else
					{
						// Find the next block in RAM
						moduleBlock = kernel.Partitions[ 2 ].Allocate( KAllocType.Low, 0, extents );
						moduleBlock.Name = string.Format( "Module {0}", results.Name );
						baseAddress = moduleBlock.Address;
					}

					results.EntryAddress += baseAddress;
					results.LowerBounds = baseAddress;
					results.UpperBounds = baseAddress + extents;
				}
				else
				{
					Debug.Assert( type == ModuleType.Boot );
					results.LowerBounds = lextents; //0x08900000;
					results.UpperBounds = extents;
				}

				// Allocate space taken by module
				if( type == ModuleType.Boot )
				{
					Debug.Assert( moduleBlock == null );
					moduleBlock = kernel.Partitions[ 2 ].Allocate( KAllocType.Specific, results.LowerBounds, results.UpperBounds - results.LowerBounds );
					moduleBlock.Name = string.Format( "Module {0}", results.Name );
				}
				else
				{
					// Should be done above
					Debug.Assert( moduleBlock != null );
				}
				Debug.Assert( moduleBlock != null );

				// Allocate sections in memory
				for( int n = 0; n < allocSectionsCount; n++ )
				{
					Elf32_Shdr* shdr = allocSections[ n ];
					uint address = baseAddress + shdr->sh_addr;
					byte* pdest = memory.Translate( address );

					switch( shdr->sh_type )
					{
						case ShType.SHT_NOBITS:
							// Write zeros?
							MemorySystem.ZeroMemory( pdest, shdr->sh_size );
							break;
						default:
						case ShType.SHT_PROGBITS:
							MemorySystem.CopyMemory( buffer + shdr->sh_offset, pdest, shdr->sh_size );
							break;
					}
				}

				// Perform relocations
				if( needsRelocation == true )
				{
					List<HiReloc> hiRelocs = new List<HiReloc>( 10 );
					uint Elf32_RelSize = ( uint )sizeof( Elf32_Rel );

					// Find symbol table
					Elf32_Shdr* symtabShdr = FindSection( buffer, ".symtab" );
					byte* symtab = null;
					if( symtabShdr != null )
						symtab = buffer + symtabShdr->sh_offset;

					// Gather relocations
					for( int n = 0; n < header->e_shnum; n++ )
					{
						Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );
						if( ( shdr->sh_type != ShType.SHT_REL ) &&
							( shdr->sh_type != ShType.SHT_PRXRELOC ) )
							continue;

						hiRelocs.Clear();

						Elf32_Shdr* targetHdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * shdr->sh_info ) );

						// If the target is not allocated, do not perform the relocation
						if( ( targetHdr->sh_flags & ShFlags.Allocate ) != ShFlags.Allocate )
							continue;

						uint count = shdr->sh_size / Elf32_RelSize;
						for( uint m = 0; m < count; m++ )
						{
							Elf32_Rel* reloc = ( Elf32_Rel* )( buffer + shdr->sh_offset + ( sizeof( Elf32_Rel ) * m ) );
							//uint rtype = ELF32_R_TYPE( reloc->r_info );
							//uint symbolIndex = ELF32_R_SYM( reloc->r_info );
							RelType rtype = ( RelType )( reloc->r_info & 0xFF );
							uint symbolIndex = reloc->r_info >> 8;
							uint offset = reloc->r_offset;

							if( rtype == RelType.None )
								continue;

							uint basea = baseAddress;
							offset += baseAddress;

							if( shdr->sh_type == ShType.SHT_REL )
							{
								// Elf style - use symbol table
								Elf32_Sym* sym = null;
								if( symbolIndex != 0 )
								{
									Debug.Assert( symtab != null );
									sym = ( Elf32_Sym* )( symtab + ( sizeof( Elf32_Sym ) * symbolIndex ) );
									basea += sym->st_value;
								}
							}
							else if( shdr->sh_type == ShType.SHT_PRXRELOC )
							{
								// PRX style - crazy!
								uint offsetHeaderN = symbolIndex & 0xFF;
								uint valueHeaderN = ( symbolIndex >> 8 ) & 0xFF;
								Debug.Assert( offsetHeaderN < header->e_phnum );
								Debug.Assert( valueHeaderN < header->e_phnum );
								Elf32_Phdr* offsetHeader = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * offsetHeaderN ) );
								Elf32_Phdr* valueHeader = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * valueHeaderN ) );

								basea += valueHeader->p_vaddr;
								offset += offsetHeader->p_vaddr;
							}

							uint* pcode = ( uint* )memory.Translate( offset );
							Debug.Assert( pcode != null );

							switch( rtype )
							{
								case RelType.MipsHi16:
									{
										HiReloc hiReloc = new HiReloc();
										hiReloc.Value = basea;
										hiReloc.CodePointer = ( uint* )pcode;
										hiRelocs.Add( hiReloc );
									}
									break;
								case RelType.MipsLo16:
									{
										uint code = *pcode;
										uint vallo = ( ( code & 0x0000FFFF ) ^ 0x00008000 ) - 0x00008000;
										while( hiRelocs.Count > 0 )
										{
											HiReloc hiReloc = hiRelocs[ hiRelocs.Count - 1 ];
											hiRelocs.RemoveAt( hiRelocs.Count - 1 );

											Debug.Assert( hiReloc.Value == basea );

											uint value2 = *hiReloc.CodePointer;
											uint temp = ( ( value2 & 0x0000FFFF ) << 16 ) + vallo;
											temp += basea;
											temp = ( ( temp >> 16 ) + ( ( ( temp & 0x00008000 ) != 0 ) ? ( uint )1 : ( uint )0 ) ) & 0x0000FFFF;
											value2 = ( uint )( ( value2 & ~0x0000FFFF ) | temp );

											//Debug.WriteLine( string.Format( "   Updating memory at 0x{0:X8} to {1:X8} (from previous HI16)", hiReloc.Address, value2 ) );
											*hiReloc.CodePointer = value2;
										}
										*pcode = ( uint )( ( code & ~0x0000FFFF ) | ( ( basea + vallo ) & 0x0000FFFF ) );
									}
									break;
								case RelType.Mips26:
									{
										uint code = *pcode;
										uint addr = ( code & 0x03FFFFFF ) << 2;
										addr += basea;
										*pcode = ( code & unchecked( ( uint )~0x03FFFFFF ) ) | ( addr >> 2 );
									}
									break;
								case RelType.Mips32:
									*pcode += basea;
									break;
							}
						}

						Debug.Assert( hiRelocs.Count == 0 );
						// Finish off hi relocs?
					}
				}

				if( parameters.AppendDatabase == true )
				{
					Debug.Assert( Diag.Instance.Database != null );
					Diag.Instance.Database.BeginUpdate();
				}

				int variableExportCount = 0;
				int functionExportCount = 0;

				// Get exports
				uint PspModuleExportSize = ( uint )sizeof( PspModuleExport );
				uint pexports = moduleInfo->exports + ( ( needsRelocation == true ) ? baseAddress : 0 );
				uint exportCount = ( moduleInfo->exp_end - moduleInfo->exports ) / PspModuleExportSize;
				for( uint n = 0; n < exportCount; n++ )
				{
					PspModuleExport* ex = ( PspModuleExport* )memory.Translate( pexports + ( PspModuleExportSize * n ) );
					string name = null;
					if( ex->name != 0x0 )
						name = kernel.ReadString( ex->name );

					// Ignore null names?
					if( ex->name == 0x0 )
						continue;
					results.ExportNames.Add( name );

					uint* pnid = ( uint* )memory.Translate( ex->exports );
					uint* pvalue = ( uint* )memory.Translate( ex->exports + ( ( uint )( ex->func_count + ex->var_count ) * 4 ) );

					for( int m = 0; m < ( ex->func_count + ex->var_count ); m++ )
					{
						uint nid = *( pnid + m );
						uint value = *( pvalue + m );

						StubExport stubExport = new StubExport();
						if( m < ex->func_count )
						{
							stubExport.Type = StubType.Function;
							Log.WriteLine( Verbosity.Verbose, Feature.Loader, "export func {0} 0x{1:X8}: {2:X8}", name, nid, value );
							functionExportCount++;
						}
						else
						{
							stubExport.Type = StubType.Variable;
							Log.WriteLine( Verbosity.Verbose, Feature.Loader, "export var {0} 0x{1:X8}: {2:X8}", name, nid, value );
							variableExportCount++;
						}
						stubExport.ModuleName = name;
						stubExport.NID = nid;
						stubExport.Address = value;
						results.Exports.Add( stubExport );
					}
				}

				int nidGoodCount = 0;
				int nidNotFoundCount = 0;
				int nidNotImplementedCount = 0;
				int moduleNotFoundCount = 0;
				List<string> missingModules = new List<string>();

				// Get imports
				uint PspModuleImportSize = ( uint )sizeof( PspModuleImport );
				uint pimports = moduleInfo->imports + ( ( needsRelocation == true ) ? baseAddress : 0 );
				uint importCount = ( moduleInfo->imp_end - moduleInfo->imports ) / PspModuleImportSize;
				for( uint n = 0; n < importCount; n++ )
				{
					PspModuleImport* im = ( PspModuleImport* )memory.Translate( pimports + ( PspModuleImportSize * n ) );
					string name = null;
					if( im->name != 0x0 )
						name = kernel.ReadString( im->name );
					Debug.Assert( name != null );

					BiosModule module = _bios.FindModule( name );
					if( module == null )
						missingModules.Add( name );

					uint* pnid = ( uint* )memory.Translate( im->nids );
					uint* pfuncs = ( uint* )memory.Translate( im->funcs );

					// Functions & variables at the same time
					for( int m = 0; m < ( im->func_count + im->var_count ); m++ )
					{
						uint nid = *( pnid + m );
						uint* pcode = ( pfuncs + ( m * 2 ) );

						BiosFunction function = _bios.FindFunction( nid );

						StubImport stubImport = new StubImport();
						if( module == null )
						{
							stubImport.Result = StubReferenceResult.ModuleNotFound;
							moduleNotFoundCount++;
							results.MissingImports.Enqueue( new DelayedImport( stubImport ) );
						}
						else if( function == null )
						{
							Log.WriteLine( Verbosity.Normal, Feature.Loader, "{0} 0x{1:X8} not found (NID not present)", name, nid );
							stubImport.Result = StubReferenceResult.NidNotFound;
							nidNotFoundCount++;
							results.MissingImports.Enqueue( new DelayedImport( stubImport ) );
						}
						else if( function.IsImplemented == false )
						{
							Log.WriteLine( Verbosity.Normal, Feature.Loader, "0x{1:X8} found and patched at 0x{2:X8} -> {0}::{3} (NI)", name, nid, ( uint )pcode, function.Name );
							stubImport.Result = StubReferenceResult.NidNotImplemented;
							nidNotImplementedCount++;
						}
						else
						{
							Log.WriteLine( Verbosity.Verbose, Feature.Loader, "0x{1:X8} found and patched at 0x{2:X8} -> {0}::{3}", name, nid, ( uint )pcode, function.Name );
							stubImport.Result = StubReferenceResult.Success;
							nidGoodCount++;
						}

						// Create dummy when needed
						if( function == null )
						{
							function = new BiosFunction( module, nid );
							_bios.RegisterFunction( function );
						}

						if( m < im->func_count )
							stubImport.Type = StubType.Function;
						else
							stubImport.Type = StubType.Variable;
						stubImport.ModuleName = name;
						stubImport.Function = function;
						stubImport.NID = nid;
						stubImport.Address = ( uint )pcode;
						results.Imports.Add( stubImport );

						function.StubAddress = ( uint )( im->funcs + ( m * 2 ) * 4 );

						// Perform fixup
						{
							uint syscall = cpu.RegisterSyscall( nid );
							*( pcode + 1 ) = ( uint )( ( syscall << 6 ) | 0xC );
						}

						// Add to debug database
						if( parameters.AppendDatabase == true )
						{
							Debug.Assert( Diag.Instance.Database != null );
							Method method = new Method( MethodType.Bios, function.StubAddress, 8, new BiosFunctionToken( stubImport.Function ) );
							Diag.Instance.Database.AddSymbol( method );
						}
					}
				}

				// If symbols are present, use those to add methods and variables
				// Otherwise, we need to try to figure them out (good luck!)
				if( parameters.AppendDatabase == true )
				{
					Debug.Assert( Diag.Instance.Database != null );
					IDebugDatabase db = Diag.Instance.Database;

					// Find symbol table
					Elf32_Shdr* symtabShdr = FindSection( buffer, ".symtab" );
					if( ( symtabShdr != null ) &&
						( symtabShdr->sh_size > 0x100 ) )
					{
						byte* strtab = FindSectionAddress( buffer, symtabShdr->sh_link );

						int symbolCount = ( int )symtabShdr->sh_size / sizeof( Elf32_Sym );
						byte* symtab = buffer + symtabShdr->sh_offset;
						byte* p = symtab;
						for( int n = 0; n < symbolCount; n++, p += sizeof( Elf32_Sym ) )
						{
							Elf32_Sym* sym = ( Elf32_Sym* )p;
							uint symType = sym->st_info & ( uint )0xF;
							if( ( symType != 0x1 ) && ( symType != 0x2 ) )
								continue;
							if( sym->st_size == 0 )
								continue;

							Elf32_Shdr* shdr = FindSection( buffer, sym->st_shndx );
							Debug.Assert( shdr != null );

							string name = null;
							if( sym->st_name != 0 )
								name = this.GetName( strtab, ( int )sym->st_name );
							uint address = baseAddress + sym->st_value;
							if( needsRelocation == true )
								address += shdr->sh_addr;
							Symbol symbol = null;
							if( symType == 0x1 )
							{
								// OBJECT
								symbol = new Variable( address, sym->st_size, name );
							}
							else if( symType == 0x2 )
							{
								// FUNC
								symbol = new Method( MethodType.User, address, sym->st_size );//, name );
							}
							if( symbol != null )
								db.AddSymbol( symbol );
						}

						results.HadSymbols = true;

						Log.WriteLine( Verbosity.Verbose, Feature.Loader, "Found {0} methods and {1} variables in the symbol table", db.MethodCount, db.VariableCount );
					}
					else
					{
						// No symbol table found - try to build the symbols
						Elf32_Shdr* textShdr = FindSection( buffer, ".text" );
						Debug.Assert( textShdr != null );
						uint textAddress = baseAddress + textShdr->sh_addr;
						byte* text = memory.TranslateMainMemory( ( int )textAddress );
						uint size = textShdr->sh_size;
						this.Analyze( db, text, size, textAddress );

						Log.WriteLine( Verbosity.Verbose, Feature.Loader, "Found {0} methods by analysis", db.MethodCount );
					}

					// End update, started above
					Diag.Instance.Database.EndUpdate();

					//foreach( Method method in db.GetMethods() )
					//    Debug.WriteLine( method.ToString() );
				}

#if DEBUG
				// Print stats
				Log.WriteLine( Verbosity.Critical, Feature.Loader, "Load complete: {0}/{1} ({2}%) NIDs good; {3} not implemented, {4} not found, {5} in missing modules = {6} total", nidGoodCount, results.Imports.Count - moduleNotFoundCount, ( nidGoodCount / ( float )( results.Imports.Count - moduleNotFoundCount ) ) * 100.0f, nidNotImplementedCount, nidNotFoundCount, moduleNotFoundCount, results.Imports.Count );
				if( missingModules.Count > 0 )
				{
					Log.WriteLine( Verbosity.Normal, Feature.Loader, "{0} missing modules (contain {1} NIDs ({2}% of total)):", missingModules.Count, moduleNotFoundCount, ( moduleNotFoundCount / ( float )results.Imports.Count ) * 100.0f );
					foreach( string moduleName in missingModules )
						Log.WriteLine( Verbosity.Normal, Feature.Loader, "\t\t{0}", moduleName );
				}
				if( ( functionExportCount > 0 ) || ( variableExportCount > 0 ) )
					Log.WriteLine( Verbosity.Critical, Feature.Loader, "Exported {0} functions and {1} variables", functionExportCount, variableExportCount );
#endif

				results.Successful = true;

				if( results.Successful == true )
				{
					// If we export things, go back and find previously loaded modules that may need fixing up
					if( ( functionExportCount > 0 ) || ( variableExportCount > 0 ) )
					{
						bool fixupResult = this.FixupDelayedImports( kernel, results );
						Debug.Assert( fixupResult == true );
					}

					// If we are the boot load - we do some special stuff like run start, etc
					// If we are a PRX, all that is taken care of for us by the user code making calls
					if( type == ModuleType.Boot )
					{
						KModule module = new KModule( kernel, new BiosModule( results.Name, results.Exports.ToArray() ) );
						module.LoadResults = results;
						kernel.UserModules.Add( module );
						Debug.Assert( kernel.MainModule == null );
						kernel.MainModule = module;
						kernel.AddHandle( module );

						// Allocate room for args
						KMemoryBlock argsBlock = kernel.Partitions[ 6 ].Allocate( KAllocType.High, 0, 0xFF ); // 256b of args - enough?
						argsBlock.Name = string.Format( "Module {0} args", results.Name );

						// Set arguments - we put these right below user space, and right above the stack
						uint args = 0;
						uint argp = argsBlock.Address;
						string arg0 = parameters.Path.AbsolutePath.Replace( '\\', '/' ) + "/";
						args += ( uint )arg0.Length + 1;
						kernel.WriteString( argp, arg0 );
						uint envp = argp;// + args;
						// write envp??

						// What we do here simulates what the modulemgr does - it creates a user mode thread that
						// runs module_start (_start, etc) and then exits when complete.
						// NOTE: If we are a PRX, the entry address will correspond to module_start, so we don't need to do anything!

						// Create a thread
						KThread thread = new KThread( kernel,
							module,
							kernel.Partitions[ 6 ],
							"kmodule_thread",
							results.EntryAddress,
							0,
							KThreadAttributes.User,
							0x4000 );
						thread.GlobalPointer = results.GlobalPointer;
						kernel.AddHandle( thread );
						thread.Start( args, argp );

						// Setup handler so that we get the callback when the thread ends and we can kill it
						cpu.SetContextSafetyCallback( thread.ContextID, new ContextSafetyDelegate( this.KmoduleThreadEnd ), ( int )thread.UID );

						Log.WriteLine( Verbosity.Verbose, Feature.Loader, "starting kmodule loading thread with UID {0}", thread.UID );

						// Schedule so that our thread runs
						kernel.Schedule();
					}
				}
			}
			finally
			{
				if( pbuffer != IntPtr.Zero )
					Marshal.FreeHGlobal( pbuffer );
			}

			return results;
		}

		private bool FixupDelayedImports( Kernel kernel, LoadResults results )
		{
			int fixupCount = 0;
			foreach( KModule previous in kernel.UserModules )
			{
				if( previous.LoadResults.MissingImports.Count == 0 )
					continue;

				LinkedListEntry<DelayedImport> e = previous.LoadResults.MissingImports.HeadEntry;
				while( e != null )
				{
					LinkedListEntry<DelayedImport> next = e.Next;
					DelayedImport import = e.Value;
					if( results.ExportNames.Contains( import.StubImport.ModuleName ) == true )
					{
						// Find export
						StubExport myExport = null;
						foreach( StubExport export in results.Exports )
						{
							if( export.NID == import.StubImport.NID )
							{
								myExport = export;
								break;
							}
						}
						if( myExport != null )
						{
							previous.LoadResults.MissingImports.Remove( e );
							fixupCount++;

							// Fixup
							Log.WriteLine( Verbosity.Verbose, Feature.Loader, "Fixing up module {0} delayed import 0x{1:X8}; value={2:X8}", previous.Name, import.StubImport.NID, myExport.Address );

							Debug.Assert( myExport.Type == import.StubImport.Type );
							if( myExport.Type == StubType.Function )
							{
								BiosFunction function = import.StubImport.Function;
								// TODO: Change function module, etc?

								// Perform fixup
								uint* pcode = ( uint* )function.StubAddress;
								{
									// j {target}
									// nop
									*( pcode + 0 ) = ( uint )( ( 2 << 26 ) | ( ( myExport.Address >> 2 ) & 0x03FFFFFF ) );
									*( pcode + 1 ) = ( uint )0;
								}
							}
							else
							{
								throw new NotImplementedException( "Cannot handle fixups of variable imports" );
							}
						}
					}
					e = next;
				}
			}
			return true;
		}

		private void KmoduleThreadEnd( int tcsId, int state )
		{
			// Our loader thread ended - kill it!
			Kernel kernel = _bios._kernel;
			KThread thread = kernel.GetHandle<KThread>( ( uint )state );
			Debug.Assert( thread != null );
			if( thread != null )
			{
				Log.WriteLine( Verbosity.Verbose, Feature.Loader, "killing kmodule loading thread with UID {0}", thread.UID );

				thread.Exit( 0 );
				kernel.RemoveHandle( thread.UID );

				kernel.Schedule();

				thread.Delete();
			}
		}
	}
}
