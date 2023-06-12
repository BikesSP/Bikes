using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/auth")]
    public class PostController : ApiControllerBase
    {
        protected PostController(IMediator mediator, ILogger<PostController> logger) : base(mediator, logger)
        {
        }

        // GET: PostController
        public ActionResult Index()
        {
            return null;
        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return null;
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return null;
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return null;
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return null;
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return null;
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return null;
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return null;
            }
        }
    }
}
