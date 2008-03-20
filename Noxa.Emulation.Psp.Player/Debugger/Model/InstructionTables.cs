// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Player.Debugger.Model
{
	static class InstructionTables
	{
		public enum InstructionEncoding
		{
			Imme = 0,
			Spec,
			Spe2,
			Spe3,
			RegI,
			Cop0,
			Cop0CO,
			Cop1,
			Cop1BC,
			Cop2,
			Cop2BC2,
			Cop2Rese,
			VFPU0,
			VFPU1,
			VFPU3,
			VFPU4Jump,
			VFPU7,
			VFPU4,
			VFPU5,
			VFPU6,
			VFPUMatrix1,
			VFPU9,
			ALLEGREX0,
			Dummy,
			Rese,
		}

		#region Flags

		public const uint IS_CONDBRANCH = 0x100;
		public const uint IS_JUMP = 0x200;
		public const uint IS_VFPU = 0x80000000;
		public const uint UNCONDITIONAL = 0x40;
		public const uint BAD_INSTRUCTION = 0x20;
		public const uint DELAYSLOT = 0x10;

		public const uint IN_RS_ADDR = 0x800;
		public const uint IN_RS_SHIFT = 0x400;
		public const uint IN_RS = 0x1000;
		public const uint IN_RT = 0x2000;
		public const uint IN_SA = 0x4000;
		public const uint IN_IMM16 = 0x8000;
		public const uint IN_IMM26 = 0x10000;
		public const uint IN_MEM = 0x20000;
		public const uint IN_OTHER = 0x40000;
		public const uint IN_FPUFLAG = 0x80000;

		public const uint OUT_RT = 0x100000;
		public const uint OUT_RD = 0x200000;
		public const uint OUT_RA = 0x400000;
		public const uint OUT_MEM = 0x800000;
		public const uint OUT_OTHER = 0x1000000;
		public const uint OUT_FPUFLAG = 0x2000000;

		#endregion

		#region Tables

		public static TableEntry[] tableImmediate = new TableEntry[] //xxxxxx ..... 64
		{
			//0
			new TableReference( InstructionEncoding.Spec ),
			new TableReference( InstructionEncoding.RegI ),
			new InstructionEntry( "j", Formatters.JumpType, IS_JUMP | IN_IMM26 | DELAYSLOT ),
			new InstructionEntry( "jal", Formatters.JumpType, IS_JUMP | IN_IMM26 | OUT_RA | DELAYSLOT ),
			new InstructionEntry( "beq", Formatters.RelBranch2, IS_CONDBRANCH | IN_RS | IN_RT | DELAYSLOT ),
			new InstructionEntry( "bne", Formatters.RelBranch2, IS_CONDBRANCH | IN_RS | IN_RT | DELAYSLOT ),
			new InstructionEntry( "blez", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | DELAYSLOT ),
			new InstructionEntry( "bgtz", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | DELAYSLOT ),
			//8
			new InstructionEntry( "addi", Formatters.addi, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "addiu", Formatters.addi, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "slti", Formatters.IType, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "sltiu", Formatters.IType, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "andi", Formatters.IType, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "ori", Formatters.ori, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "xori", Formatters.IType, IN_RS | IN_IMM16 | OUT_RT ),
			new InstructionEntry( "lui", Formatters.IType1, IN_IMM16 | OUT_RT ),
			//16
			new TableReference( InstructionEncoding.Cop0 ), //cop0
			new TableReference( InstructionEncoding.Cop1 ), //cop1
			new TableReference( InstructionEncoding.Cop2 ), //cop2
			TableEntry.Invalid, //copU
			//20
			new InstructionEntry( "beql", Formatters.RelBranch2, IS_CONDBRANCH | IN_RS | IN_RT | DELAYSLOT ), //L = likely
			new InstructionEntry( "bnel", Formatters.RelBranch2, IS_CONDBRANCH | IN_RS | IN_RT | DELAYSLOT ),
			new InstructionEntry( "blezl", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | DELAYSLOT ),
			new InstructionEntry( "bgtzl", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | DELAYSLOT ),
			//24
			new TableReference( InstructionEncoding.VFPU0 ),
			new TableReference( InstructionEncoding.VFPU1 ),
			new TableReference( InstructionEncoding.Dummy ),
			new TableReference( InstructionEncoding.VFPU3 ),
			new TableReference( InstructionEncoding.Spe2 ), //special2
			TableEntry.Invalid, //, "jalx", 0, Formatters.JumpType,
			TableEntry.Invalid,
			new TableReference( InstructionEncoding.Spe3 ), //special3
			//32
			new InstructionEntry( "lb", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lh", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lwl", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lw", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lbu", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lhu", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			new InstructionEntry( "lwr", Formatters.ITypeMem, IN_MEM | IN_IMM16 | IN_RS_ADDR | OUT_RT ),
			TableEntry.Invalid,
			//40
			new InstructionEntry( "sb", Formatters.ITypeMem, IN_IMM16 | IN_RS_ADDR | IN_RT | OUT_MEM ),
			new InstructionEntry( "sh", Formatters.ITypeMem, IN_IMM16 | IN_RS_ADDR | IN_RT | OUT_MEM ),
			new InstructionEntry( "swl", Formatters.ITypeMem, IN_IMM16 | IN_RS_ADDR | IN_RT | OUT_MEM ),
			new InstructionEntry( "sw", Formatters.ITypeMem, IN_IMM16 | IN_RS_ADDR | IN_RT | OUT_MEM ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "swr", Formatters.ITypeMem, IN_IMM16 | IN_RS_ADDR | IN_RT | OUT_MEM ),
			new InstructionEntry( "cache", Formatters.Generic, 0 ),
			//48
			new InstructionEntry( "ll", Formatters.Generic, 0 ),
			new InstructionEntry( "lwc1", Formatters.FPULS, 0 ),
			new InstructionEntry( "lv.s", Formatters.SV, IS_VFPU ),
			TableEntry.Invalid, // HIT THIS IN WIPEOUT
			new TableReference( InstructionEncoding.VFPU4Jump ),
			new InstructionEntry( "lv", Formatters.SVLRQ, IS_VFPU ),
			new InstructionEntry( "lv.q", Formatters.SVQ, IS_VFPU ), //copU
			new TableReference( InstructionEncoding.VFPU5 ),
			//56
			new InstructionEntry( "sc", Formatters.Generic, 0 ),
			new InstructionEntry( "swc1", Formatters.FPULS, 0 ), //copU
			new InstructionEntry( "sv.s", Formatters.SV, IS_VFPU ),
			TableEntry.Invalid,
			//60
			new TableReference( InstructionEncoding.VFPU6 ),
			new InstructionEntry( "sv", Formatters.SVLRQ, IS_VFPU ), //copU
			new InstructionEntry( "sv.q", Formatters.SVQ, IS_VFPU ),
			new InstructionEntry( "vflush", Formatters.Vflush, IS_VFPU ),
		};

		public static TableEntry[] tableSpecial = new TableEntry[] // 64
		{
			new InstructionEntry( "sll", Formatters.ShiftType, OUT_RD | IN_RT | IN_SA ),
			TableEntry.Invalid, //copu
			new InstructionEntry( "srl", Formatters.ShiftType, OUT_RD | IN_RT | IN_SA ),
			new InstructionEntry( "sra", Formatters.ShiftType, OUT_RD | IN_RT | IN_SA ),
			new InstructionEntry( "sllv", Formatters.VarShiftType, OUT_RD | IN_RT | IN_RS_SHIFT ),
			TableEntry.Invalid,
			new InstructionEntry( "srlv", Formatters.VarShiftType, OUT_RD | IN_RT | IN_RS_SHIFT ),
			new InstructionEntry( "srav", Formatters.VarShiftType, OUT_RD | IN_RT | IN_RS_SHIFT ),
			//8
			new InstructionEntry( "jr", Formatters.JumpRegType, 0 ),
			new InstructionEntry( "jalr", Formatters.JumpRegType, 0 ),
			new InstructionEntry( "movz", Formatters.RType3, OUT_RD | IN_RS | IN_RT ),
			new InstructionEntry( "movn", Formatters.RType3, OUT_RD | IN_RS | IN_RT ),
			new InstructionEntry( "syscall", Formatters.Syscall, 0 ),
			new InstructionEntry( "break", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			new InstructionEntry( "sync", Formatters.Generic, 0 ),
			//16
			new InstructionEntry( "mfhi", Formatters.FromHiloTransfer, OUT_RD | IN_OTHER ),
			new InstructionEntry( "mthi", Formatters.ToHiloTransfer, IN_RS | OUT_OTHER ),
			new InstructionEntry( "mflo", Formatters.FromHiloTransfer, OUT_RD | IN_OTHER ),
			new InstructionEntry( "mtlo", Formatters.ToHiloTransfer, IN_RS | OUT_OTHER ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "clz", Formatters.RType2, OUT_RD | IN_RS | IN_RT ),
			new InstructionEntry( "clo", Formatters.RType2, OUT_RD | IN_RS | IN_RT ),
			//24
			new InstructionEntry( "mult", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			new InstructionEntry( "multu", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			new InstructionEntry( "div", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			new InstructionEntry( "divu", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			new InstructionEntry( "madd", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			new InstructionEntry( "maddu", Formatters.MulDivType, IN_RS | IN_RT | OUT_OTHER ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			//32
			new InstructionEntry( "add", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "addu", Formatters.addu, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "sub", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "subu", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "and", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "or", Formatters.addu, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "xor", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "nor", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			//40
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "slt", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "sltu", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "max", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			new InstructionEntry( "min", Formatters.RType3, IN_RS | IN_RT | OUT_RD ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			//48
			new InstructionEntry( "tge", Formatters.RType3, 0 ),
			new InstructionEntry( "tgeu", Formatters.RType3, 0 ),
			new InstructionEntry( "tlt", Formatters.RType3, 0 ),
			new InstructionEntry( "tltu", Formatters.RType3, 0 ),
			new InstructionEntry( "teq", Formatters.RType3, 0 ),
			TableEntry.Invalid,
			new InstructionEntry( "tne", Formatters.RType3, 0 ),
			TableEntry.Invalid,
			//56
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableSpecial2 = new TableEntry[] // 64
		{
			new InstructionEntry( "add.s", Formatters.FPU3op, 0 ),
			new InstructionEntry( "sub.s", Formatters.FPU3op, 0 ),
			new InstructionEntry( "mul.s", Formatters.FPU3op, 0 ),
			new InstructionEntry( "div.s", Formatters.FPU3op, 0 ),
			new InstructionEntry( "sqrt.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "abs.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "mov.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "neg.s", Formatters.FPU2op, 0 ),
			//8
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			new InstructionEntry( "round.w.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "trunc.w.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "ceil.w.s", Formatters.FPU2op, 0 ),
			new InstructionEntry( "floor.w.s", Formatters.FPU2op, 0 ),
			//16
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//24
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//32
			new InstructionEntry( "cvt.s.w", Formatters.FPU2op, 0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//36
			new InstructionEntry( "cvt.w.s", Formatters.FPU2op, 0 ),
			TableEntry.Invalid,
			new InstructionEntry( "dis.int", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			//40
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//48
			new InstructionEntry( "c.f", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.un", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.eq", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ueq", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.olt", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ult", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ole", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ule", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.sf", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ngle", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.seq", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ngl", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.lt", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.nge", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.le", Formatters.FPUComp, 0 ),
			new InstructionEntry( "c.ngt", Formatters.FPUComp, 0 ),
		};

		public static TableEntry[] tableSpecial3 = new TableEntry[] // 64
		{
			new InstructionEntry( "ext", Formatters.Special3, IN_RS | OUT_RT ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "ins", Formatters.Special3, IN_RS | OUT_RT ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//16
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//32
			new TableReference( InstructionEncoding.ALLEGREX0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//40
			new TableReference( InstructionEncoding.ALLEGREX0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,

			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,

			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			new InstructionEntry( "rdhwr", Formatters.Generic, 0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableRegImm = new TableEntry[] // 32
		{
			new InstructionEntry( "bltz", Formatters.RelBranch, IS_CONDBRANCH | IN_RS ),
			new InstructionEntry( "bgez", Formatters.RelBranch, IS_CONDBRANCH | IN_RS ),
			new InstructionEntry( "bltzl", Formatters.RelBranch, IS_CONDBRANCH | IN_RS ),
			new InstructionEntry( "bgezl", Formatters.RelBranch, IS_CONDBRANCH | IN_RS ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			new InstructionEntry( "tgei", Formatters.Generic, 0 ),
			new InstructionEntry( "tgeiu", Formatters.Generic, 0 ),
			new InstructionEntry( "tlti", Formatters.Generic, 0 ),
			new InstructionEntry( "tltiu", Formatters.Generic, 0 ),
			new InstructionEntry( "teqi", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			new InstructionEntry( "tnei", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			//16
			new InstructionEntry( "bltzal", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | OUT_RA ),
			new InstructionEntry( "bgezal", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | OUT_RA ),
			new InstructionEntry( "bltzall", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | OUT_RA ), //L = likely
			new InstructionEntry( "bgezall", Formatters.RelBranch, IS_CONDBRANCH | IN_RS | OUT_RA ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//24
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			new InstructionEntry( "synci", Formatters.Generic, 0 ),
		};

		public static TableEntry[] tableCop2 = new TableEntry[] // 32
		{
			new InstructionEntry( "mfc2", Formatters.Generic, OUT_RT ),
			TableEntry.Invalid,
			new InstructionEntry( "cfc2", Formatters.Generic, 0 ),
			new InstructionEntry( "mfv", Formatters.Mftv, 0 ),
			new InstructionEntry( "mtc2", Formatters.Generic, IN_RT ),
			TableEntry.Invalid,
			new InstructionEntry( "ctc2", Formatters.Generic, 0 ),
			new InstructionEntry( "mtv", Formatters.Mftv, 0 ),
			//8
			new TableReference( InstructionEncoding.Cop2BC2 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			new InstructionEntry( "??", Formatters.Generic, 0 ),
			//16
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableCop2BC2 = new TableEntry[] // 4
		{
			new InstructionEntry( "bvf", Formatters.VBranch, IS_CONDBRANCH ),
			new InstructionEntry( "bvt", Formatters.VBranch, IS_CONDBRANCH ),
			new InstructionEntry( "bvfl", Formatters.VBranch, IS_CONDBRANCH ),
			new InstructionEntry( "bvtl", Formatters.VBranch, IS_CONDBRANCH ),
		};

		public static TableEntry[] tableCop0 = new TableEntry[] // 32
		{
			new InstructionEntry( "mfc0", Formatters.Generic, OUT_RT ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//4
			new InstructionEntry( "mtc0", Formatters.Generic, IN_RT ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "rdpgpr", Formatters.Generic, 0 ),
			new InstructionEntry( "mfmc0", Formatters.Generic, 0 ),
			//12
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "wrpgpr", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			//16-32
			new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ),
			new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ), new TableReference( InstructionEncoding.Cop0CO ),
		};

		//we won't encounter these since we only do user mode emulation
		public static TableEntry[] tableCop0CO = new TableEntry[] // 64
		{
			TableEntry.Invalid,
			new InstructionEntry( "tlbr", Formatters.Generic, 0 ),
			new InstructionEntry( "tlbwi", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "tlbwr", Formatters.Generic, 0 ),
			TableEntry.Invalid,
			//8
			new InstructionEntry( "tlbp", Formatters.Generic, 0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//16
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			//24
			new InstructionEntry( "eret", Formatters.Generic, 0 ),
			new InstructionEntry( "iack", Formatters.Generic, 0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			new InstructionEntry( "deret", Formatters.Generic, 0 ),
			//32
			new InstructionEntry( "wait", Formatters.Generic, 0 ),
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableCop1 = new TableEntry[] // 32
		{
			new InstructionEntry( "mfc1", Formatters.mxc1, OUT_RT ),
			TableEntry.Invalid,
			new InstructionEntry( "cfc1", Formatters.mxc1, 0 ),
			TableEntry.Invalid,
			new InstructionEntry( "mtc1", Formatters.mxc1, IN_RT ),
			TableEntry.Invalid,
			new InstructionEntry( "ctc1", Formatters.mxc1, 0 ),
			TableEntry.Invalid,
			//8
			new TableReference( InstructionEncoding.Cop1BC ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//12
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//16
			new TableReference( InstructionEncoding.Spe2 ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//20
			new TableReference( InstructionEncoding.Spe2 ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//24
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableCop1BC = new TableEntry[] // 32
		{
			new InstructionEntry( "bc1f", Formatters.FPUBranch, IS_CONDBRANCH | IN_FPUFLAG ),
			new InstructionEntry( "bc1t", Formatters.FPUBranch, IS_CONDBRANCH | IN_FPUFLAG ),
			new InstructionEntry( "bc1fl", Formatters.FPUBranch, IS_CONDBRANCH | IN_FPUFLAG ),
			new InstructionEntry( "bc1tl", Formatters.FPUBranch, IS_CONDBRANCH | IN_FPUFLAG ),
			//4
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
			TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid, TableEntry.Invalid,
		};

		public static TableEntry[] tableVFPU0 = new TableEntry[] // 8
		{
			new InstructionEntry( "vadd", Formatters.VectorSet3, IS_VFPU ),
			new InstructionEntry( "vsub", Formatters.VectorSet3, IS_VFPU ),
			new InstructionEntry( "vsbn", Formatters.VectorSet3, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vdiv", Formatters.VectorSet3, IS_VFPU ),
		};

		public static TableEntry[] tableVFPU1 = new TableEntry[] // 8
		{
			new InstructionEntry( "vmul", Formatters.VectorSet3, IS_VFPU ),
			new InstructionEntry( "vdot", Formatters.VectorDot, IS_VFPU ),
			new InstructionEntry( "vscl", Formatters.VScl, IS_VFPU ),
			new InstructionEntry( "vhdp", Formatters.Generic, IS_VFPU ),
			TableEntry.Invalid,
			new InstructionEntry( "vcrs", Formatters.Vcrs, IS_VFPU ),
			new InstructionEntry( "vdet", Formatters.Generic, IS_VFPU ),
			TableEntry.Invalid,
		};

		public static TableEntry[] tableVFPU3 = new TableEntry[] //011011 xxx 8
		{
			new InstructionEntry( "vcmp", Formatters.Vcmp, IS_VFPU ),
			new InstructionEntry( "v???", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmin", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmax", Formatters.Generic, IS_VFPU ),
			TableEntry.Invalid,
			new InstructionEntry( "vscmp", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vsge", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vslt", Formatters.Generic, IS_VFPU ),
		};

		public static TableEntry[] tableVFPU4Jump = new TableEntry[] //110100 xxxxx 32
		{
			new TableReference( InstructionEncoding.VFPU4 ),
			new TableReference( InstructionEncoding.VFPU7 ),
			new TableReference( InstructionEncoding.VFPU9 ),
			new InstructionEntry( "vcst", Formatters.Vcst, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//16
			new InstructionEntry( "vf2in", Formatters.Vf2i, IS_VFPU ),
			new InstructionEntry( "vf2iz", Formatters.Vf2i, IS_VFPU ),
			new InstructionEntry( "vf2iu", Formatters.Vf2i, IS_VFPU ),
			new InstructionEntry( "vf2id", Formatters.Vf2i, IS_VFPU ),
			//20
			new InstructionEntry( "vi2f", Formatters.Vf2i, IS_VFPU ),
			new InstructionEntry( "vcmov", Formatters.Vcmov, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			//24
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vwbn.s", Formatters.Generic, IS_VFPU ),
		};

		public static TableEntry[] tableVFPU7 = new TableEntry[] // 32
		{
			new InstructionEntry( "vrnds", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vrndi", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vrndf1", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vrndf2", Formatters.Generic, IS_VFPU ),
			//4
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//16
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vf2h", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vh2f", Formatters.Generic, IS_VFPU ),
			//20
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vlgb", Formatters.Generic, IS_VFPU ),
			//24
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vus2i", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vs2i", Formatters.Generic, IS_VFPU ),
			//28
			new InstructionEntry( "vi2uc", Formatters.Vi2x, IS_VFPU ),
			new InstructionEntry( "vi2c", Formatters.Vi2x, IS_VFPU ),
			new InstructionEntry( "vi2us", Formatters.Vi2x, IS_VFPU ),
			new InstructionEntry( "vi2s", Formatters.Vi2x, IS_VFPU ),
		};

		// 110100 00000 10100 0000000000000000
		// 110100 00000 10111 0000000000000000
		public static TableEntry[] tableVFPU4 = new TableEntry[] //110100 00000 xxxxx 32
		{
			new InstructionEntry( "vmov", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vabs", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vneg", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vidt", Formatters.VectorSet1, IS_VFPU ),
			new InstructionEntry( "vsat0", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vsat1", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vzero", Formatters.VectorSet1, IS_VFPU ),
			new InstructionEntry( "vone", Formatters.VectorSet1, IS_VFPU ),
			//8
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//16
			new InstructionEntry( "vrcp", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vrsq", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vsin", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vcos", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vexp2", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vlog2", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vsqrt", Formatters.VectorSet2, IS_VFPU ),
			new InstructionEntry( "vasin", Formatters.VectorSet2, IS_VFPU ),
			//24
			new InstructionEntry( "vnrcp", Formatters.VectorSet2, IS_VFPU ),
			TableEntry.Invalid,
			new InstructionEntry( "vnsin", Formatters.VectorSet2, IS_VFPU ),
			TableEntry.Invalid,
			//28
			new InstructionEntry( "vrexp2", Formatters.VectorSet2, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//32
		};

		public static TableEntry[] tableVFPU5 = new TableEntry[] //110111 xxx 8
		{
			new InstructionEntry( "vpfxs", Formatters.VPFXST, IS_VFPU ),
			new InstructionEntry( "vpfxs", Formatters.VPFXST, IS_VFPU ),
			new InstructionEntry( "vpfxt", Formatters.VPFXST, IS_VFPU ),
			new InstructionEntry( "vpfxt", Formatters.VPFXST, IS_VFPU ),
			new InstructionEntry( "vpfxd", Formatters.VPFXD, IS_VFPU ),
			new InstructionEntry( "vpfxd", Formatters.VPFXD, IS_VFPU ),
			new InstructionEntry( "viim.s", Formatters.Viim, IS_VFPU ),
			new InstructionEntry( "vfim.s", Formatters.Viim, IS_VFPU ),
		};

		public static TableEntry[] tableVFPU6 = new TableEntry[] //111100 xxx 32
		{
			//0
			new InstructionEntry( "vmmul", Formatters.MatrixMult, IS_VFPU ),
			new InstructionEntry( "vmmul", Formatters.MatrixMult, IS_VFPU ),
			new InstructionEntry( "vmmul", Formatters.MatrixMult, IS_VFPU ),
			new InstructionEntry( "vmmul", Formatters.MatrixMult, IS_VFPU ),
			//4
			new InstructionEntry( "v(h)tfm2", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm2", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm2", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm2", Formatters.Vtfm, IS_VFPU ),
			//8
			new InstructionEntry( "v(h)tfm3", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm3", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm3", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm3", Formatters.Vtfm, IS_VFPU ),
			//12
			new InstructionEntry( "v(h)tfm4", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm4", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm4", Formatters.Vtfm, IS_VFPU ),
			new InstructionEntry( "v(h)tfm4", Formatters.Vtfm, IS_VFPU ),
			//16
			new InstructionEntry( "vmscl", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmscl", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmscl", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmscl", Formatters.Generic, IS_VFPU ),
			//20
			new InstructionEntry( "vcrsp/vqm", Formatters.CrossQuat, IS_VFPU ),
			new InstructionEntry( "vcrsp/vqm", Formatters.CrossQuat, IS_VFPU ),
			new InstructionEntry( "vcrsp/vqm", Formatters.CrossQuat, IS_VFPU ),
			new InstructionEntry( "vcrsp/vqm", Formatters.CrossQuat, IS_VFPU ),
			//24
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//28
			new TableReference( InstructionEncoding.VFPUMatrix1 ),
			new InstructionEntry( "vrot", Formatters.VRot, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
		};

		public static TableEntry[] tableVFPUMatrixSet1 = new TableEntry[] //111100 11100 0xxxx (rm x is 16) 16
		{
			new InstructionEntry( "vmmov", Formatters.MatrixSet2, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vmidt", Formatters.MatrixSet1, IS_VFPU ),

			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "vmzero", Formatters.MatrixSet1, IS_VFPU ),
			new InstructionEntry( "vmone", Formatters.MatrixSet1, IS_VFPU ),

			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
		};

		public static TableEntry[] tableVFPU9 = new TableEntry[] //110100 00010 xxxxx 32
		{
			new InstructionEntry( "vsrt1", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vsrt2", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vbfy1", Formatters.Vbfy, IS_VFPU ),
			new InstructionEntry( "vbfy2", Formatters.Vbfy, IS_VFPU ),
			//4
			new InstructionEntry( "vocp", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vsocp", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vfad", Formatters.Vfad, IS_VFPU ),
			new InstructionEntry( "vavg", Formatters.Generic, IS_VFPU ),
			//8
			new InstructionEntry( "vsrt3", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vsrt4", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vsgn", Formatters.Generic, IS_VFPU ),
			TableEntry.Invalid,
			//12
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,

			//16
			new InstructionEntry( "vmfvc", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vmtvc", Formatters.Generic, IS_VFPU ),
			TableEntry.Invalid,
			TableEntry.Invalid,

			//20
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//24
			TableEntry.Invalid,
			new InstructionEntry( "vt4444", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vt5551", Formatters.Generic, IS_VFPU ),
			new InstructionEntry( "vt5650", Formatters.Generic, IS_VFPU ),

			//28
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
		};

		public static TableEntry[] tableALLEGREX0 = new TableEntry[] //111111 32
		{
			TableEntry.Invalid,
			TableEntry.Invalid,
			new InstructionEntry( "wsbh", Formatters.Allegrex2, 0 ),
			new InstructionEntry( "wsbw", Formatters.Allegrex2, 0 ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//8
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//16
			new InstructionEntry( "seb", Formatters.Allegrex, IN_RT | OUT_RD ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//20
			new InstructionEntry( "bitrev", Formatters.Allegrex, IN_RT | OUT_RD ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//24
			new InstructionEntry( "seh", Formatters.Allegrex, IN_RT | OUT_RD ),
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			//28
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
			TableEntry.Invalid,
		};

		public static int[ , ] encodingBits = new int[ , ]
		{
			{ 26, 6 }, 	// IMME
			{ 0, 6 }, 	// Special
			{ 0, 6 }, 	// special2
			{ 0, 6 }, 	// special3
			{ 16, 5 }, 	// RegImm
			{ 21, 5 }, 	// Cop0
			{ 0, 6 }, 	// Cop0CO
			{ 21, 5 }, 	// Cop1
			{ 16, 5 }, 	// Cop1BC
			{ 21, 5 }, 	// Cop2
			{ 16, 2 }, 	// Cop2BC2
			{ 0, 0 }, 	// Cop2Rese
			{ 23, 3 }, 	// VFPU0
			{ 23, 3 }, 	// VFPU1
			{ 23, 1 }, 	// VFPU3
			{ 21, 5 }, 	// VFPU4Jump
			{ 16, 5 }, 	// VFPU7
			{ 16, 5 }, 	// VFPU4
			{ 23, 3 }, 	// VFPU5
			{ 21, 5 }, 	// VFPU6
			{ 16, 3 }, 	// VFPUMatrix1
			{ 16, 5 }, 	// VFPU9
			{ 6, 5 }, 	// ALLEGREX0
			{ 24, 2 }, 	// EMUHACK
		};

		public static TableEntry[][] Tables = new TableEntry[][]
		{
			tableImmediate,
			tableSpecial,
			tableSpecial2,
			tableSpecial3,
			tableRegImm,
			tableCop0,
			tableCop0CO,
			tableCop1,
			tableCop1BC,
			tableCop2,
			tableCop2BC2,
			null,
			tableVFPU0,
			tableVFPU1,
			tableVFPU3,
			tableVFPU4Jump,
			tableVFPU7,
			tableVFPU4,
			tableVFPU5,
			tableVFPU6,
			tableVFPUMatrixSet1,
			tableVFPU9,
			tableALLEGREX0,
			null,
		};

		#endregion

		public static InstructionEntry GetInstruction( uint op )
		{
			InstructionEncoding encoding = InstructionEncoding.Imme;
			TableEntry instr = tableImmediate[ op >> 26 ];
			if( instr is InstructionEntry )
				return ( InstructionEntry )instr;
			while( instr is TableReference )
			{
				TableReference tref = ( TableReference )instr;

				TableEntry[] table = Tables[ ( int )encoding ];
				int mask = ( ( 1 << encodingBits[ ( int )encoding ,1 ] ) - 1 );
				int shift = encodingBits[ ( int )encoding , 0 ];
				int subop = ( int )( op >> shift ) & mask;
				instr = table[ subop ];
				if( encoding == InstructionEncoding.Rese )
					return null; //invalid instruction
				if( instr == null )
					return null;
				if( instr is InstructionEntry )
					return ( InstructionEntry )instr;
				encoding = tref.Reference;
			}
			return null;
		}
	}
}
