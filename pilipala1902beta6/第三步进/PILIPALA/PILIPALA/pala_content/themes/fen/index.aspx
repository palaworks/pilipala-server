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
    <title>Thaumy的博客|又一个码农的家</title>
    <style type="text/css">
        @import url("css/global.css");
        @import url("css/pic.css");
        @import url("css/basic.css");
        @import url("css/font.css");
        @import url("css/Box/Box.css");
        @import url("css/Btn/Btn.css");
        @import url("plugcss/marks.css");
    </style>
    <script src="https://cdn.bootcss.com/jquery/1.12.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdn.bootcss.com/jquery-cookie/1.4.1/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="https://cdn.bootcss.com/showdown/1.9.0/showdown.min.js" type="text/javascript"></script>
    <script src="fenjs/ShowNaviBtn.js" type="text/javascript"></script>
    <script src="fenjs/basic.js" type="text/javascript"></script>

    <script src="js/index.js"></script>

</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="indexBody" runat="Server">
    <!-- ScriptManager -->
    <asp:ScriptManager ID="indexScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/pala_services/SLS.asmx" />
        </Services>
    </asp:ScriptManager>
    <!-- ScriptManager -->


    <div class="main">

        <%if (Request.QueryString["guide"] == "1")
            {%>
        <div class="NaviCol RdiuT RdiuB">
            <div class="NaviHead Shadow fltL">
                <div class="ShowNaviBtn Tran cur"></div>
                <a onclick="GoUp();goHome()">
                    <div class="UsrPic cur"></div>
                    <div class="SiteSummary cur">THAUMY的博客</div>
                </a>
                <div class="UsrMto">“坐而言不如起而行”</div>
            </div>
            <div class="NaviCardBox Shadow fltL">

                <%foreach (LibStructs.PaText PaText in List_text_index_page)
                    {
                        LibStructs.PaText PaTextMain = SLS.getTextMain(PaText.text_id);
                %>
                <a onclick="GoUp();showTxt(<%Response.Write(PaTextMain.text_id); %>)" class="NaviCard cur RdiuT RdiuB flx Tran">
                    <div class="NaviCardTitle flxC"><%Response.Write(PaTextMain.text_title); %></div>
                </a>
                <%} %>

                <div class="CoMark">
                    Thaumy的博客©2016-2019保留所有权利<br>
                    基于pilipala开发<br>
                    based on pilipala<br>
                </div>
            </div>
        </div>
        <%} %>


        <div class="TxtCol">
            <%if (Request.QueryString["text"] == "1")
                { %>

            <%if (List_text_index_post != null)
                {
                    foreach (LibStructs.PaText idxPaText in List_text_index_post)
                    {%>

            <%LibStructs.PaText PaText = new LibStructs.PaText();
                PaText = SLS.fill(SLS.getTextMain(idxPaText.text_id), SLS.getTextSub(idxPaText.text_id));%>


            <div class="TxtBox mohu Shadow fltL RdiuB RdiuT MagnB">
                <%if (PaText.cover_url != "")
                    {  %>
                <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                    <img alt="" class="RdiuT TxtImg" src="<%Response.Write(PaText.cover_url); %>" />
                </a>
                <%} %>
                <div class="Strip <%Response.Write(indexServ.stripStyle(PaText.strip_color)); %>"></div>
                <div class="TxtBoxCntnt">
                    <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                        <div onclick="GoUp()" class="TxtTitle cur"><%Response.Write(PaText.text_title); %></div>
                        <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                    </a>
                </div>
                <div class="fltL LabelBox LabelTxt RdiuT RdiuB">
                    <div class="TimeLabel"><%Response.Write(indexServ.extime(PaText.date_created)); %></div>
                    <div class="PvLabel">阅读<%Response.Write(PaText.count_pv); %></div>
                    <div class="CommentLabel RdiuB RdiuT">评论<%Response.Write(PaText.count_comment); %></div>
                    <div class="LikeLabel RdiuB RdiuT"><%Response.Write(PaText.count_like);%></div>
                    <div class="ClassLabel"><%Response.Write(PaText.text_class); %></div>
                    <%foreach (string tag in indexServ.extags(PaText.tags))
                        { %>
                    <div class="TagLabel"><%Response.Write(tag); %></div>
                    <%} %>
                </div>
            </div>

            <%} %>
            <a onclick="loadTxt()" class="LoadPostBtn mohu cur Shadow fltL Tran RdiuB RdiuT">
                <div class="arrowDn"></div>
            </a>
            <%} %>



            <% if (List_text_index_post == null)
                {%>
            <div class="TxtBox mohu Shadow fltL RdiuB RdiuT MagnB">
                <%if (PaText.cover_url != "")
                    {%>
                <img alt="" class="RdiuT TxtImg" src="<%Response.Write(PaText.cover_url); %>" />
                <%} %>

                <div class="BoxStrip <%Response.Write(indexServ.stripStyle(PaText.strip_color)); %>"></div>
                <div class="TxtBoxCntnt">
                    <div class="TxtTitle"><%Response.Write(PaText.text_title); %></div>
                    <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                    <div class="TxtContent"><%Response.Write(PaText.text_content); %></div>
                    <!-- markdown字符转换脚本 -->
                    <script>mkdConvert($(".TxtContent").html());</script>
                </div>

            </div>
            <div class="AttriBox Shadow fltL RdiuT RdiuB MagnB">
                <div class="CopBox fltL RdiuT">
                    <div class="CopTime">此文本由 <%Response.Write(PaText.text_editor); %> 最后维护于 <%Response.Write(indexServ.extime(PaText.date_changed)); %></div>
                    <div class="CopId">文本序列号：<%Response.Write(PaText.text_id); %></div>
                </div>
                <div class="LabelBox fltL LabelTxt LabelBg RdiuB">

                    <div class="TimeLabel"><%Response.Write(indexServ.extime(PaText.date_created)); %></div>
                    <div class="PvLabel">阅读<%Response.Write(PaText.count_pv); %></div>
                    <div class="CommentBtn cur RdiuB RdiuT">评论<%Response.Write(PaText.count_comment); %></div>
                    <a onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="LikeBtn cur RdiuB RdiuT">
                        <%Response.Write(PaText.count_like); %>
                    </a>
                    <div class="ClassLabel"><%Response.Write(PaText.text_class); %></div>

                    <%foreach (string tag in indexServ.extags(PaText.tags))
                        {  %>
                    <div class="TagLabel"><%Response.Write(tag); %></div>
                    <%} %>
                </div>
            </div>
            <div>
                <!-- 推荐文章按钮 -->
                <%LibStructs.PaText rdmPaText = SLS.getTextMain(SLS.rdmTextIndex(rqst_text_id, "post").text_id);%>
                <a onclick="GoUp();showTxt(<%Response.Write(rdmPaText.text_id); %>)" class="RandomBtn mohu cur Shadow Tran RdiuB RdiuT MagnB">
                    <%Response.Write(rdmPaText.text_title); %>

                </a>
                <!-- 评论区 -->
                <div class="CommentBox mohu Shadow fltL RdiuB RdiuT MagnB">评论将于BETA测试结束后开放</div>
            </div>
            <%}%>
        </div>

    </div>
    <%} %>


    <div onclick="GoUp()" class="GoUpBtn cur Shadow fltL">
        <div class="arrowUp"></div>
    </div>
    <footer>
        <div style="color: aliceblue; font-size: 14px;">本博客目前处于第六开发板（pilipalalaBETA6），所有内容均为测试文本，可能不具备参考价值</div>
    </footer>
    <script>
        //ShowNaviCardBoxBtn点击事件
        $(".ShowNaviBtn").click(function () {
            if ($(".NaviCardBox").is(":hidden")) { ShowNaviBox(); }
            else { HideNaviBox(); }
        });
        //NaviCard点击事件
        $(".NaviCard").click(function () {
            if ($(".NaviCardBox").is(":hidden")) { ShowNaviBox(); }
            else if (document.body.clientWidth <= 1024) { HideNaviBox(); }
        });
        $(window).scroll(function () {
            if ($(window).scrollTop() > 400) {
                $(".GoUpBtn").fadeIn(350);
            }
            else {
                $(".GoUpBtn").fadeOut(350);
            }

        });
    </script>
</asp:Content>
