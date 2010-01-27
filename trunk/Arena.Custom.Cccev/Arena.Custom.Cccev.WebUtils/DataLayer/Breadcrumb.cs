/**********************************************************************
* Description: A breadcrumb.
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	11/21/2007 9:27:28
*
* $Workfile: Breadcrumb.cs $
* $Revision: 1 $ 
* $Header: /Arena/Arena.Custom.Cccev.WebUtils/DataLayer/Breadcrumb.cs   1   2007-11-28 16:25:38-07:00   nicka $
* 
* $Log: /Arena/Arena.Custom.Cccev.WebUtils/DataLayer/Breadcrumb.cs $
*  
*  Revision: 1   Date: 2007-11-28 23:25:38Z   User: nicka 
**********************************************************************/

using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Arena.DataLib;

namespace Arena.Custom.Cccev.WebUtils
{
	public class BreadcrumbData : SqlData
	{
		/// <summary>
		/// Returns a breadcrumb (page_id, page_name).
		/// </summary>
		/// <param name="currentPageID">The bottom most child to end the breadcrumb trail.</param>
		/// <returns>a datatable containing page_id and page_name</returns>
		public DataTable GetBreadcrumbs_DT( int currentPageID )
		{
			return GetBreadcrumbs_DT( currentPageID, -1 );
		}

		/// <summary>
		/// Returns a breadcrumb (page_id, page_name).
		/// </summary>
		/// <param name="currentPageID">The bottom most child to end the breadcrumb trail.</param>
		/// <param name="stopParentPageID">The ID of the parent page to begin the trail.</param>
		/// <returns>a datatable containing page_id and page_name</returns>
		public DataTable GetBreadcrumbs_DT( int currentPageID, int stopParentPageID )
		{
			ArrayList lst = new ArrayList();

			lst.Add( new SqlParameter( "@CurrentPageID", currentPageID ) );
			lst.Add( new SqlParameter( "@StopParentPageID", stopParentPageID ) );

			try
			{
				return this.ExecuteDataTable( "cust_cccev_breadcrumb_sp_get_parents", lst );
			}
			catch ( SqlException ex )
			{
				throw ex;
			}
		}
	}
}


