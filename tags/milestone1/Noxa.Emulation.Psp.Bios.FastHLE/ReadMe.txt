========================================================================
    DYNAMIC LINK LIBRARY : Noxa.Emulation.Psp.Bios.FastHLE Project Overview
========================================================================

Who'd a thunk I'd be writing a BIOS in C++/CLI??

The main reason for doing this is speed. I get a beautiful class library
(which is the whole reason I'm using .NET in the first place), as well
as the ability to run unmanaged code.

This is essentially a mixed-mode BIOS. There will be things that will
be implemented in native code while a majority will be managed. There
may even be a few that are both!

BiosApi is used to allow the CPU to query the BIOS for native interfaces
to its methods. The CPU is greedy and will try to get native pointers
to everything - it'd be smart to only implement native versions where
it matters.

Some areas that are prime targets for being native:
- semaphores
- locking
- events
- timers (maybe)
- pipes

Now that the BIOS is native, it may also be possible to implement
native interfaces for other plugins, like input. For example, the
XInput plugin is managed now, but just pinvokes to the API. It would
almost be easier to write it in C++/CLI, and it sure would save some
transitions (going from native->managed->native->managed->native
all the time). Something to investigate, for sure!
