﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public interface IClientsRepository: IDisposable
    {
        IList<Client> GetClients();
        int AddClient(Client client);

    }
}
