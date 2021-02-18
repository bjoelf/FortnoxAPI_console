using System;
using System.Collections.Generic;
using System.Net.Http;
using Fortnox.SDK;
using Fortnox.SDK.Connectors;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Search;

namespace FortnoxAPI_console
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://github.com/FortnoxAB/csharp-api-sdk/wiki

            FortnoxClient fortnoxClient = new FortnoxClient()
            {
                //Set credentials

                //DOC Production token!!
                //AccessToken = "0a181384-f85c-4a41-a96b-7bf604333654",

                //SandBox AccessToken
                AccessToken = "8c688358-181e-4d6e-a0c1-7ba50618c32d",
                ClientSecret = "PqQ6cN3Qk7",
                //Set custom http client or other settings
                HttpClient = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(10)
                }

            };

            ///The package is generally built upon connectors, entities and subsets, 
            ///where connector provides methods to handle resources in form of entities and subsets.
            ///Search data objects were added to aggregate filters and settings for Find methods.

            //Getting connector and testing call
            CustomerConnector customerConnector = fortnoxClient.Get<CustomerConnector>();
            var searchSettings = new CustomerSearch()
            {
                //limit page size to 50
                Limit = 50,

                //select page
                Page = 1, //by default

                // sort
                SortBy = Sort.By.Customer.CustomerNumber,
                SortOrder = Sort.Order.Descending,

                // do only incorporate modified in last week
                //LastModified = DateTime.Now.AddDays(-7),

                // narrow by name > ger Aleris tillbaka från DOC
                Name = "Ale"
            };

            // get a list of customers with meta-data
            var customers = customerConnector.Find(searchSettings);

            foreach (var item in customers.Entities)
            {
                // please note that this is a subset and not a full customer entity; "Kundlista"
                // however you can always get the full entity using the connector's Get method if needed; "Kundbild"
                Console.WriteLine(item.Name);
            }

            //Customer customer;
            //try
            //{
            //    customer = customerConnector.Get("1");
            //    Console.WriteLine(customer.Name);
            //    //Do something
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            Console.WriteLine("------------------ supplier call----------------------");
            //test supplier call
            var supplierConnector = fortnoxClient.Get<SupplierConnector>();
            Supplier supplier;
            try
            {
                supplier = supplierConnector.Get("1");
                Console.WriteLine(supplier.Name);
                //Do something
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            ///Test create order!
            ///Use sandbox enviroment!
            var orderConnector = fortnoxClient.Get<OrderConnector>();

            //new order to customer nr 1,
            var order = new Order() 
            { 
                CustomerNumber = "1",
                OrderRows = new List<OrderRow>()
            };

            //Add in one article to each orderrow!
            OrderRow rw = new OrderRow()
            {
                ArticleNumber = "KOAK",
                DeliveredQuantity = 30,
            };
            //Add the orderrow to the order
            order.OrderRows.Add(rw);

            //Add another orderrow to the order!
            OrderRow rw2 = new OrderRow()
            {
                ArticleNumber = "KOEL",
                DeliveredQuantity = 15,
            };
            //Add the secnd orderrow to the order
            order.OrderRows.Add(rw2);

            //send order to fortnox
            orderConnector.Create(order);
        }
    }
}
