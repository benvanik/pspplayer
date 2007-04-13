========================================================================
    DYNAMIC LINK LIBRARY : Noxa.Emulation.Psp.Cpu.R4000Ultra Project Overview
========================================================================

Yet another CPU core ^_^
This one should be much faster than the managed R4000DynRec that uses
CIL->x86, as this emits direct x86 instructions and should have low call
overhead.
It's kind of a pain in the ass to have to manage 3 CPU cores (in fact,
the interpreted core is already way behind the DynRec core and doesn't
run anything), but writing the CPU core is so much fun!


Really, really good link:
http://www.ibiblio.org/pub/historic-linux/early-ports/Mips/doc/Mips/
