<%@ Page Title="" Language="C#" MasterPageFile="~/PublicPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="index_head" ContentPlaceHolderID="head" runat="Server">

    <%Response.Write(Resources.index.headScript); %>
    <!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/index-favicon.ico?crc=306704876" />

    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/index.css?crc=144373659" id="pagesheet" />
    <!-- IE-only CSS -->
    <!--[if lt IE 9]>
    <link rel="stylesheet" type="text/css" href="css/nomq_preview_master___.css?crc=321525473"/>
    <link rel="stylesheet" type="text/css" href="css/nomq_index.css?crc=4140989764" id="nomq_pagesheet"/>
    <![endif]-->
    <!--/*

    */
    -->

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



    <!-- 菜单 -->
    <div class="clearfix grpelem" id="pu8302">
        <!-- column -->
        <div class="rounded-corners clearfix colelem" id="u8302">
            <!-- column -->
            <div class="transition museBGSize rounded-corners colelem shared_content" id="u8304" data-content-guid="u8304_content">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix colelem shared_content" id="u8297-4" data-content-guid="u8297-4_content">
                <!-- content -->
                <p>THAUMY的博客</p>
            </div>
        </div>
        <div class="clearfix colelem" id="u9859">
            <!-- group -->
            <div class="clearfix grpelem" id="u9862-4">
                <!-- content -->
                <p>正在产生付诸实际的价值</p>
            </div>
        </div>
        <div class="gradient colelem" id="u8295">
            <!-- simple frame -->
        </div>
        <%foreach (StdLib.post menuPostData in serverWb.idxMenuPost_get())
            { %>
        <div class="clearfix colelem" id="u8305">
            <!-- group -->
            <!-- 为菜单div添加链接 -->
            <a href="word.aspx?post_id=<%Response.Write(menuPostData.post_id); %>">
                <div class="clearfix grpelem" id="u10200">
                    <!-- group -->
                    <div class="transition clearfix grpelem" id="u10201-4">
                        <!-- content -->
                        <div id="u10201-3">
                            <p><%Response.Write(menuPostData.post_title); %></p>
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
    <!-- 菜单 -->

    <!-- 文列区 -->
    <div class="clearfix grpelem" id="ptop">
        <!-- column -->
        <a class="anchor_item colelem shared_content" id="top" data-sizepolicy="fixed" data-pintopage="page_fluidx" data-content-guid="top_content"></a>

        <!-- 置顶文章 -->
        <div class="clearfix colelem shared_content" id="pu8280" data-content-guid="pu8280_content">
            <!-- group -->
            <div class="shadow museBGSize rounded-corners grpelem" id="u8280">
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
            <div class="clearfix grpelem shared_content" id="u8285-4" data-content-guid="u8285-4_content">
                <!-- content -->
                <!-- 给top文章标题添加链接 -->
                <a href="word.aspx?post_id=<%Response.Write(topPostData.post_id); %>">
                    <p><%Response.Write(topPostData.post_title); %></p>
                </a>
            </div>
            <div class="clearfix grpelem" id="pu8274-5">
                <!-- column -->
                <div class="clearfix colelem shared_content" id="u8274-5" data-content-guid="u8274-5_content">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8274-3">
                        <li>阅读<%Response.Write(topPluginData.count_read); %>次</li>
                    </ul>
                </div>
                <div class="clearfix colelem shared_content" id="u8281-5" data-content-guid="u8281-5_content">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8281-3">
                        <li>评论<%Response.Write(topPluginData.count_review); %>个</li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- 置顶文章 -->

        <!-- 文列更新区 -->
        <asp:UpdatePanel ID="UpdatePanel_comnPstLst" runat="server">
            <ContentTemplate>
                <!-- 在此div输出文章列表 -->
                <div id="comnPstLst" runat="server"></div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="loadBtn" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <!-- 文列更新区 -->

        <!-- 加载文章按钮 -->
        <a id="loadBtn" onserverclick="loadBtn_Click" runat="server">
            <div class="transition rounded-corners clearfix colelem shared_content" id="u10188" data-content-guid="u10188_content">
                <!-- group -->
                <div class="museBGSize grpelem" id="u10165">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
        </a>
    </div>
    <!-- 文列区 -->

    <!-- 构架类元素位置 -->
    <div class="clearfix grpelem" id="pu8290">
        <!-- group -->
        <div id="u8290-wrapper">
            <a class="nonblock nontext anim_swing transition rounded-corners" id="u8290" href="index.aspx#top">
                <!-- simple frame -->
            </a>
        </div>
        <div id="u10103-wrapper">
            <a class="nonblock nontext anim_swing museBGSize" id="u10103" href="index.aspx#top">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </a>
        </div>
    </div>
    <div class="verticalspacer" data-offset-top="586" data-content-above-spacer="585" data-content-below-spacer="714" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
    <!-- 构架类元素位置 -->

</asp:Content>

<asp:Content ID="index_ContentAfter" ContentPlaceHolderID="PrivateContentAfter" runat="Server">

    <div class="breakpoint" id="bp_320" data-max-width="320">
        <!-- responsive breakpoint node -->
        <div class="clearfix borderbox temp_no_id" data-orig-id="page">
            <!-- column -->
            <div class="clearfix colelem temp_no_id" data-orig-id="ptop">
                <!-- group -->
                <span class="anchor_item grpelem placeholder" data-placeholder-for="top_content">
                    <!-- placeholder node -->
                </span>
                <div class="rounded-corners clearfix grpelem temp_no_id" data-orig-id="u8302">
                    <!-- group -->
                    <span class="transition museBGSize rounded-corners grpelem placeholder" data-placeholder-for="u8304_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u8297-4_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="rounded-corners colelem" id="u10592">
                <!-- simple frame -->
            </div>
            <span class="clearfix colelem placeholder" data-placeholder-for="pu8280_content">
                <!-- placeholder node -->
            </span>


            <!-- 文列更新区 -->
            <asp:UpdatePanel ID="UpdatePanel_comnPstLst320" runat="server">
                <ContentTemplate>
                    <!-- 在此div输出文章列表 -->
                    <div id="comnPstLst320" runat="server"></div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="loadBtn320" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- 文列更新区 -->




            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8301">
                <!-- column -->
                <span class="clearfix colelem placeholder" data-placeholder-for="u8285-4_content">
                    <!-- placeholder node -->
                </span>
                <div class="clearfix colelem temp_no_id" data-orig-id="pu8274-5">
                    <!-- group -->
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u8274-5_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u8281-5_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8284">
                <!-- column -->
                <div class="position_content" id="u8284_position_content">
                    <span class="clearfix colelem placeholder" data-placeholder-for="pu8278-4_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix colelem placeholder" data-placeholder-for="u8287-4_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="clearfix colelem temp_no_id" data-orig-id="pu8294">
                <!-- group -->
                <span class="shadow rounded-corners grpelem placeholder" data-placeholder-for="u8294_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u8283-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u8293-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u8286-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u8292-5_content">
                    <!-- placeholder node -->
                </span>
            </div>

            <!-- 加载文章按钮 -->
            <a id="loadBtn320" onserverclick="loadBtn_Click" runat="server">
                <span class="transition rounded-corners clearfix colelem placeholder" data-placeholder-for="u10188_content">
                    <!-- placeholder node -->
                </span>
            </a>

            <div class="verticalspacer" data-offset-top="493" data-content-above-spacer="493" data-content-below-spacer="806" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
        </div>
    </div>
    <%Response.Write(Resources.index.bodyScript); %><!-- body脚本输出 -->

</asp:Content>

