<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiListControl.ascx.cs" Inherits="UmbracoPublic.WebSite.handlers.Controls.MultiListControl" %>
<div class="box">
	<h3>All</h3>
	<div class="frame">
        <asp:PlaceHolder ID="plhSrc" runat="server"></asp:PlaceHolder>
	</div>
</div>
<div class="buttons">
	<img class="btnAdd" src="/assets/img/custom/move_right.png" /><br />
	<img class="btnRemove" src="/assets/img/custom/move_left.png"  />
</div>
<div class="box">
	<h3>Selected</h3>
	<div class="frame">
		<ul class="dstList listbox">
			<asp:Literal ID="litDstBox" runat="server"></asp:Literal>
		</ul>
	</div>
</div>
<div class="buttons">
	<img class="btnMoveUp" src="/assets/img/custom/move_up.png" /><br />
	<img class="btnMoveDown" src="/assets/img/custom/move_down.png" />
</div>