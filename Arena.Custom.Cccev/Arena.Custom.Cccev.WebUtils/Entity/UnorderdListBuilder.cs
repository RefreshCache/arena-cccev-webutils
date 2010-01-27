/**********************************************************************
* Description:	Encapsulates building templates for an HTML unordered list
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	???
*
* $Workfile: UnorderdListBuilder.cs $
* $Revision: 5 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/UnorderdListBuilder.cs   5   2009-04-20 08:25:45-07:00   JasonO $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/UnorderdListBuilder.cs $
*  
*  Revision: 5   Date: 2009-04-20 15:25:45Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-04-16 23:50:18Z   User: JasonO 
*  Fixing links in item template. 
*  
*  Revision: 2   Date: 2009-04-16 18:55:50Z   User: JasonO 
**********************************************************************/

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    public class UnorderdListBuilder : TemplateBuilder
    {
        private readonly ListItemType templateType;

        public UnorderdListBuilder(ListItemType templateType)
        {
            this.templateType = templateType;
        }

        public override void InstantiateIn(Control container)
        {
            Literal literal = new Literal();

            switch (templateType)
            {
                case ListItemType.Header:
                    literal.Text = "<ul>";
                    break;
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    literal.Text = string.Empty;
                    literal.DataBinding += Literal_DataBinding;
                    break;
                case ListItemType.Footer:
                    literal.Text = "</ul>";
                    break;
                default:
                    break;
            }

            container.Controls.Add(literal);
        }

        private static void Literal_DataBinding(object sender, EventArgs e)
        {
            Literal literal = (Literal)sender;
            RepeaterItem container = (RepeaterItem)literal.NamingContainer;
            literal.Text += string.Format("<li><a href=\"Default.aspx?page={0}\">{1}</a></li>",
                DataBinder.Eval(container.DataItem, "PortalPageID"), DataBinder.Eval(container.DataItem, "Title"));
        }
    }
}
