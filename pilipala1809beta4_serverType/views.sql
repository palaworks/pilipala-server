-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: dbname
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
-- Temporary view structure for view `view_idx_comn_plst`
--

DROP TABLE IF EXISTS `view_idx_comn_plst`;
/*!50001 DROP VIEW IF EXISTS `view_idx_comn_plst`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_idx_comn_plst` AS SELECT 
 1 AS `post_id`,
 1 AS `post_title`,
 1 AS `post_summary`,
 1 AS `post_archive`,
 1 AS `date_created`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_idx_menu_plst`
--

DROP TABLE IF EXISTS `view_idx_menu_plst`;
/*!50001 DROP VIEW IF EXISTS `view_idx_menu_plst`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_idx_menu_plst` AS SELECT 
 1 AS `post_id`,
 1 AS `post_title`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_idx_top_plst`
--

DROP TABLE IF EXISTS `view_idx_top_plst`;
/*!50001 DROP VIEW IF EXISTS `view_idx_top_plst`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_idx_top_plst` AS SELECT 
 1 AS `post_id`,
 1 AS `post_title`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_pala_root`
--

DROP TABLE IF EXISTS `view_pala_root`;
/*!50001 DROP VIEW IF EXISTS `view_pala_root`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_pala_root` AS SELECT 
 1 AS `plan_id`,
 1 AS `site_finalUrl`,
 1 AS `site_devUrl`,
 1 AS `site_isDev`,
 1 AS `site_isOpen`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_pst_post_data`
--

DROP TABLE IF EXISTS `view_pst_post_data`;
/*!50001 DROP VIEW IF EXISTS `view_pst_post_data`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_pst_post_data` AS SELECT 
 1 AS `post_id`,
 1 AS `post_title`,
 1 AS `post_summary`,
 1 AS `post_content`,
 1 AS `post_archive`,
 1 AS `post_isReadOnly`,
 1 AS `date_created`,
 1 AS `date_changed`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'dbname'
--

--
-- Dumping routines for database 'dbname'
--
/*!50003 DROP FUNCTION IF EXISTS `random_num` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`thaumymysql`@`localhost` FUNCTION `random_num`( start_num INTEGER, end_num INTEGER ) RETURNS int(11)
BEGIN
/* 在指定范围内抓取一个整数随机数并返回 */
RETURN FLOOR( start_num + RAND( ) * ( end_num - start_num + 1 ) );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `random_post` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`thaumymysql`@`localhost` PROCEDURE `random_post`(unrandom_post_id INTEGER)
BEGIN

/* 定义开始取样位置变量 */
DECLARE startplace INT;
/* 随机开始取样位置，以保证结果更加随机 */
set startplace=random_num(1,7);


SELECT 
    `pala_posts`.`post_id` AS `post_id`,
    `pala_posts`.`post_title` AS `post_title`
FROM
    `pala_posts`
WHERE
    ((`pala_posts`.`post_id` >= ((((SELECT 
            MAX(`pala_posts`.`post_id`)
        FROM
            `pala_posts`) - (SELECT 
            MIN(`pala_posts`.`post_id`)
        FROM
            `pala_posts`)) * RAND()) + (SELECT 
            MIN(`pala_posts`.`post_id`)
        FROM
            `pala_posts`)))
						/* 展示可用 */
        AND (`pala_posts`.`post_isShow` = 1)
				
				/* 排除不参与随机取样的文章 */
        AND (`pala_posts`.`post_id` <> unrandom_post_id))
				
				/* 从随机位置取1条记录 */
LIMIT startplace , 1;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_idx_comn_plst`
--

/*!50001 DROP VIEW IF EXISTS `view_idx_comn_plst`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumymysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_comn_plst` AS select `pala_posts`.`post_id` AS `post_id`,`pala_posts`.`post_title` AS `post_title`,`pala_posts`.`post_summary` AS `post_summary`,`pala_posts`.`post_archive` AS `post_archive`,`pala_posts`.`date_created` AS `date_created` from `pala_posts` where ((`pala_posts`.`post_position` like 'comn') and (`pala_posts`.`post_isShow` = 1)) order by `pala_posts`.`date_created` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_idx_menu_plst`
--

/*!50001 DROP VIEW IF EXISTS `view_idx_menu_plst`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumymysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_menu_plst` AS select `pala_posts`.`post_id` AS `post_id`,`pala_posts`.`post_title` AS `post_title` from `pala_posts` where ((`pala_posts`.`post_position` like 'menu') and (`pala_posts`.`post_isShow` = 1)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_idx_top_plst`
--

/*!50001 DROP VIEW IF EXISTS `view_idx_top_plst`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumymysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_top_plst` AS select `pala_posts`.`post_id` AS `post_id`,`pala_posts`.`post_title` AS `post_title` from `pala_posts` where ((`pala_posts`.`post_position` like 'top') and (`pala_posts`.`post_isShow` = 1)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_pala_root`
--

/*!50001 DROP VIEW IF EXISTS `view_pala_root`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumymysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_pala_root` AS select `pala_root`.`plan_id` AS `plan_id`,`pala_root`.`site_finalUrl` AS `site_finalUrl`,`pala_root`.`site_devUrl` AS `site_devUrl`,`pala_root`.`site_isDev` AS `site_isDev`,`pala_root`.`site_isOpen` AS `site_isOpen` from `pala_root` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_pst_post_data`
--

/*!50001 DROP VIEW IF EXISTS `view_pst_post_data`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumymysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_pst_post_data` AS select `pala_posts`.`post_id` AS `post_id`,`pala_posts`.`post_title` AS `post_title`,`pala_posts`.`post_summary` AS `post_summary`,`pala_posts`.`post_content` AS `post_content`,`pala_posts`.`post_archive` AS `post_archive`,`pala_posts`.`post_isReadOnly` AS `post_isReadOnly`,`pala_posts`.`date_created` AS `date_created`,`pala_posts`.`date_changed` AS `date_changed` from `pala_posts` where (`pala_posts`.`post_isShow` = 1) */;
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

-- Dump completed on 2018-09-24 10:59:21
