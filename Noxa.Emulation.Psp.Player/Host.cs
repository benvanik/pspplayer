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

namespace Noxa.Emulation.Psp.Player
{
	class Host : IEmulationHost
	{
		protected Player _player;
		protected Instance _instance;
		protected Settings _componentSettings;

		public Host()
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

			_player = new Player( this );

			this.Load();
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

		public void CreateInstance()
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
							emulationParams.IOComponents.Add( component );
							break;
						case ComponentType.UserMedia:
							emulationParams.IOComponents.Add( component );
							break;
						case ComponentType.GameMedia:
							emulationParams.IOComponents.Add( component );
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

			_instance = new Instance( this, emulationParams );
			_instance.Create();
		}
	}
}
