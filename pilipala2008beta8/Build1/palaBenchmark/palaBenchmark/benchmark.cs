using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;

namespace palaBenchmark
{
    class Benchmark
    {
        public CORE CORE;
        public PLDR PLDR;
        public PLDU PLDU;

        public void INIT()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                MySqlManager = new MySqlManager(new MySqlConnMsg
                {
                    /* 基准测试数据集 */
                    DataSource = "localhost",
                    DataBase = "pilipala_test",
                    Port = "3306",
                    User = "root",
                    PWD = "65a1561425f744e2b541303f628963f8"
                })
            };
            CORE = new CORE(PLDB);

            CORE.SetTables();
            CORE.SetViews();

            /* 初始化数据库连接 */
            CORE.Run();

            PLDR = new PLDR(CORE);
            PLDU = new PLDU(CORE);
        }
    }
}
