// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Data.SqlTypes;
using System.Drawing;

public enum UpdateResult
{
	Succeeded,
	Failed,
	BadParameters,
}

public enum AuthenticationResult
{
	Succeeded = 0,
	InvalidCredentials = -1,
	AccountDisabled = -2,
}

/// <summary>
/// Simple database adapter.
/// </summary>
public class Database : IDisposable
{
	protected SqlConnection _connection;
	protected SqlCommand _command;

	protected bool _isAuthenticated;
	protected long _userId;
	protected string _username;
	protected SecurityLevel _userLevel;

	/// <summary>
	/// The maximum size of an icon, in bytes. Since they are small, 80K should be plenty.
	/// </summary>
	public const long MaximumIconSize = 1024 * 50;

	public Database()
	{
		_connection = new SqlConnection( ConfigurationManager.AppSettings[ "ConnectionString" ] );
		_command = new SqlCommand();
		_command.Connection = _connection;

		_connection.Open();
	}

	public void Dispose()
	{
		if( _command != null )
			_command.Dispose();
		if( _connection != null )
			_connection.Dispose();
	}

	private static T? GetDBValue<T>( SqlDataReader reader, int columnOrdinal )
		where T : struct
	{
		// TODO: Optimize this - it could pick on type
		if( reader.IsDBNull( columnOrdinal ) == true )
			return null;
		else
			return new T?( ( T )reader.GetValue( columnOrdinal ) );
	}

	#region Authentication / security / etc

	public bool IsAuthenticated
	{
		get
		{
			return _isAuthenticated;
		}
	}

	public long UserID
	{
		get
		{
			return _userId;
		}
	}

	public string Username
	{
		get
		{
			return _username;
		}
	}

	public SecurityLevel SecurityLevel
	{
		get
		{
			return _userLevel;
		}
	}

	public bool CanEdit
	{
		get
		{
			if( _isAuthenticated == false )
				return false;
			if( ( _userLevel == SecurityLevel.Administrator ) ||
				( _userLevel == SecurityLevel.Editor ) )
				return true;
			else
				return false;
		}
	}

	public AuthenticationResult Authenticate( string username, string password )
	{
		if( username == null )
			throw new ArgumentNullException( "username" );
		username = username.Trim();
		if( username.Length <= 0 )
			throw new ArgumentOutOfRangeException( "username" );
		if( password == null )
			throw new ArgumentNullException( "password" );
		if( password.Length < 0 )
			throw new ArgumentOutOfRangeException( "password" );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "Authenticate";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "Username", username );
		_command.Parameters.AddWithValue( "Password", password );
		_command.Parameters.Add( "UserID", SqlDbType.BigInt );
		_command.Parameters[ "UserID" ].Direction = ParameterDirection.Output;
		_command.Parameters.Add( "Privlidges", SqlDbType.Int );
		_command.Parameters[ "Privlidges" ].Direction = ParameterDirection.Output;
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		AuthenticationResult resultCode = ( AuthenticationResult )_command.Parameters[ "Return" ].Value;
		
		if( resultCode == AuthenticationResult.Succeeded )
		{
			_userId = ( long )_command.Parameters[ "UserID" ].Value;
			Debug.Assert( _userId > 0 );
			if( _userId <= 0 )
				throw new ApplicationException( "Invalid UserID returned after login attempt." );
			_userLevel = ( SecurityLevel )_command.Parameters[ "Privlidges" ].Value;
			_isAuthenticated = true;
			_username = username;
		}

