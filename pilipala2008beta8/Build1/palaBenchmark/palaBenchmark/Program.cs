//#define 读测试
//#define 更改文章属性

//#define 单独更新拷贝测试
//#define 单独更新索引表测试

//#define 注册文章
//#define 注销文章
//#define 更新文章

//#define 删除拷贝
//#define 应用拷贝
//#define 回滚拷贝
//#define 释放拷贝

//#define RSA测试

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

            #region Welcome
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Initialization Ready");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to START palaBenchmark");
            #endregion

#if 读测试
            List<string> List = new List<string> { "技术", "生活" };

            List<Post> p = benchmark.PLDR.GetList<Archiv>
                (
                List,
                typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                typeof(CT),
                typeof(UVCount), typeof(StarCount),
                typeof(Type)
                );

            foreach (Post p2 in p)
            {
                

                /*
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
                */
                Console.WriteLine("文章被读取ID : {0} {1}", p2.ID,p2.Title);
            }
#endif

#if 注册文章
            string ContentStr = "";

            for (int i = 0; i < 30; i++)
            {
                ContentStr += "**标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            for (int i = 0; i < 100; i++)
            {
                Post Post = new Post()
                {
                    Mode = "o",
                    Type = (i % 4 == 0) ? "note" : "",
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

                Console.WriteLine("尝试注册文章：{0} / 状态：{1}", i, benchmark.PLDU.Reg(Post));
            }
#endif
#if 注销文章
            for (int i = 12001; i <= 12100; i++)
            {
                Console.WriteLine("尝试注销记录：{0} / 状态: {1}", i, benchmark.PLDU.Dispose(i));
            }
#endif
#if 更新文章
            string ContentStr2 = "";
            for (int i = 0; i < 10; i++)
            {
                ContentStr2 += "##标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            for (int i = 12001; i <= 12100; i++)
            {
                Post Post = new Post()
                {
                    ID = i,
                    Mode = i % 4 == 0 ? "" :
                            i % 4 == 1 ? "archived" :
                            i % 4 == 2 ? "scheduled" : "hidden",
                    Type = (i % 4 == 0) ? "note" : "",
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

                Console.WriteLine("尝试更新文章：{0} / 状态：{1}", i, benchmark.PLDU.Update(Post));
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
                    Console.WriteLine("尝试修改记录：{0} / 状态：{1}", GUID, benchmark.PLDU.Delete(GUID));
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
            for (int i = 12001; i <= 13000; i++)
            {
                Console.WriteLine("正在尝试回滚记录：{0} / 状态：{1}", i, benchmark.PLDU.Rollback(i));
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
                Console.WriteLine("尝试作用对象：{0} / 状态：{1}", i, benchmark.PLDU.OnDisplayMode(i));
            }
#endif

#if 单独更新拷贝测试
            for (int i = 12001; i < 13000; i++)
            {
                Console.WriteLine("尝试更改对象：{0} / 状态：{1}"
                    , i, benchmark.PLDU.UpdateCopy<Content>(i, "xxcx"));
            }
#endif
#if 单独更新索引表测试
            for (int i = 12001; i < 13000; i++)
            {
                Console.WriteLine("尝试更改对象：{0} / 状态：{1}"
                    , i, benchmark.PLDU.UpdateIndex<UVCount>(i, 123));
            }
#endif

#if RSA测试
            string s = "stringstringstringstringstring";

            WaterLibrary.stru.basic.KeyPair kp = new WaterLibrary.stru.basic.KeyPair(true);
            Console.WriteLine("原文:\n" + s);

            Console.WriteLine("公钥:\n" + kp.PublicKey);
            Console.WriteLine("私钥:\n" + kp.PrivateKey);


            /*
            string CipherText = WaterLibrary.com.basic.MathH.RSAEncrypt(kp.PublicKey, s);
            string PlainText = WaterLibrary.com.basic.MathH.RSADecrypt(kp.PrivateKey, CipherText);

            Console.WriteLine("密文:\n" + CipherText);
            Console.WriteLine("明文:\n" + PlainText);
            */



#endif
            Console.ReadKey();
        }
    }
}
