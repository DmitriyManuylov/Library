using Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models.Repositories
{
    public interface IBooksIssuanceRepo: IDisposable
    {
        IList<Issuance> GetIssuances();

        int AddIssuance(IssuanceViewModel issuance);

        void ReturnBook(int issuanceId);

    }
}
