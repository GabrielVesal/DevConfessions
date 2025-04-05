using DevConfessions.Models;
using DevConfessions.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevConfessions.Controllers
{
    public class ConfessionsController : Controller
    {
        private readonly JsonConfessionService _service;
        private const int TopConfessionsCount = 5;

        public ConfessionsController(JsonConfessionService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            var allConfessions = await _service.GetAllConfessions();
            var topConfessions = allConfessions
                .OrderByDescending(c => c.Votes)
                .Take(TopConfessionsCount)
                .ToList();

            ViewBag.ShowAll = false;
            return View(topConfessions);
        }

        public async Task<IActionResult> All()
        {
            var allConfessions = await _service.GetAllConfessions();
            var orderedConfessions = allConfessions
                .OrderByDescending(c => c.DateCreated)
                .ToList();

            ViewBag.ShowAll = true;
            return View("Index", orderedConfessions);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Vote(string id)
       {
           var cookieKey = $"voted_{id}";
           if (Request.Cookies[cookieKey] != null)
           {
               return Json(new { success = false, message = "Você já votou nesta confissão!" });
           }

           try
           {
               var newVoteCount = await _service.IncrementVote(id);
               Response.Cookies.Append(cookieKey, "true", new CookieOptions
               {
                   Expires = DateTime.Now.AddDays(1)
               });

               return Json(new { success = true, newVoteCount });
           }
           catch (Exception ex)
           {
               return Json(new { success = false, message = ex.Message });
           }
       }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Confession confession)
        {
           if (ModelState.IsValid)
           {
               if (confession.Content.Trim().Length >= 5) 
               {
                   if (string.IsNullOrWhiteSpace(confession.AuthorName))
                   {
                       confession.AuthorName = "Anônimo";
                   }

                   confession.Votes = 0;
                   confession.DateCreated = DateTime.UtcNow.AddHours(-3);
                   
                   await _service.AddConfession(confession);
               }

               return RedirectToAction(nameof(Index));
           }
           return View(confession);
        }

        public async Task<IActionResult> Share(string id)
        {
            var confession = await _service.GetConfessionById(id);
            return View(confession);
        }
    }
}
