<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarInvitePopover.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.WebUtils.CalendarInvitePopover" %>

<asp:ScriptManagerProxy ID="smpScripts" runat="server" />

<script type="text/javascript">
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function()
{
    initPopovers();
    clearEmailPopover();
});
</script>

<asp:UpdatePanel ID="upEmailPopover" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="cal-popup-light-holder">
            <div class="email-invite-holder">
                <div class="email-invite">
                    <div class="email-box">
                        <h2 class="img">Invite a Friend</h2>
                        <a class="btn" href="#">btn</a>
                        <div class="form-holder">
                            <div class="subscribe">
                                <label>To:</label>
                                <div class="text"><asp:TextBox ID="tbTo" runat="server" /></div>
                                <label class="mar-top">Email:
                                </label>
                                <div class="text">
                                    <asp:TextBox ID="tbEmailAddress" runat="server" ValidationGroup="emailPopover" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmailAddress" SetFocusOnError="true" 
                                        ValidationGroup="emailPopover" ErrorMessage="Please enter an email address." />
                                    <asp:RegularExpressionValidator ID="rvEmail" runat="server" ControlToValidate="tbEmailAddress" 
                                        ValidationExpression="^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*\.(\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|tv))$"
                                        SetFocusOnError="true" ValidationGroup="emailPopover" ErrorMessage="Please enter a valid email address." />
                                    <asp:ValidationSummary ID="vsSummary" runat="server" BorderStyle="None" ValidationGroup="emailPopover" ShowMessageBox="true" ShowSummary="false" />
                                </div>
                                <label class="mar-top">From:</label>
                                <div class="text"><asp:TextBox ID="tbFrom" runat="server" /></div>
                                <asp:ImageButton ID="ibSendEmail" runat="server" CssClass="btn" AlternateText="Send Invite" ImageUrl="/Arena/Templates/Cccev/liger/images/btn-send.gif" 
                                    ValidationGroup="emailPopover" CausesValidation="true" />
                                <input type="hidden" id="ihIDs" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ibSendEmail" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>