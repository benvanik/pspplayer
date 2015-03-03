# Things That Would be Cool #
An emulator is a large project, especially for one person to undertake. If you are feeling generous or just want to test your coding skills, please feel free to volunteer. I'm happy to give out write-access accounts to anyone who proves trustworthy and knowledgeable. For more information, see [CodeSharing](CodeSharing.md).

## Components ##
### OpenGL Video ###
I decided to write an OpenGL video plugin, in C++/CLI no less! But I still need help with it. I'm not the best when it comes to crazy topics like CLUTs and stuff. The OpenGL plugin needs a lot of love, and unfortunately I don't know how to give it. It'd be best if there was a managed OpenGL plugin so that porting down the road would be easier.

### OpenAL Audio ###
Right now there is no audio support, and it's very low on my list. Most games use [ATRAC3+](http://en.wikipedia.org/wiki/ATRAC) for their audio, and that makes it annoying. When the time does come to support it, I think OpenAL would be the best choice (unless someone has a better idea), simply because DirectSound is being phased out and its replacement XACT would be tricky to use.

### Networking ###
This would require stubbing out all the network functions commonly used in games (I'm not sure what those are yet), and then using System.Net to implement them. I think most should be pretty easy (with 1:1 mappings), but there may be some tricky things with security or other stuff the host may not have permission to do that will require emulation (such as packet writing with the proper MAC address in the Ethernet frame, etc).

### CISO Support ###
Rather trivial, but it would still be nice to have. Will probably do this one day when bored. The issue is that the current UMD driver is pretty crappy, and I'd have to rewrite it to do this right.