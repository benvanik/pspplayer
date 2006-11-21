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

/// <summary>
/// Summary description for Database
/// </summary>
public class Database : IDisposable
{
	protected SqlConnection _connection;
	protected SqlCommand _command;
	protected bool _isAuthenticated;

	public const long MaximumIconSize = 1024 * 128;

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

	public bool IsAuthenticated
	{
		get
		{
			return _isAuthenticated;
		}
	}

	public List<Game> ListGames( char letterQuery )
	{
		_command.CommandType = CommandType.StoredProcedure;
		_command.CommandText = "ListGames";
		_command.Parameters.Clear();
		if( letterQuery != 0 )
		{
			_command.Parameters.Add( "letterQuery", SqlDbType.NChar, 1 );
			_command.Parameters[ "letterQuery" ].Value = letterQuery.ToString().ToLowerInvariant();
		}

		using( SqlDataReader reader = _command.ExecuteReader() )
		{
			if( reader.HasRows == false )
				return new List<Game>();

			List<Game> games = new List<Game>( 1024 );
			Game currentGame = null;

			int columnGameId = -1;
			int columnTitle = -1;
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
					if( currentGame != null )
						games.Add( currentGame );

					currentGame = new Game();
					currentGame.GameID = gameId;
					currentGame.Title = reader.GetString( columnTitle ).Trim();
					currentGame.Releases = new List<GameRelease>();
				}

				GameRelease release = new GameRelease();
				release.GameID = gameId;
				release.ReleaseID = reader.GetInt64( columnReleaseId );
				if( reader.IsDBNull( columnRegion ) == false )
					release.Region = reader.GetInt32( columnRegion );
				release.DiscID = reader.GetString( columnDiscId ).Trim();
				if( reader.IsDBNull( columnLocalizedTitle ) == false )
					release.Title = reader.GetString( columnLocalizedTitle ).Trim();
				if( reader.IsDBNull( columnWebsite ) == false )
					release.Website = reader.GetString( columnWebsite );
				if( reader.IsDBNull( columnReleaseDate ) == false )
					release.ReleaseDate = reader.GetDateTime( columnReleaseDate ).Date;
				if( reader.IsDBNull( columnGameVersion ) == false )
					release.GameVersion = reader.GetFloat( columnGameVersion );
				if( reader.IsDBNull( columnSystemVersion ) == false )
					release.SystemVersion = reader.GetFloat( columnSystemVersion );
				release.HasIcon = reader.GetBoolean( columnHasIcon );

				currentGame.Releases.Add( release );
			}

			return games;
		}
	}

	public List<Game> SearchGames( string query )
	{
		return null;
	}

	public byte[] GetGameIcon( long gameId, long releaseId )
	{
		using( SqlDataReader reader = _command.ExecuteReader( CommandBehavior.SingleRow ) )
		{
			bool success = ( reader.HasRows == true ) &&
				( reader.Read() == true );
			Debug.Assert( success == true );
			if( success == false )
				return null;

			if( reader.IsDBNull( 0 ) == true )
				return null;

			SqlBinary result = reader.GetSqlBinary( 0 );
			if( result.IsNull == true )
				return null;

			Debug.Assert( result.Length > 0 );
			return result.Value;
		}
	}
}
