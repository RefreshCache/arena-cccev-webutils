/**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: EventProfileCollectionExtension.cs $
* $Revision: 4 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/EventProfileCollectionExtension.cs   4   2009-03-18 18:20:15-07:00   JasonO $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/EventProfileCollectionExtension.cs $
*  
*  Revision: 4   Date: 2009-03-19 01:20:15Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-03-17 00:10:56Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-03-10 20:41:49Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Arena.Event;
using Arena.Custom.Cccev.WebUtils.DataLayer;

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    public class CccevEvent
    {
        private int profileID;
        private int occurrenceID;
        private DateTime occurrenceStart;

        public int ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }

        public int OccurrenceID
        {
            get { return occurrenceID; }
            set { occurrenceID = value; }
        }

        public DateTime OccurrenceStart
        {
            get { return occurrenceStart; }
            set { occurrenceStart = value; }
        }

        public CccevEvent()
        {
        }

        public CccevEvent(int profileID, int occurrenceID, DateTime occurrenceStart)
        {
            this.profileID = profileID;
            this.occurrenceID = occurrenceID;
            this.occurrenceStart = occurrenceStart;
        }
    }

    public static class EventProfileCollectionExtension
    {
        public static List<CccevEvent> LoadEventProfilesByTopicMonthAndParentID(this EventProfileCollection profiles, int parentProfileID, DateTime startDate, DateTime endDate, string topicAreas)
        {
            List<CccevEvent> events = new List<CccevEvent>();
            SqlDataReader reader = new XEventProfileData().GetEventProfilesByTopicMonthAndParentID(parentProfileID, startDate, endDate, topicAreas);

            while (reader.Read())
            {
                profiles.Add(new EventProfile((int)reader["profile_id"]));
                events.Add(new CccevEvent((int)reader["profile_id"], (int)reader["occurrence_id"], (DateTime)reader["start"]));
            }

            reader.Close();
            return events;
        }
    }
}
