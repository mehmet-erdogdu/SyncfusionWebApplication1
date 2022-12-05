using Microsoft.AspNetCore.Mvc;
using SyncfusionWebApplication1.Models;
using System.Diagnostics;
using System.Text.Json;
using Syncfusion.EJ2.Base;

namespace SyncfusionWebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EfDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            EfDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Source([FromBody] DataManagerRequest req)
        {
            var query = _context.Tests.AsQueryable();
            var operation = new DataOperations();
            if (req.Search is { Count: > 0 })
                query = operation.PerformSearching(query, req.Search);

            if (req.Sorted is { Count: > 0 })
                query = operation.PerformSorting(query, req.Sorted);

            if (req.Where is { Count: > 0 })
                query = operation.PerformFiltering(query, req.Where, req.Where[0].Operator);

            var count = query.Count();
            if (req.Skip != 0)
                query = operation.PerformSkip(query, req.Skip);

            if (req.Take != 0) query = operation.PerformTake(query, req.Take);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return req.RequiresCounts
                ? new JsonResult(new { result = query, count }, options)
                : new JsonResult(query, options);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}