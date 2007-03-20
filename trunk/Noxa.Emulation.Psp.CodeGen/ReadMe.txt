========================================================================
    STATIC LIBRARY : Noxa.Emulation.Psp.CodeGen Project Overview
========================================================================

Simple IA32 code generation utility. It is designed to be fast, not
flexible. I tried to architect it in a way that suits dynarec, and as
such not many of the fancy features of SoftWire are supported. A focus
was put on limiting memory allocation during generation and keeping
things simple.

To generate code, create a CodeGenerator object, add instructions via
their overloads, and then call Generate() to get a pointer to the
memory containing the code. Jump in to it and you're running!

Everything is pretty straight forward, although labels are done
differently than in a normal assembler (and more in the style of what
System.Reflection does): labels are instances, not text strings.
First, get a label instance using DefineLabel() and mark it in the
stream with MarkLabel(). When you want to jump to it, simply pass
the label instance to one of the jump overloads.

Since memory usage is tightly managed, it is important that the only
thing the consumer expects to be around after the Generate() command
is the returned code block itself. The consumer must also never free
anything besides the generated code, as most objects are reused.

-- Details --

This is a 1.5-pass assembler. All instructions are emitted as their
emitted, and on Generate() any label references are resolved. In the
best case scenario of no references, the assembler generates the code
in a single pass (with some memcpy afterwards, but that doesn't count).
When labels exist, they are patched up during Generate() (in the case
of forward definitions).

Label resolution is fun. Marking is done by using the preceeding
MarkLabel call to handle state during the Encode call. Currently there
is a limitation of one label per instruction, although this could be
fixed with a ll or something. When Encode is called, if there is a
marked label, it will populate the Label structure with all the
information Generate needs to patch things up.
When a label is referenced (by a jump or whatever), if it has been
marked the proper code will be emitted right away. Otherwise, the
reference will be added to the table for the forward reference
handling code in Generate().

