/**********************************************************************
* Description: Extension methods for collections of Prayer Requests
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	05/03/2009 17:42:02
*
* $Workfile: PrayerRequestCollectionExtension.cs $
* $Revision: 1 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/PrayerRequestCollectionExtension.cs   1   2009-05-06 10:35:30-07:00   nicka $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/PrayerRequestCollectionExtension.cs $
*  
*  Revision: 1   Date: 2009-05-06 17:35:30Z   User: nicka 
**********************************************************************/

using System.Data.SqlClient;
using Arena.DataLayer.Prayer;
using Arena.Prayer;

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    public static class PrayerRequestCollectionExtension
    {
		/// <summary>
		/// Load all approved prayer requests for the given organization.
		/// </summary>
		/// <param name="requests"></param>
		/// <param name="organizationID"></param>
		/// <returns></returns>
		public static RequestCollection LoadApprovedRequestsByOrg( this RequestCollection requests, int organizationID )
		{
			SqlDataReader approvedRequests = new RequestData().GetApprovedRequests( organizationID );

			while ( approvedRequests.Read() )
			{
				Request request = new Request( (int)approvedRequests[ "request_id" ] );
				requests.Add( request );
			}
			approvedRequests.Close();

			return requests;
		}
    }
}