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
            
            ;
            
            Console.WriteLine("******** EN UYGUN SATIŞ ********\n");
             string customerId = access();
            
            Console.Write("Ürünleri görmek için lütfen 1'i tuşlayın: ");

            while (true)
            {
                int result = Convert.ToInt16(Console.ReadLine());
                if (result == 1)
                {
                    GetProduct(0);
                    break;
                }
                else 
                { 
                    Console.WriteLine("Hatalı tuşlama yaptınız. Tekrar deneyiniz"); 
                }
            }

            string[] shoppinglist = ShoppingCart();
            PrintBasket(shoppinglist, customerId);
            Rate(shoppinglist, customerId);
            AvgRate(customerId);
        }

        static void AvgRate(string csId)
        {
            Console.WriteLine("Kullanıcının aldığı ürünler ve puanları aşağıdadır.");
            SqlConnections.GetRatelist(csId);
        }


        static string[] ShoppingCart()
        {
            int shoppingleng = 0;
            int j = 0;
            Console.WriteLine("Sepete eklemek istediğiniz ürünleri birer boşluk bırakarak satır numarasını giriniz. ");
            string[] shoppingList = Console.ReadLine().Split(' ');
            shoppingleng = shoppingList.Length;
            Console.Write("Başka bir şey eklemek istiyor musunuz(y/n)? : ");
            string answer = Console.ReadLine().ToLower();
            if ( answer== "y")
            {
                Console.WriteLine("Sepete eklemek istediğiniz ürünleri birer boşluk bırakarak satır numarasını giriniz. ");
                string[] shoppingList2 = Console.ReadLine().Split(' ');
                Array.Resize(ref shoppingList, shoppingList2.Length+ shoppingList.Length);
                for (int i = shoppingleng; i < shoppingList.Length; i++)
                {
                    shoppingList[i] = shoppingList2[j];
                    j += 1;
                }
                Console.WriteLine("Sepetinizi onaylıyor musunuz(y/n)?");

            }
            else if (answer == "n")
            {
                Console.WriteLine("Sepetinizi onaylıyor musunuz(y/n)?");
                answer = Console.ReadLine().ToLower();
                if (answer == "y")
                {
                    
                }
                else if (answer =="n")
                {
                    Console.WriteLine("Sepete ürün eklemek için 1, ürün çıkartmak için 2 yi tuşlayınız :");
                    answer = Console.ReadLine().ToLower();
                    if (answer == "1") 
                    {
                        j = 0;
                        Console.WriteLine("Eklemek istediğiniz ürünlerin satır numarasını birer boşluk bırakarak giriniz.");
                        string[] newlist = Console.ReadLine().Split(' ');
                        Array.Resize(ref shoppingList, newlist.Length + shoppingList.Length);
                        for (int i = shoppingleng; i < shoppingList.Length; i++)
                        {
                            shoppingList[i] = newlist[j];
                            j += 1;
                        }

                    }
                    else if (answer =="2")
                    {
                        Console.WriteLine("çıkartmak istediğiniz ürünlerin satır numarasını birer boşluk bırakarak giriniz.");
                        string[] newlist = Console.ReadLine().Split(' ');

                        foreach (var item in newlist)
                        {
                            for (int i = 0; i < shoppingList.Length; i++)
                            {
                                if (shoppingList[i] == item)
                                {
                                    Array.Clear(shoppingList, i, 1);
                                }
                                
                            } 
                        }
                    }   
                }
                else
                {
                    Console.WriteLine("Haatalı tuşlama yaptınız.");
                }
            }
            else
            {
                Console.WriteLine("Haatalı tuşlama yaptınız.");
            }
            return shoppingList;
        }

        static void Rate(string[] shoppinglist,string csId)
        {
            var product = GetProduct(1);
            List<int> pointlist = new List<int>();
            foreach (var item in shoppinglist)
            {
                
                Console.WriteLine($"{product.Item2[Convert.ToInt32(item)-1]} için 1-5 arasında puan veriniz.");
                int rate = Convert.ToInt32(Console.ReadLine());
                pointlist.Add(rate);
                                
            }
            Tuple<List<int>, string[],string> values = new Tuple<List<int>, string[],string>(pointlist, shoppinglist,csId);
            Console.WriteLine(SqlConnections.InsertRate(values, csId));

            
        }

        static void PrintBasket(string[] shoppingList,string csId)
        {
            int TotalPrices = 0;
            
            var productsInformation = SqlConnections.GetProduct();
            List<string> ProductCart = new List<string>();
            List<int> ProductPrices = new List<int>();
            foreach (var item in shoppingList)
            {
                ProductCart.Add(productsInformation.Item2[Convert.ToInt32(item) - 1]);
                ProductPrices.Add(Convert.ToInt32(productsInformation.Item1[Convert.ToInt32(item) - 1]));
            }
            Console.WriteLine("******************  SEPETİNİZ  ******************");
            Console.WriteLine("*************************************************");
            Console.WriteLine("Satır Sayısı\tÜrünler\t\t Fiyat");
            Console.WriteLine("*************\t*******\t\t *****");
            for (int i = 0; i < ProductCart.Count(); i++)
            {
                Console.WriteLine($"  {i + 1}\t\t{ProductCart[i]}\t\t{ProductPrices[i]}");
                TotalPrices = TotalPrices + ProductPrices[i];
            }
            Console.WriteLine($"Toplam Sepet Tutarı : {TotalPrices}");
        }

        static Tuple<List<int>,List<string>> GetProduct(int select)
        {
            var productsInformation = SqlConnections.GetProduct();
            if (select != 1)
            {
                Console.WriteLine("Satır Sayısı\tÜrünler\t\t Fiyat");
                Console.WriteLine("*************\t*******\t\t *****");
                for (int i = 0; i < productsInformation.Item1.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}\t\t{productsInformation.Item2[i]}\t\t{productsInformation.Item1[i]}");
                }
            }
            return productsInformation;
            
        }

        static string access()
        {
            Console.WriteLine("Giriş Yapmak için 1 kayıt olmak için 0 yazınız.");
            string customerId = "";
            int registerQuestion = Convert.ToInt32(Console.ReadLine());
            if (registerQuestion == 1)
            {
                customerId = Login();
            }
            else if (registerQuestion == 0)
            {
                customerId = CreateNewAccount();
            }
            return customerId;
        }


        static string CreateNewAccount()
        {
            string password;
            string passwordcheck;
            string name;
            string username;
            Console.Write("Adı soyad giriniz :");
            name = Console.ReadLine();
            Console.Write("E-posta adresinizi giriniz: ");
            username = Console.ReadLine();
            while (true)
            {
                Console.Write("Şifre  giriniz :");
                password = Console.ReadLine();
                Console.Write("Şifrenizi tekrar giriniz :");
                passwordcheck = Console.ReadLine();
                if (password == passwordcheck)
                {
                    break;
                }
            }
            
            DateTime now = DateTime.Now;
            string customerStatus = "Active";
            string customerId="";
            bool accontCheck = SqlConnections.Register(name, username, password, now, customerStatus);
            if (accontCheck == true)
            {
                Console.WriteLine("Hesabınız başarılı bir şekilde oluşturuldu.Bir kaç saniye sonra giriş yapabilirsiniz.");
                customerId = Login();
                
            }
            return customerId;
        }
        static string Login()
        {
            string customerId;
            while (true)
            {

                Console.Write("Kullanıcı Adı giriniz(e-posta) :");
                string username = Console.ReadLine();
                if (username =="-1")
                {
                    CreateNewAccount();
                }
                Console.Write("Şifre giriniz :");
                string password = Console.ReadLine();
                var userCheck = SqlConnections.Login(username, password);
                var returnList = userCheck.Split(',');
                if (returnList[0] == "Active")
                {
                    
                    customerId = returnList[1];
                    Console.WriteLine($" CustomerId: {customerId}");
                    break;
                }
                else
                {
                    Console.WriteLine(returnList[0]);
                }
            }
            return customerId;

        }
    }
}