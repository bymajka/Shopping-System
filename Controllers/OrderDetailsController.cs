using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;

namespace ShoppingSystem.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ShoppingContext _context;

        public OrderDetailsController(ShoppingContext context)
        {
            _context = context;
        }

        [Route("OrderDetails/Index/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            var shoppingContext = _context.OrderDetails
                .Include(o => o.Order)
                .Where(i => i.OrderId == id).Include(o => o.Product);
            return View(await shoppingContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Quantity")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                UpdateOrderCost(UpdateTotalCost(orderDetail));
                return RedirectToAction(nameof(Index), new {id = orderDetail.OrderId});
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ID", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Quantity")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    _context.SaveChanges();
                    UpdateOrderCost(UpdateTotalCost(orderDetail));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new {id = orderDetail.OrderId});
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ID", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            UpdateOrderCost(orderDetail);
            return RedirectToAction(nameof(Index), new {id = orderDetail.OrderId});
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.Id == id);
        }

        [NonAction]
        private OrderDetail UpdateTotalCost(OrderDetail orderDetail)
        {
            orderDetail.TotalCost = (decimal) (_context.Products.Find(orderDetail.ProductId).Price * orderDetail.Quantity);
            _context.SaveChanges();
            return orderDetail;
        }

        [NonAction]
        private void UpdateOrderCost(OrderDetail orderDetail)
        {
            var order = _context.Orders.Find(orderDetail.OrderId);
            var orderDetails = _context.OrderDetails.Where(odt => order.Id == odt.OrderId);
            var costs = orderDetails .Select(o => o.TotalCost);
            _context.Orders.Find(orderDetail.OrderId).TotalCost = costs.Sum();
            _context.SaveChanges();
        }
    }
}
