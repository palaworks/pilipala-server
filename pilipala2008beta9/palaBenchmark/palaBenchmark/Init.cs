using System;
using System.Collections.Generic;
using System.Text;

using WaterLibrary;
using WaterLibrary.Utils;
using WaterLibrary.MySQL;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Database;
using WaterLibrary.pilipala.Component;

namespace palaBenchmark
{
    class Init
    {
        public Reader Reader;
        public Counter Counter;
        public CommentLake CommentLake;
        public Auth Auth;

        public PiliPala PiliPala;
        public User User;
        public ComponentFactory Fac;

        public void INIT()
        {
            var PLDatabase = new PLDatabase
            {
                Tables = new
                (
                    "pl_user",
                    "pl_meta",
                    "pl_stack",
                    "pl_archive",
                    "comment_lake"
                ),

                ViewsSet = new
                (
                    new(
                        "pos>union",
                        "neg>union"
                        ),
                    new(
                        "pos>dirty>union",
                        "neg>dirty>union"
                        )
                ),
                MySqlManager = new MySqlManager(
                    new("localhost", 3306, "root", "65a1561425f744e2b541303f628963f8")
                    , "pilipala_benchmark"
                    )
            };


            PiliPala.INIT(PLDatabase);//内核单例初始化

            Fac = new();

            Reader = Fac.GenReader(Reader.ReadMode.CleanRead);
            Counter = Fac.GenCounter();
            CommentLake = Fac.GenCommentLake();
            User = Fac.GenUser("1951327599", "thaumy12384");
            Auth = Fac.GenAuthentication(User);
        }
    }
}
