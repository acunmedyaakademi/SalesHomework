using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SalesHomework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******** EN UYGUN SATIŞ ********\n");
            Dictionary<string, string> product = new Dictionary<string, string> { };
            product = SalesHomework.SqlConnections.GetProduct();

        }
    }
}