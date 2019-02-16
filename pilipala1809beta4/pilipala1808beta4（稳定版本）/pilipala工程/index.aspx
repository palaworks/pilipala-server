<%@ Page Title="" Language="C#" MasterPageFile="~/PublicPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="index_head" ContentPlaceHolderID="head" runat="Server">

    <%Response.Write(Resources.index.headScript); %>
    <!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/index-favicon.ico?crc=306704876" />

    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/index.css?crc=61655108" id="pagesheet" />

</asp:Content>




<asp:Content ID="index_ContentBefore" ContentPlaceHolderID="PrivateContentBefore" runat="Server">

    <!-- 页面服务脚本 -->
    <asp:ScriptManager ID="indexScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/App_Services/indexService.asmx" />
        </Services>
    </asp:ScriptManager>
    <!-- 页面服务脚本 -->

</asp:Content>

<asp:Content ID="index_Content" ContentPlaceHolderID="PrivateContent" runat="Server">

    <div class="clearfix grpelem" id="pu8302">
        <!-- column -->
        <div class="rounded-corners clearfix colelem" id="u8302">
            <!-- column -->
            <div class="transition rounded-corners colelem" id="u8304">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix colelem" id="u8297-4">
                <!-- content -->
                <p>THAUMY的博客</p>
            </div>
        </div>
        <div class="clearfix colelem" id="u9859">
            <!-- group -->
            <div class="clearfix grpelem" id="u9862-4">
                <!-- content -->
                <p>立即产生付诸实际的价值</p>
            </div>
        </div>
        <div class="gradient colelem" id="u8295">
            <!-- simple frame -->
        </div>

        <%foreach (StdLib.postData menuPostData in serverWb.idxMenuPost_get())
            { %>
        <div class="clearfix colelem" id="u8305">
            <!-- group -->
            <!-- 给菜单div添加链接 -->
            <a href="post.aspx?post_id=<%Response.Write(menuPostData.post_id); %>">
                <div class="clearfix grpelem" id="u10200">
                    <!-- group -->
                    <div class="transition clearfix grpelem" id="u10201-4">
                        <!-- content -->
                        <div id="u10201-3">
                            <p>
                                <%Response.Write(menuPostData.post_title); %>
                            </p>
                        </div>
                    </div>
                    <div class="museBGSize grpelem" id="u10202">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                </div>
            </a>
        </div>
        <%} %>

        <div class="rounded-corners clearfix colelem" id="u8300">
            <!-- column -->
            <div class="clearfix colelem" id="u8288-4">
                <!-- content -->
                <p>2016—2018©Thaumy’s Blog</p>
            </div>
            <div class="clearfix colelem" id="u8277-4">
                <!-- content -->
                <p>保留所有权利</p>
            </div>
        </div>
    </div>

    <%Response.Write(Resources.index.backTopBtn); %><!-- 返回顶部Btn输出 -->

    <a class="anchor_item grpelem" id="top" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>

    <div class="clearfix grpelem" id="ppu8280">
        <!-- column -->
        <div class="clearfix colelem" id="pu8280">
            <!-- group -->
            <div class="shadow rounded-corners grpelem" id="u8280">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>

            <!-- 设置top文章的条带样式 -->
            <div class="museBGSize grpelem" id="u9269" <%Response.Write("style=\"" + webioBridge.stripStyle(topPluginData.color_strip) + "\"");%>>
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>

        </div>
        <div class="shadow rounded-corners clearfix colelem" id="u8301">
            <!-- group -->
            <div class="clearfix grpelem" id="u8285-4">
                <!-- content -->
                <!-- 给top文章标题添加链接 -->
                <a href="post.aspx?post_id=<%Response.Write(topPostData.post_id); %>">
                    <p><%Response.Write(topPostData.post_title); %></p>
                </a>
            </div>
            <div class="clearfix grpelem" id="pu8274-5">
                <!-- column -->
                <div class="clearfix colelem" id="u8274-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8274-3">
                        <li>阅读<%Response.Write(topPluginData.count_read); %>次</li>
                    </ul>
                </div>
                <div class="clearfix colelem" id="u8281-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8281-3">
                        <li>评论<%Response.Write(topPluginData.count_review); %>个</li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- 未开发完成，目标是利用AJAX实现点击加载文章按钮后加载一定数量没显示的文章 -->
        <asp:UpdatePanel ID="UpdatePanel_comnPstLst" runat="server">
            <ContentTemplate>
                <div id="comnPstLst" runat="server">
                    <!-- 调用输出文章列表的方法在此输出列表数据 -->
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="loadBtn" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>


        <!-- 加载文章按钮 -->
        <a id="loadBtn" onserverclick="loadBtn_Click" runat="server">
            <div class="transition rounded-corners clearfix colelem" id="u10188">
                <!-- group -->
                <div class="museBGSize grpelem" id="u10165">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
        </a>

    </div>
    <div class="verticalspacer" data-offset-top="586" data-content-above-spacer="585" data-content-below-spacer="714" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
</asp:Content>

<asp:Content ID="index_ContentAfter" ContentPlaceHolderID="PrivateContentAfter" runat="Server">

    <%Response.Write(Resources.index.bodyScript); %><!-- body脚本输出 -->

</asp:Content>

