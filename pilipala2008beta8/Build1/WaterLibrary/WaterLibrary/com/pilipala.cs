using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

using WaterLibrary.com.MySQL;
using WaterLibrary.com.basic;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.stru.pilipala.Post.Property;

namespace WaterLibrary.com.pilipala
{
    /// <summary>
    /// pilipala内核
    /// </summary>
    public class CORE
    {
        /// <summary>
        /// 配件连接委托
        /// </summary>
        /// <param name="CORE">内核对象</param>
        public delegate void LinkEventHandler(CORE CORE);
        /// <summary>
        /// 开始配件连接事件
        /// </summary>
        public event LinkEventHandler LinkOn;

        /// <summary>
        /// 核心表结构
        /// </summary>
        public PLTables Tables { get; private set; }
        /// <summary>
        /// 核心视图结构
        /// </summary>
        public PLViews Views { get; private set; }
        /// <summary>
        /// MySql控制器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        internal readonly string UserName;
        internal readonly string UserPWD;

        /// <summary>
        /// 初始化pilipala内核
        /// </summary>
        /// <param name="PLDB">pilipala数据库信息</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPWD">用户密码</param>
        public CORE(PLDB PLDB, string UserName, string UserPWD)
        {
            MySqlManager = PLDB.MySqlManager;
            SetTables(PLDB.Tables.User, PLDB.Tables.Index, PLDB.Tables.Backup);
            SetViews(PLDB.Views.PosUnion, PLDB.Views.NegUnion);

            /* 用户信息录入 */
            this.UserName = UserName;
            this.UserPWD = UserPWD;
        }

