/**********************************************************************
* Description:  Outputs player for podcast videos
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: ???
*
* $Workfile: $
* $Revision: $
* $Header: $
*
* $Log: $
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Arena.Feed;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.Cccev.WebUtils
{
    public partial class PodcastVideoPlayer : PortalControl
    {
        [PageSetting("Podcast Channel Topic Page", "Page listing topics of a channel feed.", true)]
        public string PodcastChannelTopicPageSetting { get { return Setting("PodcastChannelTopicPage", "", true); } }

        [TextSetting("Flash Video Player URL", "URL pointing to the Flash movie player to be loaded. (Leave blank for default player).", false)]
        public string FlashVideoUrlSetting { get { return Setting("FlashVideoUrl", "UserControls/Custom/Cccev/WebUtils/misc/podcast/player.swf", false); } }

        [NumericSetting("Flash Movie Height", "Height (in px) of the Flash movie.", true)]
        public string FlashHeightSetting { get { return Setting("FlashHeight", "", true); } }

        [NumericSetting("Flash Movie Width", "Width (in px) of the Flash movie.", true)]
        public string FlashWidthSetting { get { return Setting("FlashWidth", "", true); } }

        [BooleanSetting("Allow Full Screen", "Allows movie to expand to full screen.", false, true)]
        public string AllowFullScreenSetting { get { return Setting("AllowFullScreen", "true", false); } }

        [TextSetting("Error Message Setting", "Error message displayed if an invalid item ID is passed via query string.", false)]
        public string InvalidTopicIDMessageSetting { get { return Setting("InvalidTopicIDMessage", "Invalid podcast item.", false); } }

        [TextSetting("Streamer Server URL Setting", "URL to podcast stream.", true)]
        public string StreamerServerSetting { get { return Setting("StreamerServer", "", true); } }

        [BooleanSetting("Auto-Play Video", "Determines whether or not the video will auto-play on load (defaults to 'true').", false, true)]
        public string AutoPlaySetting { get { return Setting("AutoPlay", "true", false); } }

        private Item item;

        private enum ItemFormatTypes
        {
            Video,
            Audio
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadItem();

            if (!Page.IsPostBack)
            {
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/swfobject.js"));

                if (!lError.Visible)
                {
                    GetPodcastFeed();
                }
            }
        }

        private void LoadItem()
        {
            if (Request.QueryString["item"] != null && Request.QueryString["item"] != "-1")
            {
                try
                {
                    item = new Item(int.Parse(Request.QueryString["item"]));

                    if (item.ItemId == -1 || !item.Active || item.PublishDate > DateTime.Now)
                    {
                        ShowError();
                    }
                    else
                    {
                        lError.Visible = false;
                    }
                }
                catch (FormatException)
                {
                    ShowError();
                }
            }
            else
            {
                ShowError();
            }
        }

        private void ShowError()
        {
            item = new Item(-1);
            lError.Text = string.Format("<h2 style=\"color: #C4B413;\">{0}<h2>", InvalidTopicIDMessageSetting);
            lError.Visible = true;
        }

        private void GetPodcastFeed()
        {
            List<ItemFormat> formats = GetFormats(ItemFormatTypes.Video).ToList();

            if (formats.Count > 0)
            {
                BuildSwfObject(formats);
            }
            else
            {
                formats = GetFormats(ItemFormatTypes.Audio).ToList();

                if (formats.Count > 0)
                {
                    ServeAudio(formats.First().EnclosureUrl);
                }
            }
        }

        private void BuildSwfObject(IEnumerable<ItemFormat> formats)
        {
            StringBuilder swfobject = new StringBuilder();
            swfobject.Append("\n\t\t<script type=\"text/javascript\">\n");
            swfobject.AppendFormat("\t\t\tvar so = new SWFObject('{0}','mpl','{1}','{2}','9');\n", FlashVideoUrlSetting, FlashWidthSetting, FlashHeightSetting);
            swfobject.Append("\t\t\tso.addParam('allowscriptaccess','always');\n");
            swfobject.AppendFormat("\t\t\tso.addParam('allowfullscreen','{0}');\n", AllowFullScreenSetting);
            swfobject.Append("\t\t\tso.addParam('wmode', 'transparent');\n");  // Consider refactoring into custom list module setting
            swfobject.AppendFormat("\t\t\tso.addParam('flashvars','&file={0}&streamer={1}&plugins=googlytics-1&bufferlength=6&controlbar=over&stretching=none&skin=UserControls/Custom/Cccev/WebUtils/misc/podcast/Modieus/stylish.swf&quality=true&autostart={2}');\n", 
                Server.UrlEncode(GetFileName(formats.First().EnclosureUrl)), Server.UrlEncode(GetStreamerUrl(formats.First().EnclosureUrl)), AutoPlaySetting);
            swfobject.Append("\t\t\tso.write('flash_player');\n");
            swfobject.Append("\t\t</script>\n");
            phSwfObject.Controls.Add(new LiteralControl(swfobject.ToString()));
        }

        private void ServeAudio(string url)
        {
            Response.Redirect(url);
        }

        private static string GetFileName(string enclosureUrl)
        {
            return enclosureUrl.Substring(enclosureUrl.LastIndexOf('/') + 1);
        }

        private string GetStreamerUrl(string enclosureUrl)
        {
            Regex regex = new Regex(@"(http://[^/]+)/(.+)/.+\.\w\w\w");
            string url = string.Empty;

            if (regex.IsMatch(enclosureUrl))
            {
                Match match = regex.Match(enclosureUrl);
                url = string.Format("{0}/{1}", StreamerServerSetting, match.Groups[2].Value);
            }

            return url;
        }

        private IEnumerable<ItemFormat> GetFormats(ItemFormatTypes type)
        {
            IEnumerable<ItemFormat> formats;

            switch (type)
            {
                case ItemFormatTypes.Video:
                    formats = from itemFormat in item.ItemFormats
                              where itemFormat.MimeType.ToLower() == "video/mp4" && itemFormat.PublicFormat
                              select itemFormat;
                    break;
                case ItemFormatTypes.Audio:
                    formats = from itemFormat in item.ItemFormats
                              where itemFormat.MimeType.ToLower() == "audio/mpeg" && itemFormat.PublicFormat
                              select itemFormat;
                    break;
                default:
                    formats = new List<ItemFormat>();
                    break;
            }

            return formats;
        }

        protected string GetSeriesPageUrl()
        {
            return string.Format("default.aspx?page={0}&channel={1}", PodcastChannelTopicPageSetting, item.ChannelId);
        }

        protected string GetServiceType()
        {
            return new Channel(item.ChannelId).Title;
        }

        protected string GetSermonSeriesName()
        {
            if (item.ItemId != -1)
            {
                return string.Format("{0} - {1}", item.Topic.Title, item.Title);
            }

            return string.Empty;
        }

        protected string GetDescription()
        {
            if ( item.ItemId != -1 )
			{
				return item.Description;
			}

            return string.Empty;
        }

        protected string GetMailto()
        {
            return string.Format("mailto:?subject={0}&body={1}", Server.UrlEncode(item.Topic.Title), Server.UrlEncode(Request.Url.AbsoluteUri));
        }
    }
}