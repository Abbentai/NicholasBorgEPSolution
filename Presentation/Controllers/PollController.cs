using DataAccess.Interfaces;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {

        //FromServices being here is methodInjection
        public IActionResult Index([FromServices] IPollRepository pollRepository)
        {
            var polls = pollRepository.GetPolls().OrderByDescending(poll => poll.DateCreated);
            return View(polls);
        }

        [HttpGet] //used to load the page with empty test boxes
        public IActionResult Create()
        {
            Poll currentPoll = new Poll();
            return View(currentPoll);
        }

        [HttpPost]
        public IActionResult Create(Poll poll, [FromServices] IPollRepository pollRepository)
        {
            if (pollRepository.GetPoll(poll.Id) != null)
            {
                TempData["error"] = "Poll already exists!";
                return RedirectToAction("Index");
            }
            else
            {
                pollRepository.CreatePoll(poll);
                return RedirectToAction("Index");
            }
        }

        [HttpGet] 
        public IActionResult Vote(int pollId, [FromServices] IPollRepository pollRepository)
        {
            Poll poll = pollRepository.GetPoll(pollId);
            return View(poll);
        }

        [HttpPost]
        public IActionResult Vote(int pollId, int selectedOption, [FromServices] IPollRepository pollRepository)
        {
            bool success = pollRepository.Vote(pollId, selectedOption);

            if (!success)
            {
                TempData["error"] = "Voting Failed; option or poll is invalid";
                return RedirectToAction("Index");
            }

            TempData["success"] = "Vote submitted successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult View(int pollId, [FromServices] IPollRepository pollRepository)
        {
            Poll poll = pollRepository.GetPoll(pollId);
            return View(poll);
        }
    }
}
