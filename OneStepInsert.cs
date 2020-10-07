using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace OneStepInsert
{
    public class OneStepInsert
    {
        public static List<ResultOfInsertTransaction> InsertMethod(List<Order> OrderList)
        {
            List<ResultOfInsertTransaction> Transactions = new List<ResultOfInsertTransaction>();
            StringBuilder sb;
            foreach (Order order in OrderList)
            {
                sb = new StringBuilder();

                sb.AppendLine("BEGIN TRANSACTION ");
                sb.AppendLine("BEGIN TRY ");
                sb.AppendLine(" DECLARE @OrderGUID varchar(60) set @OrderGUID=NEWID() INSERT INTO dbo.[Order] (Id,OrderNumber,TotalPrice,TotalDiscount,CustomerFirstName,CustomerLastName,CargoTrackingNumber,OrderDate)");
                sb.AppendLine(string.Format("SELECT @OrderGUID,{0},{1},{2},{3},{4},{5},getdate()",
                    StringForDatabase(order.OrderNumber),
                    NumberForDatabase(order.TotalPrice),
                    NumberForDatabase(order.TotalDiscount),
                    StringForDatabase(order.CustomerFirstName),
                    StringForDatabase(order.CustomerLastName),
                    StringForDatabase(order.CargoTrackingNumber)
                    ));
                sb.AppendLine(" INSERT INTO dbo.[OrderDetails] (Id,OrderId,ProductCode,ProductName,Discount,Price,Amount)");

                for (int i = 0; i < order.OrderDetails.Count; i++)
                {
                    if (!i.Equals(0))
                        sb.AppendLine(" UNION ALL ");
                    sb.AppendLine(string.Format(" SELECT NEWID(),@OrderGUID,{0},{1},{2},{3},{4}",
                        StringForDatabase(order.OrderDetails[i].ProductCode),
                         StringForDatabase(order.OrderDetails[i].ProductName),
                       NumberForDatabase(order.OrderDetails[i].Discount),
                        NumberForDatabase(order.OrderDetails[i].Price),
                        NumberForDatabase(order.OrderDetails[i].Amount)
                        ));
                }
                sb.AppendLine(" COMMIT TRANSACTION ");
                sb.AppendLine(" END TRY ");
                sb.AppendLine(" BEGIN CATCH ");
                sb.AppendLine(" ROLLBACK TRANSACTION ");
                sb.AppendLine(" END CATCH ");
                string deneme = sb.ToString();
                string Result = InsertToDatabase(sb.ToString());
                ResultOfInsertTransaction Transaction = new ResultOfInsertTransaction();
                if (Result.Equals(""))
                {
                    Transaction.OrderNumber = order.OrderNumber;
                    Transaction.Reason = "";
                    Transaction.Result = true;
                }
                else
                {
                    Transaction.OrderNumber = order.OrderNumber;
                    Transaction.Reason = Result;
                    Transaction.Result = false;
                }
                Transactions.Add(Transaction);
            }
            return Transactions;
        }
        public static void CreateList()
        {
            //
            List<Order> OrderList = new List<Order>();
            Order order = new Order();
            order.OrderDetails = new List<OrderDetails>();

            order.OrderNumber = "ABC"; order.TotalDiscount = 5.10; order.TotalPrice = 20; order.CustomerFirstName = "KAAN";
            order.CustomerLastName = "ERHAN"; order.CargoTrackingNumber = "XYZ1";

            OrderDetails orderDetails = new OrderDetails();
            orderDetails.Amount = 1; orderDetails.Discount = 4.10; orderDetails.Price = 15; orderDetails.ProductCode = "ABC1";
            orderDetails.ProductName = "Kaan's Item";
            order.OrderDetails.Add(orderDetails);

            orderDetails = new OrderDetails();
            orderDetails.Amount = 1; orderDetails.Discount = 1; orderDetails.Price = 5; orderDetails.ProductCode = "ABC2";
            orderDetails.ProductName = "Kaan's Item2";
            order.OrderDetails.Add(orderDetails);

            OrderList.Add(order);


            order = new Order();
            order.OrderDetails = new List<OrderDetails>();

            order.OrderNumber = "ABD"; order.TotalDiscount = 6.10; order.TotalPrice = 22; order.CustomerFirstName = "FAHRI";
            order.CustomerLastName = "ERHAN"; order.CargoTrackingNumber = "XYZ2";

            orderDetails = new OrderDetails();
            orderDetails.Amount = 1; orderDetails.Discount = 4.10; orderDetails.Price = 15; orderDetails.ProductCode = "ABC3";
            orderDetails.ProductName = "Kaan's Item3";
            order.OrderDetails.Add(orderDetails);

            orderDetails = new OrderDetails();
            orderDetails.Amount = 1; orderDetails.Discount = 2; orderDetails.Price = 7; orderDetails.ProductCode = "ABC4";
            orderDetails.ProductName = "Kaan's Item4";
            order.OrderDetails.Add(orderDetails);

            OrderList.Add(order);

            InsertMethod(OrderList);
        }
        public static string StringForDatabase(object value)
        {
            string result = "";
            if (value == null)
                return "NULL";
            else
            {
                result = value.ToString();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("'"); stringBuilder.Append(result.Replace("'", "''")); stringBuilder.Append("'");
                result = stringBuilder.ToString();
            }
            return result;
        }
        public static string NumberForDatabase(object value)
        {
            string result = "";
            if (value == null)
                return "NULL";
            else
            {
                result = value.ToString().Replace(',', '.');
            }
            return result;
        }
        public class Order
        {
            public string Id { get; set; }
            public string OrderNumber { get; set; }
            public double TotalPrice { get; set; }
            public double TotalDiscount { get; set; }
            public string CustomerFirstName { get; set; }
            public string CustomerLastName { get; set; }
            public string CargoTrackingNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public List<OrderDetails> OrderDetails { get; set; }
        }
        public class OrderDetails
        {
            public string Id { get; set; }
            public string OrderId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public double Discount { get; set; }
            public double Price { get; set; }
            public double Amount { get; set; }
        }
        public static string InsertToDatabase(string query)
        {
            string result = "";
            SqlConnection sqlConnection = new SqlConnection(@"YOUR DB CONNECTION STRING");
            try
            {
                sqlConnection.Open();
                SqlCommand sqlcom = new SqlCommand(query, sqlConnection);
                sqlcom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }
        public class ResultOfInsertTransaction
        {
            public string OrderNumber { get; set; }
            public bool Result { get; set; }
            public string Reason { get; set; }
        }
    }
}
