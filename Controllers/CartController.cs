using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly Next_StoreDbContext _context;
       

        public CartController(Next_StoreDbContext context)
        {
            _context = context;
          
        }
        // GET /cart
        
        public IActionResult Index()
        {
            List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartViewModel = new CartViewModel
            {
                CartItems = cartItemsList,
                GrandTotal=cartItemsList.Sum(m=>m.Price*m.Quantity)
            };
            return View(cartViewModel);
        }
        // GET /cart/add/id
        [Route("/Cart/Add/{id}")]
        public async Task<IActionResult> Add(int id)
        {
            ProductsModel products = await _context.Products.FindAsync(id);
            List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItem = cartItemsList.Where(m=>m.ProductId==id).FirstOrDefault();
            if(cartItem==null)
            {
                cartItemsList.Add(new CartItem(products));
            }
            else
            {
                cartItem.Quantity++;
            }
            // add cart item to session
            HttpContext.Session.SetJson("Cart", cartItemsList);

            if (HttpContext.Request.Headers["X-Requested-With"]!="XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }


            return ViewComponent("SmallCart");
        }
        // GET /cart/Decrease/id
        [Route("/Cart/Decrease/{id}")]
        public IActionResult Decrease(int id)
        {

            List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cartItemsList.Where(m=>m.ProductId==id).FirstOrDefault();
            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cartItemsList.RemoveAll(m => m.ProductId==id);
            }
            // check if the cart is empty( if yes, session is deleted
            if(cartItemsList.Count==0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                // add cartitem object to session
                HttpContext.Session.SetJson("Cart", cartItemsList);
            }
           
            return RedirectToAction("Index");
        }
        // GET /cart/Remove/id
        [Route("/Cart/Remove/{id}")]
        public IActionResult Remove(int id)
        {

            List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            
            cartItemsList.RemoveAll(m => m.ProductId == id);
            // check if the cart is empty( if yes, session is deleted
            if (cartItemsList.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                // add cartitem object to session
                HttpContext.Session.SetJson("Cart", cartItemsList);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
          
            HttpContext.Session.Remove("Cart");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Ok();
        }
    }
}
