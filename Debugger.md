This page contains information about features/functionality that may not yet be implemented. This is mainly an area where I can put my ideas down so I don't forget them :)

# Features #
  * **Out of process debugging**
    * The debugger is a separate application that communicates with the emulator. This allows for the debugger to resist emulator crashes, remain active while the emulator is being debugged by VS, and not have to worry about fighting for resources with the emulator
  * **Per-game persistence**
    * Breakpoints, options, etc are saved across runs of games
  * **Detailed statistics**
    * Statistics today are pretty poorly implemented - need a nice Performance Counter like setup for gathering, recording, and reporting stats
    * Stats on every component (already have CPU and Video today, but information like context switches/sec, IO bytes/sec, etc from the BIOS and others would be nice)
  * **Lots of Flow Control Options**
    * Normal stepping, step in/out/over, and defined breakpoints
    * Break on any/specific BIOS routines
    * Break on memory access
    * Break on events (sourced from any component, such as context switches from the BIOS)
    * Break on CPU/BIOS/etc errors (such as invalid memory access, file not found, etc)
  * **Detailed inspection**
    * Component state visualization: CPU registers/etc, video state (buffers, textures, etc), BIOS handles (threads, file handles, etc), audio stuff...
    * Memory inspection: dumping, differences, searching (including differential searches)
  * **Code display/analysis**
    * Callstacks
    * Regular disassembly dumps
    * Basic code annotation (jump targets, flow constructs (if(...), for(...)))
    * Infotips like VS (hover over anything to get more information)
    * 'Find All References'-like support for code locations (find code that calls this method, find code that references this address, etc)

# Debug Architecture #
The debugger is set up in a client/server fashion, with the emulator acting as the server via the DebugHost class and the actual debugger acting as the client via the IDebugger interface. On startup of the emulator, the DebugHost begins listening for connections; at any time, a debugger can connect and attach itself. The user can then start a game via the normal emulation interfaces, or via the debugger (which will instruct the emulator to start a game). If the user starts the emulator via the command line, if the --debug option is present the emulator will wait until a debugger attaches before continuing; the 'Start with Debugging' toolbar item does the same thing.

# TODO #
  * Everything