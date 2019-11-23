<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/pala_custom/theme/field2/cut/FRAME.Master"
    AutoEventWireup="true"
    CodeBehind="LIST.aspx.cs"
    Inherits="PILIPALA.pala_custom.theme.field2.cut.LIST" %>

<%@ Import Namespace="PILIPALA.pala_system.service" %>
<%@ Import Namespace="PILIPALA.pala_custom.theme.field2.web_service" %>

<asp:Content ID="Content1" ContentPlaceHolderID="RefreshBlock" runat="server">

    <!-- 置顶文章输出 -->
    <%foreach (int text_id in BS.getTextIDList(text_archiv_list_top))
        { %>
    <%LibStruct.pilipala.PaText PaText = BS.getTextSub(text_id); %>

    <div class="Card L M bSha bRds">
        <div onclick="up();showText(<%Response.Write(PaText.text_id); %>)" class="contain L cur bRds w250">

            <%Response.Write(PaText.before_html); %>

            <div class="Title"><%Response.Write(BS.getTextTitle(text_id)); %></div>
            <div class="Summary">
                <%Response.Write(BS.doSummary(text_id, 120));%>
            </div>
        </div>

        <div class="AtBox L">
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
            <div class="Comment"><%Response.Write(PaText.count_comment); %></div>
            <div class="Star"><%Response.Write(PaText.count_star); %></div>
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

    <%} %>

    <!-- 其他文章输出 -->
    <%foreach (int text_id in BS.getTextIDList(text_archiv_list))
        { %>
    <%LibStruct.pilipala.PaText PaText = BS.getTextSub(text_id); %>

    <%if (BS.getTextTitle(text_id) == "")
        {%>
    <div class="Card L M bSha bRds">
        <div class="contain L bRds w250">
            <div class="Content bRds">
                <div class="Content bRds"><%Response.Write(BS.getTextContent(text_id)); %></div>
            </div>
        </div>
        <div class="AtBox L">
            <div class="Pv"><%Response.Write(PaText.count_pv); %></div>
            <div class="Comment"><%Response.Write(PaText.count_comment); %></div>
            <div class="Star"><%Response.Write(PaText.count_star); %></div>
            <div class="Time">
                <%Response.Write(FieldService.toTime2(PaText.date_created)); %>
            </div>
        </div>
    </div>
    <%}
        else
        { %>

    <div class="Card L M bSha bRds">
        <div onclick="up();showText(<%Response.Write(PaText.text_id); %>)" class="contain L cur bRds w250">

            <%Response.Write(PaText.before_html); %>

            <div class="Title"><%Response.Write(BS.getTextTitle(text_id)); %></div>
            <div class="Summary">
                <%Response.Write(BS.doSummary(text_id, 120));%>
            </div>
        </div>

        <div class="AtBox L">
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
            <div class="Comment"><%Response.Write(PaText.count_comment); %></div>
            <div class="Star"><%Response.Write(PaText.count_star); %></div>
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

    <%} %>

    <%} %>
</asp:Content>
