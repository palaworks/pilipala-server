<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="index.aspx.cs"
    Inherits="PILIPALA.pala_content.themes.fen.index"
    Debug="true"
    MasterPageFile="~/pala_content/index.master" %>

<%@ Import Namespace="PILIPALA.pala_services" %>
<%@ Import Namespace="PILIPALA.pala_content.themes.fen.services" %>

<asp:Content ID="head" ContentPlaceHolderID="indexHead" runat="Server">
    <!-- fenVersion -->
    <!-- 1921702152$cdb16e5096524a2fa257c4a0da72be2a -->
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes" />
    <base target="_blank">
    <link rel="shortcut icon" href="img/favicon.ico" type="image/x-icon">
    <style type="text/css">
        @import url("css/global.css");
        @import url("css/pic.css");
        @import url("css/basic.css");
        @import url("css/font.css");
        @import url("css/marks.css");
        @import url("css/Box/Box.css");
        @import url("css/Btn/Btn.css");
    </style>
    <script src="https://cdn.bootcss.com/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://cdn.bootcss.com/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
    <script src="https://cdn.bootcss.com/showdown/1.9.0/showdown.min.js"></script>
    <script src="js/index.js"></script>
    <style>
        body, .glass::before {
            background: url("img/bodyBg.jpg") fixed;
        }
    </style>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="indexBody" runat="Server">
    <!-- ScriptManager -->
    <asp:ScriptManager ID="indexScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/pala_services/SLS.asmx" />
        </Services>
    </asp:ScriptManager>
    <!-- ScriptManager -->

    <div class="siteBg"></div>

    <div class="main">

        <%if (Request.QueryString["guide"] == "1")
            {%>
        <div class="NaviCol">
            <div class="NaviBox">
                <div class="NaviHead fltL RdiuT">
                    <a onclick="goHome()">
                        <div class="Usr UsrPic"></div>
                        <div class="SiteSummary">THAUMY的博客</div>
                    </a>
                    <div class="UsrMto fltL">“坐而言不如起而行”</div>
                </div>
                <div class="NaviCardBox fltL RdiuB MagnB">
                    <%foreach (LibStructs.PaText PaText in List_text_index_page)
                        {
                            LibStructs.PaText PaTextMain = SLS.getTextMain(PaText.text_id);
                    %>
                    <a onclick="showTxt(<%Response.Write(PaTextMain.text_id); %>)" class="NaviCard RdiuB RdiuT flx Tran">
                        <div class="NaviCardTitle flx_center"><%Response.Write(PaTextMain.text_title); %></div>
                    </a>
                    <%} %>
                    <div class="CoMark">
                        Thaumy的博客©2016-2019保留所有权利<br>
                        基于pilipala开发<br>
                        based on pilipala<br>
                        背景图pixivID：
                    </div>
                </div>
            </div>
        </div>
        <%} %>

        <a href="#top" target="_self" class="GoUpBtn RdiuT RdiuB Tran fltL">
            <div class="arrow_up GoUpBtn_pic"></div>
        </a>

        <%if (Request.QueryString["text"] == "1")
            { %>
        <div class="TxtCol">
            <div>
                <%if (List_text_index_post != null)
                    {
                        foreach (LibStructs.PaText idxPaText in List_text_index_post)
                        {%>
                <%
                    LibStructs.PaText PaText = new LibStructs.PaText();
                    PaText = SLS.fill(SLS.getTextMain(idxPaText.text_id), SLS.getTextSub(idxPaText.text_id));
                %>
                <div class="TxtBox glass fltL RdiuB RdiuT Shadow MagnB">
                    <%if (PaText.cover_url != "")
                        {  %>
                    <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                        <img alt="" class="RdiuT TxtImg" src="<%Response.Write(PaText.cover_url); %>" />
                    </a>
                    <%} %>
                    <div class="BoxStrip <%Response.Write(indexServ.stripStyle(PaText.strip_color)); %>"></div>
                    <div class="TxtBoxCntnt">
                        <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                            <div class="TxtTitle"><%Response.Write(PaText.text_title); %></div>
                            <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                        </a>
                    </div>
                    <div class="fltL LabelBox LabelTxt RdiuB">
                        <div class="LabelCntnt LabelTxt">
                            <div class="LBL_time"><%Response.Write(indexServ.extime(PaText.date_created)); %></div>
                            <div class="LBL_class"><%Response.Write(PaText.text_class); %></div>
                            <div class="LBL_comment RdiuB RdiuT">评论<%Response.Write(PaText.count_comment); %></div>
                            <div class="LBL_like RdiuB RdiuT"><%Response.Write(PaText.count_like);%></div>
                            <div class="LBL_pv">阅读<%Response.Write(PaText.count_pv); %></div>
                            <%foreach (string tag in indexServ.extags(PaText.tags))
                                { %>
                            <div class="LBL_tag"><%Response.Write(tag); %></div>
                            <%} %>
                        </div>
                    </div>
                </div>
                <%} %>
                <a class="LoadPostBtn  fltL Tran RdiuB RdiuT" onclick="loadTxt()">
                    <div class="arrow_down LoadPostBtn_pic"></div>
                </a>
                <%} %>
            </div>
            <div>
                <%if (List_text_index_post == null)
                    {%>
                <div class="TxtBox glass fltL RdiuB RdiuT Shadow MagnB">
                    <%if (PaText.cover_url != "")
                        {%>
                    <img alt="" class="RdiuT TxtImg" src="<%Response.Write(PaText.cover_url); %>" />
                    <%} %>
                    <div class="BoxStrip <%Response.Write(indexServ.stripStyle(PaText.strip_color)); %>"></div>
                    <div class="TxtBoxCntnt">
                        <div class="TxtTitle"><%Response.Write(PaText.text_title); %></div>
                        <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                        <div class="TxtContent linked"><%Response.Write(PaText.text_content); %></div>
                        <script>mkdConvert($(".TxtContent").html());</script>
                    </div>
                    <div class="TxtCoMark fltL RdiuT">
                        <div class="TxtCoMarkTime">此文本由 <%Response.Write(PaText.text_editor); %> 最后维护于 <%Response.Write(indexServ.extime(PaText.date_changed)); %></div>
                        <div class="TxtCoMarkId">文本序列号：<%Response.Write(PaText.text_id); %></div>
                    </div>
                    <div class="LabelBox fltL LabelTxt RdiuB">
                        <div class="LabelCntnt LabelTxt">
                            <div class="LBL_time"><%Response.Write(indexServ.extime(PaText.date_created)); %></div>
                            <div class="LBL_class"><%Response.Write(PaText.text_class); %></div>
                            <a class="Btn_comment RdiuB RdiuT">评论<%Response.Write(PaText.count_comment); %></a>
                            <a onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="Btn_like RdiuB RdiuT"><%Response.Write(PaText.count_like); %></a>
                            <div class="LBL_pv">阅读<%Response.Write(PaText.count_pv); %></div>
                            <%foreach (string tag in indexServ.extags(PaText.tags))
                                {  %>
                            <div class="LBL_tag"><%Response.Write(tag); %></div>
                            <%} %>
                        </div>
                    </div>

                </div>
                <div>
                    <!-- 随机文本按钮 -->
                    <%LibStructs.PaText rdmPaText = SLS.getTextMain(SLS.rdmTextIndex(rqst_text_id, "post").text_id);%>
                    <a onclick="showTxt(<%Response.Write(rdmPaText.text_id); %>)" class="RandomBtn Tran RdiuB RdiuT MagnB">
                        <%Response.Write(rdmPaText.text_title); %>
                    </a>
                    <!-- 评论区 -->
                    <div class="CommentBox fltL RdiuB RdiuT Shadow MagnB">评论功能将于BETA测试结束后开放</div>
                </div>
                <%}%>
            </div>
        </div>
        <%} %>
    </div>

    <footer>
        <div style="color: aliceblue">本博客目前处于第六开发板（pilipalalaBETA6），所有内容均为测试文本，可能不具备参考价值</div>
    </footer>
</asp:Content>
