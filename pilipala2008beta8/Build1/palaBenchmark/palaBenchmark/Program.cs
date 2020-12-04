#define 读测试
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

using System.Timers;
using System.Threading;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.Post;
using WaterLibrary.stru.pilipala.Post.Property;
using WaterLibrary.stru.pilipala;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
#endregion
using Type = WaterLibrary.stru.pilipala.Post.Property.Type;

namespace palaBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 300;//测试规模
            #region Welcome
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Initialization Ready");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to START palaBenchmark");
            #endregion
            /* 负载初始化 */
            Benchmark benchmark = new Benchmark();
            benchmark.INIT();
            var iwatch = new System.Diagnostics.Stopwatch();
            iwatch.Start();




#if 读测试
            System.Type[] Properties = { typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                                   typeof(CT),
                                   typeof(UVCount), typeof(StarCount),
                                   typeof(Type)};


            List<Post> List1 = benchmark.PLDR.GetPost<Archiv>
                (
                "技术|生活", Properties
                );

            List<Post> List2 = benchmark.PLDR.GetPost<Archiv>
                (
                "技术|生活"
                );

            for (int ID = 12001; ID < 12000 + number; ID++)
            {
                string Title = (string)benchmark.PLDR.GetProperty<Title>(ID);
                benchmark.PLDR.GetProperty<Summary>(ID);
                benchmark.PLDR.GetProperty<Content>(ID);

                if (Title != "")
                {
                    benchmark.PLDR.GetPost<Title>(Title);
                }

                benchmark.PLDR.Bigger<ID>(ID);
                benchmark.PLDR.Bigger<ID>(ID, "置顶|生活|技术", typeof(Archiv));

                benchmark.PLDR.Smaller<ID>(ID);
                benchmark.PLDR.Smaller<ID>(ID, "置顶|生活|技术", typeof(Archiv));

                Console.WriteLine("尝试读取文章 : {0} {1} {2}",
                    ID, benchmark.PLDR.GetPost(ID).GUID, benchmark.PLDR.GetPost(ID).Title);
            }
#endif

#if 注册文章
            string ContentStr = "";

            for (int i = 0; i < 30; i++)
            {
                ContentStr += "**标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            for (int i = 0; i < number; i++)
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
            for (int i = 12001; i <= 12000 + number; i++)
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

            for (int i = 12001; i <= 12000 + number; i++)
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
            for (int i = 12001; i <= 12000 + number; i++)
            {
                Console.WriteLine("正在尝试回滚记录：{0} / 状态：{1}", i, benchmark.PLDU.Rollback(i));
            }
#endif
#if 释放拷贝
            for (int i = 12001; i <= 12000 + number; i++)
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
            iwatch.Stop();
            Console.WriteLine("负载耗时：\n" + iwatch.Elapsed.ToString());
            Console.ReadKey();
        }
    }
}
