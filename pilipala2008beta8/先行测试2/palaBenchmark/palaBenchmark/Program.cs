//#define 读测试
//#define 更改文章属性

//#define 注册文章*
//#define 注销文章*
//#define 更新文章*

//#define 删除拷贝*
//#define 应用拷贝*
//#define 回滚拷贝*
//#define 释放拷贝*

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.PostKey;
using WaterLibrary.stru.pilipala;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
#endregion
using Type = WaterLibrary.stru.pilipala.PostKey.Type;

namespace palaBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            /* 测试初始化 */
            Benchmark benchmark = new Benchmark();
            benchmark.INIT();

            #region 欢迎信息
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Initialization Ready");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to START palaBenchmark");
            #endregion

            #region 读测试
#if 读测试
            List<string> List = new List<string> { "置顶", "技术", "生活" };

            foreach (int ID in benchmark.PLDR.GetIDList())
            {
                benchmark.PLDR.GetList<Archiv>
                (
                List,
                typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                typeof(CT),
                typeof(UVCount), typeof(StarCount),
                typeof(Type)
                );

                benchmark.PLDR.GetTotal(ID);

                string Title = benchmark.PLDR.Get<Title>(ID);
                benchmark.PLDR.Get<Summary>(ID);
                benchmark.PLDR.Get<Content>(ID);

                if (Title != "")
                {
                    benchmark.PLDR.MatchID<Title>(Title);
                    benchmark.PLDR.MatchPost<Title>(Title);
                }

                benchmark.PLDR.GetIndex(ID);
                benchmark.PLDR.GetPrimary(ID);
                benchmark.PLDR.GetTotal(ID);

                benchmark.PLDR.PrevID(ID);
                benchmark.PLDR.PrevID<Title>(ID);
                benchmark.PLDR.PrevID("置顶|生活|技术", ID);

                benchmark.PLDR.NextID(ID);
                benchmark.PLDR.NextID<Title>(ID);
                benchmark.PLDR.NextID("置顶|生活|技术", ID);

                Console.WriteLine("文章被读取ID : {0}", ID);
            }
#endif
            #endregion

#if 注册文章
            string ContentStr = "";

            for (int i = 0; i < 64; i++)
            {
                ContentStr += "**标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            for (int i = 0; i < 1000; i++)
            {
                Post Post = new Post()
                {
                    Mode = "show",
                    Type = (i % 4 == 0) ? "n" : "p",
                    User = "Pinn",
                    UVCount = 999,
                    StarCount = 999,

                    Title = "palaBenchmark",
                    Summary = "标准性能基准测试",
                    Content = ContentStr,
                    Archiv = "技术",
                    Label = "基准测试",
                    Cover = "<div>基准测试Cover</div>"
                };
                benchmark.PLDU.Reg(Post);

                Console.WriteLine("已注册文章ID : {0}", i);
            }
#endif
#if 注销文章
            for (int i = 12001; i <= 13000; i++)
            {
                benchmark.PLDU.Dispose(i);
                Console.WriteLine("已注销：" + i);
            }
#endif
#if 更新文章
            string ContentStr2 = "";
            for (int i = 0; i < 64; i++)
            {
                ContentStr2 += "##标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            for (int i = 12001; i <= 13000; i++)
            {
                Post Post = new Post()
                {
                    ID = i,
                    Mode = "show",
                    Type = (i % 4 == 0) ? "n" : "p",
                    User = "Pinn2",
                    UVCount = 99,
                    StarCount = 99,

                    Title = "##palaBenchmark",
                    Summary = "##性能基准测试",
                    Content = ContentStr2,
                    Archiv = "生活",
                    Label = "##基准测试",
                    Cover = "<div>##基准测试Cover</div>"
                };
                benchmark.PLDU.Update(Post);

                Console.WriteLine("已修改记录：{0}", i);
            }
#endif
#if 删除拷贝
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("当前测试：删除拷贝");
                Console.ForegroundColor = ConsoleColor.White;

                string GUID = Console.ReadLine();
                if (GUID == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("已修改记录：{0}", benchmark.PLDU.Delete(GUID));
                }
            }
#endif
#if 应用拷贝
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("当前测试：应用拷贝");
                Console.ForegroundColor = ConsoleColor.White;

                string GUID = Console.ReadLine();
                if (GUID == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("已修改记录：{0}", benchmark.PLDU.Apply(GUID));
                }
            }
#endif
#if 回滚拷贝
            for (int i = 12011; i <= 13000; i++)
            {
                benchmark.PLDU.Rollback(i);

                Console.WriteLine("已修改记录：{0}", i);
            }
#endif
#if 释放拷贝
            for (int i = 12001; i <= 13000; i++)
            {
                benchmark.PLDU.Release(i);

                Console.WriteLine("已修改记录：{0}", i);
            }
#endif
#if 更改文章属性
            for (int i = 12001; i <= 13000; i++)
            {
                Console.WriteLine("当前操作对象：" + i);
                Console.WriteLine("已更改为ArchivReg");
                benchmark.PLDU.ArchivReg(i);
            }
#endif
        }
    }
}
