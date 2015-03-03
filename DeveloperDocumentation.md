# Code Style #

To keep everyones code looking the same so VS doesn't auto restyle it all the time there are some standard styles, to load them up:
Go to Tools->Import and Export Settings in VS, and import the setting file (trunk/CodeFormatting.vssettings). These ONLY specify C# editor settings. Now, when you write code, it will auto format to the code style used everywhere else in the project.

# Game Docs #
  * [Every Extend Extra](EveryExtendExtra.md)

# Audio / Video support #
Currently most of the Audio functionality is not implemented / does nothing so we can run more programs. We will probably need to include a lib to do decoding of video/audio for us.

libavcodec/ffmpeg looks like our best bet. It seems to have Atrac3 decoding, and it definately has MP3/WMA/H263/H264 support.
  * http://ffmpeg.mplayerhq.hu/

## libAtrac3 / AudioCodec (Not sure on actual module name(s), FIXME) ##

Example usage of Atrac3/AudioCodec libs on PSP:
  * http://forums.ps2dev.org/viewtopic.php?t=5500 (Moonlight's example)
  * http://insomniac.0x89.org/pgeAt3.rar (InsertWittyName Example, thanks!)
  * http://forums.ps2dev.org/viewtopic.php?t=8357 (Another Example)
  * http://forums.ps2dev.org/viewtopic.php?t=8469 (MP3/AAC Example)

# Networking #

## sceNet/Inet/Apctl/Resolver ##

danzel is working on these, most can be directly mapped to c# functions.

The resolver/simple samples in the SDK are good test targets, don't forget to disable #define printf pspDebugScreenPrintf.

# Video Driver #
  * OpenGL
    * http://www.idevgames.com/forum/archive/index.php/t-10348.html (advanced combining - would be required for sfix/dfix)
    * http://www.devmaster.net/forums/archive/index.php/t-3087.html (more combining)
    * http://www.gpgpu.org/forums/viewtopic.php?=&p=15888 (good use of multiple framebuffers - may be useful for allowing framebuffer reads/writes)
    * **Tools**
      * [GLIntercept](http://glintercept.nutty.org/index.html)
      * [OpenGLExtractor](http://ogle.eyebeamresearch.org/)
  * GU
    * http://forums.ps2dev.org/viewtopic.php?t=2197 (general notes)
    * http://forums.ps2dev.org/viewtopic.php?t=3960 (DXT)
    * http://forums.ps2dev.org/viewtopic.php?p=22980 (DXT)

# CPU #
  * General
    * http://www.mono-project.com/Mono_DataConvert <- may be useful
  * x86
    * http://www.piclist.com/techref/intel/32bit/32bitx86tips.htm
  * .NET
    * [ILStream viz](http://blogs.msdn.com/haibo_luo/archive/2005/10/25/484861.aspx)
    * [Ref.Emit debugging](http://blogs.msdn.com/jmstall/archive/2005/02/03/366429.aspx)
    * [LCG debugging?](http://blogs.msdn.com/jmstall/archive/2006/11/14/lcg-feedback.aspx)
    * [PInvoke Extras](http://msdn.microsoft.com/msdnmag/issues/08/01/CLRInsideOut/default.aspx)
  * VFPU
    * http://wiki.fx-world.org/doku.php