using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Next_Store.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        // Get /roles/index
        public async Task<IActionResult> Index() => View(await _roleManager.Roles.ToListAsync());
        

        // Get /roles/Create
        public IActionResult Create()=> View();
        // Get /roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2, ErrorMessage ="Minimum number of characters is 2"), Required] string name)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role created!";
                    return RedirectToAction("Index");
                }
                foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                
            }
            ModelState.AddModelError("", "Minimum length is 2 letters");

            return View("Create", name);
        }
        // Get /roles/edit/id
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            var userList = await _userManager.Users.ToListAsync();
            // loop through the list of all users assigning them to the appropriate list(either Members or NonMembers)
            foreach (AppUser user in userList)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEdit
            {
                Role=role,
                Members=members,
                NonMembers=nonMembers
            });
                
        }
        // Post /roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEdit roleEdit, string RoleId)
        {
            IdentityResult result;
            var role = await _roleManager.FindByIdAsync(RoleId);

            foreach (var AddId in roleEdit.AddIds ?? new string[] {})
            {
                var User = await _userManager.FindByIdAsync(AddId);
                result = await _userManager.AddToRoleAsync(User, role.Name);
            }
            foreach (var DeleteId in roleEdit.DeleteIds ?? new string[] { })
            {
                var User = await _userManager.FindByIdAsync(DeleteId);
                result = await _userManager.RemoveFromRoleAsync(User, role.Name);
            }
            return Redirect(Request.Headers["Referer"].ToString());

        }
        // Get /roles/Delete/id
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}



