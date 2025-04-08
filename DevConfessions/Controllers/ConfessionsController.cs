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
            // Checar o cookie de tempo
            var lastConfessionTimeStr = Request.Cookies["LastConfessionTime"];
            if (lastConfessionTimeStr != null &&
                DateTime.TryParse(lastConfessionTimeStr, out var lastConfessionTime))
            {
                if (DateTime.Now < lastConfessionTime.AddMinutes(30)) // corrigido para 10 min
                {
                    // Já fez uma confissão recentemente
                    TempData["ConfessionError"] = "Chega de confissão por hoje, já traumatizou o sistema! 💻🔥";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (ModelState.IsValid)
            {
                if (confession.Content.Trim().Length >= 5)
                {
                    if (string.IsNullOrWhiteSpace(confession.AuthorName))
                    {
                        confession.AuthorName = "Anônimo";
                    }

                    confession.Votes = 0;
                    confession.DateCreated = DateTime.Now;

                    await _service.AddConfession(confession);

                    // Definir novo cookie com a hora atual
                    Response.Cookies.Append("LastConfessionTime", DateTime.Now.ToString(),
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddMinutes(10),
                            HttpOnly = true
                        });
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
