using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using StdLib.ViewHandler.WebSite;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Post p = new Post(null);

            Console.WriteLine(p.outputFileString("https://www.baidu.com/"));
            Console.WriteLine(p.outputFileString(@"C:\Users\Thaumy\Desktop\baidu",4096,false,"utf-8"));
            Console.WriteLine(p.outputFileString(@"C:\Users\Thaumy\Desktop\baidu.b", 4096, false, "utf-8"));
            Console.ReadKey();
        }
    }
}
