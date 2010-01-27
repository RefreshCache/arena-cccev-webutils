/**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: TemplateBuilder.cs $
* $Revision: 1 $ 
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/TemplateBuilder.cs   1   2009-04-20 08:25:38-07:00   JasonO $
* 
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.WebUtils/Entity/TemplateBuilder.cs $
*  
*  Revision: 1   Date: 2009-04-20 15:25:38Z   User: JasonO 
**********************************************************************/

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    public abstract class TemplateBuilder : System.Web.UI.ITemplate
    {
        public abstract void InstantiateIn(System.Web.UI.Control container);
    }
}
