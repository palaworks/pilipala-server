<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="index.aspx.cs"
    Inherits="PILIPALA.pala_content.themes.field.index"
    Debug="true"
    MasterPageFile="~/pala_content/index.master" %>

<%@ Import Namespace="PILIPALA.pala_services" %>
<%@ Import Namespace="PILIPALA.pala_content.themes.field.field_service" %>

<asp:Content ID="head" ContentPlaceHolderID="indexHead" runat="Server">

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
    <asp:scriptmanager id="indexScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/pala_services/SLS.asmx" />
        </Services>
    </asp:scriptmanager>
    <!-- ScriptManager -->

    <div class="main">

        <%if (Request.QueryString["guide"] == "1")
            {%>
        <div class="NaviCol rT rB">
            <div class="NaviHead blk50 rT bSha L">

                <a onclick="GoUp();goHome()">
                    <div class="Avatar bSha cur"></div>
                    <div class="SiteName cur">THAUMY的小破站</div>
                    <div class="SiteIntro"><span class="hiWord"></span></div>
                    <script src="ui_js/hiWord.js" type="text/javascript"></script>
                </a>
            </div>
            <div class="ShowNaviBtn blk55 Tran cur">
            </div>
            <div class="NaviCardBox blk50 rB bSha L">

                <%foreach (LibStructs.PaText PaText in List_text_index_page)
                    {
                        LibStructs.PaText PaTextMain = SLS.getTextMain(PaText.text_id);
                %>
                <a onclick="GoUp();showTxt(<%Response.Write(PaTextMain.text_id); %>)" class="NaviCard cur rT rB flx Tran">
                    <div class="NaviCardTitle flxC"><%Response.Write(PaTextMain.text_title); %></div>
                </a>
                <%} %>

                <div class="CoMark">
                    Thaumy的博客©2016-2019保留所有权利<br>
                    基于pilipala构建<br>
                    Field Theme By Thaumy<br>
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



            <div class="LabelBox L rB rT MagnB">
                <div class="TxtBox pSha bSha L rB rT">
                    <%if (PaText.cover_url != "")
                        {  %>
                    <img onclick="showTxt(<%Response.Write(PaText.text_id); %>)" class="rT pic-max" src="<%Response.Write(PaText.cover_url); %>" />
                    <%} %>
                    <div class="<%Response.Write(indexServ.qianStyle(PaText.strip_color)); %>"></div>
                    <div class="TxtBoxFrame">
                        <a onclick="showTxt(<%Response.Write(PaText.text_id); %>)">
                            <div onclick="GoUp()" class="TxtTitle cur"><%Response.Write(PaText.text_title); %></div>
                            <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                        </a>
                    </div>
                </div>
                <div class="DateLabel"><%Response.Write(indexServ.trsDate(PaText.date_created)); %></div>
                <div class="PvLabel">阅读 <%Response.Write(PaText.count_pv); %></div>
                <div class="CommentLabel rB rT">评论 <%Response.Write(PaText.count_comment); %></div>
                <div onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="LikeLabel rB rT cur">点赞 <%Response.Write(PaText.count_like); %></div>
                <div class="TimeLabel"><%Response.Write(indexServ.trsTime(PaText.date_created)); %></div>
                <div class="ClassLabel"><%Response.Write(PaText.text_class); %></div>
                <%foreach (string tag in indexServ.trsTags(PaText.tags))
                    {  %>
                <div class="TagLabel"><%Response.Write(tag); %></div>
                <%} %>
            </div>



            <div class="TxtBox pSha bSha L rB rT MagnB">
            </div>

            <%} %>
            <a onclick="loadTxt()" class="LoadPostBtn blk50 cur bSha L Tran rB rT"></a>
            <%} %>



            <% if (List_text_index_post == null)
                {%>
            <div class="TxtBox pSha bSha L rB rT MagnB">
                <%if (PaText.cover_url != "")
                    {%>
                <img alt="" class="rT pic-max" src="<%Response.Write(PaText.cover_url); %>" />
                <%} %>

                <div class="<%Response.Write(indexServ.qianStyle(PaText.strip_color)); %>"></div>
                <div class="TxtBoxFrame">
                    <div class="TxtTitle"><%Response.Write(PaText.text_title); %></div>
                    <div class="TxtSummary"><%Response.Write(PaText.text_summary); %></div>
                    <div class="TxtContent pSha"><%Response.Write(PaText.text_content); %></div>
                    <!-- markdown字符转换脚本 -->
                    <script>mkdConvert($(".TxtContent").html());</script>
                </div>


                <%LibStructs.PaText nextText = SLS.getTextMain(SLS.nextTextID(rqst_text_id));%>
                <%if (nextText.text_title != null)
                    { %>
                <div onclick="GoUp();showTxt(<%Response.Write(nextText.text_id); %>)" class="nxtBtn cur L"><%Response.Write(nextText.text_title); %></div>
                <%} %>
                <%LibStructs.PaText prevText = SLS.getTextMain(SLS.prevTextID(rqst_text_id));%>
                <%if (prevText.text_title != null)
                    { %>
                <div onclick="GoUp();showTxt(<%Response.Write(prevText.text_id); %>)" class="pvsBtn cur R"><%Response.Write(prevText.text_title); %></div>
                <%} %>
            </div>

            <div class="CopBox blk50 L rT rB MagnB">
                <div class="CopTime">此文本由 <%Response.Write(PaText.text_editor); %> 最后维护于 <%Response.Write(indexServ.trsDate(PaText.date_changed)); %></div>
                <div class="CopId">文本序列号：<%Response.Write(PaText.text_id); %></div>
                <div class="LabelBox L LabelTxt rT rB bSha">
                    <div class="DateLabel"><%Response.Write(indexServ.trsDate(PaText.date_created)); %></div>
                    <div class="PvLabel">阅读 <%Response.Write(PaText.count_pv); %></div>
                    <div class="CommentBtn rB rT">评论 <%Response.Write(PaText.count_comment); %></div>
                    <a onclick="refre_count_like(<%Response.Write(PaText.text_id); %>)" class="LikeBtn rB rT cur"><%Response.Write(PaText.count_like); %></a>
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
                <a onclick="GoUp();showTxt(<%Response.Write(rdmPaText.text_id); %>)" class="RandomBtn blk50 L cur bSha Tran rB rT MagnB">
                    <%Response.Write(rdmPaText.text_title); %>
                </a>
                <!-- 评论区 -->
                <div class="CommentBox bSha L rB rT MagnB">评论将于BETA测试结束后开放</div>
            </div>
            <%}%>
        </div>

    </div>
    <%} %>


    <div onclick="GoUp()" class="GoUpBtn rB rT blk55 cur bSha L"></div>


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
