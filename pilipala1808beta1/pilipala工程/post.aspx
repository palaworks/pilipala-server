<%@ Page Language="C#" AutoEventWireup="true" CodeFile="post.aspx.cs" Inherits="post" %>

<%webioBridge wb = new webioBridge(); %>

<%StdLib.postData post_pD = bkstg_pD;/* 将后端获取到的pD传给前端 */ %>

<!DOCTYPE html>
<html class="nojs html css_verticalspacer" lang="zh-CN">
<head>

    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="generator" content="2018.1.0.386" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <%Response.Write(Resources.post.headScript); %><!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/post-favicon.ico?crc=306704876" />
    <title>Thaumy的博客|<%Response.Write(post_pD.post_title); %></title>
    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/site_global.css?crc=4038565194" />
    <link rel="stylesheet" type="text/css" href="css/post.css?crc=61438293" id="pagesheet" />
    <!--/*

*/
-->
</head>
<body class="always_vert_scroll">

    <div class="clearfix borderbox" id="page">
        <!-- group -->
        <div class="clearfix grpelem" id="pu9951">
            <!-- column -->
            <div class="rounded-corners clearfix colelem" id="u9951">
                <!-- column -->
                <a class="nonblock nontext transition rounded-corners colelem" id="u9953" href="index.aspx#top">
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

            <%foreach (StdLib.postData menu_pD in wb.pstlst_menu_get())
                { %>
            <div class="clearfix colelem" id="u9949">
                <!-- group -->
                <div class="transition clearfix grpelem" id="u9954-4">
                    <!-- content -->
                    <div id="u9954-3">

                        <a href="post.aspx?post_id=<%Response.Write(menu_pD.post_id); %>" style="text-decoration: none; color: white">
                            <p><%Response.Write(menu_pD.post_title); %></p>
                        </a>
                        <!-- 给文本添加链接 -->

                    </div>
                </div>
                <div class="museBGSize grpelem" id="u9955">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
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

        <div class="clearfix grpelem" id="ptop">
            <!-- column -->
            <a class="anchor_item colelem" id="top" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>
            <div class="shadow rounded-corners clearfix colelem" id="u8882">
                <!-- column -->
                <div class="position_content" id="u8882_position_content">
                    <div class="clearfix colelem" id="pu8685">
                        <!-- group -->
                        <div class="museBGSize grpelem" id="u8685">
                            <!-- content -->
                            <div class="fluid_height_spacer"></div>
                        </div>
                        <div class="clearfix grpelem" id="u8883-4">
                            <!-- content -->
                            <p><%Response.Write(post_pD.post_title); %></p>
                        </div>
                        <div class="clearfix grpelem" id="u8884-4">
                            <!-- content -->
                            <p><%Response.Write(post_pD.post_summary); %></p>
                        </div>
                        <div class="museBGSize grpelem" id="u10097">
                            <!-- content -->
                            <div class="fluid_height_spacer"></div>
                        </div>
                    </div>
                    <div class="clearfix colelem" id="u8885-14">
                        <!-- content -->
                        <div id="u8885-13">
                            <%Response.Write(post_pD.post_content); %>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix colelem" id="u8886">
                <!-- group -->
                <div class="clearfix grpelem" id="u8887-4">
                    <!-- content -->
                    <p>此文章由Thaumy最后维护于<%Response.Write(wb.pst_time_arrange(post_pD.date_changed)); %></p>
                </div>
                <div class="clearfix grpelem" id="u8888-4">
                    <!-- content -->
                    <p>文章序列号：<%Response.Write(post_pD.post_id); %></p>
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
                        <li><%Response.Write(post_pD.post_archive); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10056-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10056-3">
                        <li><%Response.Write(post_pD.count_review); %>条留言</li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10057-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10057-3">
                        <li><%Response.Write(post_pD.count_like); %>人支持</li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10058-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10058-3">
                        <li><%Response.Write(post_pD.tagA); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10059-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10059-3">
                        <li><%Response.Write(wb.pst_time_arrange(post_pD.date_changed)); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10060-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10060-3">
                        <li><%Response.Write(post_pD.count_browse); %>人走过</li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10061-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10061-3">
                        <li><%Response.Write(post_pD.tagB); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u10062-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u10062-3">
                        <li><%Response.Write(post_pD.tagC); %></li>
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
                    <div class="transition rounded-corners clearfix grpelem" id="u8681-5">
                        <!-- content -->
                        <div id="u8681-4">
                            <ul class="list0 nls-None" id="u8681-3">
                                <li>支持这篇文章</li>
                            </ul>
                        </div>
                    </div>
                    <a class="nonblock nontext anim_swing transition rounded-corners clearfix grpelem" id="u8682-5" href="post.html#review">
                        <!-- content -->
                        <div id="u8682-4">
                            <ul class="list0 nls-None" id="u8682-3">
                                <li>帮助我们改进</li>
                            </ul>
                        </div>
                    </a>
                </div>
                <div class="clearfix colelem" id="pu8683-4">
                    <!-- group -->
                    <div class="transition rounded-corners clearfix grpelem" id="u8683-4">
                        <!-- content -->
                        <div id="u8683-3">
                            <p>这里随机取非本文章的文章标题</p>
                        </div>
                    </div>
                    <div class="museBGSize grpelem" id="u8684">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                </div>
            </div>
            <div class="shadow rounded-corners clearfix colelem" id="u8698">
                <!-- group -->
                <div class="clearfix grpelem" id="u8699-4">
                    <!-- content -->
                    <p>留言板</p>
                </div>
                <div class="grpelem" id="u8701">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
                <div class="clearfix grpelem" id="u8703-4">
                    <!-- content -->
                    <p>PinnPinnPinnPinnPinnPinn</p>
                </div>
                <div class="rounded-corners clearfix grpelem" id="u8702-4">
                    <!-- content -->
                    <p>99F</p>
                </div>
                <div class="clearfix grpelem" id="u8704-4">
                    <!-- content -->
                    <p>源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像源于镜像!</p>
                </div>
                <div class="museBGSize grpelem" id="u8710">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
            <div class="shadow rounded-corners clearfix colelem" id="u8705">
                <!-- group -->
                <div class="clearfix grpelem" id="pu8707-4">
                    <!-- column -->
                    <div class="rounded-corners clearfix colelem" id="u8707-4">
                        <!-- content -->
                        <div id="u8707-3">
                            <p>您的昵称</p>
                        </div>
                    </div>
                    <div class="rounded-corners clearfix colelem" id="u8709-4">
                        <!-- content -->
                        <div id="u8709-3">
                            <p>留言（限制在72个中文字符内）留言（BETA&nbsp; EDI暂时限制在72个中文字符内）留言（限制在72个中文字符内）</p>
                        </div>
                    </div>
                </div>
                <div class="clearfix grpelem" id="pu8706-5">
                    <!-- group -->
                    <div class="transition rounded-corners clearfix grpelem" id="u8706-5">
                        <!-- content -->
                        <div id="u8706-4">
                            <ul class="list0 nls-None" id="u8706-3">
                                <li>留言+1</li>
                            </ul>
                        </div>
                    </div>
                    <div class="rounded-corners clearfix grpelem" id="u8708-4">
                        <!-- content -->
                        <p>验证密钥</p>
                    </div>
                    <div class="museBGSize grpelem" id="u8711">
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                    <a class="anchor_item grpelem" id="review" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>
                </div>
            </div>
        </div>

        <%Response.Write(Resources.post.backTopBtn); %><!-- 返回顶部Btn输出 -->

        <div class="verticalspacer" data-offset-top="1012" data-content-above-spacer="1216" data-content-below-spacer="287" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
    </div>

    <%Response.Write(Resources.post.bodyScript); %><!-- body脚本输出 -->

</body>
</html>
