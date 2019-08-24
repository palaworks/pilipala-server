<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/pala_custom/theme/field2/cut/FRAME.Master"
    AutoEventWireup="true"
    CodeBehind="LIST.aspx.cs"
    Inherits="PILIPALA.pala_custom.theme.field2.cut.LIST" %>

<%@ Import Namespace="PILIPALA.pala_custom.theme.field2.web_service" %>

<asp:Content ID="Content1" ContentPlaceHolderID="RefreshBlock" runat="server">

    <%foreach (int text_id in BS.getTextIDList("post"))
        { %>
    <%LibStruct.pilipala.PaText PaText = BS.getTextSub(text_id); %>

    <div class="Card L M bSha bRds">
        <div onclick="up();showText(<%Response.Write(PaText.text_id); %>)" class="contain L cur bRds w250">

            <%if (PaText.cover_url != "")
                {  %>
            <img class="pXL" src="<%Response.Write(PaText.cover_url); %>" />
            <%} %>

            <div class="Title"><%Response.Write(BS.getTextTitle(text_id)); %></div>
            <div class="Summary"><%Response.Write(BS.getTextSummary(text_id)); %></div>
        </div>

        <div class="AtBox L">
            <div class="Date"><%Response.Write(FieldService.toTime1(PaText.date_created, "-")); %></div>
            <div class="Pv"><%Response.Write(PaText.count_pv); %></div>
            <div class="Comment"><%Response.Write(PaText.count_comment); %></div>
            <div class="Star"><%Response.Write(PaText.count_star); %></div>
            <div class="Time"><%Response.Write(FieldService.toTime2(PaText.date_changed)); %></div>
            <div class="Archiv"><%Response.Write(PaText.text_archiv); %></div>

            <%foreach (string str in FieldService.toTags(PaText.tags))
                {  %>
            <div class="Tag"><%Response.Write(str); %></div>
            <%} %>
        </div>
    </div>

    <%} %>
</asp:Content>
