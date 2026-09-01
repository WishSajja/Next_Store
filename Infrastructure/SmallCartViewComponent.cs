using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Next_Store.Models;

namespace Next_Store.Infrastructure
{
    public class SmallCartViewComponent : ViewComponent
    {
        public int NumberOfItems { get; set; }
        public decimal GrandTotal { get; set; }
        
        public IViewComponentResult Invoke()
        {
            List<CartItem> cartItemsList = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            SmallCartViewComponent smallCartViewComponent;
            if(cartItemsList.Count ==0)
            {
                smallCartViewComponent = null;
            }
            else
            {
                smallCartViewComponent = new SmallCartViewComponent
                {
                    NumberOfItems = cartItemsList.Sum(m => m.Quantity),
                    GrandTotal = cartItemsList.Sum(m => m.Quantity * m.Price)
                };
                
            }
            return View(smallCartViewComponent);
        }

    }
}
