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

#region using
using System;
using WaterLibrary;
using WaterLibrary.Utils;
using WaterLibrary.MySQL;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Database;
using WaterLibrary.pilipala.Component;
#endregion

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
            Init benchmark = new Init();
            benchmark.INIT();
            var iwatch = new System.Diagnostics.Stopwatch();
            iwatch.Start();

            var pr = new PostRecord("27b266a26c704355a4607bd5d2a1c9ca", benchmark.Reader);
            Console.WriteLine($"ID : {pr.ID}");
            Console.WriteLine($"UUID : {pr.UUID}");
            Console.WriteLine($"LCT : {pr.LCT}");
            Console.WriteLine($"Title : {pr.Title}");
            Console.WriteLine($"Summary : {pr.Summary}");
            Console.WriteLine($"Content : {pr.Content}");
            Console.WriteLine($"Label : {pr.Label}");
            Console.WriteLine($"Cover : {pr.Cover}");
            Console.WriteLine($"ArchiveName : {pr.ArchiveName}");
            Console.WriteLine($"CT : {pr.CT}");
            Console.WriteLine($"Mode : {pr.Mode}");
            Console.WriteLine($"Type : {pr.Type}");
            Console.WriteLine($"User : {pr.User}");
            Console.WriteLine($"UVCount : {pr.UVCount}");
            Console.WriteLine($"StarCount : {pr.StarCount}");
            Console.WriteLine($"ArchiveID : {pr.ArchiveID}");

#if 读测试
            System.Type[] Properties = { typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                                   typeof(CT),
                                   typeof(UVCount), typeof(StarCount),
                                   typeof(Type)};


            List<Post> List1 = benchmark.Reader.GetPost<Archiv>
                (
                "技术|生活", Properties
                );

            List<Post> List2 = benchmark.Reader.GetPost<Archiv>
                (
                "技术|生活"
                );

            for (int ID = 12001; ID < 12000 + number; ID++)
            {
                string Title = (string)benchmark.Reader.GetProperty<Title>(ID);
                benchmark.Reader.GetProperty<Summary>(ID);
                benchmark.Reader.GetProperty<Content>(ID);

                if (Title != "")
                {
                    benchmark.Reader.GetPost<Title>(Title);
                }

                benchmark.Reader.Bigger<ID>(ID);
                benchmark.Reader.Bigger<ID>(ID, "置顶|生活|技术", typeof(Archiv));

                benchmark.Reader.Smaller<ID>(ID);
                benchmark.Reader.Smaller<ID>(ID, "置顶|生活|技术", typeof(Archiv));

                Console.WriteLine("尝试读取文章 : {0} {1} {2}",
                    ID, benchmark.Reader.GetPost(ID).GUID, benchmark.Reader.GetPost(ID).Title);
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
                    Mode = "",
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

                Console.WriteLine("尝试注册文章：{0} / 状态：{1}", i, benchmark.Writer.Reg(Post));
            }
#endif
#if 注销文章
            for (int i = 12001; i <= 12000 + number; i++)
            {
                Console.WriteLine("尝试注销记录：{0} / 状态: {1}", i, benchmark.Writer.Dispose(i));
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

                Console.WriteLine("尝试更新文章：{0} / 状态：{1}", i, benchmark.Writer.Update(Post));
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
                    Console.WriteLine("尝试修改记录：{0} / 状态：{1}", GUID, benchmark.Writer.Delete(GUID));
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
                    Console.WriteLine("已修改记录：{0}", benchmark.Writer.Apply(GUID));
                }
            }
#endif
#if 回滚拷贝
            for (int i = 12001; i <= 12000 + number; i++)
            {
                Console.WriteLine("正在尝试回滚记录：{0} / 状态：{1}", i, benchmark.Writer.Rollback(i));
            }
#endif
#if 释放拷贝
            for (int i = 12001; i <= 12000 + number; i++)
            {
                benchmark.Writer.Release(i);

                Console.WriteLine("已修改记录：{0}", i);
            }
#endif
#if 更改文章属性
            for (int i = 12001; i <= 13000; i++)
            {
                Console.WriteLine("尝试作用对象：{0} / 状态：{1}", i, benchmark.Writer.OnDisplayMode(i));
            }
#endif

#if 单独更新拷贝测试
            for (int i = 12001; i < 13000; i++)
            {
                Console.WriteLine("尝试更改对象：{0} / 状态：{1}"
                    , i, benchmark.Writer.UpdateCopy<Content>(i, "xxcx"));
            }
#endif
#if 单独更新索引表测试
            for (int i = 12001; i < 13000; i++)
            {
                Console.WriteLine("尝试更改对象：{0} / 状态：{1}"
                    , i, benchmark.Writer.UpdateIndex<UVCount>(i, 123));
            }
#endif

            iwatch.Stop();
            Console.WriteLine($"负载耗时 : {iwatch.Elapsed}");
            Console.ReadKey();
        }
    }
}
