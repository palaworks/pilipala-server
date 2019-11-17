<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/pala_custom/theme/field2/cut/FRAME.Master"
    AutoEventWireup="true"
    CodeBehind="CONTENT.aspx.cs"
    Inherits="PILIPALA.pala_custom.theme.field2.cut.CONTENT" %>

<%@ Import Namespace="PILIPALA.pala_system.service" %>
<%@ Import Namespace="PILIPALA.pala_custom.theme.field2.web_service" %>

<asp:Content ID="Content1" ContentPlaceHolderID="RefreshBlock" runat="server">
    <div class="Card L M bSha bRds">
        <div class="contain L bRds w250">

            <% if (PaText.text_title != "")
                { //如果标题不为空则输出%>
            <div class="Title"><%Response.Write(PaText.text_title); %></div>
            <div class="Summary"><%Response.Write(PaText.text_summary); %></div>
            <%} %>

            <div class="Content bRds"><%Response.Write(PaText.text_content); %></div>

            <%if (nextTextTitle != null)
                {%>
            <div class="nxtBtn L bRds cur" onclick="up();showText(<%Response.Write(nextTextID); %>)"><%Response.Write(nextTextTitle); %></div>
            <%} %>

            <%if (prevTextTitle != null)
                {%>
            <div class="pvsBtn R bRds cur" onclick="up();showText(<%Response.Write(prevTextID); %>)"><%Response.Write(prevTextTitle); %></div>
            <%} %>

            <script>$(".CardCol>.Card>.contain>.Content").html(marked($(".CardCol>.Card>.contain>.Content").html()));</script>
        </div>

        <!-- 代码高亮启用 -->
        <script>
            hljs.initHighlighting.called = false;
            hljs.initHighlighting();
        </script>

    </div>
    <div class="CoBox L M bSha b50">

        <span class="text_auth L">由<%Response.Write(PaText.text_editor); %>最后编辑于<%Response.Write(FieldService.toTime1(PaText.date_changed, "/")); %></span>
        <span class="text_mId R"><%Response.Write(Basic.toMD5(PaText.text_content).Substring(0, 7)); %></span>

        <div class="AtBox L bRds w250">
            <div class="Date">
                <%
                    if (Basic.timeFromNow(PaText.date_created) == null)
                    {
                        Response.Write(FieldService.toTime1(PaText.date_created, "-"));
                    }
                    else
                    {
                        Response.Write(Basic.timeFromNow(PaText.date_created));
                    }
                %>
            </div>
            <div class="Pv"><%Response.Write(PaText.count_pv); %></div>
            <div class="Comment cur"><%Response.Write(PaText.count_comment); %></div>
            <div class="Star cur" onclick="refre_countStar(<%Response.Write(PaText.text_id); %>)"><%Response.Write(PaText.count_star); %></div>
            <div class="Time">
                <%Response.Write(FieldService.toTime2(PaText.date_created)); %>
            </div>

            <%if (PaText.text_archiv != "")
                {/* 如果归档不为空就输出 */
            %>
            <div class="Archiv"><%Response.Write(PaText.text_archiv); %></div>
            <%} %>

            <%if (PaText.tags != "")
                {/* 如果标签不为空就输出 */
                    foreach (string str in FieldService.toTags(PaText.tags))
                    {
            %>
            <div class="Tag"><%Response.Write(str); %></div>
            <%}
                }%>
        </div>
    </div>

    <div class="CommentBox L bSha bRds w250">
        <div>评论(暂不可用)</div>
        <div style="font-size: 14px">-comments-</div>
    </div>
</asp:Content>
