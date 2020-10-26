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

using Type = WaterLibrary.stru.pilipala.PostKey.Type;

namespace PalaBenchmarkFiller
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Press any key to START palaBenchmark");
            Console.ReadKey();

            /* 测试初始化 */
            Benchmark benchmark = new Benchmark();
            benchmark.Benchmark_INIT();



            List<string> 测试列表 = new List<string> { "置顶", "技术", "生活" };

            int i = 0;
            foreach (Post Post in benchmark.PLDR.GetList<Archiv>(测试列表,
                typeof(Title), typeof(Summary), typeof(Content), typeof(Cover),
                typeof(CT),
                typeof(UVCount), typeof(StarCount),
                typeof(Type)))
            {

                benchmark.PLDR.GetTotal(Post.ID);

                if (Post.Title != "")
                {
                    benchmark.PLDR.MatchID<Title>(Post.Title);
                    benchmark.PLDR.MatchPost<Title>(Post.Title);
                }

                benchmark.PLDR.GetIndex(Post.ID);
                benchmark.PLDR.GetPrimary(Post.ID);
                benchmark.PLDR.GetTotal(Post.ID);

                benchmark.PLDR.Get<Title>(Post.ID);
                benchmark.PLDR.Get<Summary>(Post.ID);
                benchmark.PLDR.Get<Content>(Post.ID);

                benchmark.PLDR.PrevID(Post.ID);
                benchmark.PLDR.PrevID<Title>(Post.ID);
                benchmark.PLDR.PrevID("置顶|生活|技术", Post.ID);

                benchmark.PLDR.NextID(Post.ID);
                benchmark.PLDR.NextID<Title>(Post.ID);
                benchmark.PLDR.NextID("置顶|生活|技术", Post.ID);

                Console.WriteLine("当前测试数量:{0}", ++i);
            }


            #region 读写测试
            /*
            string ContentStr = "";

            int k;
            for (k = 0; k < 64; k++)
            {
                ContentStr += "标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试标准性能基准测试  \n";
                Console.WriteLine("生成Content中......");
            }

            int j;
            for (j = 0; j < 898; j++)
            {
                Post Post = new Post()
                {
                    Mode = "show",
                    Type = (j % 4 == 0) ? "n" : "p",
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

                Console.WriteLine("已添加记录：{0}", j + 1);
            }
            */
            #endregion


            Console.WriteLine("Press any key to EXIT palaBenchmark");
            Console.ReadKey();

        }
    }
}
