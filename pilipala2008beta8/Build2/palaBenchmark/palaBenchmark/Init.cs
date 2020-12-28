using System;
using System.Collections.Generic;
using System.Text;

using WaterLibrary;
using WaterLibrary.Tools;
using WaterLibrary.MySQL;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Database;
using WaterLibrary.pilipala.Components;

namespace palaBenchmark
{
    class Init
    {

        public Reader Reader;
        public Writer Writer;
        public Counter Counter;
        public CommentLake CommentLake;
        public Authentication Authentication;

        public CORE CORE;
        public User User;
        public ComponentFactory Fac = new();

        public void INIT()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDatabase PLDB = new()
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
            CORE = new CORE(PLDB);

            CORE.SetTables();

            CORE.CoreReady += Fac.Ready;
            User = CORE.Run("1522700134", "pinn12384");

            Reader = Fac.GenReader();
            Writer = Fac.GenWriter();
            Counter = Fac.GenCounter();
            CommentLake = Fac.GenCommentLake();
            Authentication = Fac.GenAuthentication();
        }
    }
}
