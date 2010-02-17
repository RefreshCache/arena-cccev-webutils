/**********************************************************************
* Description:	Displays Podcast Channels.
* Created By:	DallonF
* Date Created:	3/18/2009 5:11:53 PM
*
* $Workfile: PodcastChannelViewer.ascx.cs $
* $Revision: 6 $ 
* $Header: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/PodcastChannelViewer.ascx.cs   6   2010-02-17 09:52:47-07:00   JasonO $
* 
* $Log: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/PodcastChannelViewer.ascx.cs $
*  
*  Revision: 6   Date: 2010-02-17 16:52:47Z   User: JasonO 
*  Fixing jquery include issues. 
*  
*  Revision: 5   Date: 2010-01-27 22:49:28Z   User: JasonO 
*  Cleaning up. 
*  
*  Revision: 4   Date: 2009-04-16 18:48:36Z   User: JasonO 
*  Removing un-needed dependencies. 
*  
*  Revision: 3   Date: 2009-03-25 20:05:15Z   User: DallonF 
*  Fixed &amp; issue 
*  
*  Revision: 2   Date: 2009-03-25 00:16:01Z   User: JasonO 
*  adding comments 
*  
*  Revision: 1   Date: 2009-03-23 23:28:24Z   User: DallonF 
**********************************************************************/

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Arena.Custom.Cccev.DataUtils;
using Arena.Portal;
using Arena.Feed;
using Arena.DataLayer.Feed;

namespace ArenaWeb.UserControls.Custom.Cccev.WebUtils
{

	public partial class PodcastChannelViewer : PortalControl
	{

        [ListFromSqlSetting("Video Channel Format", "The Channel Format for the Video feed", false, "-1",
            "SELECT format_id, title FROM feed_format ORDER BY title")]
        public int VideoFormatSetting { get { return int.Parse(Setting("VideoFormat", "-1", false)); } }

        [ListFromSqlSetting("Audio Channel Format", "The Channel Format for the Audio feed", false, "-1",
            "SELECT format_id, title FROM feed_format ORDER BY title")]
        public int AudioFormatSetting { get { return int.Parse(Setting("AudioFormat", "-1", false)); } }

        [ListFromSqlSetting("Flash Video Channel Format", "The Channel Format for the Flash Video feed", false, "-1",
            "SELECT format_id, title FROM feed_format ORDER BY title")]
        public int FlashFormatSetting { get { return int.Parse(Setting("FlashFormat", "-1", false)); } }

        //TODO: Make SmartPageSetting.
        [PageSetting("Channel Details Page", "The page to go to when a channel is selected", true)]
        public int DetailsPageSetting { get { return int.Parse(Setting("DetailsPage", "-1", true)); } }

        [NumericSetting("Channel Width", "The width, in pixels, of a single channel icon.", false)]
        public int PageWidthSetting { get { return int.Parse(Setting("PageWidth", "717", false)); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                smpScripts.Scripts.Add(new ScriptReference(string.Format("~/{0}", BasePage.JQUERY_INCLUDE)));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/jquery.tabSlide.js"));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/main.js"));
            }
        }

		public string GetDetailsLink(int channelId, int formatId)
        {
            return string.Format("Default.aspx?page={0}&channel={1}&format={2}", DetailsPageSetting, channelId, formatId);
        }

        public string GetRssLink(int channelId, int formatId)
        {
            return string.Format("rss.aspx?c={0}&f={1}", channelId, formatId);
        }

        public string GetImage(Channel channel)
        {
            return string.Format("CachedBlob.aspx?guid={0}", channel.Image.GUID);
        }

        public IEnumerable<Channel> GetChannels()
        {
            return new ChannelData().GetChannelList(Constants.NULL_STRING, true).AsEnumerable()
                .Select(r => new Channel((int)r["channel_id"]));
        }

        public int GetCurrentChannel(IEnumerable<Channel> channels)
        {
            if (int.Parse(Request.QueryString["channel"]) != -1)
            {
                return int.Parse(Request.QueryString["channel"]);
            }
            
            return channels.First().ChannelId;
        }

	    public int GetIndexOfCurrentChannel(IEnumerable<Channel> channels)
        {
            int count = 0;
            int currentId = GetCurrentChannel(channels);
            foreach (var channel in channels)
            {
                if (channel.ChannelId == currentId)
                {
                    break;
                }

                count++;
            }
            return count;
        }

	}
}