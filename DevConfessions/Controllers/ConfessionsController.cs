using DevConfessions.Models;
using DevConfessions.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevConfessions.Controllers
{
    public class ConfessionsController : Controller
    {
        private readonly JsonConfessionService _service;
        //private const string UserTokenCookie = "DevConfessionToken";
        private const int TopConfessionsCount = 5;

        public ConfessionsController(JsonConfessionService service) => _service = service;

        public IActionResult Index()
        {
            var topConfessions = _service.GetAllConfessions()
                .OrderByDescending(c => c.Votes)
                .Take(TopConfessionsCount)
                .ToList();

            ViewBag.ShowAll = false;
            return View(topConfessions);
        }

        public IActionResult All()
        {
            var allConfessions = _service.GetAllConfessions()
                .OrderByDescending(c => c.Votes)
                .ToList();

            ViewBag.ShowAll = true;
            return View("Index", allConfessions);
        }

        [HttpPost]
        public IActionResult Vote(string id)
        {
            var confessions = _service.GetAllConfessions();
            var confession = confessions.FirstOrDefault(c => c.Id == id);

            if (confession != null)
            {
                confession.Votes++;
                _service.SaveConfessions(confessions);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Confession confession)
        {

                if (string.IsNullOrWhiteSpace(confession.AuthorName))
                {
                    confession.AuthorName = "Anônimo";
                }

                confession.Id = Guid.NewGuid().ToString();
                confession.Votes = 0;
                confession.DateCreated = DateTime.Now;
                //confession.AuthorToken = Guid.NewGuid().ToString();

                _service.AddConfession(confession);

                // Armazena o token em cookie
                //Response.Cookies.Append("DevConfessionToken", confession.AuthorToken, new CookieOptions
                //{
                //    Expires = DateTime.Now.AddYears(1),
                //    HttpOnly = true,
                //    Secure = true
                //});

                return RedirectToAction(nameof(Index));
            return View(confession);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(string id, Confession updatedConfession)
        //{
        //    var userToken = Request.Cookies["DevConfessionToken"];
        //    if (!_service.IsAuthor(id, userToken))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var existingConfession = _service.GetConfessionById(id);
        //        if (existingConfession == null)
        //        {
        //            return NotFound();
        //        }

        //        // Mantém os valores originais que não devem ser alterados
        //        updatedConfession.Id = existingConfession.Id;
        //        updatedConfession.Votes = existingConfession.Votes;
        //        updatedConfession.DateCreated = existingConfession.DateCreated;
        //        updatedConfession.AuthorToken = existingConfession.AuthorToken;

        //        // Atualiza hashtags
        //        updatedConfession.Hashtags = _service.ExtractHashtags(updatedConfession.Content);

        //        var confessions = _service.GetAllConfessions();
        //        var index = confessions.FindIndex(c => c.Id == id);
        //        if (index != -1)
        //        {
        //            confessions[index] = updatedConfession;
        //            _service.SaveConfessions(confessions);
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(updatedConfession);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(string id)
        //{
        //    var userToken = Request.Cookies["DevConfessionToken"];
        //    if (!_service.IsAuthor(id, userToken))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var confessions = _service.GetAllConfessions();
        //    var confession = confessions.FirstOrDefault(c => c.Id == id);

        //    if (confession != null)
        //    {
        //        confessions.Remove(confession);
        //        _service.SaveConfessions(confessions);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Share(string id)
        {
            var confession = _service.GetAllConfessions().FirstOrDefault(c => c.Id == id);
            return View(confession);
        }
    }
}
