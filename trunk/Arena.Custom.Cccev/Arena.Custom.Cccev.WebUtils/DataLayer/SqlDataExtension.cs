/**********************************************************************
* Description: 
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	05/03/2009 17:42:02
*
* $Workfile: SqlDataExtension.cs $
* $Revision: 1 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/DataLayer/SqlDataExtension.cs   1   2009-05-04 10:40:32-07:00   nicka $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/DataLayer/SqlDataExtension.cs $
*  
*  Revision: 1   Date: 2009-05-04 17:40:32Z   User: nicka 
**********************************************************************/

using System.Data.SqlClient;
using System.Data;
using Arena.DataLib;
using System.Collections;

namespace Arena.Custom.Cccev.WebUtils.DataLayer
{
	public static class SqlDataExtension
	{
		/// <summary>
		/// A public version of Arena's ExecuteSqlDataReader class.
		/// </summary>
		/// <param name="sqlData"></param>
		/// <param name="storedProcedure"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static SqlDataReader ExecuteSqlDataReader( this SqlData sqlData, string storedProcedure, ArrayList list )
		{
			SqlDataReader reader;
			SqlConnection dbConnection = new SqlDbConnection().GetDbConnection();
			SqlCommand command = new SqlCommand( storedProcedure, dbConnection );
			command.CommandTimeout = 360;
			command.CommandType = CommandType.StoredProcedure;

			for ( int i = 0; i <= ( list.Count - 1 ); i++ )
			{
				command.Parameters.Add( (SqlParameter)list[ i ] );
			}

			try
			{
				dbConnection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );
			}
			catch ( SqlException exception )
			{
				throw exception;
			}
			return reader;
		}
	}
}