		return resultCode;
	}

	public void Logout()
	{
		_isAuthenticated = false;
		_userLevel = SecurityLevel.Unknown;
		_username = null;
		_userId = 0;
	}

	private void DemandMinimumSecurity( SecurityLevel securityLevel )
	{
		if( _isAuthenticated == false )
			throw new NotAuthenticatedException();
		if( ( int )_userLevel < ( int )securityLevel )
			throw new PermissionDeniedException( _userLevel, securityLevel );
		Debug.Assert( _userId > 0 );
		if( _userId <= 0 )
			throw new NotAuthenticatedException();
	}

	#endregion

	#region Searching / listing

	public List<Game> ListGames()
	{
		return this.ListGames( ( char )0 );
	}

	public List<Game> ListGames( char letterQuery )
	{
		return this.ListGames( letterQuery, false );
	}

	public List<Game> ListGames( char letterQuery, bool showPendingApproval )
	{
		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "ListGames";
		_command.Parameters.Clear();
		if( letterQuery != 0 )
		{
			_command.Parameters.Add( "LetterQuery", SqlDbType.NChar, 1 );
			_command.Parameters[ "LetterQuery" ].Value = letterQuery.ToString().ToLowerInvariant();
		}
		if( showPendingApproval == true )
		{
			this.DemandMinimumSecurity( SecurityLevel.Editor );
			_command.Parameters.AddWithValue( "UserID", _userId );
			_command.Parameters.AddWithValue( "ShowPendingApproval", showPendingApproval );
		}

		using( SqlDataReader reader = _command.ExecuteReader() )
		{
			if( reader.HasRows == false )
				return new List<Game>();

			List<Game> games = new List<Game>( 1024 );
			Game currentGame = null;

			int columnGameId = -1;
			int columnTitle = -1;
			int columnGameWebsite = -1;
			int columnReleaseId = -1;
			int columnRegion = -1;
			int columnDiscId = -1;
			int columnLocalizedTitle = -1;
			int columnWebsite = -1;
			int columnReleaseDate = -1;
			int columnGameVersion = -1;
			int columnSystemVersion = -1;
			int columnHasIcon = -1;

			while( reader.Read() == true )
			{
				if( columnGameId == -1 )
				{
					columnGameId = reader.GetOrdinal( "GameID" );
					columnTitle = reader.GetOrdinal( "Title" );
					columnGameWebsite = reader.GetOrdinal( "GameWebsite" );
					columnReleaseId = reader.GetOrdinal( "ReleaseID" );
					columnRegion = reader.GetOrdinal( "Region" );
					columnDiscId = reader.GetOrdinal( "DiscID" );
					columnLocalizedTitle = reader.GetOrdinal( "LocalizedTitle" );
					columnWebsite = reader.GetOrdinal( "Website" );
					columnReleaseDate = reader.GetOrdinal( "ReleaseDate" );
					columnGameVersion = reader.GetOrdinal( "GameVersion" );
					columnSystemVersion = reader.GetOrdinal( "SystemVersion" );
					columnHasIcon = reader.GetOrdinal( "HasIcon" );
				}

				long gameId = reader.GetInt64( columnGameId );

				if( ( currentGame == null ) ||
					( currentGame.GameID != gameId ) )
				{
					currentGame = new Game();
					currentGame.GameID = gameId;
					currentGame.Title = reader.GetString( columnTitle );
					if( reader.IsDBNull( columnGameWebsite ) == false )
						currentGame.Website = reader.GetString( columnGameWebsite );
					currentGame.Releases = new List<GameRelease>();
					games.Add( currentGame );
				}

				// Games may not have releases
				if( reader.IsDBNull( columnReleaseId ) == false )
				{
					GameRelease release = new GameRelease();
					release.GameID = gameId;
					release.ReleaseID = reader.GetInt64( columnReleaseId );
					if( reader.IsDBNull( columnRegion ) == false )
						release.Region = reader.GetInt32( columnRegion );
					release.DiscID = reader.GetString( columnDiscId );
					if( reader.IsDBNull( columnLocalizedTitle ) == false )
						release.Title = reader.GetString( columnLocalizedTitle );
					if( reader.IsDBNull( columnWebsite ) == false )
						release.Website = reader.GetString( columnWebsite );
					if( reader.IsDBNull( columnReleaseDate ) == false )
						release.ReleaseDate = reader.GetDateTime( columnReleaseDate ).Date;
					if( reader.IsDBNull( columnGameVersion ) == false )
						release.GameVersion = new float?( ( float )reader.GetDouble( columnGameVersion ) );
					if( reader.IsDBNull( columnSystemVersion ) == false )
						release.SystemVersion = new float?( ( float )reader.GetDouble( columnSystemVersion ) );
					release.HasIcon = reader.GetBoolean( columnHasIcon );

					currentGame.Releases.Add( release );
				}
			}

			games.TrimExcess();
			return games;
		}
	}

	public List<Game> SearchGames( string query )
	{
		throw new NotImplementedException();
	}

	public List<Game> ListGameNames()
	{
		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "ListGameNames";
		_command.Parameters.Clear();
		
		using( SqlDataReader reader = _command.ExecuteReader() )
		{
			if( reader.HasRows == false )
				return new List<Game>();

			List<Game> games = new List<Game>( 1024 );
			
			int columnGameId = -1;
			int columnTitle = -1;
			int columnWebsite = -1;

			while( reader.Read() == true )
			{
				if( columnGameId == -1 )
				{
					columnGameId = reader.GetOrdinal( "GameID" );
					columnTitle = reader.GetOrdinal( "Title" );
					columnWebsite = reader.GetOrdinal( "Website" );
				}

				long gameId = reader.GetInt64( columnGameId );

				Game currentGame = new Game();
				currentGame.GameID = gameId;
				currentGame.Title = reader.GetString( columnTitle );
				if( reader.IsDBNull( columnWebsite ) == false )
					currentGame.Website = reader.GetString( columnWebsite );
				currentGame.Releases = new List<GameRelease>();
				games.Add( currentGame );
			}

			games.TrimExcess();
			return games;
		}
	}

	public Game GetGame( long gameId )
	{
		long dummy;
		return this.GetGame( true, gameId, null, out dummy );
	}

	public Game GetGame( string discId, out long releaseId )
	{
		return this.GetGame( false, 0, discId, out releaseId );
	}

	public Game GetGame( bool useGameID, long gameId, string discId, out long releaseId )
	{
		releaseId = 0;

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "GetGame";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "GameID", gameId );
		_command.Parameters.AddWithValue( "DiscID", discId );
		_command.Parameters.Add( "ReleaseID", SqlDbType.BigInt );
		_command.Parameters[ "ReleaseID" ].Direction = ParameterDirection.Output;
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;

		//int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		//if( returnValue == 1 )
		//    return UpdateResult.Succeeded;
		//else
		//    return UpdateResult.Failed;

		using( SqlDataReader reader = _command.ExecuteReader() )
		{
			if( reader.HasRows == false )
				return null;

			if( useGameID == false )
				releaseId = ( long )_command.Parameters[ "ReleaseID" ].Value;

			Game currentGame = null;

			int columnGameId = -1;
			int columnTitle = -1;
			int columnGameWebsite = -1;
			int columnReleaseId = -1;
			int columnRegion = -1;
			int columnDiscId = -1;
			int columnLocalizedTitle = -1;
			int columnWebsite = -1;
			int columnReleaseDate = -1;
			int columnGameVersion = -1;
			int columnSystemVersion = -1;
			int columnHasIcon = -1;

			while( reader.Read() == true )
			{
				if( columnGameId == -1 )
				{
					columnGameId = reader.GetOrdinal( "GameID" );
					columnTitle = reader.GetOrdinal( "Title" );
					columnGameWebsite = reader.GetOrdinal( "GameWebsite" );
					columnReleaseId = reader.GetOrdinal( "ReleaseID" );
					columnRegion = reader.GetOrdinal( "Region" );
					columnDiscId = reader.GetOrdinal( "DiscID" );
					columnLocalizedTitle = reader.GetOrdinal( "LocalizedTitle" );
					columnWebsite = reader.GetOrdinal( "Website" );
					columnReleaseDate = reader.GetOrdinal( "ReleaseDate" );
					columnGameVersion = reader.GetOrdinal( "GameVersion" );
					columnSystemVersion = reader.GetOrdinal( "SystemVersion" );
					columnHasIcon = reader.GetOrdinal( "HasIcon" );
				}

				if( ( currentGame == null ) ||
					( currentGame.GameID != gameId ) )
				{
					currentGame = new Game();
					currentGame.GameID = gameId;
					currentGame.Title = reader.GetString( columnTitle );
					if( reader.IsDBNull( columnGameWebsite ) == false )
						currentGame.Website = reader.GetString( columnGameWebsite );
					currentGame.Releases = new List<GameRelease>();
				}

				// Games may not have releases
				if( reader.IsDBNull( columnReleaseId ) == false )
				{
					GameRelease release = new GameRelease();
					release.GameID = gameId;
					release.ReleaseID = reader.GetInt64( columnReleaseId );
					if( reader.IsDBNull( columnRegion ) == false )
						release.Region = reader.GetInt32( columnRegion );
					release.DiscID = reader.GetString( columnDiscId );
					if( reader.IsDBNull( columnLocalizedTitle ) == false )
						release.Title = reader.GetString( columnLocalizedTitle );
					if( reader.IsDBNull( columnWebsite ) == false )
						release.Website = reader.GetString( columnWebsite );
					if( reader.IsDBNull( columnReleaseDate ) == false )
						release.ReleaseDate = reader.GetDateTime( columnReleaseDate ).Date;
					if( reader.IsDBNull( columnGameVersion ) == false )
						release.GameVersion = new float?( ( float )reader.GetDouble( columnGameVersion ) );
					if( reader.IsDBNull( columnSystemVersion ) == false )
						release.SystemVersion = new float?( ( float )reader.GetDouble( columnSystemVersion ) );
					release.HasIcon = reader.GetBoolean( columnHasIcon );

					currentGame.Releases.Add( release );
				}
			}

			return currentGame;
		}
	}

	#endregion

	#region Game utilities

	public UpdateResult ApproveAll()
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "ApprovePendingGames";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue == 1 )
			return UpdateResult.Succeeded;
		else
			return UpdateResult.Failed;
	}

	public UpdateResult ApproveGame( long gameId )
	{
		return this.ApproveGameRelease( gameId, null );
	}

	public UpdateResult ApproveGameRelease( long gameId, long? releaseId )
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "ToggleGame";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.AddWithValue( "GameID", gameId );
		if( releaseId.HasValue == true )
			_command.Parameters.AddWithValue( "ReleaseID", releaseId );
		else
			_command.Parameters.Add( "ReleaseID", SqlDbType.BigInt );
		_command.Parameters.AddWithValue( "Approved", true );
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue == 1 )
			return UpdateResult.Succeeded;
		else
			return UpdateResult.Failed;
	}

	public UpdateResult RemoveGameRelease( long gameId, long? releaseId )
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "RemoveGameRelease";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.AddWithValue( "GameID", gameId );
		if( releaseId.HasValue == true )
			_command.Parameters.AddWithValue( "ReleaseID", releaseId );
		else
			_command.Parameters.Add( "ReleaseID", SqlDbType.BigInt );
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue == 1 )
			return UpdateResult.Succeeded;
		else
			return UpdateResult.Failed;
	}

	public long AddGame( string name, string website )
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		if( ( name == null ) || ( name.Length == 0 ) )
			throw new ArgumentNullException( "name" );
		if( website == string.Empty )
			website = null;

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "AddGame";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.AddWithValue( "Title", name );
		_command.Parameters.Add( "Website", SqlDbType.VarChar, 512 );
		_command.Parameters[ "Website" ].Value = website;
		_command.Parameters.Add( "GameID", SqlDbType.BigInt );
		_command.Parameters[ "GameID" ].Direction = ParameterDirection.Output;
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue != 1 )
			return 0;

		if( _command.Parameters[ "GameID" ].Value == null )
			return 0;
		long gameId = ( long )_command.Parameters[ "GameID" ].Value;
		return gameId;
	}

	public long AddGameRelease( long gameId, GameRelease release )
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		if( gameId <= 0 )
			throw new ArgumentOutOfRangeException( "gameId" );
		if( release == null )
			throw new ArgumentNullException( "release" );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "AddGameRelease";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.AddWithValue( "GameID", gameId );

		_command.Parameters.AddWithValue( "Region", release.Region );
		_command.Parameters.AddWithValue( "DiscID", release.DiscID );
		_command.Parameters.AddWithValue( "Title", release.Title );
		_command.Parameters.AddWithValue( "Website", release.Website );
		if( release.ReleaseDate.HasValue == true )
			_command.Parameters.AddWithValue( "ReleaseDate", release.ReleaseDate.Value );
		else
			_command.Parameters.Add( "ReleaseDate", SqlDbType.DateTime );
		if( release.GameVersion.HasValue == true )
			_command.Parameters.AddWithValue( "GameVersion", ( double )release.GameVersion.Value );
		else
			_command.Parameters.Add( "GameVersion", SqlDbType.Float );
		if( release.GameVersion.HasValue == true )
			_command.Parameters.AddWithValue( "SystemVersion", ( double )release.SystemVersion.Value );
		else
			_command.Parameters.Add( "SystemVersion", SqlDbType.Float );
		_command.Parameters.Add( "ReleaseID", SqlDbType.BigInt );
		_command.Parameters[ "ReleaseID" ].Direction = ParameterDirection.Output;
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue != 1 )
			return -1;

		if( _command.Parameters[ "ReleaseID" ].Value == null )
			return -1;
		long releaseId = ( long )_command.Parameters[ "ReleaseID" ].Value;
		return releaseId;
	}

	public byte[] GetGameIcon( long gameId, long releaseId )
	{
		if( gameId <= 0 )
			throw new ArgumentOutOfRangeException( "gameId" );
		if( releaseId <= 0 )
			throw new ArgumentOutOfRangeException( "releaseId" );

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "GetGameIcon";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "GameID", gameId );
		_command.Parameters.AddWithValue( "ReleaseID", releaseId );
		_command.Parameters.Add( "IconData", SqlDbType.VarBinary, int.MaxValue );
		_command.Parameters[ "IconData" ].Direction = ParameterDirection.Output;
		_command.ExecuteNonQuery();
		byte[] iconData = _command.Parameters[ "IconData" ].Value as byte[];
		if( ( iconData == null ) ||
			( iconData.Length == 0 ) )
			return null;
		else
			return iconData;
	}

	public UpdateResult SetGameIcon( long gameId, long releaseId, byte[] icon )
	{
		this.DemandMinimumSecurity( SecurityLevel.Editor );

		if( gameId <= 0 )
			throw new ArgumentOutOfRangeException( "gameId" );
		if( releaseId <= 0 )
			throw new ArgumentOutOfRangeException( "releaseId" );
		if( icon == null )
			throw new ArgumentNullException( "icon" );
		if( ( icon.Length == 0 ) ||
			( icon.Length > MaximumIconSize ) )
			throw new ArgumentOutOfRangeException( "icon" );

		// Try to parse the image and see if it is legit - this is hacky and DANGEROUS
		try
		{
			using( MemoryStream stream = new MemoryStream( icon, false ) )
			using( System.Drawing.Image bitmap = System.Drawing.Image.FromStream( stream, true, true ) )
			{
				if( bitmap == null )
					throw new InvalidDataException();
				if( ( bitmap.Width != 144 ) ||
					( bitmap.Height != 80 ) )
					throw new InvalidDataException();
			}
		}
		catch
		{
			Debug.Assert( false, "Invalid icon data passed." );
			return UpdateResult.BadParameters;
		}

		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "SetGameIcon";
		_command.Parameters.Clear();
		_command.Parameters.AddWithValue( "UserID", _userId );
		_command.Parameters.AddWithValue( "GameID", gameId );
		_command.Parameters.AddWithValue( "ReleaseID", releaseId );
		_command.Parameters.Add( "IconData", SqlDbType.VarBinary, int.MaxValue );
		_command.Parameters[ "IconData" ].Value = icon;
		_command.Parameters.Add( "Return", SqlDbType.Int );
		_command.Parameters[ "Return" ].Direction = ParameterDirection.ReturnValue;
		_command.ExecuteNonQuery();
		int returnValue = ( int )_command.Parameters[ "Return" ].Value;
		if( returnValue == 1 )
			return UpdateResult.Succeeded;
		else
			return UpdateResult.Failed;
	}

	#endregion
}

#region Exceptions

[Serializable]
public class NotAuthenticatedException : ApplicationException
{
	public NotAuthenticatedException()
	{
	}
	public NotAuthenticatedException( string message ) : base( message )
	{
	}
	public NotAuthenticatedException( string message, Exception inner ) : base( message, inner )
	{
	}
	protected NotAuthenticatedException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context )
		: base( info, context )
	{
	}
}

[Serializable]
public class PermissionDeniedException : ApplicationException
{
	public SecurityLevel CurrentLevel;
	public SecurityLevel RequiredLevel;

	public PermissionDeniedException()
	{
	}
	public PermissionDeniedException( SecurityLevel currentLevel, SecurityLevel requiredLevel )
	{
		this.CurrentLevel = currentLevel;
		this.RequiredLevel = requiredLevel;
	}
	public PermissionDeniedException( string message ) : base( message )
	{
	}
	public PermissionDeniedException( string message, Exception inner ) : base( message, inner )
	{
	}
	protected PermissionDeniedException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context )
		: base( info, context )
	{
	}
}

#endregion
