using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Next_Store.Clients;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {

        [TempData]
        public string TotalAmount { get; set; } = null;

        private readonly PaypalClient _paypalClient;
        public CheckoutController(PaypalClient paypalClient)
        {
            this._paypalClient = paypalClient;
        }



        public IActionResult Index()
        {


            ViewBag.ClientId = _paypalClient.ClientId;

            try
            {
                var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
                ViewBag.cart = cart;
                ViewBag.DollarAmount = cart.Sum(item => item.Price * item.Quantity);
                ViewBag.total = ViewBag.DollarAmount;
                int total = ViewBag.total;
                TotalAmount = total.ToString();
                TempData["TotalAmount"] = cart.Sum(item => item.Price * item.Quantity);

            }
            catch (Exception)
            {


            }
            return View();

        }
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            //var optionCust = new CustomerCreateOptions
            //{
            //    Email = stripeEmail,
            //    Name = "Rizwan Yousaf",
            //    Phone = "338595119"
            //};
            //var serviceCust = new CustomerService();
            //Customer customer = serviceCust.Create(optionCust);
            //var optionsCharge = new ChargeCreateOptions
            //{
            //    Amount = Convert.ToInt64(TempData["TotalAmount"]),
            //    Currency = "USD",
            //    Description="Pet Selling amount",
            //    Source=stripeToken,
            //    ReceiptEmail=stripeEmail

            //};
            //var serviceCharge = new ChargeService();
            //Charge charge = serviceCharge.Create(optionsCharge);
            //if(charge.Status=="successded")
            //{
            //    ViewBag.AmountPaid = charge.Amount;
            //    ViewBag.Customer = customer.Name;
            //}
            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Order(CancellationToken cancellationToken)
        {
            try
            {
                List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart");
                // set the transaction price and currency
                var price = cartItemsList.Sum(m => m.Price * m.Quantity).ToString();
                var currency = "USD";

                // "reference" is the transaction key
                var reference = GetRandomInvoiceNumber();// "INV002";

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
