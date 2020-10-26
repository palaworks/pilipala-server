using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.PostKey;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;

namespace PalaBenchmarkFiller
{
    class Benchmark
    {
        public PLSYS PLSYS;
        public PLDR PLDR;
        public PLDU PLDU;

        public void Benchmark_INIT()
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

            /* 初始化数据库连接 */
            PLSYS.DBCHINIT(new MySqlConn
            {
                /* 基准测试数据集 */
                DataSource = "localhost",
                DataBase = "pilipala_benchmark",
                Port = "3306",
                User = "root",
                PWD = "65a1561425f744e2b541303f628963f8"
            });

            PLDR = new PLDR(PLSYS);
            PLDU = new PLDU(PLSYS);
        }
    }
}
