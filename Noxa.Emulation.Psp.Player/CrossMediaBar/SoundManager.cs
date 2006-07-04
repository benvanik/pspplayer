using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.DirectX.Xact;
using Xact = Microsoft.DirectX.Xact;
using System.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	enum Sounds
	{
		Tick = 0
	}

	class SoundManager
	{
		protected Engine _engine;
		protected WaveBank _waveBank;
		protected SoundBank _soundBank;

		protected Dictionary<Sounds, short> _sounds = new Dictionary<Sounds, short>();
		
		public SoundManager()
		{
		}

		public void Update()
		{
			if( _engine != null )
				_engine.DoWork();
		}

		public void Play( Sounds sound )
		{
			_soundBank.Play( _sounds[ sound ] );
		}

		public void Recreate()
		{
			this.Disable();
			this.Enable();
		}

		public void Enable()
		{
			if( _engine != null )
				return;

			_engine = new Engine( false );

			RuntimeParameters rtParams = new RuntimeParameters();
			rtParams.LookAheadTime = 250;
			_engine.Initialize( rtParams );

			Microsoft.DirectX.GraphicsBuffer gb;
			int fileSize;

			gb = new Microsoft.DirectX.GraphicsBuffer();
			fileSize = ( int )( new FileInfo( "Resources/Wave Bank.xwb" ).Length );
			gb.AllocateNew( fileSize );
			gb.Write( File.ReadAllBytes( "Resources/Wave Bank.xwb" ) );
			_waveBank = new WaveBank( _engine, gb );

			gb = new Microsoft.DirectX.GraphicsBuffer();
			fileSize = ( int )( new FileInfo( "Resources/Sound Bank.xsb" ).Length );
			gb.AllocateNew( fileSize );
			gb.Write( File.ReadAllBytes( "Resources/Sound Bank.xsb" ) );
			_soundBank = new SoundBank( _engine, gb );

			short tickIndex = _soundBank.GetCueIndex( "PspTick" );
			if( tickIndex != Xact.Constants.IndexInvalid )
				_sounds.Add( Sounds.Tick, tickIndex );
			_soundBank.Prepare( tickIndex );
		}

		public void Disable()
		{
			if( _engine == null )
				return;

			_sounds.Clear();

			_soundBank.Destroy();
			_soundBank = null;

			_waveBank.Destroy();
			_waveBank = null;

			_engine.ShutDown();
			_engine.Dispose();
			_engine = null;
		}

		public void Cleanup()
		{
			this.Disable();
		}
	}
}
