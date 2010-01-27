/**********************************************************************
* Description:	Adds Email Invite Popover effect to a page with SideBarCalendar.
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	4/08/2009
*
* $Workfile: CalendarInvitePopover.ascx.cs $
* $Revision: 10 $ 
* $Header: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/CalendarInvitePopover.ascx.cs   10   2009-07-28 11:06:15-07:00   JasonO $
* 
* $Log: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/CalendarInvitePopover.ascx.cs $
*  
*  Revision: 10   Date: 2009-07-28 18:06:15Z   User: JasonO 
*  
*  Revision: 9   Date: 2009-07-28 17:49:27Z   User: JasonO 
*  Refactoring to remove hard-coded elements from module. 
*  
*  Revision: 8   Date: 2009-04-08 16:44:20Z   User: JasonO 
*  
*  Revision: 7   Date: 2009-04-08 00:18:45Z   User: JasonO 
*  
*  Revision: 6   Date: 2009-04-08 00:12:11Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-04-07 23:41:18Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-04-06 20:53:55Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-04-01 23:05:37Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-04-01 20:53:13Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-04-01 18:16:21Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Web.UI;
using Arena.Portal;
using Arena.Core.Communications;
using Arena.Event;

namespace ArenaWeb.UserControls.Custom.Cccev.WebUtils
{
    public partial class CalendarInvitePopover : PortalControl
    {
        [TextSetting("Event Email Subject", "Subject of event email.  Available merge fields: ##To##, ##ToEmail##, ##From##, ##EventTitle##, ##EventSummary##", true)]
        public string EventEmailSubjectSetting { get { return Setting("EventEmailSubject", "", true); } }

        [TextSetting("Event Email Body", "Body of event email.  Available merge fields: ##To##, ##ToEmail##, ##From##, ##EventTitle##, ##EventSummary##, ##EventImage##, ##URL##", false)]
        public string EventEmailBodySetting { get { return Setting("EventEmailBody", "", false); } }

        [PageSetting("Event Details Page", "Page that displays event details.", true)]
        public string EventDetailsPageSetting { get { return Setting("EventDetailsPage", "", true); } }

        [NumericSetting("Email Image Height", "Height of image to be embedded in email. Defaults to 174.", false)]
        public string EventEmailImageHeightSetting { get { return Setting("EventEmailImageHeight", "174", false); } }

        [NumericSetting("Email Image Width", "Height of image to be embedded in email. Defaults to 400.", false)]
        public string EventEmailImageWidthSetting { get { return Setting("EventEmailImageWIdth", "400", false); } }

        [TextSetting("Default From Email Address", "Default address to send email from if nobody is logged inJason.", false)]
        public string DefaultFromEmailAddressSetting { get { return Setting("DefaultFromEmailAddress", "info@cccev.com", false); } }

        [TextSetting("CSS File Path", "Relative path to css file.", false)]
        public string CssFileSetting { get { return Setting("CssFile", "/arena/Templates/Cccev/liger/css/calendarPopover.css", false); } }

        protected void Page_Init(object sender, EventArgs e)
        {
            ibSendEmail.Click += ibSendEmail_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && CssFileSetting.Trim() != string.Empty)
            {
                smpScripts.Scripts.Add(new ScriptReference(BasePage.JQUERY_INCLUDE));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/popover.js"));
                BasePage.AddCssLink(Page, CssFileSetting);
            }
        }

        private void ibSendEmail_Click(object sender, ImageClickEventArgs e)
        {
            string[] ids = ihIDs.Value.Split(new[] { '_' });
            SendEmail(new EventProfile(int.Parse(ids[0])), int.Parse(ids[1]));
            ihIDs.Value = string.Empty;
        }

        private void SendEmail(EventProfile profile, int occurrenceID)
        {
            string subject = EventEmailSubjectSetting;
            string body = EventEmailBodySetting.Trim() != string.Empty ? EventEmailBodySetting : CurrentModule.Details;

            foreach (KeyValuePair<string, string> field in BuildMergeFields(profile, occurrenceID))
            {
                subject = subject.Replace(field.Key, field.Value);
                body = body.Replace(field.Key, field.Value);
            }

            string fromEmail = CurrentPerson != null ? CurrentPerson.Emails.FirstActive : DefaultFromEmailAddressSetting;
            new PersonCommunicationType().Send(fromEmail, tbFrom.Text, tbEmailAddress.Text, subject, body);
        }

        private Dictionary<string, string> BuildMergeFields(EventProfile profile, int occurrenceID)
        {
            Dictionary<string, string> fields = 
                new Dictionary<string, string>
                {
                    { "##To##", tbTo.Text.Trim() != string.Empty ? tbTo.Text : tbEmailAddress.Text }, 
                    { "##ToEmail##", tbEmailAddress.Text }, 
                    { "##From##", tbFrom.Text.Trim() != string.Empty ? tbFrom.Text : DefaultFromEmailAddressSetting }, 
                    { "##EventTitle##", profile.Title }, 
                    { "##EventSummary##", profile.Summary }, 
                    { "##EventImage##", string.Format("<img src=\"{0}/CachedBlob.aspx?guid={1}&width={2}&height={3}\" height=\"{3}\" width=\"{2}\">", 
                        Utilities.GetApplicationPath(), profile.Image.GUID, EventEmailImageWidthSetting, EventEmailImageHeightSetting) }, 
                    { "##URL##", string.Format("{0}/Default.aspx?page={1}&profileId={2}&occurrenceId={3}",
                        Utilities.GetApplicationPath(), EventDetailsPageSetting, profile.ProfileID, occurrenceID) }
                };

            return fields;
        }
    }
}