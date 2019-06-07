<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="index.aspx.cs"
    Inherits="PILIPALA.pala_content.themes.field.index"
    Debug="true"
    MasterPageFile="~/pala_content/index.master" %>

<%@ Import Namespace="PILIPALA.pala_services" %>
<%@ Import Namespace="PILIPALA.pala_content.themes.field.field_service" %>

<asp:Content ID="head" ContentPlaceHolderID="indexHead" runat="Server">
    <!-- field theme -->
    <!-- 201940201317b7fd -->
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes" />
    <base target="_blank">
    <link rel="shortcut icon" href="ui_img/favicon.ico" type="image/x-icon">
    <title>Thaumy的小破站|又一个码农的家</title>

    <style type="text/css">
        @import url("ui_css/global.css");
        @import url("ui_css/pic.css");
        @import url("ui_css/basic.css");
        @import url("ui_css/font.css");
        @import url("ui_css/Box/Box.css");
        @import url("ui_css/Btn/Btn.css");
    </style>

    <script src="https://cdn.bootcss.com/jquery/1.12.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdn.bootcss.com/jquery-cookie/1.4.1/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="https://cdn.bootcss.com/showdown/1.9.0/showdown.min.js" type="text/javascript"></script>
    <script src="https://cdn.bootcss.com/typed.js/2.0.10/typed.min.js"></script>
    <script src="ui_js/ShowNaviBtn.js" type="text/javascript"></script>
    <script src="ui_js/basic.js" type="text/javascript"></script>

    <!-- field_js -->
    <script src="field_js/index.js" type="text/javascript"></script>
    <!-- field_js -->
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
            <div class="NaviHead bSha fltL">
                <div class="ShowNaviBtn Tran cur">
                    <div class="arrowDn"></div>
                </div>
                <a onclick="GoUp();goHome()">
                    <div class="UsrPic cur"></div>
                    <div class="SiteSummary cur">THAUMY的小破站</div>
                    <div class="UsrMto"><span class="hiWord"></span></div>
                    <script src="ui_js/hiWord.js" type="text/javascript"></script>
                </a>
            </div>
            <div class="NaviCardBox bSha fltL">

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
                    <!-- CNZZ -->
                    <script type="text/javascript" src="https://s19.cnzz.com/z_stat.php?id=1262285427&web_id=1262285427"></script>
                    <!-- CNZZ -->
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


            <div class="TxtBox pSha bSha fltL RdiuB RdiuT MagnB">
                <%if (PaText.cover_url != "")
                    {  %>
                <img onclick="showTxt(<%Response.Write(PaText.text_id); %>)" class="RdiuT pic-max" src="<%Response.Write(PaText.cover_url); %>" />
                <%} %>
                <div class="<%Response.Write(indexServ.qianStyle(PaText.strip_color)); %>"></div>
                <div class="TxtBoxFrame">
                    <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                        <div onclick="GoUp()" class="TxtTitle cur"><%Response.Write(PaText.text_title); %></div>
                        <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                    </a>
                </div>
                <div class="LabelBox fltL LabelTxt RdiuB">
                    <div class="DateLabel"><%Response.Write(indexServ.trsDate(PaText.date_created)); %></div>
                    <div class="PvLabel">阅读 <%Response.Write(PaText.count_pv); %></div>
                    <div class="CommentLabel RdiuB RdiuT">评论 <%Response.Write(PaText.count_comment); %></div>
                    <div onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="LikeLabel RdiuB RdiuT cur">点赞 <%Response.Write(PaText.count_like); %></div>
                    <div class="TimeLabel"><%Response.Write(indexServ.trsTime(PaText.date_created)); %></div>
                    <div class="ClassLabel"><%Response.Write(PaText.text_class); %></div>
                    <%foreach (string tag in indexServ.trsTags(PaText.tags))
                        {  %>
                    <div class="TagLabel"><%Response.Write(tag); %></div>
                    <%} %>



                </div>

            </div>

            <%} %>
            <a onclick="loadTxt()" class="LoadPostBtn cur bSha fltL Tran RdiuB RdiuT">
                <div class="arrowDn"></div>
            </a>
            <%} %>



            <% if (List_text_index_post == null)
                {%>
            <div class="TxtBox pSha bSha fltL RdiuB RdiuT MagnB">
                <%if (PaText.cover_url != "")
                    {%>
                <img alt="" class="RdiuT pic-max" src="<%Response.Write(PaText.cover_url); %>" />
                <%} %>

                <div class="<%Response.Write(indexServ.qianStyle(PaText.strip_color)); %>"></div>
                <div class="TxtBoxFrame">
                    <div class="TxtTitle"><%Response.Write(PaText.text_title); %></div>
                    <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                    <div class="TxtContent pSha"><%Response.Write(PaText.text_content); %></div>
                    <!-- markdown字符转换脚本 -->
                    <script>mkdConvert($(".TxtContent").html());</script>
                </div>

            </div>
            <div class="AttriBox bSha fltL RdiuT RdiuB MagnB">
                <div class="CopBox fltL RdiuT">
                    <div class="CopTime">此文本由 <%Response.Write(PaText.text_editor); %> 最后维护于 <%Response.Write(indexServ.trsDate(PaText.date_changed)); %></div>
                    <div class="CopId">文本序列号：<%Response.Write(PaText.text_id); %></div>
                </div>
                <div class="LabelBox fltL LabelTxt LabelBg RdiuB">

                    <div class="DateLabel"><%Response.Write(indexServ.trsDate(PaText.date_created)); %></div>
                    <div class="PvLabel">阅读 <%Response.Write(PaText.count_pv); %></div>
                    <div class="CommentBtn RdiuB RdiuT">评论 <%Response.Write(PaText.count_comment); %></div>
                    <a onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="LikeBtn RdiuB RdiuT cur"><%Response.Write(PaText.count_like); %></a>
                    <div class="TimeLabel"><%Response.Write(indexServ.trsTime(PaText.date_created)); %></div>
                    <div class="ClassLabel"><%Response.Write(PaText.text_class); %></div>
                    <%foreach (string tag in indexServ.trsTags(PaText.tags))
                        {  %>
                    <div class="TagLabel"><%Response.Write(tag); %></div>
                    <%} %>
                </div>
            </div>
            <div>
                <!-- 推荐文章按钮 -->
                <%LibStructs.PaText rdmPaText = SLS.getTextMain(SLS.rdmTextIndex(rqst_text_id, "post").text_id);%>
                <a onclick="GoUp();showTxt(<%Response.Write(rdmPaText.text_id); %>)" class="RandomBtn fltL cur bSha Tran RdiuB RdiuT MagnB">
                    <%Response.Write(rdmPaText.text_title); %>

                </a>
                <!-- 评论区 -->
                <div class="CommentBox bSha fltL RdiuB RdiuT MagnB">评论将于BETA测试结束后开放</div>
            </div>
            <%}%>
        </div>

    </div>
    <%} %>


    <div onclick="GoUp()" class="GoUpBtn cur bSha fltL">
        <div class="arrowUp"></div>
    </div>

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
        //返回顶部按钮淡入淡出
        $(window).scroll(function () {
            if ($(window).scrollTop() > 1600) {
                $(".GoUpBtn").fadeIn(350);
            }
            else {
                $(".GoUpBtn").fadeOut(350);
            }

        });
    </script>
</asp:Content>
