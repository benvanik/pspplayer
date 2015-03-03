# Components #
Components are extensions to the core PSP emulation [Architecture](Architecture.md). Almost all logic is contained within them.

## CPU ##
The brain of the emulator lives here, and one is required to run anything. There are various techniques used to implement the CPU and they are represented in the different components:
  * [Managed DynaRec CPU](ManagedCpu.md) (deprecated)
  * [Ultra (native) DynaRec CPU](UltraCpu.md)

## BIOS ##
This is the emulated operating system. PSP Player is technically a HLE (High Level Emulator) in that it interprets calls to the operating system at a high level instead of trying to run the actual operating system itself. This makes things a lot easier, and potentially faster.
  * [Managed HLE BIOS](ManagedHle.md)

## Video ##
All video processing is done here. I'm looking for some help in this area; see [WouldBeCool](WouldBeCool.md) for more information.
  * [Native OpenGL Video](OpenGlVideo.md)

## Audio ##
There are currently no audio components. See [WouldBeCool](WouldBeCool.md) for more information.

## User Media ##
The Memory Stick emulated filesystem. Used for storage of save games, EBOOTs, and other things.
  * [User Host Media](UserHostFs.md)

## Game Media ##
The UMD emulated filesystem.
  * [Game Host Media](GameHostFs.md)
  * [UMD ISO Media](UmdIsoFs.md)

## Input ##
  * [Direct Input Keyboard](DirectInputDriver.md) (deprecated)
  * [XInput Controller](XInputDriver.md) ([Xbox 360](http://www.amazon.com/gp/product/B000BT4CF4/102-8325212-9201725?v=glance&n=172282)'s wired controller, AKA the Windows Common Controller) (deprecated)
  * [Simple Input Driver](SimpleInput.md) (both DirectInput keyboard and XInput gamepad)

## Network ##
There are currently no networking components. See [WouldBeCool](WouldBeCool.md) for more information.