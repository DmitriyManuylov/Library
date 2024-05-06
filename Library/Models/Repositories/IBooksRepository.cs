using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public interface IBooksRepository: IDisposable
    {
        IList<Book> GetAllBooks();
        Book GetBook(int id);
        int AddBook(Book book);
        void UpdateBook(Book book);
        void EditDescription(Book book);
    }
}
