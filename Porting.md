# Porting PSP Player #
I'll admit it, I own a Mac, so this is somewhat interesting to me. If you want to help port PSP Player to your platform of choice, let me know and we can chat.

## Major Problems ##
### .NET ###
First off, everything is built on .NET. This means that for non-Microsoft platforms, [mono](http://www.mono-project.com/) will be needed. The latest versions of mono support all the C# languages features I use (generics, partial classes, etc), so things should be OK, although I haven't tested it.

### PSP Player Front-end ###
The front-end cannot, in its present state, be ported easily. It depends heavily on WinForms. With minor modifications I was able to get it to start with the mono WinForms stack, however it didn't work right. Ideally, there would be a GTK# version, and I may even whip one up for fun.

### C++/CLI ###
The Ultra CPU, and OpenGL video plugin use [C++/CLI](http://en.wikipedia.org/wiki/C++/CLI), which last time I checked was not supported by mono. This means that you just can't use these. Since these are the components that are getting the most love, that leaves everyone else out of luck. Sucks. I'll be coming back to the managed world as soon as I get things working in mixed-mode land (and processors get faster).

## Other Concerns ##
### DynaRec CPU Component ###
The purely managed (C#) CPU component is really the only choice for porting. My concern here is over how good the [Reflection.Emit](http://windowssdk.msdn.microsoft.com/en-us/library/system.reflection.emit.aspx) support is in mono. I don't think I'm doing anything too crazy, but it needs to support [DynamicMethod](http://windowssdk.msdn.microsoft.com/en-us/library/system.reflection.emit.dynamicmethod.aspx) and be able to do things quickly.
'''NOTE:''' the DynaRec CPU has been deprecated - there goes hopes of running on OS X for now :( It will be worked on again in the future, though, or someone else is welcome to work on it.

### Minor OS Dependant Stuff ###
A lot of the core library and BIOS should have no problem running anywhere, however there may be issues with timing and the other areas that use ~Win32. The other kinds of things would have to be found by experimentation (either by compile-time errors or run-time errors).