using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityRolesApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ============================================
        // ASSIGN ADMINISTRATOR ROLE
        // ============================================
        public async Task<IActionResult> Assign()
        {
            string email = "testadmin@gmail.com";
            string roleName = "Administrator";

            // Find the user
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Content(
                    "User testadmin@gmail.com was not found. Please register this user first.");
            }

            // Create Administrator role if it does not exist
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    return Content(
                        "Unable to create Administrator role.");
                }
            }

            // Check whether user already has the role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return Content(
                    "testadmin@gmail.com already has the Administrator role.");
            }

            // Assign Administrator role
            var result = await _userManager.AddToRoleAsync(
                user,
                roleName);

            if (!result.Succeeded)
            {
                return Content(
                    "Failed to assign Administrator role.");
            }

            return Content(
                "SUCCESS: testadmin@gmail.com has been assigned the Administrator role.");
        }


        // ============================================
        // ASSIGN CUSTOMER ROLE
        // ============================================
        public async Task<IActionResult> AssignCustomer()
        {
            string email = "testcustomer@gmail.com";
            string roleName = "Customer";

            // Find the user
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Content(
                    "User testcustomer@gmail.com was not found. Please register this user first.");
            }

            // Create Customer role if it does not exist
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    return Content(
                        "Unable to create Customer role.");
                }
            }

            // Check whether user already has Customer role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return Content(
                    "testcustomer@gmail.com already has the Customer role.");
            }

            // Assign Customer role
            var result = await _userManager.AddToRoleAsync(
                user,
                roleName);

            if (!result.Succeeded)
            {
                return Content(
                    "Failed to assign Customer role.");
            }

            return Content(
                "SUCCESS: testcustomer@gmail.com has been assigned the Customer role.");
        }


        // ============================================
        // ADMINISTRATOR-ONLY PAGE
        // ============================================
        [Authorize(Roles = "Administrator")]
        public IActionResult Admin()
        {
            return View();
        }


        // ============================================
        // CUSTOMER-ONLY PAGE
        // ============================================
        [Authorize(Roles = "Customer")]
        public IActionResult Customer()
        {
            return View();
        }
    }
}