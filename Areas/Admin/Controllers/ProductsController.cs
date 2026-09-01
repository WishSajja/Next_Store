using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly Next_StoreDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(Next_StoreDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int p=1)
        {
            int itemsPerPage = 6;
           ViewBag.PageNumber =p; 
           ViewBag.PageRange =itemsPerPage; 
           ViewBag.TotalPages =(int)Math.Ceiling((decimal)_context.Products.Count()/itemsPerPage); 
      
           return View(await _context.Products.OrderByDescending(m => m.Id).Include(m => m.Category)
                .Skip((p-1)*itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync());

        }
        // GET /Admin/Products/Index/id
        public async Task<IActionResult> Details(int id)
        {
            ProductsModel? products = await _context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        // Create new Products(GET)
        public IActionResult Create()
        {

            ViewBag.CategoryId = _context.Categories.OrderBy(m => m.Sorting).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            });
            return View();
        }
        
        // Create new Products(POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsModel products)
        {
            if (ModelState.IsValid)
            {
                products.Slug = products.Name.ToLower().Replace(" ", "_");
           
            var slug = await _context.Products.FirstOrDefaultAsync(d => d.Slug == products.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The Product name is Already in use");
                return View(products);
            }
            string imageName = "IMG-20260719-WA7477.jpg";
            if (products.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "Media/Products");
                imageName = Guid.NewGuid().ToString() + "_" + products.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                using(FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await products.ImageUpload.CopyToAsync(fs);
                }
                

            }
            products.Image = imageName;
            _context.Products.Add(products);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product Created!!";
            return RedirectToAction("Index");
            }
            return View("Create", products);
        }
        // GET Edit Details
        public async Task<IActionResult> Edit(int id)
        {
            ProductsModel products = await _context.Products.Include(p=>p.Category).FirstAsync(p => p.Id == id);
            ViewBag.CategoryId =  _context.Categories.OrderBy(m=>m.Sorting).Select(m=> new SelectListItem
            { 
                Value=m.Id.ToString(),
                Text=m.Name
            });
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // Edit Products(POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsModel products)
        {
            if (ModelState.IsValid)
            {
                products.Slug = products.Name.ToLower().Replace(" ", "_");
            
            var slug = await _context.Products.Where(d=>d.Id!=id).FirstOrDefaultAsync(d => d.Slug == products.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The Category is Already in use");
                return View(products);
            }
            
            if (products.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "Media/Products");
                // check if the image is default noimage
                if(!string.Equals(products.Image, "IMG-20260719-WA7477.jpg"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, products.Image);
                    // checking whether the image exists
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                string imageName = Guid.NewGuid().ToString() + "_" + products.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await products.ImageUpload.CopyToAsync(fs);
                }
                products.Image = imageName;

            }
            
            _context.Products.Update(products);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product Updated!!";
            return RedirectToAction("Index");
            }
            return View("Create", products);
        }
        // GET Admin/Products/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            ProductsModel products = await _context.Products.FirstAsync(p => p.Id == id);
            if (products == null)
            {
                TempData["Error"] = "Product does not exist!!";
            }
            else
            {  // check if the image is default noimage
                if (!string.Equals(products.Image, "IMG-20260719-WA7477.jpg"))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "Media/Products");
                    string oldImagePath = Path.Combine(uploadsDir, products.Image);
                    // checking whether the image exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _context.Products.Remove(products);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product Deleted!!";
            }
            return RedirectToAction("Index");

        }
      


    }
}
