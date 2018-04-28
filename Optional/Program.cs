using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    class Program
    {
        static void Main(string[] args)
        {
            Variant<int, string> v = "3";
            Variant<string, int> v2 = v;

            Console.WriteLine(v2.Map(t => t * 1).FlatMap(t => int.Parse(t)));
            Console.Read();
        }
    }
}
