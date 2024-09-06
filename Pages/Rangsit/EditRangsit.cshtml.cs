using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace information.Pages.Rangsit
{
    public class EditRangsitModel : PageModel
    {
        public StockInfo stockInfo = new StockInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String itemid = Request.Query["itemid"];

            if (String.IsNullOrEmpty(itemid))
            {
                errorMessage = "Item ID is required.";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:information124.database.windows.net,1433;Initial Catalog=information;Persist Security Info=False;User ID=romtham;Password=Rmtmm12.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM stocks WHERE itemid=@itemid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemid", itemid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stockInfo.itemid = reader.GetInt32(0).ToString();
                                stockInfo.item = reader.GetString(1);
                                stockInfo.storeid = reader.GetString(2);
                                stockInfo.supplier = reader.GetString(3);
                                stockInfo.amount = reader.GetString(4);
                            }
                            else
                            {
                                errorMessage = "Item not found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
{
            stockInfo.itemid = Request.Form["itemid"];
            stockInfo.item = Request.Form["item"];
            stockInfo.storeid = Request.Form["storeid"];
            stockInfo.supplier = Request.Form["supplier"];
            stockInfo.amount = Request.Form["amount"];

            if (String.IsNullOrEmpty(stockInfo.itemid) ||
                String.IsNullOrEmpty(stockInfo.item) ||
                String.IsNullOrEmpty(stockInfo.storeid) ||
                String.IsNullOrEmpty(stockInfo.supplier) ||
                String.IsNullOrEmpty(stockInfo.amount))
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:information124.database.windows.net,1433;Initial Catalog=information;Persist Security Info=False;User ID=romtham;Password=Rmtmm12.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE stocks " +
                                 "SET item=@item, storeid=@storeid, supplier=@supplier, amount=@amount " +
                                 "WHERE itemid=@itemid;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemid", stockInfo.itemid);
                        command.Parameters.AddWithValue("@item", stockInfo.item);
                        command.Parameters.AddWithValue("@storeid", stockInfo.storeid);
                        command.Parameters.AddWithValue("@supplier", stockInfo.supplier);
                        command.Parameters.AddWithValue("@amount", stockInfo.amount);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Update successful.";
            Response.Redirect("/Rangsit/IndexRangsit");
        }
    }    
}