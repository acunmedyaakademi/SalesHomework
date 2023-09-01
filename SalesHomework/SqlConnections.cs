using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SalesHomework
{
    internal class SqlConnections
    {
        IDictionary<string, string> GetProduct ()
        {
            Dictionary<string,string> productList = new Dictionary<string,string> ();
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand product = new SqlCommand(" select ProductsName , ProductsPrices from [HOMEWORK3_1].[dbo].[Products]",sqlConnection);
            SqlDataReader read = product.ExecuteReader();
            for (int i = 0; i < read.FieldCount; i++)
            {
                productList.Add(read["ProductsName"].ToString(), read["ProductsPrices"].ToString());
            }
            return productList;
        }
        void Register ()
        {

        }
    }
}
