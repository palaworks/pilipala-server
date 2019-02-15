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
 1 AS `date_created`,
 1 AS `count_read`,
 1 AS `count_review`,
 1 AS `count_like`,
 1 AS `tagA`,
 1 AS `tagB`,
 1 AS `tagC`,
 1 AS `color_strip`*/;
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
 1 AS `post_title`,
 1 AS `count_read`,
 1 AS `count_review`,
 1 AS `color_strip`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_pst_post_count`
--

DROP TABLE IF EXISTS `view_pst_post_count`;
/*!50001 DROP VIEW IF EXISTS `view_pst_post_count`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_pst_post_count` AS SELECT 
 1 AS `post_id`,
 1 AS `count_read`,
 1 AS `count_review`,
 1 AS `count_like`*/;
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
 1 AS `date_changed`,
 1 AS `count_read`,
 1 AS `count_review`,
 1 AS `count_like`,
 1 AS `tagA`,
 1 AS `tagB`,
 1 AS `tagC`,
 1 AS `color_strip`*/;
SET character_set_client = @saved_cs_client;

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
/*!50013 DEFINER=`thaumy01mysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_comn_plst` AS select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,`st_posts`.`post_summary` AS `post_summary`,`st_posts`.`post_archive` AS `post_archive`,`st_posts`.`date_created` AS `date_created`,`st_posts`.`count_read` AS `count_read`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`count_like` AS `count_like`,`st_posts`.`tagA` AS `tagA`,`st_posts`.`tagB` AS `tagB`,`st_posts`.`tagC` AS `tagC`,`st_posts`.`color_strip` AS `color_strip` from `st_posts` where ((`st_posts`.`post_position` like 'comn') and (`st_posts`.`post_isShow` = 1)) order by `st_posts`.`date_created` desc */;
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
/*!50013 DEFINER=`thaumy01mysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_menu_plst` AS select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title` from `st_posts` where ((`st_posts`.`post_position` like 'menu') and (`st_posts`.`post_isShow` = 1)) */;
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
/*!50013 DEFINER=`thaumy01mysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_idx_top_plst` AS select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,`st_posts`.`count_read` AS `count_read`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`color_strip` AS `color_strip` from `st_posts` where ((`st_posts`.`post_position` like 'top') and (`st_posts`.`post_isShow` = 1)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_pst_post_count`
--

/*!50001 DROP VIEW IF EXISTS `view_pst_post_count`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`thaumy01mysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_pst_post_count` AS select `st_posts`.`post_id` AS `post_id`,`st_posts`.`count_read` AS `count_read`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`count_like` AS `count_like` from `st_posts` where (`st_posts`.`post_isShow` = 1) */;
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
/*!50013 DEFINER=`thaumy01mysql`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_pst_post_data` AS select `st_posts`.`post_id` AS `post_id`,`st_posts`.`post_title` AS `post_title`,`st_posts`.`post_summary` AS `post_summary`,`st_posts`.`post_content` AS `post_content`,`st_posts`.`post_archive` AS `post_archive`,`st_posts`.`post_isReadOnly` AS `post_isReadOnly`,`st_posts`.`date_created` AS `date_created`,`st_posts`.`date_changed` AS `date_changed`,`st_posts`.`count_read` AS `count_read`,`st_posts`.`count_review` AS `count_review`,`st_posts`.`count_like` AS `count_like`,`st_posts`.`tagA` AS `tagA`,`st_posts`.`tagB` AS `tagB`,`st_posts`.`tagC` AS `tagC`,`st_posts`.`color_strip` AS `color_strip` from `st_posts` where (`st_posts`.`post_isShow` = 1) */;
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

-- Dump completed on 2018-08-30 20:24:21
