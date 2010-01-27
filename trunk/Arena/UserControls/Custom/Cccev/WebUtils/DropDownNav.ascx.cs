/**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: DropDownNav.ascx.cs $
* $Revision: 13 $ 
* $Header: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/DropDownNav.ascx.cs   13   2009-04-20 14:34:53-07:00   JasonO $
* 
* $Log: /trunk/Arena/UserControls/Custom/Cccev/WebUtils/DropDownNav.ascx.cs $
*  
*  Revision: 13   Date: 2009-04-20 21:34:53Z   User: JasonO 
*  
*  Revision: 12   Date: 2009-04-20 20:42:27Z   User: JasonO 
*  
*  Revision: 11   Date: 2009-04-16 23:42:20Z   User: JasonO 
*  wrapping links horizontally rather than vertically for greater flexibility 
*  of control. 
*  
*  Revision: 10   Date: 2009-04-09 18:23:21Z   User: JasonO 
*  
*  Revision: 9   Date: 2009-03-27 00:03:40Z   User: JasonO 
*  fixing bug where child pages were being rendered as regular pages while 
*  display third level = false 
*  
*  Revision: 8   Date: 2009-03-26 22:30:13Z   User: JasonO 
*  
*  Revision: 7   Date: 2009-03-11 21:34:37Z   User: nicka 
*  fixed "Sequence contains no elements" linq bug. 
*  
*  Revision: 6   Date: 2009-03-10 17:09:43Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-03-05 22:20:48Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Portal;
using Arena.Security;
using Arena.Custom.Cccev.WebUtils.Entity;

namespace ArenaWeb.UserControls.Custom.Cccev.WebUtils
{
    public partial class DropDownNav : PortalControl
    {
        [PageSetting("Root Nav Page", "Page ID of the navigation's entry point.", false)]
        public string RootPageSetting { get { return Setting("RootPage", "", false); } }

        private PortalPage page;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                smpScripts.Scripts.Add(new ScriptReference(BasePage.JQUERY_INCLUDE));
                smpScripts.Scripts.Add(new ScriptReference("~/Templates/Cccev/liger/js/drop.js"));
            }

            page = new PortalPage(int.Parse(RootPageSetting));
            BuildNavBar();
        }

        private void BuildNavBar()
        {
            var rootNav = (from p in page.ChildPages.OfType<PortalPage>()
                           where (p.PortalPageID != -1 && p.DisplayInNav && p.Permissions.Allowed(OperationType.View, CurrentUser))
                           select p).Distinct().OrderBy(p => p.PageOrder);

            phNav.Controls.Add(new LiteralControl("<ul class=\"main-nav\">"));

            foreach (PortalPage p in rootNav)
            {
                var publicChildren = from cp in p.ChildPages.OfType<PortalPage>()
                                     where (cp.DisplayInNav && cp.Permissions.Allowed(OperationType.View, CurrentUser))
                                     select cp;

                if (publicChildren.Count() > 0)
                {
                    BuildDropDown(p);
                }
                else
                {
                    phNav.Controls.Add(new LiteralControl(string.Format("<li><a href=\"Default.aspx?page={0}\">{1}</a></li>", p.PortalPageID, p.Title)));
                }
            }

            phNav.Controls.Add(new LiteralControl("</ul>"));
        }

        private void BuildDropDown(PortalPage portalPage)
        {
            LiteralControl listItem = new LiteralControl("<li>");
            phNav.Controls.Add(listItem);
            phNav.Controls.Add(new LiteralControl(string.Format("<a href=\"#\" class=\"drop-down\"><span>{0}</span></a>\n", portalPage.Title)));
            phNav.Controls.Add(BuildDropDownContainer(portalPage));
            listItem = new LiteralControl("</li>");
            phNav.Controls.Add(listItem);
        }

        private Control BuildDropDownContainer(PortalPage portalPage)
        {
            Panel panel = new Panel
            {
                ID = "pnlDropHolder", 
                CssClass = "drop-holder"
            };

            panel.Attributes.Add("style", "display: none");

            panel.Controls.Add(new LiteralControl("<div class=\"drop\">"));
            panel.Controls.Add(new LiteralControl("<div class=\"drop-top\"></div>"));
            panel.Controls.Add(BuildColumns(portalPage));
            panel.Controls.Add(new LiteralControl("<div class=\"link-holder\"><a href=\"#\" class=\"close\">close</a></div>"));
            panel.Controls.Add(new LiteralControl("</div>"));
            return panel;
        }

        private Control BuildColumns(PortalPage portalPage)
        {
            Panel box = new Panel
            {
                ID = "pnlBox", 
                CssClass = "box"
            };

            Panel list1 = new Panel
            {
                ID = "pnlList1", 
                CssClass = "list-1"
            };

            Repeater col1 = BuildRepeater();
            List<PortalPage> column1 = new List<PortalPage>();

            Panel list2 = new Panel
            {
                ID = "pnlList2", 
                CssClass = "list-2"
            };

            Repeater col2 = BuildRepeater();
            List<PortalPage> column2 = new List<PortalPage>();

            Panel list3 = new Panel
            {
                ID = "pnlList3", 
                CssClass = "list-3"
            };

            Repeater col3 = BuildRepeater();
            List<PortalPage> column3 = new List<PortalPage>();

            int col = 1;

            foreach (PortalPage p in portalPage.ChildPages)
            {
                if (p.DisplayInNav && p.Permissions.Allowed(OperationType.View, CurrentUser))
                {
                    switch (col)
                    {
                        case 1:
                            column1.Add(p);
                            break;
                        case 2:
                            column2.Add(p);
                            break;
                        case 3:
                            column3.Add(p);
                            break;
                        default:
                            break;
                    }

                    if (col < 3)
                    {
                        col++;
                    }
                    else
                    {
                        col = 1;
                    }
                }
            }

            col1.DataSource = column1;
            col1.DataBind();
            list1.Controls.Add(col1);

            col2.DataSource = column2;
            col2.DataBind();
            list2.Controls.Add(col2);

            col3.DataSource = column3;
            col3.DataBind();
            list3.Controls.Add(col3);

            box.Controls.Add(list1);
            box.Controls.Add(list2);
            box.Controls.Add(list3);
            return box;
        }

        private static Repeater BuildRepeater()
        {
            Repeater repeater = new Repeater
            {
                HeaderTemplate = new UnorderdListBuilder(ListItemType.Header),
                ItemTemplate = new UnorderdListBuilder(ListItemType.Item),
                AlternatingItemTemplate = new UnorderdListBuilder(ListItemType.AlternatingItem),
                FooterTemplate = new UnorderdListBuilder(ListItemType.Footer)
            };

            return repeater;
        }
    }
}