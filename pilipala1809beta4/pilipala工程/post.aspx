<%@ Page Title="" Language="C#" MasterPageFile="~/PublicPage.master" AutoEventWireup="true" CodeFile="post.aspx.cs" Inherits="post" %>

<asp:Content ID="post_head" ContentPlaceHolderID="head" runat="Server">

    <%Response.Write(Resources.post.headScript); %><!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/post-favicon.ico?crc=306704876" />

    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/post.css?crc=481607909" id="pagesheet" />
    <!-- IE-only CSS -->
    <!--[if lt IE 9]>
    <link rel="stylesheet" type="text/css" href="css/nomq_preview_master___.css?crc=321525473"/>
    <link rel="stylesheet" type="text/css" href="css/nomq_post.css?crc=4109875981" id="nomq_pagesheet"/>
    <![endif]-->
    <!--/*

    */
    -->

</asp:Content>




<asp:Content ID="post_ContentBefore" ContentPlaceHolderID="PrivateContentBefore" runat="Server">

    <!-- 页面服务脚本 -->
    <asp:ScriptManager ID="postScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/App_Services/postService.asmx" />
        </Services>
    </asp:ScriptManager>
    <!-- 页面服务脚本 -->

</asp:Content>

<asp:Content ID="post_Content" ContentPlaceHolderID="PrivateContent" runat="Server">

    <!-- 菜单 -->
    <div class="clearfix grpelem" id="pu9951">
        <!-- column -->
        <div class="rounded-corners clearfix colelem" id="u9951">
            <!-- column -->
            <a class="nonblock nontext transition museBGSize rounded-corners colelem shared_content" id="u9953" href="index.aspx" data-content-guid="u9953_content">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </a>
            <div class="clearfix colelem shared_content" id="u9952-4" data-content-guid="u9952-4_content">
                <!-- content -->
                <p>THAUMY的博客</p>
            </div>
        </div>
        <div class="clearfix colelem" id="u9945">
            <!-- group -->
            <div class="clearfix grpelem" id="u9956-4">
                <!-- content -->
                <p>正在产生付诸实际的价值</p>
            </div>
        </div>
        <div class="gradient colelem" id="u9950">
            <!-- simple frame -->
        </div>
        <%foreach (StdLib.postData menu_pD in serverWb.idxMenuPost_get())
            { %>
        <div class="clearfix colelem" id="u9949">
            <!-- group -->

            <!-- 给菜单div添加链接 -->
            <a href="post.aspx?post_id=<%Response.Write(menu_pD.post_id); %>">
                <div class="clearfix grpelem" id="u10197">
                    <!-- group -->
                    <div class="transition clearfix grpelem" id="u9954-4">
                        <!-- content -->
                        <div id="u9954-3">
                            <p><%Response.Write(menu_pD.post_title); %></p>
                        </div>
                    </div>
                    <div class="museBGSize grpelem" id="u9955">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                </div>
            </a>
        </div>
        <%} %>
        <div class="rounded-corners clearfix colelem" id="u9946">
            <!-- column -->
            <div class="clearfix colelem" id="u9947-4">
                <!-- content -->
                <p>2016—2018©Thaumy’s Blog</p>
            </div>
            <div class="clearfix colelem" id="u9948-4">
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
        <div class="shadow rounded-corners clearfix colelem" id="u8882">
            <!-- column -->
            <div class="position_content" id="u8882_position_content">
                <div class="clearfix colelem" id="pu8685">
                    <!-- group -->


                    <!-- 普通条带 -->
                    <div class="museBGSize grpelem shared_content" id="u8685" data-content-guid="u8685_content" runat="server">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>



                    <div class="clearfix grpelem shared_content" id="u8883-4" data-content-guid="u8883-4_content">
                        <!-- content -->
                        <p><%Response.Write(postData.post_title); %></p>
                    </div>
                    <div class="clearfix grpelem shared_content" id="u8884-4" data-content-guid="u8884-4_content">
                        <!-- content -->
                        <p><%Response.Write(postData.post_summary); %></p>
                    </div>


                    <!-- READONLY条带 -->
                    <div class="museBGSize grpelem shared_content" id="u10097" data-content-guid="u10097_content" runat="server">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>

                </div>
                <div class="clearfix colelem shared_content" id="u8885-14" data-content-guid="u8885-14_content">
                    <!-- content -->
                    <div id="u8885-13">
                        <%Response.Write(postData.post_content); %>
                    </div>
                </div>
                <div class="rounded-corners clearfix colelem" id="u8886">
                    <!-- group -->
                    <div class="clearfix grpelem shared_content" id="u8887-4" data-content-guid="u8887-4_content">
                        <!-- content -->
                        <p>此文章由ThaumyCheng最后维护于<%Response.Write(postData.date_changed); %></p>
                    </div>
                    <div class="clearfix grpelem shared_content" id="u8888-4" data-content-guid="u8888-4_content">
                        <!-- content -->
                        <p>文章序列号：<%Response.Write(postData.post_id); %></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix colelem" id="pu10054">
            <!-- group -->
            <div class="shadow rounded-corners grpelem shared_content" id="u10054" data-content-guid="u10054_content">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix grpelem shared_content" id="u10055-5" data-content-guid="u10055-5_content">
                <!-- content -->
                <ul class="list0 nls-None" id="u10055-3">
                    <li><%Response.Write(postData.post_archive); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem" id="u10056-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10056-3">
                    <li>留言<%Response.Write(pluginData.count_review); %>条</li>
                </ul>
            </div>



            <div class="clearfix grpelem shared_content" id="u10057-5" data-content-guid="u10057-5_content">
                <!-- content -->
                <asp:UpdatePanel ID="UpdatePanel_countLike" runat="server">
                    <ContentTemplate>
                        <ul class="list0 nls-None" id="u10057-3">

                            <li id="countLike" runat="server"></li>

                        </ul>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



            <div class="clearfix grpelem" id="u10058-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10058-3">
                    <li><%Response.Write(pluginData.tagA); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem shared_content" id="u10059-5" data-content-guid="u10059-5_content">
                <!-- content -->
                <ul class="list0 nls-None" id="u10059-3">
                    <li><%Response.Write(webioBridge.timeToStr(postData.date_created)); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem shared_content" id="u10060-5" data-content-guid="u10060-5_content">
                <!-- content -->
                <ul class="list0 nls-None" id="u10060-3">



                    <li id="countRead" runat="server"></li>



                </ul>
            </div>
            <div class="clearfix grpelem" id="u10061-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10061-3">
                    <li><%Response.Write(pluginData.tagB); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem" id="u10062-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10062-3">
                    <li><%Response.Write(pluginData.tagC); %></li>
                </ul>
            </div>
        </div>

        <!-- 反馈栏 -->
        <div class="shadow rounded-corners clearfix colelem" id="u8679">
            <!-- column -->
            <div class="clearfix colelem" id="pu8680-4">
                <!-- group -->
                <div class="clearfix grpelem shared_content" id="u8680-4" data-content-guid="u8680-4_content">
                    <!-- content -->
                    <p>此文章是否对您有帮助？</p>
                </div>
                <a class="nonblock nontext anim_swing transition clearfix grpelem shared_content" id="u10224" href="post.aspx#review" data-content-guid="u10224_content">
                    <!-- group -->
                    <div class="transition clearfix grpelem" id="u8682-5">
                        <!-- content -->
                        <ul class="list0 nls-None" id="u8682-3">
                            <li>帮助我们改进</li>
                        </ul>
                    </div>
                </a>


                <asp:UpdatePanel ID="UpdatePanel_likeBtn" runat="server">
                    <ContentTemplate>
                        <!-- 支持文章按钮 -->
                        <a id="likeBtn" onserverclick="likeBtn_Click" runat="server">
                            <div class="transition clearfix grpelem shared_content" id="u10230" data-content-guid="u10230_content">
                                <!-- group -->
                                <div class="transition clearfix grpelem" id="u8681-5">
                                    <!-- content -->
                                    <ul class="list0 nls-None" id="u8681-3">
                                        <!-- 支持文章按钮文本 -->
                                        <li id="txt_likeBtn" runat="server"></li>
                                    </ul>
                                </div>
                            </div>
                        </a>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="likeBtn" EventName="ServerClick" />
                    </Triggers>
                </asp:UpdatePanel>



            </div>


            <!-- 随机浏览文章按钮 -->
            <a id="randomPostBtn" runat="server">
                <div class="transition rounded-corners clearfix colelem" id="u10209">
                    <!-- group -->
                    <div class="transition clearfix grpelem shared_content" id="u8683-4" data-content-guid="u8683-4_content">
                        <!-- content -->
                        <div id="u8683-3">
                            <!-- 随机浏览文章按钮文本 -->
                            <p id="txt_randomPostBtn" runat="server"></p>
                        </div>
                    </div>
                    <div class="museBGSize grpelem" id="u8684">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                </div>
            </a>
        </div>
        <!-- 反馈栏 -->

        <!-- 评论栏 -->
        <div class="shadow rounded-corners clearfix colelem" id="u8698">
            <!-- group -->
            <div class="clearfix grpelem shared_content" id="u8699-4" data-content-guid="u8699-4_content">
                <!-- content -->
                <p>评论</p>
            </div>
            <div class="museBGSize grpelem shared_content" id="u8701" data-content-guid="u8701_content">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix grpelem shared_content" id="u8703-4" data-content-guid="u8703-4_content">
                <!-- content -->
                <p>PinnPinnPinnPinnPinnPinn*此功能尚待开发</p>
            </div>
            <div class="rounded-corners clearfix grpelem shared_content" id="u8702-4" data-content-guid="u8702-4_content">
                <!-- content -->
                <p>99F</p>
            </div>
            <div class="clearfix grpelem shared_content" id="u8704-5" data-content-guid="u8704-5_content">
                <!-- content -->
                <p id="u8704-3"><span id="u8704">源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像!</span><span id="u8704-2">*此功能尚待开发</span></p>
            </div>
            <div class="museBGSize grpelem shared_content" id="u8710" data-content-guid="u8710_content">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
        </div>
        <div class="shadow rounded-corners clearfix colelem" id="u8705">
            <!-- group -->
            <div class="clearfix grpelem" id="preview">
                <!-- column -->
                <a class="anchor_item colelem shared_content" id="review" data-sizepolicy="fixed" data-pintopage="page_fluidx" data-content-guid="review_content"></a>
                <div class="rounded-corners clearfix colelem shared_content" id="u8707-5" data-content-guid="u8707-5_content">
                    <!-- content -->
                    <div id="u8707-4">
                        <p>您的昵称*此功能尚待开发</p>
                    </div>
                </div>
                <div class="rounded-corners clearfix colelem shared_content" id="u8709-4" data-content-guid="u8709-4_content">
                    <!-- content -->
                    <div id="u8709-3">
                        <p>发表评论*此功能尚待开发</p>
                    </div>
                </div>
            </div>
            <div class="clearfix grpelem" id="pu8708-4">
                <!-- group -->
                <div class="rounded-corners clearfix grpelem shared_content" id="u8708-4" data-content-guid="u8708-4_content">
                    <!-- content -->
                    <p>验证</p>
                </div>
                <div class="transition rounded-corners clearfix grpelem shared_content" id="u10221" data-content-guid="u10221_content">
                    <!-- group -->
                    <div class="transition clearfix grpelem" id="u8706-5">
                        <!-- content -->
                        <div id="u8706-4">
                            <ul class="list0 nls-None" id="u8706-3">
                                <li>评论</li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="museBGSize grpelem shared_content" id="u8711" data-content-guid="u8711_content">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
        </div>
        <!-- 评论栏 -->
    </div>
    <!-- 文列区 -->

    <!-- 构架类元素位置 -->
    <div class="clearfix grpelem" id="pu10191">
        <!-- group -->
        <div id="u10191-wrapper">
            <a class="nonblock nontext anim_swing transition rounded-corners" id="u10191" href="post.aspx#top">
                <!-- simple frame -->
            </a>
        </div>
        <div id="u10192-wrapper">
            <a class="nonblock nontext anim_swing museBGSize" id="u10192" href="post.aspx#top">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </a>
        </div>
    </div>
    <div class="verticalspacer" data-offset-top="991" data-content-above-spacer="991" data-content-below-spacer="308" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
    <!-- 构架类元素位置 -->

