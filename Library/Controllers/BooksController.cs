using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        IBooksRepository _booksRepository;
        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        // GET: Book
        public ActionResult List()
        {
            IList<Book> books;

            using (_booksRepository)
            {
                try
                {
                    books = _booksRepository.GetAllBooks();
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }
            return View(books);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Description))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            using (_booksRepository)
            {
                try
                {
                    book.Id = _booksRepository.AddBook(book);
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public ActionResult EditDescription(int bookId)
        {
            Book book;
            using (_booksRepository)
            {
                try
                {
                    book = _booksRepository.GetBook(bookId);
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult EditDescription(Book book)
        {
            if (book == null || string.IsNullOrEmpty(book.Description))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (_booksRepository)
            {
                try
                {
                    _booksRepository.EditDescription(book);
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