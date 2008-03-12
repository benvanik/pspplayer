// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Noxa.Emulation.Psp.Configuration;

namespace Noxa.Emulation.Psp.Player.Configuration
{
	partial class Options : Form
	{
		protected Settings _componentSettings;

		protected Dictionary<ComponentType, List<IComponent>> _components = new Dictionary<ComponentType, List<IComponent>>();
		protected Dictionary<IComponent, ComponentParameters> _params = new Dictionary<IComponent, ComponentParameters>();

		protected Dictionary<ComponentType, ComboBox> _comboLookup = new Dictionary<ComponentType, ComboBox>();
		protected Dictionary<ComponentType, LinkLabel> _linkLookup = new Dictionary<ComponentType, LinkLabel>();
		protected Dictionary<ComponentType, Button> _configLookup = new Dictionary<ComponentType, Button>();

		public Options()
		{
			InitializeComponent();
		}

		public Options( Settings componentSettings )
			: this()
		{
			_componentSettings = componentSettings;
			if( _componentSettings == null )
				_componentSettings = new Settings();
		}

		public Settings ComponentSettings
		{
			get
			{
				return _componentSettings;
			}
		}

		private void Options_Load( object sender, EventArgs e )
		{
			List<string> paths = new List<string>();
			string playerPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ).ToLowerInvariant();
			if( playerPath[ playerPath.Length - 1 ] != '\\' )
				playerPath += '\\';
			paths.Add( playerPath );
			paths.Add( Path.Combine( playerPath, "plugins" ) );
			foreach( string path in Properties.Settings.Default.PluginSearchPaths )
			{
				string lowerPath = path.ToLowerInvariant();
				if( lowerPath[ lowerPath.Length - 1 ] != '\\' )
					lowerPath += '\\';
				if( paths.Contains( lowerPath ) == false )
					paths.Add( lowerPath );
			}

			// Get all types
			List<Type> types = FindComponents( paths.ToArray() );

			foreach( Type type in types )
			{
				IComponent component = Activator.CreateInstance( type ) as IComponent;
				if( component == null )
					continue;

				if( _components.ContainsKey( component.Type ) == false )
					_components.Add( component.Type, new List<IComponent>() );
				List<IComponent> category = _components[ component.Type ];
				category.Add( component );
			}

			// Sort by name
			foreach( List<IComponent> category in _components.Values )
			{
				Comparison<IComponent> comparison = delegate( IComponent x, IComponent y )
				{
					return x.Name.CompareTo( y.Name );
				};
				category.Sort( comparison );
			}

			List<Settings> settingsList = new List<Settings>();
			Settings[] settings = _componentSettings.GetValue<Settings[]>( "settingsList" );
			if( settings != null )
				settingsList.AddRange( settings );
			this.LoadConfigs( settingsList );

			_comboLookup.Add( ComponentType.Audio, this.audioComboBox );
			_linkLookup.Add( ComponentType.Audio, this.audioLinkLabel );
			_configLookup.Add( ComponentType.Audio, this.audioConfigButton );
			_comboLookup.Add( ComponentType.Bios, this.biosComboBox );
			_linkLookup.Add( ComponentType.Bios, this.biosLinkLabel );
			_configLookup.Add( ComponentType.Bios, this.biosConfigButton );
			_comboLookup.Add( ComponentType.Cpu, this.cpuComboBox );
			_linkLookup.Add( ComponentType.Cpu, this.cpuLinkLabel );
			_configLookup.Add( ComponentType.Cpu, this.cpuConfigButton );
			_comboLookup.Add( ComponentType.UserMedia, this.userMediaComboBox );
			_linkLookup.Add( ComponentType.UserMedia, this.userMediaLinkLabel );
			_configLookup.Add( ComponentType.UserMedia, this.userMediaConfigButton );
			_comboLookup.Add( ComponentType.GameMedia, this.gameMediaComboBox );
			_linkLookup.Add( ComponentType.GameMedia, this.gameMediaLinkLabel );
			_configLookup.Add( ComponentType.GameMedia, this.gameMediaConfigButton );
			_comboLookup.Add( ComponentType.Input, this.inputComboBox );
			_linkLookup.Add( ComponentType.Input, this.inputLinkLabel );
			_configLookup.Add( ComponentType.Input, this.inputConfigButton );
			_comboLookup.Add( ComponentType.Network, this.networkComboBox );
			_linkLookup.Add( ComponentType.Network, this.networkLinkLabel );
			_configLookup.Add( ComponentType.Network, this.networkConfigButton );
			_comboLookup.Add( ComponentType.Video, this.videoComboBox );
			_linkLookup.Add( ComponentType.Video, this.videoLinkLabel );
			_configLookup.Add( ComponentType.Video, this.videoConfigButton );