</asp:Content>

<asp:Content ID="post_ContentAfter" ContentPlaceHolderID="PrivateContentAfter" runat="Server">

    <div class="breakpoint" id="bp_320" data-max-width="320">
        <!-- responsive breakpoint node -->
        <div class="clearfix borderbox temp_no_id" data-orig-id="page">
            <!-- column -->
            <div class="clearfix colelem temp_no_id" data-orig-id="ptop">
                <!-- group -->
                <span class="anchor_item grpelem placeholder" data-placeholder-for="top_content">
                    <!-- placeholder node -->
                </span>
                <div class="rounded-corners clearfix grpelem temp_no_id" data-orig-id="u9951">
                    <!-- group -->
                    <span class="nonblock nontext transition museBGSize rounded-corners grpelem placeholder" data-placeholder-for="u9953_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u9952-4_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="rounded-corners colelem" id="u11067">
                <!-- simple frame -->
            </div>
            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8882">
                <!-- column -->
                <div class="clearfix colelem temp_no_id" data-orig-id="pu8685">
                    <!-- group -->
                    <span class="museBGSize grpelem placeholder" data-placeholder-for="u8685_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u8883-4_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="museBGSize grpelem placeholder" data-placeholder-for="u10097_content">
                        <!-- placeholder node -->
                    </span>
                </div>
                <span class="clearfix colelem placeholder" data-placeholder-for="u8884-4_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix colelem placeholder" data-placeholder-for="u8885-14_content">
                    <!-- placeholder node -->
                </span>
                <div class="rounded-corners clearfix colelem temp_no_id" data-orig-id="u8886">
                    <!-- column -->
                    <span class="clearfix colelem placeholder" data-placeholder-for="u8887-4_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="clearfix colelem placeholder" data-placeholder-for="u8888-4_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="clearfix colelem temp_no_id" data-orig-id="pu10054">
                <!-- group -->
                <span class="shadow rounded-corners grpelem placeholder" data-placeholder-for="u10054_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u10055-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u10057-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u10059-5_content">
                    <!-- placeholder node -->
                </span>
                <span class="clearfix grpelem placeholder" data-placeholder-for="u10060-5_content">
                    <!-- placeholder node -->
                </span>
            </div>
            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8679">
                <!-- column -->
                <span class="clearfix colelem placeholder" data-placeholder-for="u8680-4_content">
                    <!-- placeholder node -->
                </span>
                <span class="transition clearfix colelem placeholder" data-placeholder-for="u10230_content">
                    <!-- placeholder node -->
                </span>
                <span class="nonblock nontext anim_swing transition clearfix colelem placeholder" data-placeholder-for="u10224_content">
                    <!-- placeholder node -->
                </span>
                <div class="transition rounded-corners clearfix colelem temp_no_id" data-orig-id="u10209">
                    <!-- group -->
                    <span class="transition clearfix grpelem placeholder" data-placeholder-for="u8683-4_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8698">
                <!-- column -->
                <div class="clearfix colelem" id="pu8699-4">
                    <!-- group -->
                    <span class="clearfix grpelem placeholder" data-placeholder-for="u8699-4_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="museBGSize grpelem placeholder" data-placeholder-for="u8710_content">
                        <!-- placeholder node -->
                    </span>
                </div>
                <div class="clearfix colelem" id="pu8701">
                    <!-- group -->
                    <span class="museBGSize grpelem placeholder" data-placeholder-for="u8701_content">
                        <!-- placeholder node -->
                    </span>
                    <div class="clearfix grpelem" id="pu8703-4">
                        <!-- column -->
                        <span class="clearfix colelem placeholder" data-placeholder-for="u8703-4_content">
                            <!-- placeholder node -->
                        </span>
                        <span class="rounded-corners clearfix colelem placeholder" data-placeholder-for="u8702-4_content">
                            <!-- placeholder node -->
                        </span>
                    </div>
                </div>
                <span class="clearfix colelem placeholder" data-placeholder-for="u8704-5_content">
                    <!-- placeholder node -->
                </span>
            </div>
            <div class="shadow rounded-corners clearfix colelem temp_no_id" data-orig-id="u8705">
                <!-- column -->
                <div class="position_content" id="u8705_position_content">
                    <div class="clearfix colelem" id="pu8707-5">
                        <!-- group -->
                        <span class="rounded-corners clearfix grpelem placeholder" data-placeholder-for="u8707-5_content">
                            <!-- placeholder node -->
                        </span>
                        <span class="rounded-corners clearfix grpelem placeholder" data-placeholder-for="u8708-4_content">
                            <!-- placeholder node -->
                        </span>
                        <span class="anchor_item grpelem placeholder" data-placeholder-for="review_content">
                            <!-- placeholder node -->
                        </span>
                        <span class="museBGSize grpelem placeholder" data-placeholder-for="u8711_content">
                            <!-- placeholder node -->
                        </span>
                    </div>
                    <span class="rounded-corners clearfix colelem placeholder" data-placeholder-for="u8709-4_content">
                        <!-- placeholder node -->
                    </span>
                    <span class="transition rounded-corners clearfix colelem placeholder" data-placeholder-for="u10221_content">
                        <!-- placeholder node -->
                    </span>
                </div>
            </div>
            <div class="verticalspacer" data-offset-top="1556" data-content-above-spacer="1555" data-content-below-spacer="61" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
        </div>
    </div>
    <%Response.Write(Resources.post.bodyScript); %><!-- body脚本输出 -->

</asp:Content>

