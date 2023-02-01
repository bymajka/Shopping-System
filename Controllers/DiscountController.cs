using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;

namespace ShoppingSystem.Controllers
{
    public class DiscountController : Controller
    {
        private readonly ShoppingContext _context;

        public DiscountController(ShoppingContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "OnlyGoldenAndWholeSale")]
        public async Task<IActionResult> Index()
        {            
            return View(await _context.Customers.Where(x => x.User.Email == User.Identity.Name).ToListAsync());
        }

    }
}
