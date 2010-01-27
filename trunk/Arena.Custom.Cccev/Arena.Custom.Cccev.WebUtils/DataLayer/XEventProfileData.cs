/**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: XEventProfileData.cs $
* $Revision: 3 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/DataLayer/XEventProfileData.cs   3   2009-03-16 17:10:53-07:00   JasonO $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/DataLayer/XEventProfileData.cs $
*  
*  Revision: 3   Date: 2009-03-17 00:10:53Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-03-10 20:42:05Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections;
using System.Data.SqlClient;
using Arena.DataLib;

namespace Arena.Custom.Cccev.WebUtils.DataLayer
{
    public class XEventProfileData : SqlData
    {
        public SqlDataReader GetEventProfilesByTopicMonthAndParentID(int parentID, DateTime startDate, DateTime endDate, string topicAreas)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ParentID", parentID));
            list.Add(new SqlParameter("@StartDate", startDate));
            list.Add(new SqlParameter("@EndDate", endDate));
            list.Add(new SqlParameter("@TopicAreas", topicAreas));

            try
            {
                return ExecuteSqlDataReader("cust_cccev_webu_sp_get_eventsByMonthTopicAndParentID", list);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
