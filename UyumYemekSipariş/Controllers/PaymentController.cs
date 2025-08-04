using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;
using System.Web.Script.Serialization; 

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities(); 

        [HttpPost]
        public ActionResult ProcessPayment(string cartData, int restaurantId, decimal subTotal, decimal deliveryFee, decimal totalAmount, string paymentMethod)
        {
            try
            {
              
                var userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

                if (userId == 0)
                {
                    return Json(new { success = false, message = "Lütfen giriş yapınız." });
                }

               
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var cartItems = serializer.Deserialize<List<Dictionary<string, object>>>(cartData);

              
                var userAddress = db.Addresses.FirstOrDefault(a => a.UserID == userId);
                int addressId = userAddress != null ? userAddress.AddressID : 0;

            
                var order = new Order
                {
                    CustomerID = userId,
                    RestaurantID = restaurantId,
                    AddressID = addressId,
                    OrderDate = DateTime.Now,
                    Status = "Beklemede",
                    SubTotal = subTotal,
                    DeliveryFee = deliveryFee,
                    TotalAmount = totalAmount,
                    PaymentMethod = paymentMethod,
                    Notes = ""
                };

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in cartItems)
                {
                    int productId = (int)Convert.ToDouble(item["id"]);
                    var product = db.Products.Find(productId);

                    if (product == null)
                    {
                        return Json(new { success = false, message = $"Ürün ID {productId} veritabanında bulunamadı." });
                    }

                    int quantity = (int)Convert.ToDouble(item["quantity"]);
                    decimal price = Convert.ToDecimal(item["price"]);

                    db.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = productId,
                        Quantity = quantity,
                        UnitPrice = price,
                        TotalPrice = price * quantity
                    });
                }


                
                var payment = new Payment
                {
                    OrderID = order.OrderID,
                    PaymentMethod = paymentMethod,
                    Amount = totalAmount,
                    PaymentStatus = "Başarılı",
                    PaymentDate = DateTime.Now,
                    TransactionID = Guid.NewGuid().ToString()
                };

                db.Payments.Add(payment);

               
                var userCartItems = db.Carts.Where(c => c.UserID == userId).ToList();
                db.Carts.RemoveRange(userCartItems);

                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Siparişiniz başarıyla alındı! Sipariş numaranız: " + order.OrderID,
                    orderId = order.OrderID
                });
            }
            catch (Exception ex)
            {
                string GetFullExceptionMessage(Exception e)
                {
                    if (e == null) return "";
                    return e.Message + (e.InnerException != null ? " --> " + GetFullExceptionMessage(e.InnerException) : "");
                }

                var fullMessage = GetFullExceptionMessage(ex);

                return Json(new
                {
                    success = false,
                    message = "Ödeme işlemi sırasında hata oluştu: " + fullMessage
                });
            }

        }
    }
}