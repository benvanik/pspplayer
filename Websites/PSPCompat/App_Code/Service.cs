// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://psp.ixtli.com/pspcompat/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
	public Service()
	{
	}

	[WebMethod( false )]
	public Game[] ListGames()
	{
		List<Game> games = null;
		using( Database db = new Database() )
			games = db.ListGameNames();
		if( games != null )
			return games.ToArray();
		else
			return new Game[]{};
	}

	[WebMethod( false )]
	public Game GetGame( long gameId )
	{
		Game game = null;
		using( Database db = new Database() )
			game = db.GetGame( gameId );
		return game;
	}

	[WebMethod( false )]
	public AddResult AddGame( string username, string password, GameRelease release, byte[] icon )
	{
		return this.AddRelease( username, password, null, release, icon );
	}

	[WebMethod( false )]
	public AddResult AddRelease( string username, string password, long? gameId, GameRelease release, byte[] icon )
	{
		if( username == null )
			throw new ArgumentNullException( "username" );
		if( password == null )
			throw new ArgumentNullException( "password" );
		if( ( gameId.HasValue == true ) && ( gameId <= 0 ) )
			throw new ArgumentOutOfRangeException( "gameId" );
		if( release == null )
			throw new ArgumentNullException( "release" );

		using( Database db = new Database() )
		{
			try
			{
				switch( db.Authenticate( username, password ) )
				{
					case AuthenticationResult.Succeeded:
						break;
					case AuthenticationResult.InvalidCredentials:
					case AuthenticationResult.AccountDisabled:
						return AddResult.PermissionDenied;
				}

				if( gameId.HasValue == false )
				{
					gameId = db.AddGame( release.Title, null );
					if( gameId < 0 )
						return AddResult.Failed;
					else if( gameId == 0 )
						return AddResult.Redundant;
				}

				long releaseId = db.AddGameRelease( gameId.Value, release );
				if( releaseId < 0 )
					return AddResult.Failed;
				else if( releaseId == 0 )
					return AddResult.Redundant;

				if( ( icon != null ) &&
					( icon.Length > 0 ) )
				{
					UpdateResult iconResult = db.SetGameIcon( gameId.Value, releaseId, icon );
					if( iconResult != UpdateResult.Succeeded )
						return AddResult.Failed;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				db.Logout();
			}

			return AddResult.Succeeded;
		}
	}

	[WebMethod( false )]
	public CompatibilityResults CheckCompatibility( string discId, ComponentInfo[] components )
	{
		CompatibilityResults results = new CompatibilityResults();

		using( Database db = new Database() )
		{
			long releaseId;
			Game game = db.GetGame( discId, out releaseId );
			if( game == null )
			{
				results.Found = false;
				return results;
			}

			results.GameID = game.GameID;
			results.ReleaseID = releaseId;
			results.Game = game;
		}

		return results;
	}
}
