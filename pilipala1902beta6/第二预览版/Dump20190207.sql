-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: pala_database
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `pala_root`
--

DROP TABLE IF EXISTS `pala_root`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pala_root` (
  `root_id` int(2) NOT NULL AUTO_INCREMENT,
  `root_definer` varchar(255) DEFAULT NULL,
  `site_debug` tinyint(1) NOT NULL DEFAULT '0',
  `site_access` tinyint(1) NOT NULL DEFAULT '1',
  `site_url` varchar(255) NOT NULL DEFAULT '',
  `site_title` varchar(255) DEFAULT NULL,
  `site_summary` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`root_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pala_root`
--

LOCK TABLES `pala_root` WRITE;
/*!40000 ALTER TABLE `pala_root` DISABLE KEYS */;
INSERT INTO `pala_root` VALUES (2,'ThaumyCheng',0,0,'http://www.thaumy.cn','又一个码农的家','Thaumy的博客');
/*!40000 ALTER TABLE `pala_root` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pala_text_index`
--

DROP TABLE IF EXISTS `pala_text_index`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pala_text_index` (
  `text_id` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `text_mode` varchar(8) NOT NULL DEFAULT '\0',
  `text_type` varchar(8) NOT NULL DEFAULT 'comn',
  PRIMARY KEY (`text_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12376 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pala_text_index`
--

LOCK TABLES `pala_text_index` WRITE;
/*!40000 ALTER TABLE `pala_text_index` DISABLE KEYS */;
INSERT INTO `pala_text_index` VALUES (12347,'onshow','post'),(12348,'onshow','post'),(12349,'onshow','post'),(12350,'onshow','post'),(12351,'onshow','post'),(12353,'onshow','post'),(12356,'onshow','post'),(12357,'onshow','post'),(12358,'hidden','post'),(12359,'onshow','post'),(12360,'onshow','post'),(12363,'onshow','post'),(12364,'onshow','post'),(12365,'onshow','post'),(12366,'onshow','post'),(12368,'onshow','page'),(12369,'onshow','page'),(12371,'onshow','page'),(12373,'onshow','page'),(12374,'onshow','page'),(12375,'onshow','page');
/*!40000 ALTER TABLE `pala_text_index` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`pala_database_user`@`localhost`*/ /*!50003 TRIGGER `sync_index.row_insert` AFTER INSERT ON `pala_text_index` FOR EACH ROW begin

INSERT INTO `pala_database`.`pala_text_main` (`text_id`) VALUES (new.text_id);
INSERT INTO `pala_database`.`pala_text_sub` (`text_id`) VALUES (new.text_id);

end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`pala_database_user`@`localhost`*/ /*!50003 TRIGGER `sync_index.row_delete` AFTER DELETE ON `pala_text_index` FOR EACH ROW begin

DELETE FROM `pala_database`.`pala_text_main` WHERE `text_id`= old.text_id ;
DELETE FROM `pala_database`.`pala_text_sub` WHERE `text_id`= old.text_id ;

end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `pala_text_main`
--

DROP TABLE IF EXISTS `pala_text_main`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pala_text_main` (
  `text_id` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `text_title` varchar(32) DEFAULT '标题。。。找不到了。。',
  `text_summary` varchar(64) DEFAULT '奇怪的是。。。连简介都没有了呢。。。w(ﾟДﾟ)w',
  `text_content` longtext,
  PRIMARY KEY (`text_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12376 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pala_text_main`
--

LOCK TABLES `pala_text_main` WRITE;
/*!40000 ALTER TABLE `pala_text_main` DISABLE KEYS */;
INSERT INTO `pala_text_main` VALUES (12347,'AdvancedAjaxPageLoader插件实现全站ajax','标准文章','<span style=\"color: #ff0000;\">注意！此文章已于2017.7.26归档并且不再进行维护，新的通用化pjax网站解决方案已经发布：</span><a href=\"http://www.thaumy.cn/article/%e7%bd%91%e7%ab%99%e9%80%9a%e7%94%a8%e5%8c%96pjax%e8%a7%a3%e5%86%b3%e6%96%b9%e6%a1%88\"><span style=\"color: #00ccff;\"><u>点我查看</u></span></a>\r\n\r\n<span style=\"color: #993366;\">最近查找了wordpress的全站ajax实现方法，发现了很好的解决方案，在这里我将详细讲解插件化ajax的实现方法。</span>\r\n<ul>\r\n 	<li>第一步</li>\r\n</ul>\r\n<blockquote><span style=\"color: #ff9900;\">AdvancedAjaxPageLoader</span>\r\n\r\n一款能让你博客实现AJAX的wordpress插件，并具有多功能支持。</blockquote>\r\n<span style=\"text-decoration: underline; color: #ff9900;\">下载上述插件并安装</span>\r\n<ul>\r\n 	<li>第二步</li>\r\n</ul>\r\n打开你的博客，启动F12开发者检查工具，你会看到类似如下的代码：\r\n<pre class=\"lang:xhtml decode:true \">&lt;div id = \"typical_id\" class = \"typical_class\"&gt;\r\n</pre>\r\n记录代码中<span style=\"color: #ff9900;\">id</span>对应的<span style=\"color: #ff9900;\">typical_id</span>和<span style=\"color: #ff9900;\">class</span>对应的<span style=\"color: #ff9900;\">typical_class</span>\r\n<ul>\r\n 	<li>第三步</li>\r\n</ul>\r\n现在让我们打开AdvancedAjaxPageLoader的设置页面：\r\n<ol>\r\n 	<li><span style=\"color: #ff9900;\">将typical_id填写到Content Element ID栏目中</span></li>\r\n 	<li><span style=\"color: #ff9900;\">将typical_class填写到Search Form CLASS栏目中</span></li>\r\n 	<li>点击保存</li>\r\n</ol>\r\n<ul>\r\n 	<li>第四步</li>\r\n</ul>\r\n重新打开你的wordpress网页，检查AJAX是否开启\r\n<ul>\r\n 	<li>常见错误及解决方案*FAQ</li>\r\n</ul>\r\n<blockquote><span style=\"color: #ff9900;\">1.我的网页没有类似上述代码中的id和class</span>\r\n\r\n<strong>打</strong>开你的主题代码编辑页面，在index.php、header.php和footer.php中寻找类似上述的代码，找到后进行第三步操作。（建议除样式表和模板函数以外都查找一遍）</blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff9900;\">2.我仔细的进行了上述设置，但是仍然没有开启AJAX</span>\r\n\r\n<strong>如</strong>果你真的确定一切操作没有问题，请清除你当前的浏览器缓存及记录再进行查看或用手机查看你的网页是否打开AJAX</blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff9900;\">3.我的网页开启了AJAX，但是有时加载新的页面报错</span>\r\n\r\n<b>可</b>能你的网页服务器响应速度不够快，提升你的服务器速度或优化你的网站，保证被访问的固定链接符合要求，留出足够的系统资源解决该问题（有些硬件老旧的电脑或过老的浏览器貌似也会出现该情况，解决方案在这里不过多解释）</blockquote>\r\n&nbsp;\r\n<ul>\r\n 	<li><span style=\"color: #339966;\">这里感谢Hodpel的博文参考，本文是在其基础上加以完备和修改后发表的。</span></li>\r\n 	<li><span style=\"color: #339966;\">目前我的博客已经采用全站化pjax加载(2017.7.16)，所以本篇文章将不再进行维护。</span></li>\r\n</ul>'),(12348,'StdLib1.03#public发布','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.03湛蓝宣传画(%E5%8E%8B%E7%BC%A9).jpg\" width=\"800\" height=\"716\" />\r\n\r\n<span style=\"color: #ff9900;\">StdLib1.03#public</span>\r\n\r\n<span style=\"color: #ff6600;\">这里是StdLib1.03的发布信息，如果你对它感兴趣并想深入了解它，请仔细浏览这篇技术日志。当然，要是你对我的工程感兴趣，我十分乐意抽出几个小时的时间和你分享源代码和其他的技术细节！</span></blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">StdLib1.03新增内容：</span></li>\r\n</ul>\r\n<ol>\r\n 	<li>优化了很多代码，这使得该应用程序拓展的运行速度提升了一个层次</li>\r\n 	<li>更新了Srr矩阵加密算法，现在你可以根据相应的接口规则自定义密码、字典集、密钥。</li>\r\n 	<li>增添了Stf_if源信息，实例化它看看您所使用的StdLib版本拥有哪些特征！</li>\r\n</ol>\r\n</blockquote>\r\n&nbsp;\r\n\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">以上是StdLib1.03的更新介绍，下面让我们来看看如何约束你的代码来使用StdLib！</span></li>\r\n</ul>\r\n</blockquote>\r\n&nbsp;\r\n<blockquote>StdLib的新型Srr算法有新的接口规则，你需要传递三个参数来完成接口。\r\n<pre class=\"lang:c# decode:true\" title=\"CSHARP_CODE\">Console.WriteLine(e.srr(i1, i2, i3));//例如这样</pre>\r\n</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">我们来看一个简短的例程，了解StdLib的SRR具体实现方法：</span></li>\r\n</ul>\r\n<pre class=\"lang:c# decode:true\">StdLib.SrrLib e = new SrrLib();\r\n\r\n            int[] i1 = { 7, 8, 9, 3, 2, 1, 6, 5, 4, 7 };\r\n            int[] i2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };\r\n            int[] i3 = { 1, 2, 4, 6, 2, 6, 5, 2, 8, 0 };\r\n            \r\n            Console.WriteLine(e.srr(i1, i2, i3));//传递三个参数，返回组坐标值</pre>\r\n<ul>\r\n 	<li><span style=\"color: #993366;\">技术参数————</span></li>\r\n</ul>\r\n<span style=\"color: #993366;\">i1<span style=\"color: #339966;\">//</span></span>加密密码，整型数列，Length=10\r\n\r\n<span style=\"color: #993366;\">i2<span style=\"color: #339966;\">//</span></span>字典集，整型数列，Length=10\r\n\r\n<span style=\"color: #993366;\">i3<span style=\"color: #339966;\">//</span></span>被加密数列，整型数列，Length=10</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">SRR实现过程及其内部接口：</span></li>\r\n</ul>\r\n<pre class=\"lang:c# decode:true\">public string srr\r\n\r\n            (\r\n            int[] sar/*密码，10个数*/,\r\n            int[] dic/*int[] dic = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };字典集，一共10个字符*/,\r\n            int[] num/*int[] num = { 8, 4, 5, 5, 5, 3, 7, 9, 2, 9 }; 被加密的10个整数*/\r\n            )\r\n/*传参部分*/\r\n\r\n\r\n\r\nstring outputs = null;//初始化坐标数列\r\n\r\n            for (int p = 0; p &lt; 10; p++)\r\n            {\r\n                outputs = outputs + temp[p];\r\n            }\r\n\r\n            return outputs;//返回坐标数列\r\n//返回</pre>\r\n&nbsp;</blockquote>\r\n好了，以上就是StdLib1.03的技术细节，更多详细内容将发布1.03时更新此文章\r\n<blockquote><span style=\"color: #993366;\">ArcaneinterstellarStudios-Thaumy-1.21</span></blockquote>\r\n\r\n<hr />\r\n\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">StdLib1.03正式版增添的内容</span></li>\r\n</ul>\r\n&nbsp;\r\n<blockquote>StdLib1.03中新增了一个类，实例化它并调用：\r\n\r\n命名空间   <span style=\"color: #ff6600;\">StdLib_inf   <span style=\"color: #000000;\">中的类   <span style=\"color: #ff6600;\">inf   <span style=\"color: #000000;\">新增了一系列参数：</span></span></span></span>\r\n\r\n<span style=\"color: #ff6600;\">Libver_total   <span style=\"color: #339966;\">//总版本，整型(int)，通常为版面头，例如1.132中的1</span></span>\r\n\r\n<span style=\"color: #ff6600;\">Libver_deta   <span style=\"color: #339966;\">//副版本，双精度浮点数(double)，例如1.132中的0.132</span></span>\r\n\r\n<span style=\"color: #ff6600;\">test   <span style=\"color: #339966;\">//测试发行版，布尔值(bool)，若StdLib的版本为测试版，那么该值为true，反之为false</span></span>\r\n\r\n<span style=\"color: #ff6600;\">gobal_Safe   <span style=\"color: #339966;\">//全局安全，布尔值(bool)，是否使用了防破解反编译代码和措施</span></span>\r\n\r\n<span style=\"color: #ff6600;\">gobal_Past   <span style=\"color: #339966;\">//全局兼容，布尔值(bool)，是否全部兼容上一版本</span></span>\r\n\r\n<span style=\"color: #ff6600;\">devInf   <span style=\"color: #339966;\">//开发者信息，字符串(string)</span></span></blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff0000;\">值得注意的是，以上参数类型为只读的私有常量，这使得其成为各个版本StdLib的技术认证。</span></blockquote>\r\n\r\n<hr />\r\n\r\n<blockquote><span style=\"color: #ff00ff;\">StdLib将来还有很长的一段路要走，为了引出更加清晰的实现过程，StdLib会进行重新架构。因此，StdLib1.04将不再支持零点启动器和其他的调用，新的发布版将会在2017.1.27发布。</span></blockquote>\r\n<p style=\"text-align: right;\">以上</p>\r\n<p style=\"text-align: right;\">Thaumy</p>'),(12349,'Stdlib1.04#public更新日志','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.04%E5%AE%A3%E4%BC%A0%E7%94%BB(%E5%8E%8B%E7%BC%A9).jpg\" width=\"900\" height=\"450\" />\r\n\r\n<span style=\"color: #ff9900;\">StdLib1.04#public</span>\r\n\r\n下面是修订后的范例：</blockquote>\r\n<span style=\"color: #ff6600;\">查询StdLib版本信息：</span>\r\n<pre class=\"lang:c# decode:true\">//StdLib版本信息部分\r\n            StdLib_GobalnameSpace.StdInf inf = new StdInf();\r\n            Console.WriteLine(inf.Base_Safe);\r\n            Console.WriteLine(inf.DevEdition);\r\n            Console.WriteLine(inf.Gobal_Compatible);\r\n            Console.WriteLine(inf.PubEdition);\r\n            Console.WriteLine(inf.Stdver_Deta);\r\n            Console.WriteLine(inf.Stdver_Main);</pre>\r\n&nbsp;\r\n<pre class=\"lang:c# decode:true\" title=\"CSHARP\">//矩阵位移算法部分\r\n            StdSrr.Class_Srr srr = new Class_Srr();\r\n            int[] psw = { 7, 8, 7, 6, 6, 7, 3, 2, 1, 9 };\r\n            int[] dic = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };\r\n            int[] num = { 1, 2, 4, 6, 2, 6, 5, 2, 8, 0 };\r\n            int[] temp = (srr.Main_Srr(psw, dic, num));//在这里StdSrr的方法中的开发者调试功能帮助输出了矩阵\r\n\r\n            for(int i = 0; i &lt; 10; i++)\r\n            {\r\n                if (temp[i] &lt; 10)\r\n                {\r\n                    Console.Write(\"[\" + \"0\" + temp[i] + \"]\");//当x值为0时才可能出现输出单数坐标的情况，\r\n                }                                           //单数坐标都小于10，在这里提取单数坐标并在前面加一个0以表示x坐标\r\n                else\r\n                {\r\n                    Console.Write(\"[\" + temp[i] + \"]\");\r\n                }\r\n            }\r\n            Console.WriteLine();//集体换行\r\n\r\n\r\n            //输出矩阵部分\r\n            int[,] NumsTemp = srr.NumList[0];\r\n            for (int i2 = 0; i2 &lt; NumsTemp.GetLength(0); i2++)//循环非泛型数组组的每一行\r\n            {\r\n                for(int i3 = 0; i3 &lt; NumsTemp.GetLength(1); i3++)\r\n                {\r\n                    Console.Write(NumsTemp[i2,i3]);\r\n                }\r\n                Console.WriteLine();\r\n            }</pre>\r\n<span style=\"color: #ff6600;\">另外增加了对MC启动信息的支持：</span>\r\n<pre class=\"lang:c# decode:true \">//MC启动器数据层部分\r\n            StdMcl.Class_Mcl xml = new Class_Mcl();//实例化\r\n\r\n            //输出Inf.xml中的信息\r\n            Console.WriteLine(xml.Sys_Copyright);\r\n            Console.WriteLine(xml.Sys_Memory);\r\n            Console.WriteLine(xml.User_Email);\r\n            Console.WriteLine(xml.User_Name);\r\n            Console.WriteLine(xml.User_PassWord);\r\n\r\n            xml.Mcl_Save(true, 2233, \"hahaha\", \"faq\", \"123\");//保存更改\r\n\r\n            //再次输出检测\r\n            Console.WriteLine(xml.Sys_Copyright);\r\n            Console.WriteLine(xml.Sys_Memory);\r\n            Console.WriteLine(xml.User_Email);\r\n            Console.WriteLine(xml.User_Name);\r\n            Console.WriteLine(xml.User_PassWord);\r\n\r\n            Console.ReadKey();</pre>\r\n<span style=\"color: #ff0000;\">此文章在后期修订，解释不全。</span>'),(12350,'StdLib1.06#public更新日志','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.06%E5%AE%A3%E4%BC%A0%E7%94%BB(%E5%8E%8B%E7%BC%A9).jpg\" width=\"900\" height=\"437\" />\r\n\r\n<span style=\"color: #ff9900;\">StdLib1.06#public</span>\r\n\r\n<span style=\"color: #ff9900;\">经过一周的研究和一周的开发，我对高级算法的理解能力并没有取得多少长进，所以本次S1.06p的更新使用的是我自己研究的算法，我将它成为动下标错位判等算法，其实现原理很简单：</span></blockquote>\r\n<pre class=\"lang:c# decode:true\" title=\"LSS Codes\"><span style=\"color: #000000;\">for (int path = 0; path &lt; a.Length; path++)</span>\r\n            {\r\n                for (int i = 0; i &lt; a.Length; i++)\r\n                {\r\n                    if (i + 1 &lt; a.Length)\r\n                    {\r\n                        int tmp = 0;\r\n                        if (a[i] &gt; a[i + 1])\r\n                        {\r\n                            tmp = a[i];\r\n                            a[i] = a[i + 1];\r\n                            a[i + 1] = tmp;\r\n                        }\r\n                    }\r\n\r\n                }\r\n            }</pre>\r\n<blockquote><span style=\"color: #ff6600;\">这里是调用：</span></blockquote>\r\n<pre class=\"lang:c# decode:true \" title=\"LSS\">#region LSS算法测试\r\n            int[] a = { 1, 65, 6, 723, 847, 5874, 884, 041, 6, 76, 1, 847, 3, 79, 715, 09, 46, 235, 886, 45, 87, 122, 98, 15, 01, 4 };\r\n            foreach(int p in Class_LSS.Main_LSS(a))\r\n            {\r\n                Console.WriteLine(p);\r\n            }\r\n            #endregion</pre>\r\n&nbsp;\r\n\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff6600;\">接着是ANSW2Dcode的更新，ANSW是我在2016年暑假期间写的图像识别模块，不够稳定的它不乏创新性。经过我一个周末的修改，ANSW的实体化程序更正完成，现已集成到1.06中，识别具有特定组织方式的BMP图像，进行解码和编码。其调用规则如下：</span></blockquote>\r\n<pre class=\"lang:c# decode:true \" title=\"ANSW\">#region ANSW测试\r\n\r\n            Class_ANSW aw = new Class_ANSW();\r\n\r\n            #region ANSW解码测试\r\n            Console.WriteLine(aw.DeANSW(@\"L:\\C#项目开发\\StdLib1.06\\answ\"));\r\n            #endregion\r\n\r\n            #region ANSW转码测试\r\n            Bitmap bp = aw.ToANSW(\"4869212069206c6f76652074686520776f726c6421\", @\"L:\\C#项目开发\\StdLib1.06\\source\");\r\n            Image i = bp;\r\n            i.Save(@\"L:\\C#项目开发\\StdLib1.06\\answ.bmp\");\r\n            #endregion\r\n\r\n            //注意：\r\n            //在ANSW解码中，输入流可以为有.bmp后缀的文件名或无后缀的文件名\r\n            //在ANSW转码中，输入与解码规则相同，输出分两种情况如下：\r\n\r\n            //1.流错误输出所指定的流必须带.bmp后缀\r\n            //2.正常输出所指定的流可以为有.bmp后缀文件名或无后缀文件名\r\n\r\n            #endregion</pre>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff6600;\">另外还增加了一个很实用的结构体用于初始化XmlCreater的各种静态方法，其结构如下：</span></blockquote>\r\n<pre class=\"lang:c# decode:true \" title=\"struct XmlInf\">public struct XmlInf\r\n        {\r\n\r\n            #region 结构私有属性\r\n\r\n            private string Path;\r\n            private string InStream;\r\n            private string FileName;\r\n            private string XmlName;\r\n            private string RootName;\r\n            private string NodeName;\r\n            private string AttName;\r\n            private string AttValue;\r\n            private string InnerText;\r\n            private string Type;\r\n\r\n            #endregion\r\n\r\n            #region 属性访问器\r\n\r\n            /// &lt;summary&gt;\r\n            /// 节点地址，如父节点、实节点、子节点的地址，用于XmlCreater类中除reStream、CreateXml方法外的所有方法\r\n            /// &lt;/summary&gt;\r\n            public string path\r\n            {\r\n                get { return Path; }\r\n                set { Path = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 被读取的Xml文档文件流，在初始化时使用，用于XmlCreater类的reStream方法\r\n            /// &lt;/summary&gt;\r\n            public string inStream\r\n            {\r\n                get { return InStream; }\r\n                set { InStream = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 被创建的Xml文档的文件地址，用于XmlCreater类的CreateXml方法\r\n            /// &lt;/summary&gt;\r\n            public string fileName\r\n            {\r\n                get { return FileName; }\r\n                set { FileName = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 被创建的Xml文档的文件名，用于XmlCreater类的CreateXml方法\r\n            /// &lt;/summary&gt;\r\n            public string xmlName\r\n            {\r\n                get { return XmlName; }\r\n                set { XmlName = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 被创建的Xml文档的根元素名，用于XmlCreater类的CreateXml方法\r\n            /// &lt;/summary&gt;\r\n            public string rootName\r\n            {\r\n                get { return RootName; }\r\n                set { RootName = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 节点名，可表示子节点、父节点、新建空\\实节点名，用于XmlCreater类的AddRealNode、AddEmptyNode、RemoveNode方法\r\n            /// &lt;/summary&gt;\r\n            public string nodeName\r\n            {\r\n                get { return NodeName; }\r\n                set { NodeName = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 节点的属性名，用于XmlCreater类的AddRealNode、ReadAtt方法\r\n            /// &lt;/summary&gt;\r\n            public string attName\r\n            {\r\n                get { return AttName; }\r\n                set { AttName = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 节点的属性值，用于XmlCreater类的AddRealNode方法\r\n            /// &lt;/summary&gt;\r\n            public string attValue\r\n            {\r\n                get { return AttValue; }\r\n                set { AttValue = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 节点的子文本，用于XmlCreater类的AddRealNode方法\r\n            /// &lt;/summary&gt;\r\n            public string innerText\r\n            {\r\n                get { return InnerText; }\r\n                set { InnerText = value; }\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 读取类型，可选值有\"_name\"、\"_value\"，用于XmlCreater类的ReadNode方法\r\n            /// &lt;/summary&gt;\r\n            public string type\r\n            {\r\n                get { return Type; }\r\n                set { Type = value; }\r\n            }\r\n\r\n            #endregion\r\n\r\n        }</pre>\r\n<blockquote><span style=\"color: #ff6600;\">这样，在使用XmlCreater类的各种方法时，都可以传递该结构体作为第二重载执行，极大的提升了代码的可读性和维护性，同时也降低了程序的耦合度。</span></blockquote>\r\n&nbsp;\r\n<blockquote>注意：我们把所有命名空间装入了一个名为StdLib1_16的命名空间，这样提升了版本间的辨识度。所以在使用1.06时除了引用该dll以外，应先引用主\"StdLib1_16\"命名空间，再引用子空间。如下：\r\n\r\nusing StdLib1_16;\r\nusing StdLib1_16.StdEct;\r\nusing StdLib1_16.StdDal;</blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff6600;\">我在StdLib的编写过程中遵循的是两个发行版一兼容的模式，例如从1.03开始，1.04与1.05相兼容，1.06与1.07相兼容。这样有利于每个版本的大更新与小更新，及偶数版本更新内容，奇数版本增加优化。</span></blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #99cc00;\">#文章的封插画是我的旧方正电脑的主板，为什么使用它作为插画是因为在清明节的时候我本应该着手1.06版本的开发，可是遭遇了数据丢失，而该主板帮我拯救了数据并恢复了系统。</span></blockquote>'),(12351,'StdLib1.07#public更新日志','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/NewStdLib1.07%E5%AE%A3%E4%BC%A0%E7%94%BB(%E5%8E%8B%E7%BC%A9).jpg\" width=\"630\" height=\"477\" />\r\n\r\n<span style=\"color: #ff9900;\">StdLib1.07#public</span>\r\n\r\n<span style=\"color: #ff6600;\">1. </span>增加了二分法找值算法\r\n<span style=\"color: #ff6600;\">2. </span>更正ANSW算法中字符串的分割机制，现在只能分割10000个字符，解决1.06大编码情况下运算时间长问题\r\n<span style=\"color: #ff6600;\">3. </span>ANSW编码现已支持十六进制大写\r\n\r\n<span style=\"color: #ff6600;\">4. </span>ANSW编/解码现已转为异步多线程处理\r\n<span style=\"color: #ff6600;\">5. </span>ANSW编码增添了第二重载，可根据需要选择传递怎样的参数以编译ANSW\r\n\r\n<span style=\"color: #ff6600;\">6. </span>ANSW编码现在会在右下角空出一个像素，作为图像正确方向的标记\r\n\r\n<span style=\"color: #ff6600;\">7. </span>对Hash/MD5算法做多线程优化\r\n<span style=\"color: #ff6600;\">8. </span>增加了像素化LibLogo</blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff6600;\">下面是测试代码，足够的代码可以了解新内容：</span></blockquote>\r\n&nbsp;\r\n<pre class=\"lang:c# decode:true\">using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nusing StdLib1_17;\r\nusing StdLib1_17.StdLib_GobalnameSpace;\r\nusing StdLib1_17.StdEct;\r\nusing StdLib1_17.StdDal;\r\nusing System.Drawing;\r\nusing System.Windows.Forms;\r\n\r\nnamespace 泛用型控制台测试\r\n{\r\n    class Program\r\n    {\r\n        static void Main(string[] args)\r\n        {\r\n\r\n            #region ANSW TEST\r\n            /*\r\n            string[,] strArray = new string[100, 100];\r\n            string _hex = \"57656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e67732157656c636f6d6520746f2075736520414e53573264436f64652c2020697473206120766572792075736566756c20636f64652c202069742063616e207472616e736c61746520796f757220696e666f726d6174696f6e20696e746f20612073616665203264436f646520424d5020746f2068656c7020796f752073617665207468696e677321\";\r\n            string text = _hex;\r\n            int len = _hex.Length;\r\n            #region 字符串转矩阵处理\r\n            int i = 0;\r\n            for (int a = 1; a &lt; len; a++)\r\n            {\r\n                i++;\r\n                _hex = _hex.Insert(a + i - 1, \" \");\r\n            }\r\n            i = 0;\r\n            for (int y = 0; y &lt; 100; y++)\r\n            {\r\n                for (int x = 0; x &lt; 100; x++)\r\n                {\r\n                    strArray[x, y] = null;\r\n                }\r\n            }\r\n            for (int y = 0; y &lt; 100; y++)\r\n            {\r\n                for (int x = 0; x &lt; 100; x++)\r\n                {\r\n\r\n                    if (i &lt; _hex.Split(\' \').Length)\r\n                    {\r\n                        strArray[x, y] = _hex.Split(\' \')[i];\r\n                    }\r\n                    i++;\r\n                }\r\n            }\r\n            #endregion\r\n\r\n            object obj1 = text;\r\n            object obj2 = _hex;\r\n            object obj3 = strArray;\r\n\r\n            Class_ANSW aw = new Class_ANSW();\r\n\r\n            Image img1 = aw.ToANSW(text, @\"D:\\source.bmp\");\r\n            img1.Save(@\"D:\\img1.bmp\");\r\n\r\n            Image img2 = aw.ToANSW(obj1, @\"D:\\source.bmp\", \"text\");\r\n            img2.Save(@\"D:\\img2.bmp\");\r\n\r\n            Image img3 = aw.ToANSW(obj2, @\"D:\\source.bmp\", \"_hex\");\r\n            img3.Save(@\"D:\\img3.bmp\");\r\n\r\n            Image img4 = aw.ToANSW(obj3, @\"D:\\source.bmp\", \"ary\");\r\n            img4.Save(@\"D:\\img4.bmp\");\r\n            */\r\n            #endregion\r\n\r\n            #region XmlCreater测试\r\n            /*\r\n            XmlInf inf1;\r\n            inf1.path = \"//StdLib\";\r\n            inf1.nodeName = \"MyNode\";\r\n            inf1.attName = \"att\";\r\n            inf1.attValue = \"Att233\";\r\n            inf1.innerText = \"This is my innerText!\";\r\n\r\n            XmlInf inf2;\r\n            inf2.path = \"//StdLib\";\r\n            inf2.nodeName = \"XN1\";\r\n            inf2.attName = \"att\";\r\n            inf2.attValue = null;\r\n            inf2.innerText = \"Type\";\r\n\r\n            XmlInf inf3;\r\n            inf3.path = \"//StdLib\";\r\n            inf3.nodeName = \"NO_0\";\r\n\r\n            XmlInf inf4;\r\n            inf4.path = \"//StdLib//NO_0\";\r\n            inf4.nodeName = \"NO_1\";\r\n\r\n            XmlInf inf5;\r\n            inf5.path = \"//StdLib//NO_1\";\r\n            inf5.nodeName = \"NO_2\";\r\n\r\n            XmlInf inf6;\r\n            inf6.path = \"//StdLib//NO_1\";\r\n            inf6.nodeName = \"Node233\";\r\n            inf6.attName = \"att\";\r\n            inf6.attValue = null;\r\n            inf6.innerText = \"innerText\";\r\n\r\n            XmlInf inf7;\r\n            inf7.path = \"//StdLib\";\r\n            inf7.nodeName = \"goodXN\";\r\n            inf7.attName = \"att\";\r\n            inf7.attValue = null;\r\n            inf7.innerText = \"Type\";\r\n\r\n            XmlInf inf8;\r\n            inf8.path = \"//StdLib\";\r\n            inf8.nodeName = \"badXN\";\r\n            inf8.attName = \"att\";\r\n            inf8.attValue = null;\r\n            inf8.innerText = \"Type\";\r\n\r\n            XmlInf inf9;\r\n            inf9.path = \"//StdLib//NO_0//NO_1//Node233\";\r\n            inf9.type = \"_name\";\r\n\r\n            XmlInf inf10;\r\n            inf10.path = \"//StdLib//goodXN\";\r\n            inf10.type = \"_value\";\r\n\r\n            XmlInf inf11;\r\n            inf11.path = \"//StdLib//MyNode\";\r\n            inf11.attName = \"att\";\r\n\r\n            XmlInf sys;\r\n            sys.path = @\"D:\\\";\r\n            sys.xmlName = \"Xml_1\";\r\n            sys.rootName = \"StdLib\";\r\n\r\n            //XML初始化测试\r\n            Class_XmlCreater.CreateXml(sys);\r\n            Class_XmlCreater.reStream(@\"D:\\Xml_1.xml\");\r\n\r\n            //Xml写入测试\r\n            Class_XmlCreater.AddRealNode(inf1);\r\n\r\n            //节点重名测试\r\n            Class_XmlCreater.AddRealNode(inf2);\r\n            Class_XmlCreater.AddRealNode(inf2);\r\n\r\n            //节点嵌套测试\r\n            Class_XmlCreater.AddEmptyNode(inf3);\r\n            Class_XmlCreater.AddEmptyNode(inf4);\r\n            Class_XmlCreater.AddEmptyNode(inf5);\r\n\r\n            //添加嵌套里的有多个参数的节点测试\r\n            Class_XmlCreater.AddRealNode(inf6);\r\n\r\n            //节点删除测试\r\n            Class_XmlCreater.AddRealNode(inf7);\r\n            Class_XmlCreater.AddRealNode(inf8);\r\n            Class_XmlCreater.RemoveNode(inf8);\r\n            //被嵌套节点删除测试\r\n            Class_XmlCreater.RemoveNode(inf5);\r\n\r\n            //节点信息读取测试\r\n            Console.WriteLine(Class_XmlCreater.ReadNode(inf9));\r\n            Console.WriteLine(Class_XmlCreater.ReadNode(inf10));\r\n            Console.WriteLine(Class_XmlCreater.ReadAtt(inf11));\r\n            */\r\n            #endregion\r\n\r\n            #region IStdInf接口测试\r\n\r\n            IStdInf I = new StdInf();\r\n            Console.WriteLine(I.Stdver_newPub);\r\n\r\n            #endregion\r\n\r\n            #region HASH/MD5异步测试\r\n\r\n            Console.WriteLine(Class_MD5.Main_MD5(@\"jgadsklfhjkfgnjdd5sf@#$%^&amp;*())ag416h4gsf6h3g4j56s74gs54h!@#$%^&amp;*()h\"));\r\n            Console.WriteLine(Class_Hash.Main_Hash(@\"j!@~)(_+)/sklfgnjkgnhnjfdsfag4575g3$#%^)(*s6y4456w68f$#%^g434dsf4\"));\r\n\r\n            Console.WriteLine(Class_MD5.Main_MD5(@\"\"));\r\n            Console.WriteLine(Class_Hash.Main_Hash(@\"\"));\r\n\r\n            Console.WriteLine(Class_MD5.Main_MD5(null));\r\n            Console.WriteLine(Class_Hash.Main_Hash(null));\r\n\r\n            #endregion\r\n\r\n            #region 输出资源测试\r\n            /*\r\n            Image i = I.Lib_logo;\r\n            i.Save(@\"D:\\interface_Lib_logo.bmp\");\r\n\r\n            StdInf inf = new StdInf();\r\n            i = inf.Lib_logo;\r\n            i.Save(@\"D:\\virtual_Lib_logo.bmp\");\r\n            */\r\n            #endregion\r\n\r\n            #region 二分法找值测试\r\n            int[] a1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };\r\n            Console.WriteLine(_BinarySrch.BinSrch(7, a1));\r\n            Console.WriteLine(_BinarySrch.BinSrch(10, a1));\r\n\r\n            double[] a2 = { 1.1, 1.3, 1.5, 1.7 };\r\n            Console.WriteLine(_BinarySrch.BinSrch(1.7, a2));\r\n            Console.WriteLine(_BinarySrch.BinSrch(5.6, a2));\r\n            #endregion\r\n\r\n            Console.ReadKey();\r\n\r\n        }\r\n    }\r\n}</pre>\r\n对应XML：\r\n<pre class=\"lang:xhtml decode:true\" title=\"Xml_1.xml\">&lt;?xml version=\"1.0\" encoding=\"utf-8\"?&gt;\r\n&lt;StdLib&gt;\r\n  &lt;MyNode att=\"Att233\"&gt;This is my innerText!&lt;/MyNode&gt;\r\n  &lt;XN1 att=\"\"&gt;Type&lt;/XN1&gt;\r\n  &lt;XN1 att=\"\"&gt;Type&lt;/XN1&gt;\r\n  &lt;NO_0&gt;\r\n    &lt;NO_1&gt;\r\n      &lt;Node233 att=\"\"&gt;innerText&lt;/Node233&gt;\r\n    &lt;/NO_1&gt;\r\n  &lt;/NO_0&gt;\r\n<span style=\"color: #000000;\">  &lt;goodXN att=\"\"&gt;Type&lt;/goodXN&gt;\r\n&lt;/StdLib&gt;</span></pre>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff6600;\">由于本次开发是最后一次基于S04架构（从1.04开始的标准架构），所以过多的介绍不会有很大帮助</span>\r\n\r\n<span style=\"color: #ff6600;\">在接下来的StdLib1.08中，我们将会更改许多内容，使用更加标准的S08架构。</span></blockquote>\r\n&nbsp;\r\n<p style=\"text-align: right;\">时间不是很充裕，百忙之中有幸能够撰写这篇日志。</p>\r\n<p style=\"text-align: right;\">以上 Thaumy</p>'),(12353,'[新的内容]虚拟主机、域名已被更换！','标准文章','<blockquote><span style=\"color: #3366ff;\"><span style=\"color: #ff6600;\">虚拟主机已经更换为 </span>XVMHOST (www.xvmhost.com) , </span><span style=\"color: #ff6600;\">服务器在香港</span>\r\n\r\n<span style=\"color: #ff6600;\">替代了原来的主(卡)机(机)屋(屋)虚拟主机，这下快多了</span>\r\n\r\n<span style=\"color: #33cccc;\">原来的www.thaumy.ml已不可用，新的博客地址更改为www.thaumy.link</span>\r\n\r\n<span style=\"color: #33cccc;\">原来的虚拟主机改换为外链数据仓库，减缓博客初始化压力以提升性能</span>\r\n\r\n<span style=\"color: #33cccc;\">数据仓库地址:database.thaumy.ml或database.thaumy.link</span></blockquote>\r\n<!--more-->\r\n<ul>\r\n 	<li><span style=\"color: #993366;\">以上是20170607的内容，现在数据仓库已不可用，并且由github来托管网页cdn代码和多媒体数据。</span></li>\r\n</ul>'),(12356,'应用程序三层架构与MVC(文章摘要)','标准文章','<span style=\"color: #333333;\"><span style=\"color: #993366;\">三层架构（3-tier application）</span> 通常意义上的三层架构就是将整个业务应用划分为：表现层（UI）、业务逻辑层（BLL）、数据访问层（DAL）。区分层次的目的即为了“高内聚，低耦合”的思想。</span>\r\n<blockquote><span style=\"color: #ff9900;\"> 1、表现层（UI）：<span style=\"color: #333399;\">通俗讲就是展现给用户的界面，即用户在使用一个系统的时候他的所见所得。</span></span>\r\n\r\n<span style=\"color: #ff9900;\">2、业务逻辑层（BLL）：<span style=\"color: #333399;\">针对具体问题的操作，也可以说是对数据层的操作，对数据业务逻辑处理。</span></span>\r\n\r\n<span style=\"color: #ff9900;\">3、数据访问层（DAL）：<span style=\"color: #333399;\">该层所做事务直接操作数据库，针对数据的增添、删除、修改、更新、查找等。</span></span></blockquote>\r\n&nbsp;\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">MVC是 Model-View-Controller，</span>严格说这三个加起来以后才是三层架构中的UI层，也就是说，MVC把三层架构中的UI层再度进行了分化，分成了控制器、视图、实体三个部分，控制器完成页面逻辑，通过实体来与界面层完成通话；而C层直接与三层中的BLL进行对话。</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">MV<span style=\"color: #ff6600;\">C</span></span><span style=\"color: #ff6600;\">可以是三层中的一个表现层框架，</span>属于表现层。三层和MVC可以共存。三层是基于业务逻辑来分的，而MVC是基于页面来分的。MVC主要用于表现层，3层主要用于体系架构，3层一般是表现层、中间层、数据层，其中表现层又可以分成M、V、C，（Model View Controller）模型－视图－控制器</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">MVC模式是GUI界面开发的指导模式，</span>基于表现层分离的思想把程序分为三大部分：Model-View-Controller，呈三角形结构。Model是指数据以及应用程序逻辑，View是指 Model的视图，也就是用户界面。这两者都很好理解，关键点在于Controller的角色以及三者之间的关系。在MVC模式中，Controller和View同属于表现层，通常成对出现。Controller被设计为处理用户交互的逻辑。一个通常的误解是认为Controller负责处理View和Model的交互，而实际上View和Model之间是可以直接通信的。由于用户的交互通常会涉及到Model的改变和View的更新，所以这些可以认为是Cont roller的副作用。</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">MVC 是表现层的架构，</span>MVC的Model实际上是ViewModel，即供View进行展示的数据。 ViewModel不包含业务逻辑，也不包含数据读取。</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">而在N层架构中，</span>一般还会有一个Model层，用来与数据库的表相对应，也就是所谓ORM中的O.这个Model可能是POCO，也可能是包含一些验证逻辑的实体类，一般也不包含数据读取。进行数据读取的是数据访问层。而作为UI层的MVC一般不直接操作数据访问层，中间会有一个业务逻辑层封装业务逻辑、调用数据访问层。UI层（Controller）通过业务逻辑层来得到数据（Model），并进行封装（ViewModel），然后选择相应的View.</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">MVC本来是存在于Desktop程序中的，</span>M是指数据模型，V是指用户界面，C则是控制器。使用MVC的目的是将M和V的实现代码分离，从而使同一个程序可以使用不同的表现形式。比如一批统计数据你可以分别用柱状图、饼图来表示。C存在的目的则是确保M和V的同步，一旦M改变，V应该同步更新。\r\nMVC如何工作MVC是一个设计模式，它强制性的使应用程序的输入、处理和输出分开。使用MVC应用程序被分成三个核心部件：模型、视图、控制器。它们各自处理自己的任务。</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">V视图：</span>用户看到并与之交互的界面。对老式的Web应用程序来说，视图就是由HTML元素组成的界面，在新式的Web应用程序中，HTML依旧在视图中扮演着重要的角色，但一些新的技术已层出不穷，它们包括Macromedia Flash和象XHTML，XML/XSL，WML等一些标识语言和Web services.如何处理应用程序的界面变得越来越有挑战性。MVC一个大的好处是它能为你的应用程序处理很多不同的视图。在视图中其实没有真正的处理发生，不管这些数据是联机存储的还是一个雇员列表，作为视图来讲，它只是作为一种输出数据并允许用户操纵的方式。</li>\r\n 	<li></li>\r\n 	<li><span style=\"color: #ff6600;\">模型M：</span>模型表示企业数据和业务规则。在MVC的三个部件中，模型拥有最多的处理任务。被模型返回的数据是中立的，就是说模型与数据格式无关，这样一个模型能为多个视图提供数据。由于应用于模型的代码只需写一次就可以被多个视图重用，所以减少了代码的重复性。</li>\r\n</ul>'),(12357,'StdLib1.05#public更新日志','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.05%E5%AE%A3%E4%BC%A0%E7%94%BB(%E5%8E%8B%E7%BC%A9).jpg\" width=\"710\" height=\"470\" /><span style=\"color: #ff9900;\">StdLibx1.05#Public</span>\r\n#此版本为StdLib项目设立以来最庞大的一次更新，新增代码量超过300行，最大的特点就是代码的报错机制和重载的应用，此次更新重在于提高业务逻辑层的开发效率，简化数据层的代码量，提高代码复用性。\r\nBeta版本更新技术概要:</blockquote>\r\n<blockquote>1.增加了散列函数(Tash Function)，该函数提供了一个input值，返回sha1的40字符数的执行结果，该函数所在的命名空间为StdEct，所在类为Class_Hash(静态)，方法名为Main_hash(静态).</blockquote>\r\n&nbsp;\r\n<blockquote>2.增加了MD5函数，提供input值，返回md5的32字符数的执行结果，命名空间与散列函数相同，类为Class_MD5，方法名为Main_md5，两者均为静态.</blockquote>\r\n&nbsp;\r\n<blockquote>3.调用StdLib的StdLib_GobalnameSpace.StdInf并实例化，在对象的参数中新增了常量Stdver_newPub和Stdver_downloadURL，以获得StdLib最新的发行版版本参数，和最新发行版的下载地址。</blockquote>\r\n&nbsp;\r\n<blockquote>4.在StdDal中新增了Class_Run，其方法Run支持二重重载，传递参数(string stream)以调用流中的一个或两个程序</blockquote>\r\n&nbsp;\r\n<blockquote><span style=\"color: #ff9900;\">5.重大更新：</span>\r\n在StdDal数据层命名空间中，我们基于.net System.Xml进行了简化开发，新增类Class_Xml用于简化对于xml文档的操作，极大的提高了数据层开发效率，提高了后期代码归整的高集成性。\r\n\r\n<span style=\"color: #ff9900;\">CreatXml方法：</span>传递实参(string path，string fileName，string rootID)，创建一个Xml文档在path目录，其文件名为fileName，根元素名为rootID\r\n<span style=\"color: #ff9900;\">AddNode方法：</span>传递实参(string path，string nodeName，string attName，string AttValue，string Value)，指定父节点为path添加一个子节点，名为nodeName，并创建一个属性属性attName，包含的值为Value.值得注意的是，只有attValue和Value能被赋值null\r\n<span style=\"color: #ff9900;\">AddChild方法：</span>传递实参(string path,childName)，指定path为父节点，在该节点中创建一个子节点，名为childName\r\n<span style=\"color: #ff9900;\">RemoveNode方法：</span>传递实参(string path,string nodeName)，指定path为父节点，利用遍历的方法查找父节点下是否有符合NodeName的节点名，如果有则该节点将被删除，查找失败返回false\r\n<span style=\"color: #ff9900;\">ReadNode方法：</span>二重重载，传递实参(string path，string Xtype)，指定节点名为path，读取节点的Xtype的值，Xtype可选传值有：“_name”，“_value”，其他的传值将被驳回，并返回错误。传递实参2(string path，string Xtype，string attName)，使用方法与第一重载相同，但Xtype支持\"_att\"可选文本，即读取path名为attName的属性值，返回该值。</blockquote>\r\n&nbsp;\r\n\r\n6.在所有命名空间中的所有内容都添加了程序运行try-catch块，避免崩溃的发生。\r\n<pre class=\"lang:c# decode:true \" title=\"StdLib例程\">using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nusing StdLib_GobalnameSpace;\r\nusing StdEct;\r\n\r\nnamespace StdLibx1._05测试例程\r\n{\r\n    class Program\r\n    {\r\n        static void Main(string[] args)\r\n        {\r\n            {\r\n                //StdLib_GobalnameSpace.StdInf inf = new StdLib_GobalnameSpace.StdInf(\"http://www.thaumy.cn/Std_index/StdWebInf.html\");\r\n                StdLib_GobalnameSpace.StdInf inf = new StdLib_GobalnameSpace.StdInf();\r\n                Console.WriteLine(inf.Stdver_newPub);\r\n                Console.WriteLine(inf.Stdver_downloadURL);\r\n            }\r\n            {\r\n                string s = Console.ReadLine();\r\n                Console.WriteLine(Class_Hash.Main_Hash(s));//输出Hash加密后的s\r\n                Console.WriteLine(Class_MD5.Main_MD5(s));//输出MD5加密后的s\r\n            }\r\n\r\n            {\r\n                //bool b;\r\n                //bool b2;\r\n                /*应用程序单重载运行，成功返回true，失败返回false*/\r\n                //b=StdDal.Class_Run.Run(@\"Y:\\AdobeSoft\\Adobe Photoshop CS6 (64 Bit)\\Adobe Photoshop CS6 (64 Bit)\\Photoshop.exe\");\r\n                /*应用程序双重载运行,同上*/\r\n                //b2=StdDal.Class_Run.Run(@\"Y:\\AdobeSoft\\Adobe Flash CS6\\Adobe Flash CS6\\Flash.exe\", @\"Y:\\AdobeSoft\\Adobe Illustrator CS6\\Support Files\\Contents\\Windows\\Illustrator.exe\");\r\n                //Console.WriteLine(b.ToString()+b2.ToString());\r\n            }\r\n            {\r\n                StdDal.Class_Xml.CreateXml(@\"Y:\\其他程序\\数据集\", \"Xml_1\",\"StdLib\");\r\n                \r\n                StdDal.Class_Xml StdXml = new StdDal.Class_Xml(\"Y:\\\\其他程序\\\\数据集\\\\Xml_1.xml\");\r\n\r\n                StdXml.AddNode(\"//StdLib\", \"MyNode\", \"att\", \"Att233\", \"This is my innerText!\");//Xml写入测试\r\n\r\n                StdXml.AddNode(\"//StdLib\", \"XN1\", \"att\", null, \"Type\");//节点重名测试\r\n                StdXml.AddNode(\"//StdLib\", \"XN1\", \"att\", null, \"Type\");\r\n                \r\n                StdXml.AddChild(\"//StdLib\", \"NO_0\");//节点嵌套测试\r\n                StdXml.AddChild(\"//StdLib//NO_0\", \"NO_1\");\r\n                StdXml.AddChild(\"//StdLib//NO_1\", \"NO_2\");\r\n\r\n                StdXml.AddNode(\"//StdLib//NO_1\", \"Node233\", \"att\", null, \"innerText\");//添加嵌套里的有多个参数的节点测试\r\n\r\n                {\r\n                    StdXml.AddNode(\"//StdLib\", \"goodXN\", \"att\", null, \"Type\");//节点删除测试\r\n                    StdXml.AddNode(\"//StdLib\", \"badXN\", \"att\", null, \"Type\");\r\n                    StdXml.RemoveNode(\"//StdLib\", \"badXN\");//删除节点测试\r\n                    StdXml.RemoveNode(\"//StdLib//NO_1\", \"NO_2\");//被嵌套节点删除测试\r\n                }\r\n                \r\n                Console.WriteLine(StdXml.ReadNode(\"//StdLib//NO_0//NO_1//Node233\", \"_name\"));\r\n                Console.WriteLine(StdXml.ReadNode(\"//StdLib//goodXN\", \"_value\"));\r\n                Console.WriteLine(StdXml.ReadNode(\"//StdLib//MyNode\", \"_att\", \"att\"));\r\n            }\r\n            \r\n            Console.ReadKey();\r\n        }\r\n    }\r\n\r\n}\r\n</pre>\r\n<pre class=\"lang:xhtml decode:true \" title=\"对应XML\">&lt;?xml version=\"1.0\" encoding=\"utf-8\"?&gt;\r\n&lt;StdLib&gt;\r\n  &lt;MyNode att=\"Att233\"&gt;This is my innerText!&lt;/MyNode&gt;\r\n  &lt;XN1 att=\"\"&gt;Type&lt;/XN1&gt;\r\n  &lt;XN1 att=\"\"&gt;Type&lt;/XN1&gt;\r\n  &lt;NO_0&gt;\r\n    &lt;NO_1&gt;\r\n      &lt;Node233 att=\"\"&gt;innerText&lt;/Node233&gt;\r\n    &lt;/NO_1&gt;\r\n  &lt;/NO_0&gt;\r\n  &lt;goodXN att=\"\"&gt;Type&lt;/goodXN&gt;\r\n&lt;/StdLib&gt;</pre>\r\n&nbsp;'),(12358,'HiMyNewBlog!','标准文章','<span style=\"color: #ff6600;\">我的博客主站已转到此站www.thaumy.link</span>\r\n\r\n<span style=\"color: #33cccc;\">第一季度的技术测试期已经结束，欢迎访问我的博客！</span>\r\n<span style=\"color: #ff0000;\">嘿，来听一曲呗！</span>\r\n<iframe width=\"100%\" height=\"433\" src=\"//music.163.com/outchain/player?type=0&amp;id=489400343&amp;auto=1&amp;height=430\" frameborder=\"no\" marginwidth=\"0\" marginheight=\"0\"></iframe>'),(12359,'浅谈wordpress数据库优化','标准文章','<span style=\"color: #993366;\">众所周知，wordpress是在数据库里存储数据的，这些数据包括文章、用户数据、媒体路径等，在这里不过多列举。</span>\r\n\r\n<span style=\"color: #993366;\">有效数据是wp运作的基础，无效数据则会减缓网页运行速度，带来未知的bug和不必要的资源占用，甚至是浪费你的服务器资源(我的数据库在优化前有20MB大，优化后只有1MB)。</span>\r\n\r\n<span style=\"color: #993366;\">这里则是一篇基础文章，谈及最基本的数据库优化，表达我个人的见解。</span>\r\n<blockquote><span style=\"color: #00ccff;\">wordpress在使用时会产生大量不必要的数据库信息，这些信息虽然只占用了很小一部分的服务器资源，但随着日积月累，这将会是一个庞大的资源开销，甚至撑爆你的数据库空间！</span></blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">wp_posts</span></li>\r\n</ul>\r\n这个表被用来存储文章信息，包括文章标题、内容、父级、别名和一些其他列，当一个wp站点运作时间较长，且更新活跃时，这里便成为一个数据垃圾场。\r\n\r\nwp在你每一次文章更改时都会保存文章的信息，这也包括自动草稿。每一次的文章更名都会在该表内增添一个新行，它不在原有的基础上修改，而是通过建立一个新行实现更改，这意味着之前所有的数据都会被保存。\r\n\r\n我删除了大量的文章、清空了回收站，当我浏览数据库时，发现wp_posts竟然有700多行数据，这意味着我的700次文章、页面修改全都被记录。这是非常恐怖的，仅仅200天就有800行数据，占用了多达20M的数据库空间。这意味着每一次打开博客，存储引擎就要在700行数据中select出真正有用的几十篇文章。虽然存储引擎是高效的，但是如果有上万行，速度延迟便变得明显，这对用户体验并不友好。\r\n\r\n一般来讲，wp_posts表中的post_status列的值为inherit时，这篇文章是可以被安全删除的。\r\n\r\n在控制台执行SQL语句：\r\n<pre class=\"lang:mysql decode:true \" title=\"SQL\">DELETE FROM `wp_posts` WHERE `wp_posts`.`post_status` = \"inherit\";</pre>\r\n另外，自动草稿也是可以被安全删除的：\r\n<pre class=\"lang:mysql decode:true \" title=\"SQL\">DELETE FROM `wp_posts` WHERE `wp_posts`.`post_title` = \"自动草稿\";</pre>\r\n</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">wp_postmeta</span></li>\r\n</ul>\r\n它被用来存储部分插件、文章自定义栏目数据，当删除一个插件时，这些数据并不会被清除。如果你经常安装插件，查看一下这个表的数据还是有必要的。</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">wp_comments</span></li>\r\n</ul>\r\n这个表存储了评论内容(wp原生的)，每一次评论都会被详细记录。这包括评论者的终端系统、运行环境、浏览器及其内核、IP地址、日期等。曾经有效的评论都会在这里被保存，不论你是否删除过或清空回收站。如果你的站点是开放评论的，请酌情考虑这些数据的价值(往往这些数据在指导对用户行为分析时是有用的)</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">wp_users</span></li>\r\n</ul>\r\n用户的基本信息都被存储在这里，它包括用户名、密码(被MD5加密)、邮箱、注册日期等。如果你的站点有用户注册，考虑这些信息是否被值得优化。</blockquote>\r\n&nbsp;\r\n<blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">wp_usermeta</span></li>\r\n</ul>\r\n某些插件会使用该表，用来为用户增添新的属性。这方面的插件修改较多时，考虑这里的数据。\r\n\r\n&nbsp;\r\n\r\n<span style=\"color: #ff0000;\">无论垃圾数据量有多少，都应按照实际情况考虑。不要强硬地更改，这可能导致不必要的数据损失。适当保有数据作为备份，是很有必要的。存储引擎的速度差距只有在上万行时才可能影响体验。</span></blockquote>'),(12360,'CreativeDesktop图标1706更新','标准文章','<img class=\"alignnone size-medium\" src=\"https://raw.githubusercontent.com/Thaumy/MyBlog/master/img/article/cdkp_all(%E5%8E%8B%E7%BC%A9).jpg\" width=\"1162\" height=\"722\" />\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">CreativeDesktop是Developerslab的一个分支项目，致力于UI的设计与美化。</span></li>\r\n 	<li><span style=\"color: #ff6600;\">这里是一份图标组的源文件。您可以修改它定制您的个性化图标。</span></li>\r\n</ul>\r\n<blockquote><a href=\"https://github.com/Thaumy/DevelopersLab\"><span style=\"color: #00ccff;\">https://github.com/Thaumy/DevelopersLab</span></a>\r\n\r\n<span style=\"color: #ff6600;\">&lt;跳往github的超链接↑&gt;</span></blockquote>'),(12363,'log2017','标准文章','<blockquote><span style=\"color: #800000;\">我们先来谈一谈2017这个年份吧。2017，四个数字，简短但又巨大。我对于数字7的热衷使得我在这一年里有很有力的干劲。在飞过2016这个讨厌的年份之后（我最讨厌6、8、3这几个数字了，它们让我不自觉地想到懒散、提不起精神——似乎是因为它们都具有平滑的形状的原因。但2是个例外，在某些专业领域我还是对2比较喜欢的。但是在一些通行ID上开头出现2的情况是我最不乐意的），我迎来了崭新的2017。十年一次啊，兴奋的我也感到时间流逝的飞快了。我想我应该在这一年里做些什么。搞搞项目挖挖坑？这些都不重要。再不努力恐怕就起不来了，所以我买了很多书籍丰富我的知识（数据库啊、oop的语言啊、前后端啊，样样都有样样都不精通）。毕竟在这个领域里但当涉猎和实践的提升是最快的。这一年处理的事情也会很多啊~马上就要离开中学了呢。听同学说高中时基本没假放的，那我还有什么闲情雅致搞这些呢！</span></blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #33cccc;\">今年已经过去一半了诶，我没搞出什么东西，仍旧在走15年的老路子，搞些@#$&amp;W$()_)*^+_(@@(此处一千字省略)什么的：没有什么用，只不过是换了种语言罢。</span></li>\r\n</ul>\r\n<blockquote><span style=\"color: #00ccff;\">其实吧，无论是学习这个还是学习那个，刚开始接触感觉很高大上，很牛逼，等到会了的时候也就真的觉得没什么了不起的了，所以不想谈了——只不过在脑中回想当初钻研的困难就是了。</span></blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">时间不多了，如果预料是准确的，那么我在升入高中后开发时间会大幅度减少。到那时，真正的考验便到了。</span></li>\r\n</ul>\r\n<p style=\"text-align: right;\">Thaum2017.5.29</p>'),(12364,'我对递归的研究和总结','标准文章','<span style=\"color: #ff6600;\">上个周的某一天我开始研究递归这个编程范式，什么是递归呢？这里我阐述一下自己的见解。</span>\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">我的理解</span></li>\r\n</ul>\r\n<a href=\"http://baike.baidu.com/link?url=vtnjbv5K54Ch9hn9Qi1duM-ucK3_LSjN9TMNeykQpAIA_uLxX8W-naC2yCG78UFCq3eTQl1y74EwqM8E9LlfcmljplvFblihQOquWIlxFra\" target=\"_blank\"><span style=\"color: #0000ff;\">递归</span></a>是一种可靠的编程范式（除了某些情况下滥用递归会导致爆栈）。递归的基本思想是通过程序不断调用自身来解决所需解决的问题。这是一种很聪明的解决思路，并且还会减少代码量，使程序更加简洁。\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">百度百科上的解释(占用文章字数啦~!日常水博)</span></li>\r\n</ul>\r\n程序调用自身的编程技巧称为递归 (recursion) 。递归做为一种<a href=\"http://baike.baidu.com/item/%E7%AE%97%E6%B3%95\" target=\"_blank\" rel=\"noopener\">算法</a>在<a href=\"http://baike.baidu.com/item/%E7%A8%8B%E5%BA%8F%E8%AE%BE%E8%AE%A1%E8%AF%AD%E8%A8%80\" target=\"_blank\" rel=\"noopener\">程序设计语言</a>中广泛应用。 一个过程或<a href=\"http://baike.baidu.com/item/%E5%87%BD%E6%95%B0\" target=\"_blank\" rel=\"noopener\">函数</a>在其定义或说明中有直接或间接调用自身的一种方法，它通常把一个大型复杂的问题层层转化为一个与原问题相似的规模较小的问题来求解，递归策略只需少量的程序就可描述出解题过程所需要的多次重复计算，大大地减少了程序的代码量。递归的能力在于用有限的<a href=\"http://baike.baidu.com/item/%E8%AF%AD%E5%8F%A5\" target=\"_blank\" rel=\"noopener\">语句</a>来定义对象的<a href=\"http://baike.baidu.com/item/%E6%97%A0%E9%99%90%E9%9B%86%E5%90%88\" target=\"_blank\" rel=\"noopener\">无限集合</a>。一般来说，递归需要有边界条件、递归前进段和递归返回段。当边界条件不满足时，递归前进；当边界条件满足时，递归返回。\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">构成</span></li>\r\n</ul>\r\n递归，顾名思义：传递——回归。我们在这里用一个简单的例子了解递归的基本思路。\r\n<pre class=\"lang:c# decode:true\" title=\"使用嵌套循环输出的99乘法表的实现\">using System;\r\n\r\nnamespace _99multiplication\r\n{\r\n    class Program\r\n    {\r\n        static void Main(string[] args)\r\n        {\r\n\r\n            for (int row = 1; row &lt;= 9; row++)\r\n            {\r\n\r\n                for (int i = 1; i &lt;= row; i++)\r\n                {\r\n                    Console.Write(\"{0}*{1}={2}\", row, i, row * i);\r\n                    Console.Write(\" \");\r\n                }\r\n                Console.WriteLine();\r\n\r\n            }\r\n            Console.ReadKey();\r\n\r\n        }\r\n    }\r\n}</pre>\r\n上面是使用传统模式打印99乘法表的代码，它采用了两个嵌套的for循环实现。一个循环负责横行，另一个则负责纵行。这里不过多介绍。\r\n\r\n<span style=\"color: #ff9900;\">我们再来看看下面的代码：</span>\r\n<pre class=\"lang:c# decode:true\" title=\"使用递归输出99乘法表的实现\">using System;\r\n\r\nnamespace ConsoleApplication1\r\n{\r\n    class Program\r\n    {\r\n        static void Main(string[] args)\r\n        {\r\n            multiplication(9);\r\n            Console.ReadKey();\r\n        }\r\n\r\n        public static void multiplication(int i)\r\n        {\r\n            if (i == 1)\r\n            {\r\n                Console.WriteLine(\"1*1=1 \");\r\n                Console.WriteLine();\r\n            }\r\n            else\r\n            {\r\n                multiplication(i - 1);\r\n                for (int j = 1; j &lt;= i; j++)\r\n                {\r\n                    Console.WriteLine(j + \"*\" + i + \"=\" + j * i + \"  \");\r\n                }\r\n                Console.WriteLine();\r\n            }\r\n        }\r\n    }\r\n}\r\n</pre>\r\n这是使用递归来输出99乘法表的代码。与嵌套for循环相比，它只使用了一个for循环来达到输出的目的，而初始结果直接打印 “1*1=1” 的字符串。这看起来有些不可思议：它是怎么做到的？\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">分析</span></li>\r\n</ul>\r\n在上面的两篇代码中，分别使用了不同的方式达到相同的目的。嵌套for过于简单这里不解释，递归则是通过不断调用自身达到最终目的。\r\n\r\n<span style=\"color: #ff9900;\">在这里，递归分为两个部分。我将他概括为初始化和返回:</span>\r\n<ol>\r\n 	<li><span style=\"color: #3366ff;\">初始化</span>：程序不断调用自身的方法，我们把调用方法这个行为看作是初始化：为接下来的循环做准备。</li>\r\n 	<li><span style=\"color: #3366ff;\">返回</span>：每次初始化都会进行判断，当某次初始化后的判断成立时，程序不再调用自身<span style=\"color: #ffcc00;\">(也就是达到了所谓的递归边界准备返回结束)</span>，并且结束当前方法，进行返回。返回后执行的便是上一个(父级)方法初始化完成后下面的代码：循环输出。因为每次初始化的传参都不一样并且有大到小，所以在此过程中，循环可以由小到大依次输出乘法表。</li>\r\n</ol>\r\n<span style=\"color: #ff9900;\">可能这样讲解还是有些模糊，于是我画了一幅图增进我的理解：</span>\r\n<img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/33cfb.jpg\" width=\"650\" height=\"867\" />\r\n\r\n图这里使用的是33乘法表进行分析，可以看到它分为两个部分：执行和回溯，也就是我说的初始化(调用)和返回。\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">总结</span></li>\r\n</ul>\r\n我其实对递归还是一知半解的，希望这次学习能够对我在算法研究领域添上一臂之力(上次研究归并算法的时候没少吃苦头。。。)，所以呢，这里就结束啦~！\r\n\r\n<img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/dgmb.jpg\" width=\"210\" height=\"182\" />'),(12365,'StdLib1.08#public更新日志','标准文章','<blockquote><img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.08%E5%AE%A3%E4%BC%A0%E7%94%BB(%E5%8E%8B%E7%BC%A9).jpg\" width=\"700\" height=\"525\" />\r\n\r\n<span style=\"color: #ff6600;\">经过一大半月的跳票，久违的StdLib1.08终于横空出世了。我们来看一看它有哪些新特性。</span></blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #993366;\">全新的参数列表</span></li>\r\n</ul>\r\n由于在前几代StdLib中，S04架构不是很规范，所以在这个版本往后，将改换为S08架构，保证代码的整洁和维护易用性。另外大多数静态类和方法都修改为非静态，这意味着在使用时需要实例化。\r\n<ul>\r\n 	<li>StdLib信息空间更名为LibInformation，相应接口也更换为ILibInformation。</li>\r\n</ul>\r\nILibInformation接口规范：\r\n<pre class=\"lang:c# decode:true\" title=\"ILibInformation\">            /// &lt;summary&gt;\r\n            /// 主版本\r\n            /// &lt;/summary&gt;\r\n            int verFirst\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 次版本\r\n            /// &lt;/summary&gt;\r\n            double verSecond\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 是否为开发者版本\r\n            /// &lt;/summary&gt;\r\n            bool devEdition\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 是否为发行版\r\n            /// &lt;/summary&gt;\r\n            bool pubEdition\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 联网时的最新发行版\r\n            /// &lt;/summary&gt;\r\n            string webVer\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 联网最新发行版的下载URL\r\n            /// &lt;/summary&gt;\r\n            string webURL\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 针对最近一次发行版的全局兼容性\r\n            /// &lt;/summary&gt;\r\n            bool compatibleLast\r\n            {\r\n                get;\r\n            }\r\n            /// &lt;summary&gt;\r\n            /// 获取到像素化的StdLib_logo\r\n            /// &lt;/summary&gt;\r\n            Bitmap logo\r\n            {\r\n                get;\r\n            }</pre>\r\n<ul>\r\n 	<li>用于获取StdLib版本信息的LibInformation:LibInformation类</li>\r\n</ul>\r\npublic LibInformation()方法：使用默认Url获取更新信息\r\n\r\npublic LibInformation(string InfUrl)第二重载：指定特定的URL获取更新信息\r\n<ul>\r\n 	<li>将算法空间更名为Algorithm</li>\r\n</ul>\r\nEncryptor类：包含两个方法：md5和hash，用于对字符串进行单向加密操作\r\n<ul>\r\n 	<li>矩阵算法类更名为ArrayAlgorithm，其具体方法作用在summary注释中有详细说明。</li>\r\n 	<li>二维码算法类更名为PixelGraphic，其具体方法作用在summary注释中有详细说明。</li>\r\n</ul>\r\n编译二维码图像的type字符串可选参数：\r\n\r\n\"hex\"//根据不带空格的16进制文本编译ANSW\r\n\"_hex\"//根据带空格的16进制文本编译ANSW\r\n\"array\"//根据16进制矩阵编译ANSW\r\n<ul>\r\n 	<li>排序算法类Sorter</li>\r\n</ul>\r\n这个类包含了基本的排序算法，其中easySorter方法是原生的LSS排序算法。\r\n<ul>\r\n 	<li>检索类Searcher</li>\r\n</ul>\r\n这个类包含了基本的检索算法。\r\n<ul>\r\n 	<li>数据层命名空间更名为DataLayer</li>\r\n 	<li>MCInformation类</li>\r\n</ul>\r\n这个类用于提取MC运行的必要信息，其具体方法作用在summary注释中有详细说明。\r\n<ul>\r\n 	<li>程序启动类ProgramLoader</li>\r\n</ul>\r\n用于启动程序的类，其具体方法作用在summary注释中有详细说明。\r\n<ul>\r\n 	<li>XmlCreater类</li>\r\n</ul>\r\n用于创建简单XML表的类，其具体方法作用在summary注释中有详细说明。\r\n<ul>\r\n 	<li><span style=\"color: #ff6600;\">错误机制</span></li>\r\n</ul>\r\n在StdLib1.08中，提供了非常完善的错去机制。这有利于debug和项目维护，根除了StdLib1.07中报错提示混杂的诟病。\r\n<pre class=\"lang:c# decode:true \" title=\"报错文档(非代码)\">标准报错文本:StdLibError ec[报错值]\r\n\r\nec7540：在初始化StdLib命名空间的StdInf类(第一重载)时，webPubs数组第0元素发生关键性错误，通常是由没有网络连接或是HTML错误导致，一般7540报错与7541报错同时发生，该报错具体表现为返回标准报错文本\r\n\r\nec7541：在初始化StdLib命名空间的StdInf类(第一重载)时，webPubs数组第1元素发生关键性错误，通常是由没有网络连接或是HTML错误导致，一般7540报错与7541报错同时发生，该报错具体表现为返回标准报错文本\r\n\r\nec7480：在初始化StdLib命名空间的StdInf类(第二重载)时，webPubs数组第0元素发生关键性错误，通常是由没有网络连接或是HTML错误导致，一般7480报错与7481报错同时发生，该报错具体表现为返回标准报错文本\r\n\r\nec7481：在初始化StdLib命名空间的StdInf类(第二重载)时，webPubs数组第1元素发生关键性错误，通常是由没有网络连接或是HTML错误导致，一般7480报错与7481报错同时发生，该报错具体表现为返回标准报错文本\r\n\r\n\r\n\r\n\r\nec2390：在使用StdEct命名空间的_SRR类时，算法主方法(即矩阵处理)过程中发生致命性错误，可能是由于dic数组或num数组错误传参导致，该报错具体表现为返回内容为2390的单元素数组\r\n\r\nec4580：在使用StdEct命名空间的_ANSW类时，DeANSW方法中发生关键性错误，通常是由于未能成功初始化Bitmap或传递了错误的Bitmap导致，该报错具体表现为返回标准报错文本\r\n\r\nec5880：在使用StdEct命名空间的_BinSrch类时，search方法(第一重载)中发生致命性错误，该报错具体表现为返回内容为5880的int值\r\n\r\nec5881：在使用StdEct命名空间的_BinSrch类时，search方法(第二重载)中发生致命性错误，该报错具体表现为返回内容为5881的double值\r\n\r\n\r\n\r\n\r\nec1120：在使用StdDal命名空间的_XmlCreater类时，ReadNode方法(第一重载)中发生关键性错误，通常是由于传递了错误的读取类型参数导致，该报错具体表现为返回标准报错文本\r\n\r\nec1121：在使用StdDal命名空间的_XmlCreater类时，ReadNode方法(第一重载)中发生致命性错误，可能是由无法查找到相关类型的值导致，该报错具体表现为返回标准报错文本\r\n\r\nec1127：在使用StdDal命名空间的_XmlCreater类时，ReadNode方法(第二重载)中发生关键性错误，通常是由于传递了错误的读取类型参数导致，该报错具体表现为返回标准报错文本\r\n\r\nec1128：在使用StdDal命名空间的_XmlCreater类时，ReadNode方法(第二重载)中发生致命性错误，可能是由无法查找到相关类型的值导致，该报错具体表现为返回标准报错文本\r\n\r\n\r\n\r\n\r\nec1440：在使用StdDal命名空间的_XmlCreater类时，ReadAtt方法(第一重载)中发生致命性错误，可能是由无法查找到节点属性导致，该报错具体表现为返回标准报错文本\r\n\r\nec1441：在使用StdDal命名空间的_XmlCreater类时，ReadAtt方法(第二重载)中发生致命性错误，可能是由无法查找到节点属性导致，该报错具体表现为返回标准报错文本</pre>\r\n当程序的运行出错时，相应的报错会生效。当然，有些方法的返回值类型是布尔型，这类方法发生错误时会返回false。\r\n<blockquote><span style=\"color: #333333;\">StdLib各个版本的下载地址：</span>\r\n\r\n<a href=\"https://github.com/Thaumy/StdLib1x\" target=\"_blank\" rel=\"noopener\"><span style=\"color: #00ccff;\">https://github.com/Thaumy/StdLib1x</span></a>\r\n\r\n<span style=\"color: #ff9900;\">&lt;跳往GitHub的链接&gt;</span></blockquote>'),(12366,'通用化站点pjax解决方案','标准文章','<span style=\"color: #800000;\">在上周进行网站优化和日常维护中，我发现ajax插件在一定程度上造成了我博客的臃肿，过于庞大的插件使我需要一个全新的方式载入文章。于是我查找了诸多文档，终于发现了更好的解决方案。值得注意的是，它适合绝大多数架构规范的网站，不仅仅局限于wordpress——这里是详细的解决方案。</span>\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">准备工作</span></li>\r\n</ul>\r\n<blockquote>电脑一台，十分钟的阅读时间和十分钟的部署时间。\r\n\r\n注意，这篇教程不面向前端零基础的读者，如果你连header.php都找不到，那么请使用插件达到目的。\r\n\r\n<span style=\"color: #00ccff;\">或许这篇文章更适合你：</span>\r\n<h3 class=\"post_title\"><a href=\"http://www.thaumy.cn/article/%e6%8f%92%e4%bb%b6%e5%8c%96ajax%e4%bd%bf%e7%94%a8%e6%96%b9%e6%b3%95\"><span style=\"color: #3366ff;\"><u>AdvancedAjaxPageLoader插件实现全站ajax</u></span></a></h3>\r\n<span style=\"color: #00ccff;\">&lt;跳往博客文章的超链接↑&gt;</span></blockquote>\r\n&nbsp;\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">项目资源</span></li>\r\n</ul>\r\n<pre class=\"lang:js decode:true\" title=\"用于实现pjax的javascript代码\">var ajaxhome=\'您的站点地址\';\r\nvar ajaxcontent = \'被刷新的文章容器id\';\r\nvar ajaxsearch_class = \'searchform\';\r\nvar ajaxignore_string = new String(\'#, /wp-, .pdf, .zip, .rar, /goto\');\r\nvar ajaxignore = ajaxignore_string.split(\', \');\r\n\r\nvar ajaxloading_code = \'此处添加加载动画代码\';\r\nvar ajaxloading_error_code = \'此处添加超时代码\';\r\n\r\nvar ajaxreloadDocumentReady = false;\r\nvar ajaxtrack_analytics = false\r\nvar ajaxscroll_top = true\r\nvar ajaxisLoad = false;\r\nvar ajaxstarted = false;\r\nvar ajaxsearchPath = null;\r\nvar ajaxua = jQuery.browser;\r\njQuery(document).ready(function() {\r\n    ajaxloadPageInit(\"\");\r\n});\r\nwindow.onpopstate = function(event) {\r\n    if (ajaxstarted === true &amp;&amp; ajaxcheck_ignore(document.location.toString()) == true) {\r\n        ajaxloadPage(document.location.toString(),1);\r\n    }\r\n};\r\nfunction ajaxloadPageInit(scope){\r\n    jQuery(scope + \"a\").click(function(event){\r\n        if (this.href.indexOf(ajaxhome) &gt;= 0 &amp;&amp; ajaxcheck_ignore(this.href) == true){\r\n            event.preventDefault();\r\n            this.blur();\r\n            var caption = this.title || this.name || \"\";\r\n            var group = this.rel || false;\r\n            try {\r\n                ajaxclick_code(this);\r\n            } catch(err) {\r\n            }\r\n            ajaxloadPage(this.href);\r\n        }\r\n    });\r\n    jQuery(\'.\' + ajaxsearch_class).each(function(index) {\r\n        if (jQuery(this).attr(\"action\")) {\r\n            ajaxsearchPath = jQuery(this).attr(\"action\");;\r\n            jQuery(this).submit(function() {\r\n                submitSearch(jQuery(this).serialize());\r\n                return false;\r\n            });\r\n        }\r\n    });\r\n    if (jQuery(\'.\' + ajaxsearch_class).attr(\"action\")) {} else {\r\n    }\r\n}\r\nfunction ajaxloadPage(url, push, getData){\r\n    if (!ajaxisLoad){\r\n        if (ajaxscroll_top == true) {\r\n            jQuery(\'html,body\').animate({scrollTop: 0}, 1500);\r\n        }\r\n        ajaxisLoad = true;\r\n        ajaxstarted = true;\r\n        nohttp = url.replace(\"http://\",\"\").replace(\"https://\",\"\");\r\n        firstsla = nohttp.indexOf(\"/\");\r\n        pathpos = url.indexOf(nohttp);\r\n        path = url.substring(pathpos + firstsla);\r\n        if (push != 1) {\r\n            if (typeof window.history.pushState == \"function\") {\r\n                var stateObj = { foo: 1000 + Math.random()*1001 };\r\n                history.pushState(stateObj, \"ajax page loaded...\", path);\r\n            } else {\r\n            }\r\n        }\r\n        if (!jQuery(\'#\' + ajaxcontent)) {\r\n        }\r\n		\r\n		\r\n        //jQuery(\'#\' + ajaxcontent).append(ajaxloading_code);\r\n		jQuery(\'#\' + ajaxcontent).prepend(ajaxloading_code);\r\n		\r\n		\r\n        jQuery(\'#\' + ajaxcontent).fadeTo(\"slow\", 0.4,function() {\r\n            jQuery(\'#\' + ajaxcontent).fadeIn(\"slow\", function() {\r\n                jQuery.ajax({\r\n                    type: \"GET\",\r\n                    url: url,\r\n                    data: getData,\r\n                    cache: false,\r\n                    dataType: \"html\",\r\n                    success: function(data) {\r\n                        ajaxisLoad = false;\r\n                        datax = data.split(\'&lt;title&gt;\');\r\n                        titlesx = data.split(\'&lt;/title&gt;\');\r\n                        if (datax.length == 2 || titlesx.length == 2) {\r\n                            data = data.split(\'&lt;title&gt;\')[1];\r\n                            titles = data.split(\'&lt;/title&gt;\')[0];\r\n                            jQuery(document).attr(\'title\', (jQuery(\"&lt;div/&gt;\").html(titles).text()));\r\n                        } else {\r\n                        }\r\n                        if (ajaxtrack_analytics == true) {\r\n                            if(typeof _gaq != \"undefined\") {\r\n                                if (typeof getData == \"undefined\") {\r\n                                    getData = \"\";\r\n                                } else {\r\n                                    getData = \"?\" + getData;\r\n                                }\r\n                                _gaq.push([\'_trackPageview\', path + getData]);\r\n                            }\r\n                        }\r\n                        data = data.split(\'id=\"\' + ajaxcontent + \'\"\')[1];\r\n                        data = data.substring(data.indexOf(\'&gt;\') + 1);\r\n                        var depth = 1;\r\n                        var output = \'\';\r\n                        while(depth &gt; 0) {\r\n                            temp = data.split(\'&lt;/div&gt;\')[0];\r\n                            i = 0;\r\n                            pos = temp.indexOf(\"&lt;div\");\r\n                            while (pos != -1) {\r\n                                i++;\r\n                                pos = temp.indexOf(\"&lt;div\", pos + 1);\r\n                            }\r\n                            depth=depth+i-1;\r\n                            output=output+data.split(\'&lt;/div&gt;\')[0] + \'&lt;/div&gt;\';\r\n                            data = data.substring(data.indexOf(\'&lt;/div&gt;\') + 6);\r\n                        }\r\n                        document.getElementById(ajaxcontent).innerHTML = output;\r\n                        jQuery(\'#\' + ajaxcontent).css(\"position\", \"absolute\");\r\n                        jQuery(\'#\' + ajaxcontent).css(\"left\", \"20000px\");\r\n                        jQuery(\'#\' + ajaxcontent).show();\r\n                        ajaxloadPageInit(\"#\" + ajaxcontent + \" \");\r\n                        if (ajaxreloadDocumentReady == true) {\r\n                            jQuery(document).trigger(\"ready\");\r\n                        }\r\n                        try {\r\n                            ajaxreload_code();\r\n                        } catch(err) {\r\n                        }\r\n                        jQuery(\'#\' + ajaxcontent).hide();\r\n                        jQuery(\'#\' + ajaxcontent).css(\"position\", \"\");\r\n                        jQuery(\'#\' + ajaxcontent).css(\"left\", \"\");\r\n                        jQuery(\'#\' + ajaxcontent).fadeTo(\"slow\", 1, function() {});\r\n                    },\r\n                    error: function(jqXHR, textStatus, errorThrown) {\r\n                        ajaxisLoad = false;\r\n                        document.title = \"Error loading requested page!\";\r\n                        document.getElementById(ajaxcontent).innerHTML = ajaxloading_error_code;\r\n                    }\r\n                });\r\n            });\r\n        });\r\n    }\r\n}\r\nfunction submitSearch(param){\r\n    if (!ajaxisLoad){\r\n        ajaxloadPage(ajaxsearchPath, 0, param);\r\n    }\r\n}\r\nfunction ajaxcheck_ignore(url) {\r\n    for (var i in ajaxignore) {\r\n        if (url.indexOf(ajaxignore[i]) &gt;= 0) {\r\n            return false;\r\n        }\r\n    }\r\n    return true;\r\n}\r\nfunction ajaxreload_code() {\r\n    //add code here   \r\n}\r\nfunction ajaxclick_code(thiss) {\r\n    jQuery(\'ul.nav li\').each(function() {\r\n        jQuery(this).removeClass(\'current-menu-item\');\r\n    });\r\n    jQuery(thiss).parents(\'li\').addClass(\'current-menu-item\');\r\n}</pre>\r\n复制以上代码，新建一个文本文件并拷贝进去，不要保存，等待下一步指示。\r\n\r\n&nbsp;\r\n\r\n接下来，我们需要修改代码中的某些部分，以保证代码在你的网站中能够达到您预期的效果。\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">修改以下代码</span></li>\r\n</ul>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxhome=\'http://www.thaumy.cn\';</pre>\r\n您的站点地址，例如我的网站主页地址为 http://www.thaumy.cn ，那么这里就可以填写http://www.thaumy.cn</blockquote>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxcontent = \'被刷新的文章容器id\';</pre>\r\n被刷新的文章容器id，指的是文章列表(postlist)的父级id。例如我的id是 Central ，那么这里就可以填写Central</blockquote>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxsearch_class = \'searchform\';</pre>\r\n搜索栏目的id，默认为 searchform ，一般不用更改</blockquote>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxignore_string = new String(\'#, /wp-, .pdf, .zip, .rar, /goto\');</pre>\r\n被屏蔽的加载项目，即不进行pjax加载的链接关键字，默认为 #, /wp-, .pdf, .zip, .rar, /goto ，一般不用更改</blockquote>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxloading_code = \'此处添加加载动画代码\';&lt;br&gt;</pre>\r\n此处添加加载动画代码，使pjax加载时显示动画，例如我的蓝色线条加载动画，这里不过多解释</blockquote>\r\n<blockquote>\r\n<pre class=\"lang:js decode:true\" title=\"js\">var ajaxloading_error_code = \'此处添加超时代码或一些文本\';</pre>\r\n此处添加超时代码或一些文本，例如pjax请求失败时，文章容器所显示的内容</blockquote>\r\n&nbsp;\r\n\r\n<span style=\"color: #ff9900;\">将以上代码修改完成后，复制修改好的代码，使用 &lt;script&gt;&lt;/script&gt; 标签把代码块插入到 header.php 的 &lt;/head&gt; 之前，然后就能实现网站的pjax加载了</span><span style=\"color: #ff9900;\">。</span><span style=\"color: #000000;\">↓</span>\r\n\r\n<span style=\"color: #333333;\">不过我没有这样做，在 header.php 中插入大量的代码会让博客显得很臃肿，虽然这样能达到目的。我的建议是把js做cdn加速，然后使用wordpress自带的函数进行注册排队后正确引用，该方法在wordpress大学有明确的帮助文档：<a href=\"https://www.wpdaxue.com/wordpress-include-jquery-css.html\" target=\"_blank\" rel=\"noopener\"><span style=\"color: #00ccff;\">https://www.wpdaxue.com/wordpress-include-jquery-css.html</span></a></span>\r\n\r\n<span style=\"color: #ff99cc;\">&lt;跳往wordpress大学的超链接↑&gt;</span>\r\n<blockquote><span style=\"color: #ff0000;\">PS：使用AdvancedAjaxPageLoader插件实现的全站ajax文章教程目前已经归档，不再进行维护。</span></blockquote>'),(12368,'DevelopersLab 计划','标准文章','<img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/DevelopersLab.png\" width=\"630\" height=\"159\" />\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">DevelopersLab计划</span></li>\r\n</ul>\r\n在该计划下，许多具有代表性的项目被纳入，并不断加以完善，最新的研究成果与DevelopersLab计划是相互关联的。\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">开放性的源代码</span></li>\r\n</ul>\r\n先进的思路被更多人使用并加以二次开发，这并不开放所有源代码，它提供最基本的内容给有基础的开发者。\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">可靠的支持</span></li>\r\n</ul>\r\nDevelopersLab项目组是活跃的，它每天都准备纳入新的内容\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">安全性</span></li>\r\n</ul>\r\nDevelopersLab项目是对外保密的，它不会特意推广，只在必要时开源和发布\r\n<ul>\r\n 	<li><span style=\"color: #ff9900;\">高标准</span></li>\r\n</ul>\r\n所有的基准测试都将进行\r\n\r\n&nbsp;\r\n<blockquote><a href=\"https://github.com/Thaumy/DevelopersLab\" target=\"blank\"><span style=\"color: #00ccff;\">https://github.com/Thaumy/DevelopersLab</span></a>\r\n\r\n<span style=\"color: #ff9900;\">&lt;跳往GitHub的超链接↑&gt;</span></blockquote>'),(12369,'Arduino单片机开发感想','标准文章','<img class=\"alignnone size-medium\" src=\"https://thaumy.github.io/MyBlog/img/article/UNOR3%E5%8C%85%E8%A3%85%E7%9B%92(%E5%8E%8B%E7%BC%A9).JPG\" width=\"650\" height=\"632\" />\r\n<blockquote><span style=\"color: #ff9900;\">步入高中以后，时间很是紧张，很少有机会接触程序了。因为宿舍没有闹钟的问题，忽然就想到了单片机开发。并希望借此过程学习一下单片机知识，客制化一个闹钟。那么故事就此展开。</span></blockquote>\r\n<ul>\r\n 	<li><span style=\"color: #000000;\"><strong>首</strong>先来说说为什么选择<a href=\"https://baike.baidu.com/item/Arduino/9362389?fr=aladdin\" target=\"_blank\" rel=\"noopener\"><span style=\"color: #0000ff;\">Arduino</span></a>。</span>单片机的选择有很多种，但是我能耳熟能详的也就是51（<a href=\"https://baike.baidu.com/item/stc89c51/2895810?fr=aladdin\" target=\"_blank\" rel=\"noopener\"><span style=\"color: #0000ff;\">STC89C51</span></a>，以下简称51）和Arduino系列。经过详细查阅，发现51貌似需要更高的硬件基础（例如汇编一类），而Arduino则入门门槛较低。根据实际情况考虑，51的专业水平在目前的阶段我是不具备也是没有能力具备的，虽然选择51可能能让我更好的了解IC的运行机制与原理。最终考虑到学业和时间上的不允许，我购买了我的第一块Arduino开发板：<span style=\"color: #000000;\">Arduino UNO R3</span>（以下简称UNO R3或UNO）。</li>\r\n</ul>\r\n&nbsp;\r\n<ul>\r\n 	<li><span style=\"color: #000000;\"><strong>A</strong>rduino系列有开发板可供选择。</span>选择UNO R3时我经过了详尽考虑，在网上查阅资料得知UNO R3这款开发板有很多例程和项目，很适合初学者使用。知乎上的几位大佬也推荐使用UNO R3。相较于其他开发板，UNO的优点是它较为中庸的性能和配置（集成少，io定义合理），对于进一步学习其他型号的Arduino开发板能打下更好的基层经验。所以我选择了这款开发板（受到以前隔壁群的一位硬件大佬使用<a href=\"https://baike.baidu.com/item/Mega2560/973064?fr=aladdin\" target=\"_blank\" rel=\"noopener\"><span style=\"color: #0000ff;\">MEGA2560</span></a>的影响，我一开始很想搞一块MEGA2560来玩玩）。开箱之后，包装盒上的“我们热爱开源”字样深深地吸引了我（我也比较倾向于开源，并且想让自己做出的开源项目使大家受益），更加增益了我对Arduino的兴趣。拿到开发板，我的第一印象是UNO很小巧，大概比我预想的小了半倍。随之则是商家附加的一些入门原件：传感器、数码管、各色的发光二极管、面包板、光盘······虽然为数不多，但是对于我的入门探索已经是一个很广阔的空间了。</li>\r\n</ul>\r\n&nbsp;\r\n<ul>\r\n 	<li><strong>那</strong>么再来谈谈开发过程。由于对于单片机的不了解，我首先看了一些商家赠送的光盘例程。然后根据教程上机试验了一下程序，发现Arduino的开发并不像我想象中的那样困难。在掌握具体的原件针脚定义后，软件层面的代码编写简直如鱼得水。Arduino为开发者提供了多种开发方式，官方的ArduinoIDE使用C语言进行开发，并不需要涉及硬件底层知识。因为对C的接触有了一段时间，我并没有出现什么语言上的困难（这也是我选择Arduino的原因），反倒是一些电学的物理知识和各种排线的接法搞得我很头疼（在接四位数码管的时候我竟然接了半小时才成功）。虽然过程比较复杂，但是学起来还是很带劲的。要知道这软件编程还是有所不同，面向过程的C要比面向对象的C#简单上一个层次（不敢轻易断言，在难易程度上的判断只是相对于个人以往的开发经验来看，有望大佬评论赐教），这使得整个过程轻松而不失活力，也让我明白了有人把Arduino比作“电子积木”的原因。ArdunioIDE集成了许多基层的Arduino库，这使得你不需要涉及过多的底层代码，而专注于你的开发目标。不用涉及复杂的汇编和底层逻辑相信是很多硬件开发者的梦想，Arduino则很好的做到了这一点。扩大编程粒度、淡化细节过程的高级语言起到了关键性的作用，它为我们节省了大量的开发时间。硬件编程给我打开了新的思绪，它不像软件编程一样在解决某种程度上的bug时没有头绪。在这里更多的则是逻辑上的问题，将程序的运行过程在眼前实例化使我在调试中倍感轻松。</li>\r\n</ul>\r\n&nbsp;\r\n\r\nArduino在功耗方面还是十分优秀的(貌似这是单片机的共性)，它相对强大的性能足以满足我现阶段的学习。上万的烧写次数在某种程度上给我节省了些许钱财（穷）。文章的最后，我希望能把这块板子利用好，进一步提升我的知识层次。\r\n\r\n&nbsp;\r\n<p style=\"text-align: right;\"><span style=\"color: #ff6600;\">百忙之中抽出闲暇撰写这篇博文，望路过大佬多多赐教。</span></p>\r\n<p style=\"text-align: right;\"><span style=\"color: #000000;\">以上，Thaumy。</span></p>'),(12371,'博客重构计划EP03','本博客正处于BETA开发版，所有博客文章内容均不维护','<span style=\"color: #000000;\">很久没有撰写博文了。</span>\r\n\r\n<span style=\"color: #333333;\">        本打算通过学习php来完善我的wordpress博客，可是在实际进行的过程中发现计划并不是那样理想。我的博客主题中有诸多bug，我尝试解决它们，但是由于技术基础差劲导致的计划滞留使我没有过多精力进行一次次“碰巧”的测试。出于对C#的了解程度较于其他语言更高，我便产生了一个的大胆的想法——使用ASP.NET重构博客。</span>\r\n\r\n<span style=\"color: #000000;\">首先标明一下，本项目开源，任何人都可以获得该项目的基础源代码并加以完善。通过开源，我希望能给更多有此想法的博友们一个出力的机会，共同构建安全、易用、高效的博客系统。</span>\r\n<ul>\r\n 	<li>        新的博客在设计思路上采用三层架构，并尽量避免复杂的逻辑关系，使后续的二次开发更方便。在展示层上思路不变，继续采用HTML+CSS+JS的方案，通过StdLibx进行业务层和数据层的处理与分析，最终反馈视图（怎么感觉是MVC?算了不管它是什么架构。。）在博客编辑上，采用客户端的形式对博客文章进行编辑，我认为这可以一定程度上避免过多的网页数据加载，分担服务器压力，让服务器更注重于与访客的交互上。当然，某些选项还是会出现在博客的设置页面上，这是出于对开发便捷的需要。这是与主流网站系统最不同的地方。</li>\r\n</ul>\r\n这就是大体的想法，如果有什么好的提议，请在下方留言。\r\n\r\n<span style=\"color: #ff9900;\">这是新的ASP.NET博客地址：</span><a href=\"http://asp.thaumy.link\"><span style=\"color: #0000ff;\">asp.thaumy.link </span></a>\r\n\r\n<span style=\"color: #ff9900;\">如果你有兴趣的话，不妨来看看我的进度，虽然大部分时间我会从事在StdLib1.09的开发中。</span>\r\n\r\n<span style=\"color: #ff6600;\">另外在此注明，本博客除文章问题外，将不会有任何更新或修正出现。等待新的博客开发完成之后，本博客内容将会陆续转载到新的博客之中。若有其他内容，恕不另行通知。</span>\r\n<p style=\"text-align: right;\">以上</p>\r\n<p style=\"text-align: right;\">Thaumy</p>\r\n<p style=\"text-align: right;\">2018.2.19</p>\r\n&nbsp;'),(12373,'StdLib1.09#public现可用','标准文章','<img class=\"aligncenter\" src=\"https://thaumy.github.io/MyBlog/img/article/StdLib1.09宣传画(压缩).jpg\" alt=\"\" width=\"600\" height=\"339\" />\r\n\r\n<span style=\"color: #00ccff;\">经过一系列的调试和准备工作以及无数次的跳坑，StdLib系列终于迎来了第7个发行版。介是个里没有碗过的船新版本，挤需体验三翻中，里造会干我一样，爱象介个StdLib。</span>\r\n<span style=\"color: #333333;\">那么话不多说，在本次更新中，我们增添了对MySQL数据库的便捷访问器，以应对在将来的博客领域开发。这也确定了StdLib的新方向是博客的开发工作。</span>\r\n<ul>\r\n 	<li><span style=\"color: #800000;\">下面让我们看看增加了什么新特性:</span></li>\r\n</ul>\r\n<h1 style=\"text-align: center;\"><span style=\"color: #ff6600;\"><span style=\"color: #000000;\">&lt;</span>1<span style=\"color: #000000;\">&gt;</span></span></h1>\r\n<blockquote>\r\n<p style=\"text-align: left;\"><span style=\"color: #800080;\">namespace Data=&gt;MySqlConnectionHandler类：</span>\r\n该类提供对MySQL数据库的访问工作，它包括：查询，取表，取字段，建立连接等一系列操作方法。\r\n该类多数方法的第二重载可以使用新增的connStr结构接受参数以建立连接。</p>\r\n</blockquote>\r\n<h1 style=\"text-align: center;\"><span style=\"color: #000000;\">&lt;<span style=\"color: #ff6600;\">2</span>&gt;</span></h1>\r\n<blockquote><span style=\"color: #800080;\">Display命名空间：</span>\r\n该命名空间负责前端的输出工作，是视图控制器。\r\n目前仅限于网页控制，后续可能会增加更多新特性，例如WPF控制。</blockquote>\r\n<h1 style=\"text-align: center;\">&lt;<span style=\"color: #ff6600;\">3</span>&gt;</h1>\r\n<blockquote><span style=\"color: #800080;\">namespace Display=&gt;Web命名空间：</span>\r\n\r\n该命名空间负责网页前端的输出工作，是网页的视图控制器。</blockquote>\r\n<h1 style=\"text-align: center;\">&lt;<span style=\"color: #ff6600;\">4</span>&gt;</h1>\r\n<blockquote>\r\n<p style=\"text-align: left;\"><span style=\"color: #800080;\">namespace Display\\Web=&gt;Post类：</span>\r\n该类提供契合MySqlConnectionHandler类的网页输出工具，目前只能分析整理并输出字符串。\r\n该类使用PostData结构进行装载每个文章的信息。</p>\r\n</blockquote>\r\n&nbsp;\r\n<ul>\r\n 	<li><span style=\"color: #800000;\">请于GitHub下载这一类库，进行上机应用。(若图片中的二维码无法扫描，请右键它并在新标签页中打开该图片进行扫描)</span></li>\r\n</ul>\r\n<span style=\"color: #33cccc;\">在StdLib1.1系列中，我们会对StdLib在网页支持上的应用进行全方位的变革，以建立一个高可用、可拓展、方便安全的网页架构。</span>'),(12374,'噼里啪啦BETA4已经部署','我有史以来bug最多的博客系统，于中秋佳节发布','THAUMY的博客现已更新至噼里啪啦BETA4，开发代号：HeavenlyBlue'),(12375,'噼里啪啦BETA5已经部署','代号“RETURNV”','beta5更新内容\r\n后台代码进一步优化，简化了站点的云部流程\r\n将代码整合分类，使得webiob清晰明了\r\n使用stdlib1.12进行建构，提供优于1.11版本的逻辑架构和允许效率。\r\n新增pala root与pala user支持，用以操作啪啦元数据和用户数据\r\n修改了部分注释和名称，现版本逻辑的构建优于历史最高\r\npalaDB的命名进一步规范\r\n新增菜单“pilipala计划”\r\n');
/*!40000 ALTER TABLE `pala_text_main` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pala_text_sub`
--

DROP TABLE IF EXISTS `pala_text_sub`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pala_text_sub` (
  `text_id` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `text_class` varchar(8) DEFAULT '未归档',
  `text_editor` char(32) DEFAULT '1000',
  `date_created` datetime DEFAULT NULL,
  `date_changed` datetime DEFAULT NULL,
  `count_pv` int(5) unsigned DEFAULT '7',
  `count_comment` int(5) unsigned DEFAULT '0',
  `count_like` int(5) unsigned DEFAULT '2',
  `tags` varchar(128) DEFAULT '特征A',
  `cover_url` varchar(128) DEFAULT NULL,
  `strip_color` varchar(3) DEFAULT '',
  PRIMARY KEY (`text_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12376 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pala_text_sub`
--

LOCK TABLES `pala_text_sub` WRITE;
/*!40000 ALTER TABLE `pala_text_sub` DISABLE KEYS */;
INSERT INTO `pala_text_sub` VALUES (12347,'技术归档','ThaumyCheng','2017-01-15 06:18:32','2017-08-05 13:33:45',1,0,2,'噼里啪啦$2233',NULL,'org'),(12348,'项目','ThaumyCheng','2017-01-21 11:40:19','2017-07-23 14:27:05',7,0,2,'演示$测试',NULL,'org'),(12349,'项目','ThaumyCheng','2017-02-18 06:04:57','2017-07-23 14:24:41',7,0,2,'吃驚$演示$测试$噼里啪啦','','org'),(12350,'项目','ThaumyCheng','2017-04-21 23:05:03','2017-07-23 14:23:10',7,0,2,'吃驚$演示$测试','img/txtbg2.svg','org'),(12351,'项目','ThaumyCheng','2017-05-20 04:24:44','2017-10-28 15:49:54',7,0,2,'演示$测试',NULL,'org'),(12353,'技术归档','ThaumyCheng','2017-06-07 11:42:43','2017-07-12 10:13:30',1,0,2,'2233',NULL,'prp'),(12356,'技术归档','ThaumyCheng','2017-06-17 04:16:29','2017-07-12 10:13:21',7,0,2,'演示$测试',NULL,'prp'),(12357,'项目','ThaumyCheng','2017-03-17 04:21:57','2017-07-23 14:23:59',7,0,2,'测试',NULL,'prp'),(12358,'生涯归档','ThaumyCheng','2016-11-05 08:28:02','2017-06-17 05:26:13',7,0,2,'吃驚$演示$测试',NULL,'prp'),(12359,'技术归档','ThaumyCheng','2017-06-18 00:00:01','2017-07-12 10:13:16',7,0,2,'吃驚',NULL,'prp'),(12360,'技术归档','ThaumyCheng','2017-07-04 05:35:39','2017-07-26 09:45:45',7,0,2,'噼里啪啦$2233',NULL,'prp'),(12363,'生涯归档','ThaumyCheng','2016-12-11 02:17:22','2017-07-12 10:14:00',7,0,2,'吃驚$测试$噼里啪啦$2233',NULL,'prp'),(12364,'技术归档','ThaumyCheng','2017-07-11 03:10:51','2017-07-12 10:13:06',1,0,2,'测试$噼里啪啦',NULL,'prp'),(12365,'项目','ThaumyCheng','2017-07-21 02:24:07','2017-08-15 03:46:00',7,0,2,'吃驚$演示$测试',NULL,'prp'),(12366,'技术归档','ThaumyCheng','2017-07-26 09:16:30','2018-04-14 22:53:20',1,0,2,'测试$噼里啪啦',NULL,'blu'),(12368,'A&M归档','ThaumyCheng','2017-05-27 14:15:13','2017-10-04 07:30:24',7,0,2,'噼里啪啦$2233',NULL,'blu'),(12369,'技术归档','ThaumyCheng','2017-12-02 07:06:14','2018-04-14 22:52:19',7,0,2,'吃驚$演示$噼里啪啦$2233',NULL,'blu'),(12371,'技术归档','ThaumyCheng','2018-02-19 03:02:29','2018-04-14 22:52:14',1,0,2,'测试$噼里啪啦',NULL,'blu'),(12373,'项目','ThaumyCheng','2018-04-05 22:46:31','2018-04-14 22:55:40',7,0,2,'吃驚$演示$测试',NULL,'blu'),(12374,'技术归档','ThaumyCheng','2018-09-24 09:58:06','2018-09-24 09:58:10',7,0,2,'吃驚$演示$测试$2233',NULL,'blu'),(12375,'技术归档','ThaumyCheng','2018-10-21 07:12:14','2018-10-21 07:12:14',7,0,2,'演示$测试',NULL,'blu');
/*!40000 ALTER TABLE `pala_text_sub` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `view_text>index.page`
--

DROP TABLE IF EXISTS `view_text>index.page`;
/*!50001 DROP VIEW IF EXISTS `view_text>index.page`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_text>index.page` AS SELECT 
 1 AS `text_id`,
 1 AS `text_mode`,
 1 AS `text_type`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_text>index.post`
--

DROP TABLE IF EXISTS `view_text>index.post`;
/*!50001 DROP VIEW IF EXISTS `view_text>index.post`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_text>index.post` AS SELECT 
 1 AS `text_id`,
 1 AS `text_mode`,
 1 AS `text_type`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_text>main`
--

DROP TABLE IF EXISTS `view_text>main`;
/*!50001 DROP VIEW IF EXISTS `view_text>main`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_text>main` AS SELECT 
 1 AS `text_id`,
 1 AS `text_title`,
 1 AS `text_summary`,
 1 AS `text_content`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_text>sub`
--

DROP TABLE IF EXISTS `view_text>sub`;
/*!50001 DROP VIEW IF EXISTS `view_text>sub`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_text>sub` AS SELECT 
 1 AS `text_id`,
 1 AS `text_class`,
 1 AS `text_editor`,
 1 AS `date_created`,
 1 AS `date_changed`,
 1 AS `count_pv`,
 1 AS `count_comment`,
 1 AS `count_like`,
 1 AS `tags`,
 1 AS `cover_url`,
 1 AS `strip_color`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'pala_database'
--

--
-- Dumping routines for database 'pala_database'
--
/*!50003 DROP FUNCTION IF EXISTS `random$number` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` FUNCTION `random$number`( start_num INTEGER, end_num INTEGER ) RETURNS int(11)
BEGIN
/* 在指定范围内抓取一个整数随机数并返回 */
RETURN FLOOR( start_num + RAND( ) * ( end_num - start_num + 1 ) );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_root` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` PROCEDURE `get_root`(IN `root_id` int)
BEGIN
	#Routine body goes here...
	
	SELECT
*
FROM
	`pala_root` 
WHERE
	( `pala_root`.`root_id` = root_id) ;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_text>main` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` PROCEDURE `get_text>main`(IN `text_id` int)
BEGIN
	#Routine body goes here...
	
	SELECT
*
FROM
	`view_text>main` 
WHERE
	( `view_text>main`.`text_id` = text_id) ;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `get_text>sub` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` PROCEDURE `get_text>sub`(IN `text_id` int)
BEGIN
	#Routine body goes here...
	
	SELECT
*
FROM
	`view_text>sub` 
WHERE
	( `view_text>sub`.`text_id` = text_id) ;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `random_text>index.page` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` PROCEDURE `random_text>index.page`(excluded_text_id INTEGER)
BEGIN

SELECT
*
FROM
    `pala_text_index`
WHERE
    ((`pala_text_index`.`text_id` >= ((((SELECT 
            MAX(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`) - (SELECT 
            MIN(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`)) * RAND()) + (SELECT 
            MIN(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`)))
						/* 展示可用 */
        AND (`pala_text_index`.`text_mode` = 'onshow')
				
				/* 排除不参与随机取样的文章 */
        AND (`pala_text_index`.`text_id` <> excluded_text_id)
				
				AND (`pala_text_index`.`text_type` = 'page'))
				/* 从第一位置取一条记录 */
LIMIT 0 , 1;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `random_text>index.post` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`pala_database_user`@`localhost` PROCEDURE `random_text>index.post`(excluded_text_id INTEGER)
BEGIN

SELECT
*
FROM
    `pala_text_index`
WHERE
    ((`pala_text_index`.`text_id` >= ((((SELECT 
            MAX(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`) - (SELECT 
            MIN(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`)) * RAND()) + (SELECT 
            MIN(`pala_text_index`.`text_id`)
        FROM
            `pala_text_index`)))
						/* 展示可用 */
        AND (`pala_text_index`.`text_mode` = 'onshow')
				
				/* 排除不参与随机取样的文章 */
        AND (`pala_text_index`.`text_id` <> excluded_text_id)
				
				AND (`pala_text_index`.`text_type` = 'post'))
				/* 从第一位置取一条记录 */
LIMIT 0 , 1;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_text>index.page`
--

/*!50001 DROP VIEW IF EXISTS `view_text>index.page`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`pala_database_user`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_text>index.page` AS select `pala_text_index`.`text_id` AS `text_id`,`pala_text_index`.`text_mode` AS `text_mode`,`pala_text_index`.`text_type` AS `text_type` from `pala_text_index` where ((`pala_text_index`.`text_type` like 'page') and (`pala_text_index`.`text_mode` = 'onshow')) order by `pala_text_index`.`text_id` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_text>index.post`
--

/*!50001 DROP VIEW IF EXISTS `view_text>index.post`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`pala_database_user`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_text>index.post` AS select `pala_text_index`.`text_id` AS `text_id`,`pala_text_index`.`text_mode` AS `text_mode`,`pala_text_index`.`text_type` AS `text_type` from `pala_text_index` where ((`pala_text_index`.`text_type` like 'post') and (`pala_text_index`.`text_mode` = 'onshow')) order by `pala_text_index`.`text_id` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_text>main`
--

/*!50001 DROP VIEW IF EXISTS `view_text>main`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`pala_database_user`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_text>main` AS select `pala_text_main`.`text_id` AS `text_id`,`pala_text_main`.`text_title` AS `text_title`,`pala_text_main`.`text_summary` AS `text_summary`,`pala_text_main`.`text_content` AS `text_content` from `pala_text_main` order by `pala_text_main`.`text_id` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_text>sub`
--

/*!50001 DROP VIEW IF EXISTS `view_text>sub`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`pala_database_user`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_text>sub` AS select `pala_text_sub`.`text_id` AS `text_id`,`pala_text_sub`.`text_class` AS `text_class`,`pala_text_sub`.`text_editor` AS `text_editor`,`pala_text_sub`.`date_created` AS `date_created`,`pala_text_sub`.`date_changed` AS `date_changed`,`pala_text_sub`.`count_pv` AS `count_pv`,`pala_text_sub`.`count_comment` AS `count_comment`,`pala_text_sub`.`count_like` AS `count_like`,`pala_text_sub`.`tags` AS `tags`,`pala_text_sub`.`cover_url` AS `cover_url`,`pala_text_sub`.`strip_color` AS `strip_color` from `pala_text_sub` order by `pala_text_sub`.`text_id` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-02-07  9:55:40