        /// <summary>
        /// 内核启动
        /// </summary>
        /// <returns>返回用户数据</returns>
        public stru.pilipala.User Run()
        {
            MySqlManager.Open();
            string SQL = string.Format("SELECT COUNT(*) FROM {0} WHERE Name = ?UserName AND PWD = ?UserPWD", Tables.User);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "UserName", Val = UserName },
                    new MySqlParm() { Name = "UserPWD", Val = ConvertH.ToMD5(UserPWD) }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                if (MySqlManager.GetKey(MySqlCommand).ToString() == "1")
                {
                    /* 通知所有订阅到当前内核的所有配件内核已经准备完成，并分发内核到各配件 */
                    LinkOn(this);
                    /* 取得用户数据并返回 */
                    DataRow Row = MySqlManager.GetRow(string.Format("SELECT GUID,Bio,`Group`,Email,Avatar FROM {0} WHERE Name = '{1}'", Tables.User, UserName));
                    return new stru.pilipala.User()
                    {
                        Name = UserName,
                        PWD = UserPWD,

                        GUID = Row["GUID"].ToString(),
                        Bio = Row["Bio"].ToString(),
                        Group = Row["Group"].ToString(),
                        Email = Row["Email"].ToString(),
                        Avatar = Row["Avatar"].ToString()
                    };
                }
                else
                {
                    MySqlManager.Close();
                    throw (new Exception("非法的用户签名"));
                }
            }
        }
        /// <summary>
        /// 关闭内核
        /// </summary>
        public void Shutdown()
        {
            MySqlManager.Close();
        }

        /// <summary>
        /// 设置内核所需要的表
        /// </summary>
        /// <param name="User">用户表</param>
        /// <param name="Index">索引表</param>
        /// <param name="Backup">备份表</param>
        /// <param name="Comment">备份表</param>
        public void SetTables(string User = "pl_user", string Index = "pl_index", string Backup = "pl_backup", string Comment = "comment_lake")
        {
            Tables = new PLTables(User, Index, Backup, Comment);

        }
        /// <summary>
        /// 设置内核所需要的视图
        /// </summary>
        /// <param name="PosUnion">积极联合视图</param>
        /// <param name="NegUnion">消极联合视图</param>
        public void SetViews(string PosUnion = "pos>union", string NegUnion = "neg>union")
        {
            Views = new PLViews(PosUnion, NegUnion);
        }
    }

    namespace Components
    {
        /// <summary>
        /// 啪啦数据读取器
        /// </summary>
        public class Reader : IPLComponent
        {
            private PLViews Views { get; set; }
            private MySqlManager MySqlManager { get; set; }

            /// <summary>
            /// 准备读取器
            /// </summary>
            /// <param name="CORE"></param>
            public void Ready(CORE CORE)
            {
                Views = CORE.Views;
                MySqlManager = CORE.MySqlManager;
            }



            /// <summary>
            /// 获取指定文章数据
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public Post GetPost(int ID)
            {
                string SQL = string.Format
                (
                "SELECT * FROM `{0}` WHERE ID={1}"
                , Views.PosUnion, ID
                );

                DataRow Row = MySqlManager.GetRow(SQL);

                return new Post
                {
                    ID = Convert.ToInt32(Row["ID"]),
                    GUID = Convert.ToString(Row["GUID"]),

                    CT = Convert.ToDateTime(Row["CT"]),
                    LCT = Convert.ToDateTime(Row["LCT"]),
                    Title = Convert.ToString(Row["Title"]),
                    Summary = Convert.ToString(Row["Summary"]),
                    Content = Convert.ToString(Row["Content"]),

                    Archiv = Convert.ToString(Row["Archiv"]),
                    Label = Convert.ToString(Row["Label"]),
                    Cover = Convert.ToString(Row["Cover"]),

                    Mode = Convert.ToString(Row["Mode"]),
                    Type = Convert.ToString(Row["Type"]),
                    User = Convert.ToString(Row["User"]),

                    UVCount = Convert.ToInt32(Row["UVCount"]),
                    StarCount = Convert.ToInt32(Row["StarCount"])
                };

            }
            /// <summary>
            /// 取得指定文章属性
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public dynamic GetProperty<T>(int ID) where T : IProperty
            {
                string SQL = string.Format
                    (
                    "SELECT {0} FROM `{1}` WHERE ID = ?ID"
                    , typeof(T).Name, Views.PosUnion
                    );

                List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "ID", Val = ID }
                };

                using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
                {
                    return MySqlManager.GetKey(MySqlCommand);
                }
            }

            /// <summary>
            /// 获取文章数据
            /// </summary>
            /// <typeparam name="T">正则表达式匹配的属性类型</typeparam>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="IncludeNeg">是否包含消极文章(备份)</param>
            /// <returns></returns>
            public List<Post> GetPost<T>(string REGEXP, bool IncludeNeg = false) where T : IProperty
            {
                string SQL;
                if (IncludeNeg == false)
                {
                    SQL = string.Format
                    (
                    "SELECT * FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY CT DESC"
                    , Views.PosUnion, typeof(T).Name
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT * FROM `{0}` WHERE {1} REGEXP ?REGEXP ORDER BY CT DESC"
                    , Views.NegUnion, typeof(T).Name
                    );
                }

                List<Post> List = new List<Post>();

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

                using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
                {
                    foreach (DataRow Row in MySqlManager.GetTable(MySqlCommand).Rows)
                    {
                        List.Add(new Post
                        {
                            ID = Convert.ToInt32(Row["ID"]),
                            GUID = Convert.ToString(Row["GUID"]),

                            CT = Convert.ToDateTime(Row["CT"]),
                            LCT = Convert.ToDateTime(Row["LCT"]),
                            Title = Convert.ToString(Row["Title"]),
                            Summary = Convert.ToString(Row["Summary"]),
                            Content = Convert.ToString(Row["Content"]),

                            Archiv = Convert.ToString(Row["Archiv"]),
                            Label = Convert.ToString(Row["Label"]),
                            Cover = Convert.ToString(Row["Cover"]),

                            Mode = Convert.ToString(Row["Mode"]),
                            Type = Convert.ToString(Row["Type"]),
                            User = Convert.ToString(Row["User"]),

                            UVCount = Convert.ToInt32(Row["UVCount"]),
                            StarCount = Convert.ToInt32(Row["StarCount"])
                        });
                    }
                }

                return List;
            }
            /// <summary>
            /// 获取文章数据
            /// </summary>
            /// <typeparam name="T">正则表达式匹配的属性类型</typeparam>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Properties">所需属性类型</param>
            /// <param name="IncludeNeg">是否包含消极文章(备份)</param>
            /// <returns></returns>
            public List<Post> GetPost<T>(string REGEXP, System.Type[] Properties, bool IncludeNeg = false) where T : IProperty
            {
                /* 键名字符串格式化 */
                string KeysStr = ConvertH.ListToString(Properties, "Name", ',');
                string SQL;
                if (IncludeNeg == false)
                {
                    SQL = string.Format
                    (
                    "SELECT {0} FROM `{1}` WHERE {2} REGEXP ?REGEXP ORDER BY CT DESC"
                    , KeysStr, Views.PosUnion, typeof(T).Name
                    );
                }
                else//显示消极
                {
                    SQL = string.Format
                    (
                    "SELECT {0} FROM `{1}` WHERE {2} REGEXP ?REGEXP ORDER BY CT DESC"
                    , KeysStr, Views.NegUnion, typeof(T).Name
                    );
                }


                List<Post> PostList = new List<Post>();

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

                using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
                {
                    foreach (DataRow Row in MySqlManager.GetTable(MySqlCommand).Rows)
                    {
                        Post Post = new Post();

                        for (int i = 0; i < Properties.Length; i++)
                        {
                            Post[Properties[i].Name] = Row.ItemArray[i];
                        }

                        PostList.Add(Post);
                    }
                }
                return PostList;
            }

            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Bigger<T>(int ID)
            {
                string SQL;

                if (typeof(T) == typeof(ID))/* 对查询ID有优化 */
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE ID=( SELECT min(ID) FROM `{0}` WHERE ID > {1})"
                    , Views.PosUnion, ID
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE ID = {2} ))"
                    , Views.PosUnion, typeof(T).Name, ID
                    );
                }
                object NextID = MySqlManager.GetKey(SQL);

                return NextID == null ? -1 : Convert.ToInt32(NextID);

            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更大的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Property">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Bigger<T>(int ID, string REGEXP, System.Type Property)
            {
                string SQL;
                if (typeof(T) == typeof(ID))
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE ID > {2} AND {3} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, ID, Property.Name
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT min({1}) FROM `{0}` WHERE {1} > ( SELECT {1} FROM `{0}` WHERE ID = ?ID ) AND {2} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, Property.Name
                    );
                }


                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID },
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

                using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
                {
                    object NextID = MySqlManager.GetKey(MySqlCommand);

                    return NextID == null ? -1 : Convert.ToInt32(NextID);
                }
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Smaller<T>(int ID)
            {
                string SQL;

                if (typeof(T) == typeof(ID))/* 对查询ID有优化 */
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE ID=( SELECT max(ID) FROM `{0}` WHERE ID < {1})"
                    , Views.PosUnion, ID
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE ID = {2} ))"
                    , Views.PosUnion, typeof(T).Name, ID
                    );
                }
                object PrevID = MySqlManager.GetKey(SQL);

                return PrevID == null ? -1 : Convert.ToInt32(PrevID);
            }
            /// <summary>
            /// 取得具有比目标文章的指定属性具有更小的值的文章ID
            /// </summary>
            /// <typeparam name="T">指定属性</typeparam>
            /// <param name="ID">目标文章的ID</param>
            /// <param name="REGEXP">正则表达式</param>
            /// <param name="Property">用于被正则表达式筛选的属性</param>
            /// <returns>不存在符合要求的ID时，返回-1</returns>
            public int Smaller<T>(int ID, string REGEXP, System.Type Property)
            {
                string SQL;
                if (typeof(T) == typeof(ID))
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE ID < {2} AND {3} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, ID, Property.Name
                    );
                }
                else
                {
                    SQL = string.Format
                    (
                    "SELECT ID FROM `{0}` WHERE {1}=( SELECT max({1}) FROM `{0}` WHERE {1} < ( SELECT {1} FROM `{0}` WHERE ID = ?ID ) AND {2} REGEXP ?REGEXP )"
                    , Views.PosUnion, typeof(T).Name, Property.Name
                    );
                }

                List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "ID", Val = ID },
                    new MySqlParm() { Name = "REGEXP", Val = REGEXP }
                };

                using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
                {
                    object PrevID = MySqlManager.GetKey(MySqlCommand);

                    return PrevID == null ? -1 : Convert.ToInt32(PrevID);
                }
            }
        }
        /// <summary>
        /// 啪啦数据修改器
        /// </summary>
        public class Writer : IPLComponent
        {
            private PLTables Tables { get; set; }
            private MySqlManager MySqlManager { get; set; }

            private string UserName;

            /// <summary>
            /// 准备修改器
            /// </summary>
            /// <param name="CORE"></param>
            public void Ready(CORE CORE)
            {
                Tables = CORE.Tables;
                MySqlManager = CORE.MySqlManager;
                UserName = CORE.UserName;
            }

            /// <summary>
            /// 得到最大文章ID（私有）
            /// </summary>
            /// <returns></returns>
            internal int GetMaxID()
            {
                string SQL = string.Format("SELECT MAX(ID) FROM {0}", Tables.Index);
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 得到最小文章ID（私有）
            /// </summary>
            /// <returns>错误则返回-1</returns>
            internal int GetMinID()
            {
                string SQL = string.Format("SELECT MIN(ID) FROM {0}", Tables.Index);
                var value = MySqlManager.GetKey(SQL);
                /* 若取不到最大ID(没有任何文章时)，返回12000作为初始ID */
                return Convert.ToInt32(value == DBNull.Value ? 12000 : value);
            }
            /// <summary>
            /// 获取指定文章的积极备份的GUID
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            internal string GetPositiveGUID(int ID)
            {
                return Convert.ToString(MySqlManager.GetKey(string.Format("SELECT GUID FROM {0} WHERE ID={1}", Tables.Index, ID)));
            }
            /// <summary>
            /// 获取指定文章的消极备份的GUID
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            internal string GetNegativeGUID(int ID)
            {
                return Convert.ToString(MySqlManager.GetKey(
                    string.Format
                    (
                    "SELECT {1}.GUID FROM {0} JOIN {1} ON {0}.ID=1}.ID AND {0}.GUID<>{1}.GUID WHERE {0}.ID={2}"
                    , Tables.Index, Tables.Backup, ID
                    )
                    ));
            }


            /*
            Reg       注册文章：新建一个拷贝，并将index指向该拷贝
            Dispose   注销文章：删除所有拷贝和index指向

            Update    更新拷贝：新建一个拷贝，并将index更改为指向该拷贝
            Delete    删除拷贝：删除指定拷贝，且该拷贝不能为当前index指向
            Apply     应用拷贝：将现有index指向删除（顶出），然后将index指向设置为指定文章拷贝
            Rollback  回滚拷贝：将现有index指向删除（顶出），然后将index指向设置到另一个最近更新的拷贝
            Release   释放拷贝：删除非当前index指向的所有拷贝
            */

            /// <summary>
            /// 注册文章
            /// </summary>
            /// <param name="Post">文章数据（其中的ID、GUID、CT、LCT、User由系统生成）</param>
            /// <returns>返回受影响的行数</returns>
            public bool Reg(Post Post)
            {
                DateTime t = DateTime.Now;

                string SQL = string.Format
                    (
                    "INSERT INTO {0}" +
                    " ( ID, GUID, CT, Mode, Type, User, UVCount, StarCount) VALUES" +
                    " (?ID,?GUID,?CT,?Mode,?Type,?User,?UVCount,?StarCount);" +
                    "INSERT INTO {1}" +
                    " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                    " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);"
                    , Tables.Index, Tables.Backup
                    );


                List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = GetMaxID() + 1 },
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("N") },

                new MySqlParm() { Name = "CT", Val = t },
                new MySqlParm() { Name = "LCT", Val = t },

                new MySqlParm() { Name = "User", Val = UserName},/* 使用登录内核的用户名 */

                /* 可传参数 */
                new MySqlParm() { Name = "Mode", Val = Post.Mode },
                new MySqlParm() { Name = "Type", Val = Post.Type },

                new MySqlParm() { Name = "UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "Title", Val = Post.Title },
                new MySqlParm() { Name = "Summary", Val = Post.Summary },
                new MySqlParm() { Name = "Content", Val = Post.Content },

                new MySqlParm() { Name = "Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "Label", Val = Post.Label },
                new MySqlParm() { Name = "Cover", Val = Post.Cover }
            };

                MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
                MySqlCommand.Connection = MySqlManager.Connection;

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表和拷贝表分别添加1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 注销文章
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool Dispose(int ID)
            {
                /* int参数无法用于参数化攻击 */
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText = string.Format
                    (
                    "DELETE FROM {0} WHERE ID={2};" +
                    "DELETE FROM {1} WHERE ID={2};"
                    , Tables.Index, Tables.Backup, ID
                    ),

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() >= 2)
                {
                    /* 指向表只删除1行数据，拷贝表至少删除1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 更新文章
            /// </summary>
            /// <param name="Post">文章数据</param>
            /// <returns></returns>
            public bool Update(Post Post)
            {
                string SQL = string.Format
                    (
                    "UPDATE {0} SET GUID=?GUID, Mode=?Mode, Type=?Type, User=?User, UVCount=?UVCount, StarCount=?StarCount WHERE ID=?ID;" +
                    "INSERT INTO {1}" +
                    " ( ID, GUID, LCT, Title, Summary, Content, Archiv, Label, Cover) VALUES" +
                    " (?ID,?GUID,?LCT,?Title,?Summary,?Content,?Archiv,?Label,?Cover);"
                    , Tables.Index, Tables.Backup
                    );

                List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "GUID", Val = MathH.GenerateGUID("N") },
                new MySqlParm() { Name = "LCT", Val = DateTime.Now },

                new MySqlParm() { Name = "User", Val = UserName},/* 使用登录内核的用户名 */

                /* 可传参数 */
                new MySqlParm() { Name = "ID", Val = Post.ID },

                new MySqlParm() { Name = "Mode", Val = Post.Mode },
                new MySqlParm() { Name = "Type", Val = Post.Type },

                new MySqlParm() { Name = "UVCount", Val = Post.UVCount },
                new MySqlParm() { Name = "StarCount", Val = Post.StarCount },

                new MySqlParm() { Name = "Title", Val = Post.Title },
                new MySqlParm() { Name = "Summary", Val = Post.Summary },
                new MySqlParm() { Name = "Content", Val = Post.Content },

                new MySqlParm() { Name = "Archiv", Val = Post.Archiv },
                new MySqlParm() { Name = "Label", Val = Post.Label },
                new MySqlParm() { Name = "Cover", Val = Post.Cover }
            };

                MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
                MySqlCommand.Connection = MySqlManager.Connection;

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表添加1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                    /* 由于GUID更新，影响行始终为2，若出现其他情况则一定为错误 */
                }
            }

            /// <summary>
            /// 删除拷贝
            /// </summary>
            /// <param name="GUID">目标文章的GUID</param>
            /// <returns></returns>
            public bool Delete(string GUID)
            {
                string SQL = string.Format
                    (
                    "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID AND {1}.GUID = ?GUID"
                    , Tables.Index, Tables.Backup
                    );

                List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "GUID", Val = GUID }
            };

                MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
                MySqlCommand.Connection = MySqlManager.Connection;

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 1)
                {
                    /* 拷贝表删除一行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 应用拷贝
            /// </summary>
            /// <param name="GUID">目标拷贝的GUID</param>
            /// <returns></returns>
            public bool Apply(string GUID)
            {
                /* 此处，即使SQL注入造成了ID错误，由于第二步参数化查询的作用，GUID也会造成错误无法成功攻击 */
                object ID = MySqlManager.GetKey
                    (
                    string.Format("SELECT ID FROM {0} WHERE GUID = '{1}'", Tables.Backup, GUID)
                    );

                string SQL = string.Format
                    (
                    "DELETE FROM {1} WHERE GUID = (SELECT GUID FROM {0} WHERE ID = ?ID);" +
                    "UPDATE {0} SET GUID = ?GUID WHERE ID = ?ID;"
                    , Tables.Index, Tables.Backup
                    );

                List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "ID", Val = ID },
                new MySqlParm() { Name = "GUID", Val = GUID }
            };

                MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
                MySqlCommand.Connection = MySqlManager.Connection;

                /* 开始事务 */
                MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表删除一行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 回滚拷贝
            /// </summary>
            /// <param name="ID">目标文章的ID</param>
            /// <returns></returns>
            public bool Rollback(int ID)
            {
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText = string.Format
                    (
                    "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.GUID={1}.GUID AND {0}.ID={2};" +
                    "UPDATE {0} SET GUID = (SELECT GUID FROM {1} WHERE ID={2} ORDER BY LCT DESC LIMIT 0,1) WHERE ID={2};"
                    , Tables.Index, Tables.Backup, ID
                    ),

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() == 2)
                {
                    /* 指向表修改1行数据，拷贝表删除1行数据 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }
            /// <summary>
            /// 释放拷贝
            /// </summary>
            /// <param name="ID">目标文章的ID</param>
            /// <returns></returns>
            public bool Release(int ID)
            {
                MySqlCommand MySqlCommand = new MySqlCommand
                {
                    CommandText = string.Format
                   (
                   "DELETE {1} FROM {0} INNER JOIN {1} ON {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID AND {0}.ID={2}"
                   , Tables.Index, Tables.Backup, ID
                   ),

                    Connection = MySqlManager.Connection,

                    /* 开始事务 */
                    Transaction = MySqlManager.Connection.BeginTransaction()
                };

                if (MySqlCommand.ExecuteNonQuery() >= 0)
                {
                    /* 删除拷贝表的所有冗余，不存在冗余时影响行数为0 */
                    MySqlCommand.Transaction.Commit();
                    return true;
                }
                else
                {
                    MySqlCommand.Transaction.Rollback();
                    return false;
                }
            }

            /// <summary>
            /// 将目标文章指向的类型设为：未设置
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool UnsetType(int ID)
            {

                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Type", "");
            }
            /// <summary>
            /// 将目标文章指向的类型设为：便签
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool NoteType(int ID)
            {

                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Type", "note");
            }

            /// <summary>
            /// 将目标文章指向的模式设为：未设置
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool UnsetMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：隐藏
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool HiddenMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "hidden");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：计划中
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool ScheduledMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "scheduled");
            }
            /// <summary>
            /// 将目标文章指向的模式设为：已归档
            /// </summary>
            /// <param name="ID">目标文章ID</param>
            /// <returns></returns>
            public bool ArchivedMode(int ID)
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, "Mode", "archived");
            }

            /// <summary>
            /// 通用文章指向更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标文章ID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateIndex<T>(int ID, object Value) where T : IProperty
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Index,
                    Name = "ID",
                    Val = ID.ToString()
                };
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
            }
            /// <summary>
            /// 通用文章拷贝更新器
            /// </summary>
            /// <typeparam name="T">目标属性类型</typeparam>
            /// <param name="ID">目标拷贝GUID</param>
            /// <param name="Value">新属性值</param>
            /// <returns></returns>
            public bool UpdateBackup<T>(int ID, object Value) where T : IProperty
            {
                //初始化键定位
                MySqlKey MySqlKey = new MySqlKey
                {
                    Table = Tables.Backup,
                    Name = "GUID",
                    Val = GetPositiveGUID(ID)
                };
                return MySqlManager.UpdateKey(MySqlKey, typeof(T).Name, Convert.ToString(Value));
            }

            /// <summary>
            /// 检测ID、GUID是否匹配，之后合并Post数据表
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Backup">主表Post实例</parmm>
            /// <returns></returns>
            public static Post Join(Post Index, Post Backup)
            {
                if (Index.ID == Backup.ID && Index.GUID == Backup.GUID)
                {
                    return new Post
                    {
                        ID = Index.ID,
                        GUID = Index.GUID,

                        Mode = Index.Mode,
                        Type = Index.Type,

                        Title = Backup.Title,
                        Summary = Backup.Summary,
                        Content = Backup.Content,

                        User = Index.User,
                        Archiv = Backup.Archiv,
                        Label = Backup.Label,

                        CT = Index.CT,
                        LCT = Backup.LCT,

                        UVCount = Index.UVCount,
                        StarCount = Index.StarCount,

                        Cover = Backup.Cover
                    };
                }
                else
                {
                    throw new Exception("ERROR");
                }
            }
            /// <summary>
            /// 强制合并Post数据表（风险性重载，不考虑ID、GUID是否匹配，调用不当易引发逻辑故障）
            /// </summary>
            /// <parmm name="Index">索引表Post实例</parmm>
            /// <parmm name="Backup">主表Post实例</parmm>
            /// <returns>始终返回以Index的ID、GUID为最终合并结果的Post实例</returns>
            public static Post ForcedJoin(Post Index, Post Backup)
            {
                return new Post
                {
                    ID = Index.ID,
                    GUID = Index.GUID,

                    Mode = Index.Mode,
                    Type = Index.Type,

                    Title = Backup.Title,
                    Summary = Backup.Summary,
                    Content = Backup.Content,

                    User = Index.User,
                    Archiv = Backup.Archiv,
                    Label = Backup.Label,

                    CT = Index.CT,
                    LCT = Backup.LCT,

                    UVCount = Index.UVCount,
                    StarCount = Index.StarCount,

                    Cover = Backup.Cover
                };
            }
        }
        /// <summary>
        /// 啪啦计数管理器
        /// </summary>
        public class Counter : IPLComponent
        {
            private PLTables Tables { get; set; }
            private MySqlManager MySqlManager { get; set; }

            /// <summary>
            /// 准备计数器
            /// </summary>
            /// <param name="CORE"></param>
            public void Ready(CORE CORE)
            {
                Tables = CORE.Tables;
                MySqlManager = CORE.MySqlManager;
            }


            /// <summary>
            /// 文章计数
            /// </summary>
            public int TotalPostCount
            {
                get { return GetPostCountByMode("^"); }
            }
            /// <summary>
            /// 拷贝计数
            /// </summary>
            public int BackupCount
            {
                get { return GetBackupCount(); }
            }

            /// <summary>
            /// 隐藏文章计数
            /// </summary>
            public int HiddenCount
            {
                get { return GetPostCountByMode("^hidden$"); }
            }
            /// <summary>
            /// 展示中文章计数
            /// </summary>
            public int OnDisplayCount
            {
                get { return GetPostCountByMode("^$"); }
            }
            /// <summary>
            /// 归档中文章计数
            /// </summary>
            public int ArchivedCount
            {
                get { return GetPostCountByMode("^archived$"); }
            }
            /// <summary>
            /// 计划中文章计数
            /// </summary>
            public int ScheduledCount
            {
                get { return GetPostCountByMode("^scheduled$"); }
            }

            private int GetPostCountByMode(string REGEXP)
            {
                object Count = MySqlManager.GetKey(string.Format("SELECT Count(*) FROM {0} WHERE Mode REGEXP '{1}';", Tables.Index, REGEXP));
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
            private int GetBackupCount()
            {
                object Count = MySqlManager.GetKey(string.Format("SELECT COUNT(*) FROM {0},{1} WHERE {0}.ID={1}.ID AND {0}.GUID<>{1}.GUID;", Tables.Index, Tables.Backup));
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
        }
    }
}