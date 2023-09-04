using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace SalesHomework
{
    public class SqlConnections
    {
        public static Tuple<List<int>, List<string>> GetProduct()
        {

            List<int> productPrices = new List<int>();
            List<string> products = new List<string>();
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3_1;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand product = new SqlCommand(" select ProductsName , ProductsPrices from Products", sqlConnection);
            SqlDataReader read = product.ExecuteReader();
            while (read.Read())
            {

                productPrices.Add(Convert.ToInt32(read["ProductsPrices"]));
                products.Add(read["ProductsName"].ToString());
            }
            sqlConnection.Close();
            return Tuple.Create(productPrices, products);

        }
        
        
        public static string Login(string user, string pass)
        {
            string status = "";
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3_1;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand product = new SqlCommand($" select CustomerStatus, CustomerId from Customer WHERE CustomerAccount='{user}' AND CustomerPassword='{pass}'", sqlConnection);
            var result = product.ExecuteScalar();
            if (result != null)
            {
                SqlDataReader read = product.ExecuteReader();
                while (read.Read())
                {
                    status = read[0].ToString();
                    string csid = read["CustomerId"].ToString();
                    status = status + "," + csid;
                }
                    
            }
            else
            {
                status = "Kullanıcı Adı Veya Şifre Hatalı Lütfen Tekrar Deneyiniz";

            }
            sqlConnection.Close();
            return status;
        }

        public static bool Register(string user, string username, string pass, DateTime date, string status)
        {
            string result = "";
            bool returnValue = false;
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3_1;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand checkAccont = new SqlCommand($"SELECT COUNT(*) FROM Customer WHERE CustomerAccount = '{username}' ", sqlConnection);
            SqlDataReader read = checkAccont.ExecuteReader();
            while (read.Read())
            {
                result = read[0].ToString();
            }
            read.Close();
            if (result == "1")
            {
                returnValue = false;
            } else if (result == "0")
            {
                SqlCommand newuser = new SqlCommand($"INSERT INTO Customer (CustomerNameSurname,CustomerAccount,CustomerPassword,AccountCreatingDate,CustomerStatus) VALUES ('{user}','{username}','{pass}','{date}','{status}')", sqlConnection);
                read = newuser.ExecuteReader();
                read.Close();
                returnValue = true;

            }
            sqlConnection.Close();
            return returnValue;
        }

        public static string InsertRate(Tuple<List<int>, string[], string> values,string csId)
        {
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3_1;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            for (int i = 0; i < values.Item1.Count; i++)
            {
                SqlCommand insertRate = new SqlCommand($"INSERT INTO Sales (ProductsId,CustomerId,ProductRate) VALUES ('{values.Item2[i]}','{values.Item3}','{values.Item1[i]}')", sqlConnection);
                SqlDataReader read = insertRate.ExecuteReader();
                read.Close();
            }
            sqlConnection.Close();
            return "işlem başarılı";
        }

        public static void GetRatelist(string csId)
        {
            SqlConnection sqlConnection;
            string connectionString = @"Data Source=GRETHRAIN\sqlexpress;Initial Catalog=HOMEWORK3_1;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand GetRateList = new SqlCommand($"select S.ProductsId,P.ProductsName,P.ProductsPrices,S.ProductRate FROM Sales S  left join Products P on S.ProductsId = P.ProductsId WHERE CustomerId= {Convert.ToInt32(csId)}",sqlConnection);
            SqlDataReader read = GetRateList.ExecuteReader();
            List<int> rate = new List<int>();
            while (read.Read())
            {
                Console.WriteLine($"{read[1]}\t\t{read[2]}\t\t{read[3]}");
                rate.Add(Convert.ToInt32(read[3]));
            }
            Console.WriteLine($"kullanıcının ürünlere verdiği ortalama puan: {rate.Average()}");
            Console.ReadKey();
            read.Close();
        }

    }
}
