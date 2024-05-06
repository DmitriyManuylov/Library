using Library.Models;
using Library.Models.Repositories;
using Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class IssuanceController : Controller
    {
        IBooksIssuanceRepo _booksIssuanceRepo;

        public IssuanceController(IBooksIssuanceRepo booksIssuanceRepo)
        {
            _booksIssuanceRepo = booksIssuanceRepo;
        }


        [HttpGet]
        public ActionResult List()
        {
            IList<Issuance> issuedBooks;
            using (_booksIssuanceRepo)
            {
                issuedBooks = _booksIssuanceRepo.GetIssuances();
            }
            return View(issuedBooks);
        }

        [HttpGet]
        public ActionResult AddIssuance()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddIssuance(IssuanceViewModel issuance)
        {
            if (issuance == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (_booksIssuanceRepo)
            {
                try
                {
                    _booksIssuanceRepo.AddIssuance(issuance);
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public ActionResult ReturnBook(int issuanceId)
        {
            using(_booksIssuanceRepo)
            {
                try
                {
                    _booksIssuanceRepo.ReturnBook(issuanceId);
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }
            

            return RedirectToAction(nameof(List));
        }
    }
}