using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;

namespace ShoppingSystem
{
    public class SampleData
    {
        public static void Initialize(ShoppingContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }
            context.Products.AddRange(
                    new Product
                    {
                        Name = "Butter",
                        Price = 30.0
                    },
                    new Product
                    {
                        Name = "Banana",
                        Price = 20.50
                    },
                    new Product
                    {
                        Name = "Cola",
                        Price = 9.30
                    },
                    new Product()
                    {
                        Name = "Bread",
                        Price = 22.21
                    },
                    new Product()
                    {
                        Name = "NoodleCup",
                        Price = 30
                    }
                );
            context.SaveChanges();
            context.Customers.AddRange(
                    new Customer
                    {
                        FirstName = "Ostap",
                        LastName = "Bender",
                        Address = "Rio de Zhaneiro",
                        Discount = Discount.O
                    },
                    new Customer
                    {
                        FirstName = "Shura",
                        LastName = "Balaganov",
                        Address = "Odessa",
                        Discount = Discount.R
                    },
                    new Customer()
                    {
                        FirstName = "Boghdan",
                        LastName = "Vornik",
                        Address = "Chernihiv",
                        Discount = Discount.O
                    },
                    new Customer()
                    {
                        FirstName = "Diana",
                        LastName = "Lopov",
                        Address = "Lviv",
                        Discount = Discount.V
                    }
            );
            context.SaveChanges();
            context.SuperMarkets.AddRange(
                    new SuperMarket
                    {
                        Name = "Wellmart",
                        Address = "Lviv",
                    },
                    new SuperMarket
                    {
                        Name = "Billa",
                        Address = "Odessa",
                    },
                    new SuperMarket()
                    {
                        Name = "Silpo",
                        Address = "Kyiv"
                    },
                    new SuperMarket()
                    {
                        Name = "ATB",
                        Address = "Dnipro"
                    }
            );
            context.SaveChanges();
            context.Orders.AddRange(
                    new Order
                    {
                        CustomerId = 1,
                        SuperMarketId = 1,
                        OrderDate = DateTime.Now,
                     },
                    new Order
                    {
                        CustomerId = 1,
                        SuperMarketId = 1,
                        OrderDate = DateTime.Now,
                    },
                    new Order()
                    {
                        CustomerId = 2,
                        SuperMarketId = 3,
                        OrderDate = DateTime.Now
                    }
                );
            context.SaveChanges();
            context.OrderDetails.AddRange(
                    new OrderDetail
                    {
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 2
                    },
                    new OrderDetail
                    {
                        OrderId = 2,
                        ProductId = 2,
                        Quantity = 1
                    },
                    new OrderDetail()
                    {
                        OrderId = 3,
                        ProductId = 3,
                        Quantity = 3
                    },
                    new OrderDetail()
                    {
                        OrderId = 2,
                        ProductId = 3,
                        Quantity = 3
                    }
                );
            context.SaveChanges();
            SetTotalCostForOD(context);
            context.SaveChanges();
            SetTotalCostForO(context);
            context.SaveChanges();
        }

        private static byte[] ReadFile(string Path)
        {
            byte[] data = null;

            FileInfo fInfo = new FileInfo(Path);
            long numBytes = fInfo.Length;

            FileStream fStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fStream);

            data = br.ReadBytes((int)numBytes);

            return data;
        }

        private static void SetTotalCostForOD(ShoppingContext context)
        {
            foreach (var orderDetail in context.OrderDetails)
            {
                orderDetail.TotalCost =
                    (decimal) (context.Products.Find(orderDetail.ProductId).Price * orderDetail.Quantity);
            }
        }
        private static void SetTotalCostForO(ShoppingContext context)
        {
            foreach (var order in context.Orders)
            {
                order.TotalCost = order.OrderDetails.Sum(od => od.TotalCost);
            }
        }
    }
 }
