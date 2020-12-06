using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.com.pilipala.Components;

namespace palaBenchmark
{
    class Benchmark
    {
        public CORE CORE;
        public Reader Reader = new Reader();
        public Writer Writer = new Writer();

        public void INIT()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Views = new PLViews() { PosUnion = "pos>dirty>union", NegUnion = "neg>dirty>union" },
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
            CORE = new CORE(PLDB, "Pinn", "pinn12384");

            CORE.SetTables();

            CORE.LinkOn += Reader.Ready;
            CORE.LinkOn += Writer.Ready;

            CORE.Run();
        }
    }
}
