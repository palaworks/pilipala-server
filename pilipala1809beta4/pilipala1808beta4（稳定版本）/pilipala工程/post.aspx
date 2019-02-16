<%@ Page Title="" Language="C#" MasterPageFile="~/PublicPage.master" AutoEventWireup="true" CodeFile="post.aspx.cs" Inherits="post" %>

<asp:Content ID="post_head" ContentPlaceHolderID="head" runat="Server">

    <%Response.Write(Resources.post.headScript); %><!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/post-favicon.ico?crc=306704876" />

    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/post.css?crc=235892803" id="pagesheet" />
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

    <div class="clearfix grpelem" id="pu9951">
        <!-- column -->
        <div class="rounded-corners clearfix colelem" id="u9951">
            <!-- column -->
            <a class="nonblock nontext transition rounded-corners colelem" id="u9953" href="index.aspx">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </a>
            <div class="clearfix colelem" id="u9952-4">
                <!-- content -->
                <p>THAUMY的博客</p>
            </div>
        </div>
        <div class="clearfix colelem" id="u9945">
            <!-- group -->
            <div class="clearfix grpelem" id="u9956-4">
                <!-- content -->
                <p>立即产生付诸实际的价值</p>
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

    <div class="clearfix grpelem" id="pu8882">
        <!-- column -->
        <div class="shadow rounded-corners clearfix colelem" id="u8882">
            <!-- column -->
            <div class="position_content" id="u8882_position_content">
                <div class="clearfix colelem" id="pu8685">
                    <!-- group -->

                    <!-- 普通条带 -->
                    <div class="museBGSize grpelem" id="u8685" runat="server">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>


                    <div class="clearfix grpelem" id="u8883-4">
                        <!-- content -->
                        <p><%Response.Write(postData.post_title); %></p>
                    </div>
                    <div class="clearfix grpelem" id="u8884-4">
                        <!-- content -->
                        <p><%Response.Write(postData.post_summary); %></p>
                    </div>


                    <!-- READONLY条带 -->
                    <div class="museBGSize grpelem" id="u10097" runat="server">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>

                </div>
                <div class="clearfix colelem" id="u8885-14">
                    <!-- content -->
                    <div id="u8885-13">
                        <%Response.Write(postData.post_content); %>
                    </div>
                </div>
                <div class="rounded-corners clearfix colelem" id="u8886">
                    <!-- group -->
                    <div class="clearfix grpelem" id="u8887-4">
                        <!-- content -->
                        <p>此文章由Thaumy最后维护于<%Response.Write(webioBridge.timeToStr(postData.date_changed)); %></p>
                    </div>
                    <div class="clearfix grpelem" id="u8888-4">
                        <!-- content -->
                        <p>文章序列号：<%Response.Write(postData.post_id); %></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix colelem" id="pu10054">
            <!-- group -->
            <div class="shadow rounded-corners grpelem" id="u10054">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix grpelem" id="u10055-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10055-3">
                    <li><%Response.Write(postData.post_archive); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem" id="u10056-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10056-3">
                    <li>评论<%Response.Write(pluginData.count_review); %>个</li>
                </ul>
            </div>



            <div class="clearfix grpelem" id="u10057-5">
                <!-- content -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <div class="clearfix grpelem" id="u10059-5">
                <!-- content -->
                <ul class="list0 nls-None" id="u10059-3">
                    <li><%Response.Write(webioBridge.timeToStr(postData.date_created)); %></li>
                </ul>
            </div>
            <div class="clearfix grpelem" id="u10060-5">
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
        <div class="shadow rounded-corners clearfix colelem" id="u8679">
            <!-- column -->
            <div class="clearfix colelem" id="pu8680-4">
                <!-- group -->
                <div class="clearfix grpelem" id="u8680-4">
                    <!-- content -->
                    <p>此文章是否对您有帮助？</p>
                </div>
                <a class="nonblock nontext anim_swing transition clearfix grpelem" id="u10224" href="post.aspx#review">
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
                            <div class="transition clearfix grpelem" id="u10230">
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
                    <div class="transition clearfix grpelem" id="u8683-4">
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
        <div class="shadow rounded-corners clearfix colelem" id="u8698">
            <!-- group -->
            <div class="clearfix grpelem" id="u8699-4">
                <!-- content -->
                <p>评论</p>
            </div>
            <div class="grpelem" id="u8701">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
            <div class="clearfix grpelem" id="u8703-4">
                <!-- content -->
                <p>PinnPinnPinnPinnPinnPinn*此功能尚待开发</p>
            </div>
            <div class="rounded-corners clearfix grpelem" id="u8702-4">
                <!-- content -->
                <p>99F</p>
            </div>
            <div class="clearfix grpelem" id="u8704-5">
                <!-- content -->
                <p id="u8704-3"><span id="u8704">源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像!</span><span id="u8704-2">*此功能尚待开发</span></p>
            </div>
            <div class="museBGSize grpelem" id="u8710">
                <!-- content -->
                <div class="fluid_height_spacer"></div>
            </div>
        </div>
        <div class="shadow rounded-corners clearfix colelem" id="u8705">
            <!-- group -->
            <div class="clearfix grpelem" id="preview">
                <!-- column -->
                <a class="anchor_item colelem" id="review" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>
                <div class="rounded-corners clearfix colelem" id="u8707-5">
                    <!-- content -->
                    <div id="u8707-4">
                        <p>您的昵称*此功能尚待开发</p>
                    </div>
                </div>
                <div class="rounded-corners clearfix colelem" id="u8709-4">
                    <!-- content -->
                    <div id="u8709-3">
                        <p>发表评论*此功能尚待开发</p>
                    </div>
                </div>
            </div>
            <div class="clearfix grpelem" id="pu8708-4">
                <!-- group -->
                <div class="rounded-corners clearfix grpelem" id="u8708-4">
                    <!-- content -->
                    <p>验证</p>
                </div>
                <div class="transition rounded-corners clearfix grpelem" id="u10221">
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
                <div class="museBGSize grpelem" id="u8711">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
        </div>
    </div>

    <%Response.Write(Resources.post.backTopBtn); %><!-- 返回顶部Btn输出 -->
    <a class="anchor_item grpelem" id="top" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>

    <div class="verticalspacer" data-offset-top="991" data-content-above-spacer="991" data-content-below-spacer="308" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>

</asp:Content>

<asp:Content ID="post_ContentAfter" ContentPlaceHolderID="PrivateContentAfter" runat="Server">

    <%Response.Write(Resources.post.bodyScript); %><!-- body脚本输出 -->

</asp:Content>