			// Add null
			foreach( ComponentType type in _comboLookup.Keys )
				AddNull( type );

			foreach( ComponentType type in _comboLookup.Keys )
			{
				if( _components.ContainsKey( type ) == true )
					PopulateList( _comboLookup[ type ], _components[ type ] );
			}

			foreach( ComponentType type in _comboLookup.Keys )
			{
				_comboLookup[ type ].SelectedIndexChanged += new EventHandler( ListSelectedIndexChanged );
				_linkLookup[ type ].LinkClicked += new LinkLabelLinkClickedEventHandler( InfoLinkClicked );
				_configLookup[ type ].Click += new EventHandler( ConfigureClick );
			}

			PickComponents( _componentSettings.GetValue<Settings[]>( "pickedComponents" ) );
		}

		private void AddNull( ComponentType type )
		{
			if( _components.ContainsKey( type ) == false )
				_components.Add( type, new List<IComponent>() );
			List<IComponent> category = _components[ type ];
			category.Insert( 0, new NullComponent() );
		}

		private void PopulateList( ComboBox list, IList<IComponent> category )
		{
			list.Items.Clear();
			foreach( IComponent component in category )
			{
				int index = list.Items.Add( component );
			}

			list.Enabled = ( list.Items.Count > 0 );
			if( list.Items.Count > 0 )
				list.SelectedIndex = 0;
		}

		private ComponentType FindType<T>( Dictionary<ComponentType, T> lookup, T sender )
		{
			foreach( ComponentType componentType in lookup.Keys )
			{
				if( lookup[ componentType ].Equals( sender ) == true )
				{
					return componentType;
				}
			}

			return ComponentType.Other;
		}

		private IComponent FindComponent( string name )
		{
			foreach( List<IComponent> components in _components.Values )
			{
				foreach( IComponent component in components )
				{
					if( component.GetType().FullName == name )
						return component;
				}
			}
			return null;
		}

		private void ListSelectedIndexChanged( object sender, EventArgs e )
		{
			ComboBox comboBox = sender as ComboBox;
			ComponentType type = FindType<ComboBox>( _comboLookup, comboBox );

			LinkLabel link = _linkLookup[ type ];
			Button configButton = _configLookup[ type ];

			IComponent component = comboBox.SelectedItem as IComponent;
			if( component.Version != null )
			{
				link.Text = string.Format( "{0}{1} by {2}",
					component.Version.ToString(),
					( component.Build != ComponentBuild.Release ) ? " (" + component.Build.ToString() + ")" : "",
					component.Author );
				configButton.Enabled = component.IsConfigurable;
			}
			else
			{
				link.Text = "";
				configButton.Enabled = false;
			}
		}

		private void InfoLinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			LinkLabel link = sender as LinkLabel;
			ComponentType type = FindType<LinkLabel>( _linkLookup, link );

			IComponent component = _comboLookup[ type ].SelectedItem as IComponent;

