# Building PSP Player #

## Prerequisites ##
### Windows ###
  * [Visual Studio 2008](http://msdn.microsoft.com/vstudio/)
    * It should be possible to compile things separately with [VC# Express](http://msdn.microsoft.com/vstudio/express/visualcsharp/) and [VC++ Express](http://msdn.microsoft.com/vstudio/express/visualc/default.aspx)
  * [DirectX 9.0 SDK](http://msdn.microsoft.com/directx/sdk/)
  * [TortoiseSVN](http://tortoisesvn.tigris.org/) (or your favorite SVN client)

### Linux / OS X ###
Everything has been designed and developed under Windows, with an aim towards keeping things somewhat portable. If you are running Linux/OS X you are welcome to mess with things, however I won't give you much help. See [Porting](Porting.md) for more information.

## Getting the Code ##
The SVN repository is publicy accessible for read-only access. You'll need everything in ` trunk/ ` to compile, so make sure you grab it all.

[SVN Access Information](http://code.google.com/p/pspplayer/source)

If you haven't used SVN before, I recommend checking out the [manual](http://www.tortoisesvn.net/support). I won't help you with it, unless there is a problem with the server.
Drop the ` trunk/ ` to get the milestone branches.

## Compiling ##
Opening ` Noxa.Emulation.sln ` in Visual Studio should be enough. Just make sure you are set to ` Release ` and build. If you run in ` Debug `, things will be slow. I recommend running with Ctrl-F5 and attaching the debugger when a crash occurs. Some of this slowness will be fixed in an upcoming patch.

## Running and Initial Configuration ##
When built, you should find everything in your ` trunk/debug ` or ` trunk/release ` directory. Just run the exe and hit Configure. You'll need to pick the components you want to use (Use the Ultra CPU, ManagedHLE BIOS, and OpenGL video driver), and then make sure to set your Memory Stick folder (eg, the 'Test Stick' directory in SVN) by clicking the Configure button next to the User Media group. Save out, hit play, and select your game. Note that there is no need to restart.

## Common Problems ##
  * VC#/VC++ Express is a pain to get going unless you know what you're doing - and if you do, chances are you have VS2008 :) You'll need to rename/delete the .sln file and add the projects to a new solution in VC#E and VCE manually.
  * There may be some absolute paths that have snuck in to the projects - if you get missing lib/include errors, make sure the paths are right. Let me know and I'll fix them.
  * Try to keep the solution and all under a directory with no spaces.
  * The input plugin may require the drivers for the Windows Common Controller. This will not always be the case.
  * Visual Studio will sometimes get confused with project references. If you get a lot of missing namespace errors, remove the references to the projects and re-add them.