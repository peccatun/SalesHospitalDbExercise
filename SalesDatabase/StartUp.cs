using EfCodeFirstSalesDatabase.Data.ObjForSelect;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;
using System.Linq;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            //1.Add
            //Console.WriteLine(AddCustomer(new SalesContext()));
            //1.1 Add Customer
            //1.2 Add Product
            //Console.WriteLine(AddProduct(new SalesContext()));
            //1.3 Add Store
            //Console.WriteLine(AddStore(new SalesContext()));
            //1.4 Add Sale
            //Console.WriteLine(AddSale(new SalesContext()));
            //2.Read
            //2.1 Read Customer
            //Console.WriteLine(ReadCustomer(new SalesContext()));
            //2.2 Read Product
            //2.3 Read Store
            //2.4 Read Sale
            //Console.WriteLine(ReadSale(new SalesContext(), name));
            //3.Update
            //3.1 Update Customer
            //Console.WriteLine(UpdateCustomer(new SalesContext(), name));
            //3.2 Update Product
            //Console.WriteLine(UpdateProduct(new SalesContext(), name));
            //3.3 Update Store
            //Console.WriteLine(UpdateStore(new SalesContext(),name));
            //3.4 Update Sale
            int id = int.Parse(Console.ReadLine());
            //Console.WriteLine(UpdateSale(new SalesContext(), id));
            //TODO
            //4.Delete
            Console.WriteLine(DeleteSale(new SalesContext(), id));
            //TODO
        }


        //Add methods
        public static string AddCustomer(SalesContext context)
        {
            Console.WriteLine("Customer name:");
            string name = Console.ReadLine();
            Console.WriteLine("Customer email:");
            string email = Console.ReadLine();
            Console.WriteLine("Customer credit card number");
            string creditCardNumber = Console.ReadLine();

            var customer = new Customer()
            {
                Name = name,
                Email = email,
                CreditCardNumber = creditCardNumber
            };

            context.Customers.Add(customer);

            context.SaveChanges();

            return $"Added customer {name}";
        }

        public static string AddProduct(SalesContext context)
        {
            //name
            Console.WriteLine("Product name:");
            string productName = Console.ReadLine();
            //quantiti
            Console.WriteLine("Product Quantiti");
            double quantity = double.Parse(Console.ReadLine());
            //price
            Console.WriteLine("Price:");
            decimal price = decimal.Parse(Console.ReadLine());
            //description
            Console.WriteLine("Description?");
            Console.WriteLine("y/n");
            char yorn = char.Parse(Console.ReadLine());
            string description = string.Empty;
            if (char.ToLower(yorn) == 'y')
            {
                Console.WriteLine("Add description here:");
                description = Console.ReadLine();
            }

            var product = new Product()
            {
                Name = productName,
                Quantity = quantity,
                Price = price,
                Description = description
            };

            context.Products.Add(product);

            context.SaveChanges();


            return $"Added product {productName} with quantity {quantity} and cost {price}";
        }

        public static string AddStore(SalesContext context)
        {
            Console.WriteLine("Store name:");
            string name = Console.ReadLine();

            Store store = new Store()
            {
                Name = name
            };

            context.Stores.Add(store);

            context.SaveChanges();

            return "Added store with name: " + name;
        }

        public static string AddSale(SalesContext context)
        {
            //Date
            Console.WriteLine("Add date?");
            char dorno = char.Parse(Console.ReadLine());
            DateTime? date = new DateTime();
            if (char.ToLower(dorno) == 'y')
            {
                date = DateTime.Parse(Console.ReadLine());
            }
            else
            {
                date = null;
            }
            //ProductId
            Console.WriteLine("Product Name: ");
            string productName = Console.ReadLine();
            int productId = FindProductId(context, productName);
            if (productId == -1)
            {
                Console.WriteLine("Need to add the product before we continue");
                Console.WriteLine(AddProduct(context));
                productId = FindProductId(context, productName);
            }
            //customerId
            Console.WriteLine("Customer by name:");
            string customerName = Console.ReadLine();
            int customerId = FindCustomerId(context, customerName);
            if (customerId == -1)
            {
                Console.WriteLine("You have to add new Customer:");
                Console.WriteLine(AddCustomer(context));
                customerId = FindCustomerId(context, customerName);
            }
            //StoreId
            Console.WriteLine("Store name:");
            string storeName = Console.ReadLine();
            int storeId = FindStoreId(context, storeName);
            if (storeId == -1)
            {
                Console.WriteLine("You have to create new store!");
                Console.WriteLine(AddStore(context));
                storeId = FindStoreId(context, storeName);
            }

            if (date == null)
            {
                Sale sale = new Sale()
                {
                    ProductId = productId,
                    CustomerId = customerId,
                    StoreId = storeId
                };

                context.Sales.Add(sale);
            }
            else
            {
                Sale sale = new Sale()
                {
                    Date = (DateTime)date,
                    ProductId = productId,
                    CustomerId = customerId,
                    StoreId = storeId
                };

                context.Sales.Add(sale);
            }

            context.SaveChanges();

            return "Added new Sale";
        }

        //Read methods
        public static string ReadCustomer(SalesContext context, string name)
        {
            var customer = context
                .Customers
                .Select(c => new
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Email = c.Email,
                    CreditCardNumber = c.CreditCardNumber
                })
                .FirstOrDefault(c => c.Name == name);

            if (customer == null)
            {
                return "There is no customer with that name";
            }

            return $"Customer: {customer.Name} with email: {customer.Email} amd credit card number: {customer.CreditCardNumber}";
        }

        public static string ReadProduct(SalesContext context, string name)
        {
            var product = context
                .Products
                .Select(p => new
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Description = p.Description
                })
                .Where(p => p.Name == name)
                .FirstOrDefault();

            if (product == null)
            {
                return "There is no such product in the database";
            }

            return $"Product {product.Name} with price {product.Price} and quantity {product.Quantity} Description: {product.Description}";
        }

        public static string ReadStore(SalesContext context, string name)
        {
            var store = context
                .Stores
                .Select(s => new { Name = s.Name })
                .Where(s => s.Name == name)
                .FirstOrDefault();

            if (store == null)
            {
                return "There is no such store in the database";
            }

            else
            {
                return $"Store with name: {store.Name}";
            }

        }

        public static string ReadSale(SalesContext context, string name)
        {
            var sale = context
                .Sales
                .Select(s => new
                {
                    SaleDate = s.Date,
                    SaledProduct = s.Product.Name,
                    SaledToCustomer = s.Customer.Name,
                    StoreName = s.Store.Name
                })
                .Where(s => s.SaledProduct == name)
                .FirstOrDefault();

            if (sale == null)
            {
                return "There is no such store in the database";
            }

            return $"  Selled product: {sale.SaledProduct} on date {sale.SaleDate}" +
                $"In Store: {sale.StoreName} to customer {sale.SaledToCustomer}";

        }

        //Update methods

        public static string UpdateCustomer(SalesContext context, string name)
        {
            int customerId = FindCustomerId(context, name);

            if (customerId == -1)
            {
                return "There is no customer with that name";
            }

            var customer = context
                .Customers
                .Where(c => c.CustomerId == customerId)
                .First();

            Console.WriteLine("New Name:");
            string newName = Console.ReadLine();
            Console.WriteLine("New Email:");
            string newEmail = Console.ReadLine();
            Console.WriteLine("New Credit card number:");
            string newCreditCardNumber = Console.ReadLine();

            customer.Name = newName;
            customer.Email = newEmail;
            customer.CreditCardNumber = newCreditCardNumber;

            context.SaveChanges();

            return $"Update customer:  {name} with new values: Name: {newName} Email: {newEmail} CreditCardNumber: {newCreditCardNumber}";
        }

        public static string UpdateProduct(SalesContext context, string name)
        {
            int productId = FindProductId(context, name);

            if (productId == -1)
            {
                return "There is no product with that name";
            }

            var product = context
                .Products
                .Where(p => p.ProductId == productId)
                .First();

            //name
            Console.WriteLine("New name:");
            string newName = Console.ReadLine();
            //quantiti
            Console.WriteLine("New quantity");
            double newQuantity = double.Parse(Console.ReadLine());
            //price
            Console.WriteLine("New price");
            decimal newPrice = decimal.Parse(Console.ReadLine());
            //desc
            Console.WriteLine("Will you add Description y/n?");
            char yn = Char.Parse(Console.ReadLine());
            if (char.ToLower(yn) == 'n')
            {
                product.Name = newName;
                product.Quantity = newQuantity;
                product.Price = newPrice;

                context.SaveChanges();

                return $"Product: {name} has new values Name: {newName} quantity: {newQuantity} price: {newPrice}";
            }

            Console.WriteLine("New Description:");
            string newDescription = Console.ReadLine();

            product.Name = newName;
            product.Quantity = newQuantity;
            product.Price = newPrice;
            product.Description = newDescription;

            context.SaveChanges();

            return $"Product: {name} has new values Name: {newName} quantity: {newQuantity} price: {newPrice} Description: {newDescription}";
        }

        public static string UpdateStore(SalesContext context, string name)
        {
            int storeId = FindStoreId(context, name);

            if (storeId == -1)
            {
                return "There is no store with that name";
            }

            var store = context
                .Stores
                .Where(s => s.StoreId == storeId)
                .FirstOrDefault();

            Console.WriteLine("New name:");

            string newName = Console.ReadLine();

            store.Name = newName;

            context.SaveChanges();

            return $"Store: {name} With new name {newName}";
        }

        public static string UpdateSale(SalesContext context, int id)
        {
            var sale = context
                .Sales
                .Where(s => s.SaleId == id)
                .FirstOrDefault();

            if (sale == null)
            {
                return "No sale with current Id";
            }
            Console.WriteLine("New Date:");
            DateTime newDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("New Product id:");
            int newProductId = int.Parse(Console.ReadLine());
            var product = context
                .Products
                .Where(p => p.ProductId == newProductId)
                .Select(p => p.Name)
                .FirstOrDefault();

            if (product == null)
            {
                return "No product with that id";
            }
            Console.WriteLine("New Customer id:");
            int newCustomerId = int.Parse(Console.ReadLine());
            var customer = context
                .Customers
                .Where(c => c.CustomerId == newCustomerId)
                .Select(c => c.Name)
                .FirstOrDefault();

            if (customer == null)
            {
                return "No customer with that id";
            }
            Console.WriteLine("New store id:");
            int newStoreId = int.Parse(Console.ReadLine());
            var store = context
                .Stores
                .Where(s => s.StoreId == newStoreId)
                .Select(s => s.Name)
                .FirstOrDefault();

            if (store == null)
            {
                return "No store with that id";
            }

            sale.Date = newDate;
            sale.ProductId = newProductId;
            sale.CustomerId = newCustomerId;
            sale.StoreId = newStoreId;

            context.SaveChanges();

            return $"Sale with Id: {id} was updated successfully";

        }

        //Delete methods

        public static string DeleteCustomer(SalesContext context, string name)
        {
            int customerId = FindCustomerId(context, name);

            if (customerId == -1)
            {
                return "No customer with name: " + name;
            }

            var customer = context
                .Customers
                .Where(c => c.CustomerId == customerId)
                .First();

            context.Customers.Remove(customer);

            context.SaveChanges();

            return $"Customer {name} removed successfully";
        }

        public static string DeleteProduct(SalesContext context, string name)
        {
            int productId = FindProductId(context, name);

            if (productId == -1)
            {
                return $"No product with name {name}";
            }

            var product = context
                .Products
                .Where(p => p.ProductId == productId)
                .First();

            context
                .Products.Remove(product);

            context
                .SaveChanges();

            return "Successfully removed product: " + name;
        }

        public static string DeleteStore(SalesContext context, string name)
        {
            int storeId = FindStoreId(context, name);

            if (storeId == -1)
            {
                return "No store with name " + name;
            }

            var store = context
                .Stores
                .Where(s => s.StoreId == storeId)
                .First();

            context.Stores.Remove(store);

            context.SaveChanges();

            return $"Store {name} removed successfully";
        }

        public static string DeleteSale(SalesContext context, int id)
        {
            var sale = context
                .Sales
                .Where(s => s.SaleId == id)
                .FirstOrDefault();

            if (sale == null)
            {
                return "No sale with Id: " + id;
            }

            context.Sales.Remove(sale);

            context.SaveChanges();

            return $"Sale with id: {id} removed successfully";
        }



        #region private
        private static int FindProductId(SalesContext context, string name)
        {
            int result = -1;

            var productId = context
                .Products
                .Select(p => new
                {
                    Id = p.ProductId,
                    Name = p.Name
                })
                .Where(p => p.Name == name)
                .FirstOrDefault();

            if (productId != null)
            {
                result = productId.Id;
            }

            return result;
        }

        private static int FindCustomerId(SalesContext context, string name)
        {
            int result = -1;

            var customer = context
                .Customers
                .Select(c => new
                {
                    Id = c.CustomerId,
                    Name = c.Name
                })
                .Where(c => c.Name == name)
                .FirstOrDefault();

            if (customer != null)
            {
                result = customer.Id;
            }
            return result;
        }

        private static int FindStoreId(SalesContext context, string name)
        {
            int result = -1;

            var store = context
                .Stores
                .Select(s => new
                {
                    Id = s.StoreId,
                    Name = s.Name
                })
                .Where(s => s.Name == name)
                .FirstOrDefault();

            if (store != null)
            {
                result = store.Id;
            }

            return result;
        }

        #endregion




    }
}
