<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<%webioBridge wb = new webioBridge(); %>
<%StdLib.postData topPostData = wb.topPost_get(); %>

<!DOCTYPE html>
<html class="nojs html css_verticalspacer" lang="zh-CN">
<head>

    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="generator" content="2018.1.0.386" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <%Response.Write(Resources.index.headScript); %><!-- head脚本输出 -->

    <link rel="shortcut icon" href="images/index-favicon.ico?crc=306704876" />
    <title>Thaumy的博客|又一个码农的家</title>
    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="css/site_global.css?crc=4038565194" />
    <link rel="stylesheet" type="text/css" href="css/index.css?crc=190330108" id="pagesheet" />
    <!--/*

*/
-->
</head>
<body class="always_vert_scroll">

    <div class="clearfix borderbox" id="page">
        <!-- group -->

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

            <%foreach (StdLib.postData menuPostData in wb.menuPostLst_get())
                { %>
            <div class="clearfix colelem" id="u8305">
                <!-- group -->
                <div class="transition clearfix grpelem" id="u8298-4">
                    <!-- content -->
                    <div id="u8298-3">

                        <a href="post.aspx?post_id=<%Response.Write(menuPostData.post_id); %>" style="text-decoration: none; color: white">
                            <p><%Response.Write(menuPostData.post_title); %></p>
                        </a>
                        <!-- 给文本添加链接 -->

                    </div>
                </div>
                <div class="museBGSize grpelem" id="u8276">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
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

        <div class="clearfix grpelem" id="ptop">
            <!-- column -->
            <a class="anchor_item colelem" id="top" data-sizepolicy="fixed" data-pintopage="page_fluidx"></a>
            <div class="clearfix colelem" id="pu8280">
                <!-- group -->
                <div class="shadow rounded-corners grpelem" id="u8280">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
                <div class="museBGSize grpelem" id="u9269" <%Response.Write(wb.stripClr(wb.topPost_get().color_strip)); %>>
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
            </div>
            <div class="shadow rounded-corners clearfix colelem" id="u8301">
                <!-- group -->
                <div class="clearfix grpelem" id="u8285-4">
                    <!-- content -->

                    <a href="post.aspx?post_id=<%Response.Write(wb.topPost_get().post_id); %>" style="text-decoration: none; color: black">
                        <p><%Response.Write(wb.topPost_get().post_title); %></p>
                    </a>
                    <!-- 给文本添加链接 -->

                </div>
                <div class="clearfix grpelem" id="pu8274-5">
                    <!-- column -->
                    <div class="clearfix colelem" id="u8274-5">
                        <!-- content -->
                        <ul class="list0 nls-None" id="u8274-3">
                            <li>阅读<%Response.Write(wb.topPost_get().count_read); %>次</li>
                        </ul>
                    </div>
                    <div class="clearfix colelem" id="u8281-5">
                        <!-- content -->
                        <ul class="list0 nls-None" id="u8281-3">
                            <li>评论<%Response.Write(wb.topPost_get().count_review); %>个</li>
                        </ul>
                    </div>
                </div>
            </div>

            <%foreach (StdLib.postData comnPostData in wb.comnPostLst_get())
                { %>
            <div class="shadow rounded-corners clearfix colelem" id="u8284">
                <!-- column -->
                <div class="clearfix colelem" id="pu8278-4">
                    <!-- group -->
                    <div class="clearfix grpelem" id="u8278-4">
                        <!-- content -->

                        <a href="post.aspx?post_id=<%Response.Write(comnPostData.post_id); %>" style="text-decoration: none; color: black">
                            <p><%Response.Write(comnPostData.post_title); %></p>
                        </a>
                        <!-- 给文本添加链接 -->

                    </div>
                    <div class="museBGSize grpelem" id="u9244" <%Response.Write(wb.stripClr(comnPostData.color_strip)); %>>
                        <!-- content -->
                        <div class="fluid_height_spacer"></div>
                    </div>
                </div>
                <div class="clearfix colelem" id="u8287-4">
                    <!-- content -->
                    <p><%Response.Write(comnPostData.post_summary); %></p>
                </div>
            </div>
            <div class="clearfix colelem" id="pu8294">
                <!-- group -->
                <div class="shadow rounded-corners grpelem" id="u8294">
                    <!-- content -->
                    <div class="fluid_height_spacer"></div>
                </div>
                <div class="clearfix grpelem" id="u8283-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8283-3">
                        <li><%Response.Write(comnPostData.post_archive); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8279-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8279-3">
                        <li>评论<%Response.Write(comnPostData.count_review); %>个</li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8293-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8293-3">
                        <li><%Response.Write(comnPostData.count_like); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8299-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8299-3">
                        <li><%Response.Write(comnPostData.tagA); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8286-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8286-3">
                        <li><%Response.Write(wb.timeToStr(comnPostData.date_created)); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8292-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8292-3">
                        <li>阅读<%Response.Write(comnPostData.count_read); %>次</li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8308-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8308-3">
                        <li><%Response.Write(comnPostData.tagB); %></li>
                    </ul>
                </div>
                <div class="clearfix grpelem" id="u8296-5">
                    <!-- content -->
                    <ul class="list0 nls-None" id="u8296-3">
                        <li><%Response.Write(comnPostData.tagC); %></li>
                    </ul>
                </div>
            </div>
            <%} %>
        </div>

        <%Response.Write(Resources.index.backTopBtn); %><!-- 返回顶部Btn输出 -->

        <div class="verticalspacer" data-offset-top="517" data-content-above-spacer="1216" data-content-below-spacer="783" data-sizepolicy="fixed" data-pintopage="page_fixedLeft"></div>
    </div>

    <%Response.Write(Resources.index.bodyScript); %><!-- body脚本输出 -->

</body>
</html>
