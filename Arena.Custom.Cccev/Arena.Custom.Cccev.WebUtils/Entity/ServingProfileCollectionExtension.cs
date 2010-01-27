/**********************************************************************
* Description:  Extension methods specific to serving tags
* Created By:   Dallon Feldner @ Central Christian Church of the East Valley
* Date Created: ???
*
* $Workfile: $
* $Revision: $
* $Header: $
*
* $Log: $
**********************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using Arena.Core;
using Arena.DataLayer.Core;
using Arena.Enums;

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    public static class ServingProfileCollectionExtension
    {
        public static void GetProfileChildren(this ServingProfileCollection profiles, int parentID, int organizationID, ProfileType type)
        {
            SqlDataReader reader = new ProfileData().GetProfileHierarchy(parentID, organizationID, type, -1, string.Empty);
            Dictionary<int, ServingProfile> sps = new Dictionary<int, ServingProfile>();

            while (reader.Read())
            {
                int profileID = (int)reader["profile_id"];

                if (!sps.ContainsKey(profileID))
                {
                    sps.Add(profileID, new ServingProfile(profileID));
                    profiles.Add(sps[profileID]);
                }
            }

            reader.Close();
        }
    }
}
