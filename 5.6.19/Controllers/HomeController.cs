using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _5._6._19.Models;
using Data;
using Microsoft.Extensions.Configuration;

namespace _5._6._19.Controllers
{
    public class HomeController : Controller
    {
        private string _conn;
        public HomeController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCandidate()
        {
            //var repository = new CTRepository(_conn);
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Candidate c)
        {
            var repository = new CTRepository(_conn);
            repository.AddCandidate(c);
            return Redirect("/home/index");
        }


        public IActionResult ViewPending()
        {
            var repository = new CTRepository(_conn);
            return View(repository.GetPendingCandidates());
        }

        public IActionResult ViewDetails(int id)
        {
            var repository = new CTRepository(_conn);
            return View(repository.GetCandidateById(id));
        }

        public IActionResult ViewConfirmed()
        {
            var repository = new CTRepository(_conn);
            return View(repository.GetConfirmedCandidates());
        }

        public IActionResult ViewDeclined()
        {
            var repository = new CTRepository(_conn);
            return View(repository.GetDeclinedCandidates());
        }

        [HttpPost]
        public void Confirm(int id)
        {
            var repository = new CTRepository(_conn);
            repository.ConfirmCandidate(id);
        }

        [HttpPost]
        public void Decline(int id)
        {
            var repository = new CTRepository(_conn);
            repository.DeclineCandidate(id);
        }

        public IActionResult GetPending()
        {
            var repository = new CTRepository(_conn);
            return Json(repository.GetPendingAmount());
        }

        public IActionResult GetConfirmed()
        {
            var repository = new CTRepository(_conn);
            return Json(repository.GetConfirmedAmount());
        }

        public IActionResult GetDeclined()
        {
            var repository = new CTRepository(_conn);
            return Json(repository.GetDeclinedAmount());
        }
    }
}
