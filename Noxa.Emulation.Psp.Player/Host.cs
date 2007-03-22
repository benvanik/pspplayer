// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Noxa.Emulation.Psp.Player.Configuration;
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player
{
	class Host : IEmulationHost
	{
		protected Player _player;
		protected Instance _instance;
		protected IDebugger _debugger;
		protected Settings _componentSettings;

		public Host( string[] args )
		{
			// Ensure settings are proper
			if( Properties.Settings.Default.PluginSearchPaths == null )
				Properties.Settings.Default.PluginSearchPaths = new System.Collections.Specialized.StringCollection();

#if DEBUG
			string PluginPath = @"C:\Dev\Noxa.Emulation\trunk\debug\";
			if( Properties.Settings.Default.PluginSearchPaths.Contains( PluginPath ) == false )
				Properties.Settings.Default.PluginSearchPaths.Add( PluginPath );
#endif

			Properties.Settings.Default.Save();

			_debugger = null;
			_player = new Player( this );

			this.Load();

			// If we have args, try to start the player
			if( args.Length > 0 )
			{
				_player.StartGameDirect( args[ 0 ] );
			}
		}

		public Player Player
		{
			get
			{
				return _player;
			}
		}

		public IEmulationInstance CurrentInstance
		{
			get
			{
				return _instance;
			}
		}

		public IDebugger Debugger
		{
			get
			{
				return _debugger;
			}
		}

		public Settings ComponentSettings
		{
			get
			{
				return _componentSettings;
			}
			set
			{
				_componentSettings = value;
			}
		}

		public void Load()
		{
			string value = Properties.Settings.Default.ComponentSettings;
			if( ( value == null ) ||
				( value.Length == 0 ) )
			{
				_componentSettings = new Settings();
			}
			else
			{
				_componentSettings = new Settings( value, Settings.MaximumDepth, false );
			}
		}

		public void Save()
		{
			Properties.Settings.Default.ComponentSettings = _componentSettings.Serialize( Settings.MaximumDepth );
			Properties.Settings.Default.Save();
		}

		public bool CreateInstance( bool suppressXmb )
		{
			EmulationParameters emulationParams = new EmulationParameters();

			Dictionary<KeyValuePair<string, int>, ComponentParameters> parameters = new Dictionary<KeyValuePair<string, int>, ComponentParameters>();

			Settings[] settingsList = _componentSettings.GetValue<Settings[]>( "settingsList" );
			if( settingsList != null )
			{
				foreach( Settings settings in settingsList )
				{
					string componentName = settings.GetValue<string>( "componentName" );
					int index = settings.GetValue<int>( "componentIndex" );
					ComponentParameters p = settings.GetValue<ComponentParameters>( "parameters" );
					parameters.Add( new KeyValuePair<string, int>( componentName, index ), p );
				}
			}

			Settings[] pickedComponents = _componentSettings.GetValue<Settings[]>( "pickedComponents" );
			if( pickedComponents != null )
			{
				foreach( Settings settings in pickedComponents )
				{
					string componentName = settings.GetValue<string>( "componentName" );
					int index = settings.GetValue<int>( "componentIndex" );
					string componentPath = settings.GetValue<string>( "componentPath" );

					Assembly assembly;
					try
					{
						assembly = Assembly.LoadFile( componentPath );
					}
					catch
					{
						Debug.WriteLine( string.Format( "CreateInstance: assembly not found or not loadable: {0}", componentPath ) );
						continue;
					}
					Type type = assembly.GetType( componentName, false );
					if( type == null )
					{
						Debug.WriteLine( string.Format( "CreateInstance: component not found or not loadable: {0} in {1}", componentName, componentPath ) );
						continue;
					}
					IComponent component;
					try
					{
						component = ( IComponent )Activator.CreateInstance( type );
					}
					catch
					{
						Debug.WriteLine( string.Format( "CreateInstance: component could not be instantiated: {0} in {1}", componentName, componentPath ) );
						continue;
					}

					switch( component.Type )
					{
						case ComponentType.Audio:
							emulationParams.AudioComponent = component;
							break;
						case ComponentType.Bios:
							emulationParams.BiosComponent = component;
							break;
						case ComponentType.Cpu:
							emulationParams.CpuComponent = component;
							break;
						case ComponentType.Input:
							emulationParams.InputComponent = component;
							break;
						case ComponentType.UserMedia:
							emulationParams.MemoryStickComponent = component;
							break;
						case ComponentType.GameMedia:
							emulationParams.UmdComponent = component;
							break;
						case ComponentType.Network:
							emulationParams.IOComponents.Add( component );
							break;
						case ComponentType.Video:
							emulationParams.VideoComponent = component;
							break;
						case ComponentType.Other:
						default:
							Debug.WriteLine( string.Format( "CreateInstance: unknown component type for {0} in {1}", componentName, componentPath ) );
							continue;
					}

					ComponentParameters p = null;
					KeyValuePair<string, int> specifier = new KeyValuePair<string, int>( componentName, index );
					if( parameters.ContainsKey( specifier ) == true )
						p = parameters[ specifier ];
					if( p != null )
						emulationParams.Parameters[ component ] = p;
					else
						emulationParams.Parameters[ component ] = new ComponentParameters();
				}
			}

			_instance = new Instance( this, emulationParams, suppressXmb );

			List<ComponentIssue> issues = _instance.Test();
			bool showReport = false;
			bool allowRun = true;
			if( issues.Count > 0 )
			{
				int errorCount = 0;
				foreach( ComponentIssue issue in issues )
				{
					if( issue.Level == IssueLevel.Error )
						errorCount++;
				}
				if( errorCount == 0 )
				{
					if( Properties.Settings.Default.ShowReportOnWarnings == true )
						showReport = true;
				}
				else
				{
					showReport = true;
					allowRun = false;
				}
			}

			if( showReport == true )
			{
				IssueReport report = new IssueReport( this, issues );
				report.ShowDialog( _player );
			}

			if( allowRun == true )
				return _instance.Create();
			else
				return false;
		}

		public void AttachDebugger()
		{
			Debug.Assert( _debugger == null, "Debugger already attached!" );
			if( _debugger != null )
				return;

			//_debugger = new Noxa.Emulation.Psp.Player.Development.Debugger( this );
			//_debugger.Show();

			_instance.Cpu.EnableDebugging();
		}
	}
}