			if( ( component.Website != null ) &&
				( component.Website.Length > 0 ) )
			{
				// Ensure valid web address... not :)
				//(http|ftp)s?://(%[[:digit:]A-Fa-f][[:digit:]A-Fa-f]|[-_.!~*';/?:@&=+$,[:alnum:])+
				Process.Start( component.Website );
			}
		}

		private void ConfigureClick( object sender, EventArgs e )
		{
			Button configButton = sender as Button;
			ComponentType type = FindType<Button>( _configLookup, configButton );

			IComponent component = _comboLookup[ type ].SelectedItem as IComponent;

			ComponentParameters parameters;
			if( _params.ContainsKey( component ) == false )
				parameters = new ComponentParameters();
			else
				parameters = _params[ component ];

			ConfigurationBase panel = component.CreateConfiguration( parameters ) as ConfigurationBase;
			ConfigurationHost host = new ConfigurationHost( component, panel );
			if( host.ShowDialog( this ) != DialogResult.Cancel )
			{
				// Ok
				_params[ component ] = host.Parameters;
			}
		}

		private List<Type> FindComponents( string[] searchPaths )
		{
			List<Type> components = new List<Type>();

			foreach( string searchPath in searchPaths )
			{
				if( Directory.Exists( searchPath ) == false )
					continue;

				foreach( string assemblyPath in Directory.GetFiles( searchPath, "*.dll", SearchOption.TopDirectoryOnly ) )
				{
					Assembly assembly;
					try
					{
						// Speeds things up, but note that will we will need to reload
						// later to get the proper types!
						//assembly = Assembly.ReflectionOnlyLoadFrom( assemblyPath );
						assembly = Assembly.LoadFile( assemblyPath );
					}
					catch( Exception e )
					{
						// Failed, ignore
						Log.WriteLine( Verbosity.Critical, Feature.General, "FindComponents: Failed to load assembly " + assemblyPath + ", probably not .NET : " + e.ToString() + " " + e.Message );
						continue;
					}

					foreach( Type type in assembly.GetExportedTypes() )
					{
						Type component = type.GetInterface( "Noxa.Emulation.Psp.IComponent", false );
						if( component != null )
						{
							Log.WriteLine( Verbosity.Verbose, Feature.General, "FindComponents: Found component " + type.FullName + " in " + assemblyPath );
							components.Add( type );
						}
					}
				}
			}

			return components;
		}

		private void LoadConfigs( List<Settings> settingsList )
		{
			foreach( Settings settings in settingsList )
			{
				string componentName = settings.GetValue<string>( "componentName" );
				IComponent component = FindComponent( componentName );
				if( component == null )
					continue;
				ComponentParameters p = settings.GetValue<ComponentParameters>( "parameters" );
				_params.Add( component, p );
			}
		}

		private void PickComponents( Settings[] pickedComponents )
		{
			if( pickedComponents == null )
				return;

			foreach( Settings settings in pickedComponents )
			{
				string componentName = settings.GetValue<string>( "componentName" );
				IComponent component = FindComponent( componentName );
				if( component == null )
					continue;

				ComboBox box;
				if( component.Type == ComponentType.Other )
				{
					continue;
				}
				else
					box = _comboLookup[ component.Type ];

				box.SelectedItem = component;
			}
		}

		private Settings[] GetPickedComponents()
		{
			List<Settings> settingsList = new List<Settings>();

			Settings settings;
			IComponent component;
			foreach( ComponentType type in _comboLookup.Keys )
			{
				component = _comboLookup[ type ].SelectedItem as IComponent;

				settings = new Settings();
				settings.SetValue<string>( "componentName", component.GetType().FullName );
				settings.SetValue<string>( "componentPath", component.GetType().Assembly.Location );
				settingsList.Add( settings );
			}

			return settingsList.ToArray();
		}

		private void defaultsButton_Click( object sender, EventArgs e )
		{
			MessageBox.Show( "TODO" );
		}

		private void okButton_Click( object sender, EventArgs e )
		{
			List<Settings> settingsList = new List<Settings>();
			foreach( KeyValuePair<IComponent, ComponentParameters> pair in _params )
			{
				Settings settings = new Settings();
				settings.SetValue<string>( "componentName", pair.Key.GetType().FullName );
				settings.SetValue<ComponentParameters>( "parameters", pair.Value );
				settingsList.Add( settings );
			}
			Settings[] settingsArray = settingsList.ToArray();
			_componentSettings.SetValue<Settings[]>( "settingsList", settingsArray );
			_componentSettings.SetValue<Settings[]>( "pickedComponents", this.GetPickedComponents() );

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}