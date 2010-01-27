<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PromotionRoller.ascx.cs"
    Inherits="ArenaWeb.UserControls.Custom.Cccev.WebUtils.PromotionRoller" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:ScriptManagerProxy ID="smpScripts" runat="server" />

<div id="gallery">

    <% var promotions = GetPromotionRequests(); %>
    
    <div class="counter-holder">
        <a href="#" class="link-prev">link-prev</a>
        <div class="counter">
            <span class='this'>1</span>/<span class='all'><%= GetPageCount(promotions) %></span></div>
        <a href="#" class="link-next">link-next</a>
    </div>
    <br clear="all" />
    <div class="list">
        <ul>
            <%
                foreach (var p in promotions)
                {
            %>
            <li>
                <div class="picture-holder">
                    <div class="photo-card">
                        <a href="<%= GetDetailsUrl(p.PromotionRequestID) %>">
                            <img src="<%= GetImageUrl(p) %>" alt='<%= p.Title %>' />
                        </a>
                    </div>
                    <div class="shadow">
                        &nbsp;</div>
                </div>
            </li>
            <%
                } 
            %>
        </ul>
    </div>
</div>