using Library.Models;
using Library.Models.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Util
{
    public class NinjectRegistrations: NinjectModule
    {
        string _connectionString;
        public NinjectRegistrations(string connectionString) 
        { 
            _connectionString = connectionString;
        }
        public override void Load()
        {
            Bind<IBooksRepository>().ToMethod((context) => new BooksRepository(_connectionString));
            Bind<IClientsRepository>().ToMethod(context => new ClientsRepository(_connectionString));
            Bind<IBooksIssuanceRepo>().ToMethod(context => new BooksIssuanceRepo(_connectionString));
        }
    }
}