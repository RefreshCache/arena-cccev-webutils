/**********************************************************************
* Description:	Paging promotion roller
* Created By:	DallonF
* Date Created:	2/25/2009 2:39:25 PM
*
* $Workfile: PromotionRoller.ascx.cs $
* $Revision: 12 $ 
* $Header: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/PromotionRoller.ascx.cs   12   2010-01-13 20:22:25-07:00   nicka $
* 
* $Log: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/PromotionRoller.ascx.cs $
*  
*  Revision: 12   Date: 2010-01-14 03:22:25Z   User: nicka 
*  Added setting to control whether to randomize or not. 
*  
*  Revision: 11   Date: 2009-12-31 21:17:30Z   User: nicka 
*  Corrected typo-bug with Priority Level setting which was not working. 
*  
*  Revision: 10   Date: 2009-07-28 17:17:35Z   User: JasonO 
*  Refactoring Dallon's original code. 
*  
*  Revision: 9   Date: 2009-07-22 00:37:23Z   User: nicka 
*  Bug fix.  Is currently showing all promotion requests. 
*  
*  Revision: 8   Date: 2009-04-16 18:17:16Z   User: JasonO 
*  Fixing LINQ to work more accurately with promotion priority levels. 
*  
*  Revision: 7   Date: 2009-04-14 23:04:36Z   User: nicka 
*  Corrected issue whereby it was not showing the event details page 
*  
*  Revision: 6   Date: 2009-04-13 17:18:33Z   User: JasonO 
*  Adding priority levels to LINQ query for promotion requests.  Standardizing 
*  module settings. 
*  
*  Revision: 5   Date: 2009-03-16 22:35:12Z   User: DallonF 
*  Fine-tuned the Topic Areas module setting 
*  
*  Revision: 4   Date: 2009-03-04 22:33:43Z   User: DallonF 
*  Now correctly estimates number of pages. 
*  
*  Revision: 3   Date: 2009-03-04 22:06:53Z   User: DallonF 
*  Changed to use Document Types instead of summary images 
*  
*  Revision: 2   Date: 2009-03-03 00:25:58Z   User: DallonF 
*  Added more intuitive Topic Areas setting, improved weighted randomization 
*  algorithm for when all promotions are weighted as 1. 
*  
*  Revision: 1   Date: 2009-02-26 00:53:29Z   User: DallonF 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Arena.Marketing;
using Arena.Portal;
using System.Web.UI.WebControls;
using Arena.Document;

namespace ArenaWeb.UserControls.Custom.Cccev.WebUtils
{
	public partial class PromotionRoller : PortalControl
	{
		[CustomListSetting( "Area Filter", "Filter flag for Topic Areas.  If set to 'primary' only items whose Primary Ministry matches the Topic Area will be shown, etc.  Defaults to 'both'.", false, "both", 
            new[] { "primary", "secondary", "both", "home" }, new[] { "primary", "secondary", "both", "home" } )]
		public string AreaFilterSetting { get { return Setting("AreaFilter", "both", false); } }

        [ListFromSqlSetting("Topic Areas", "The Topic Areas in which to show promotions", true, "",
            "SELECT l.lookup_id, l.lookup_value FROM core_lookup l INNER JOIN core_lookup_type lt ON lt.lookup_type_id = l.lookup_type_id AND lt.guid = '1FE55E22-F67C-46BA-A6AE-35FD112AFD6D' WHERE active = 1 ORDER BY lookup_order",
            ListSelectionMode.Multiple)]
        public string TopicAreasSetting { get { return Setting("TopicAreas", "", true); } }

        [ListFromSqlSetting("Thumbnail Document Type", "The document type containing the thumbnail to display", true, "",
            "SELECT document_type_id, type_name FROM core_document_type")]
        public string ThumbnailSetting { get { return Setting("Thumbnail", "", true); } }

        [NumericSetting("Maximum Items", "The maximum items to display.", false)]
        public string MaxItemsSetting { get { return Setting("MaxItems", "16", false); } }

        [NumericSetting("Thumbnail Width", "The width of the thumbnail images", false)]
        public string WidthSetting { get { return Setting("Width", "32", false); } }

        [NumericSetting("Thumbnail Height", "The height of the thumbnail images", false)]
        public string HeightSetting { get { return Setting("Height", "32", false); } }

        [PageSetting("Promotion Details Page", "The page that contains the Promotion Details display module", true)]
        public string DetailsPageSetting { get { return Setting("DetailsPage", "", true); } }

		/// <summary>
		///  Magic #4222 below -- could not afford to break existing pages by making it required.
		/// </summary>
		[PageSetting( "Event Details Page", "The page that contains the Event Details display module", false, 4222 )]
		public string EventDetailsPageSetting { get { return Setting( "EventDetailsPage", "4222", false ); } }

        [NumericSetting("Estimated Items per Page", "The estimated number of items per page", false)]
        public string ItemsPerPageSetting { get { return Setting("ItemsPerPage", "4", false); } }

        [NumericSetting("Priority Level", "Priority level to constrain results by (default is 10).", false)]
        public string PriorityLevelSetting { get { return Setting("PriorityLevel", "10", false); } }

		[BooleanSetting( "Priority Randomized", "Flag indicating whether to randomize the order of events based on their priority (weighted). Default false.", false, false )]
		public bool RandomizedSetting { get { return Convert.ToBoolean( Setting( "Randomized", "false", false ) ) ; } }

		#region Event Handlers

		protected void Page_Load( object sender, EventArgs e )
		{
            if (!Page.IsPostBack)
            {
                smpScripts.Scripts.Add(new ScriptReference(BasePage.JQUERY_INCLUDE));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/jquery.galleryScroll.1.4.5.pack.js"));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/main.js"));
            }

			if ( Request[ "promotionId" ] != null )
			{
				PromotionRequest request = new PromotionRequest( int.Parse( Request[ "promotionID" ] ) );
				if ( request.WebExternalLink != string.Empty )
				{
					int result;
					if ( request.WebExternalLink.ToLower().StartsWith( "http" ) )
					{
						Response.Redirect( request.WebExternalLink, true );
					}
					else if ( int.TryParse( request.WebExternalLink, out result ) && ( result > -1 ) )
					{
						Response.Redirect( string.Concat( new object[] { Request.Url.AbsolutePath, "?page=", result, "&promotionId=", Request[ "promotionID" ] } ), true );
					}
					else
					{
						Response.Redirect( "http://" + request.WebExternalLink, true );
					}
				}
				else if ( request.EventID == -1 )
				{
					Response.Redirect( Request.Url.AbsolutePath + "?Page=" + DetailsPageSetting + "&promotionId=" + Request[ "promotionID" ] );
				}
				else
				{
					Response.Redirect( Request.Url.AbsolutePath + "?Page=" + EventDetailsPageSetting + "&eventId=" + request.EventID );
				}
			}
		}
		#endregion

        protected IEnumerable<PromotionRequest> GetPromotionRequests()
        {
            PromotionRequestCollection prc = new PromotionRequestCollection();
			prc.LoadCurrentWebRequests( TopicAreasSetting, AreaFilterSetting, -1, int.Parse( MaxItemsSetting ), false, -1 );
            Random randGen = new Random();

            var promotions = (from p in prc.OfType<PromotionRequest>()
                              where p.Priority <= int.Parse(PriorityLevelSetting) + 1
                              select p).Select(p => new
                              {
                                  Promotion = p,
								  Index = ( RandomizedSetting ) ? randGen.NextDouble() * p.Priority :  p.Priority
                              }).OrderBy(p => p.Index)
                              .Select(p => p.Promotion);

            return promotions.ToList();
        }

        protected string GetImageUrl(PromotionRequest promotion)
        {
            PromotionRequestDocument doc = promotion.Documents.GetFirstByType(int.Parse(ThumbnailSetting));
            if (doc != null)
            {
                return String.Format("CachedBlob.aspx?guid={0}&width={1}&height={2}", doc.GUID,
                WidthSetting, HeightSetting);
            }

            return string.Empty;
        }

        protected int GetPageCount(IEnumerable<PromotionRequest> promotions)
        {
            return (int)Math.Ceiling(promotions.Count() / double.Parse(ItemsPerPageSetting));
        }

        protected string GetDetailsUrl(int promotionId)
        {
            return String.Format("Default.aspx?page={0}&promotionId={1}", CurrentPortalPage.PortalPageID, promotionId);
        }

	}
}