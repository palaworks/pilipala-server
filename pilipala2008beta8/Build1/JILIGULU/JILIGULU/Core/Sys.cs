using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.CommentLake;

namespace JILIGULU.Core
{
    public class Sys
    {
        public PLSYS PLSYS;
        public PLDR PLDR;
        public PLDU PLDU;
        public PLDC PLCH;
        public CommentLake CommentLake;

        public void INIT()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Tables = PLSYS.DefTables,
                Views = PLSYS.DefViews,
                MySqlManager = new MySqlManager()
            };
            PLSYS = new PLSYS(2, PLDB);

            PLSYS.DefaultSysTables();
            PLSYS.DefaultSysViews();

            MySqlConn MySqlConn = new MySqlConn
            {
                /* 基准测试数据集 */
                DataSource = "localhost",
                DataBase = "pilipala_benchmark",
                Port = "3306",
                User = "root",
                PWD = "65a1561425f744e2b541303f628963f8"
            };

            /* 初始化数据库连接 */
            PLSYS.DBCHINIT(MySqlConn);

            PLDR = new PLDR(PLSYS);
            PLDU = new PLDU(PLSYS);
            PLCH = new PLDC(PLSYS);
            CommentLake = new CommentLake(MySqlConn, "comment_lake");
        }
    }
}