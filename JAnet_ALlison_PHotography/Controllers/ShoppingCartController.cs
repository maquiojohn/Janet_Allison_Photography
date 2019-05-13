using JAnet_ALlison_PHotography.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: ShoppingCart
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Cart"] == null)
            {
                List<ShoppingCart> lsCart = new List<ShoppingCart>
                {
                    new ShoppingCart(db.ProductPictures.Find(id),1)
                };
                Session["Cart"] = lsCart;
            }
            else
            {
                List<ShoppingCart> cart = (List<ShoppingCart>)Session["Cart"];
                int check = isExistingCheck(id);
                if (check == -1)
                {
                    cart.Add(new ShoppingCart(db.ProductPictures.Find(id), 1));
                }
                else
                {
                    cart[check].Quantity++;
                }
                Session["Cart"] = cart;
            }
            return View("Index");
        }

        private int isExistingCheck(int? id)
        {
            List<ShoppingCart> lscart = (List<ShoppingCart>)Session["Cart"];
            for (int i = 0; i < lscart.Count; i++)
            {
                if (lscart[i].Picture.Picture_Id == id)
                    return i;
            }
            return -1;
        }
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int check = isExistingCheck(id);
            List<ShoppingCart> lscart = (List<ShoppingCart>)Session["Cart"];
            lscart.RemoveAt(check);
            return View("Index");
        }
        [Authorize]
        public ActionResult CheckOut()
        {

            return View("CheckOut");
        }
        [Authorize]
        public ActionResult Success()
        {

            return View();
        }
        [Authorize]
        public ActionResult Failure()
        {

            return View();
        }


        //Paypal with Paypal payment
        [Authorize]
        public ActionResult PaymentWithPaypal(Receipt receipt, string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    else
                    {
                        List<ShoppingCart> cart = (List<ShoppingCart>)Session["Cart"];
                        // gets the username of the current user
                        receipt.Email = System.Web.HttpContext.Current.User.Identity.GetUserName();
                        //gets and store the real time date.
                        receipt.Receipt_Date = DateTime.Now;
                        //get the infor of the current user using its email
                        var userInfo = (from user in db.Users
                                        where user.Email.Equals(receipt.Email)
                                        select new
                                        { user.FirstName, user.LastName, user.PhoneNumber, user.Address }).ToList();
                        //setting name equals to firstname and lastname
                        foreach (var user in userInfo)
                        {
                            receipt.FirstName = user.FirstName;
                            receipt.LastName = user.LastName;
                            receipt.Address = user.Address;
                            receipt.PhoneNumber = user.PhoneNumber;
                        }
                        db.Receipts.Add(receipt);
                        db.SaveChanges();
                        foreach (ShoppingCart shopcart in cart)
                        {
                            OrderDetail orderDetail = new OrderDetail()
                            {
                                Receipt_Id = receipt.Receipt_Id,
                                Picture_Id = shopcart.Picture.Picture_Id,
                                Picture = shopcart.Picture.Picture,
                                Title = shopcart.Picture.Title,
                                Price = shopcart.Picture.Price,
                                Description = shopcart.Picture.Description
                            };
                            db.OrderDetails.Add(orderDetail);
                            db.SaveChanges();
                        }
                        //remove all in the cart session
                        Session.Remove("Cart");
                    }
                }
            }
            catch (PayPal.PaymentsException ex)
            {
                PayPalLogger.Log("Error" + ex.Message);
                return View("Failure");
            }
            //on successful payment, show success page to user.  
            return View("Success");
        }

        //Createe ExecutePayment Method
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        //Createa payment method using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };


            //Adding Item Details like name, currency, price etc  
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["Cart"];
            foreach (var item in cart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Picture.Title,
                    currency = "CAD",
                    price = item.Picture.Price.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                shipping = "0",
                subtotal = cart.Sum(x => x.Quantity * x.Picture.Price).ToString(),
                tax = ((cart.Sum(x => x.Quantity * x.Picture.Price) * 0.15)).ToString("#,##0.00"),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "CAD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Janet Testing Transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}