using Library.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class ClientsController : Controller
    {
        IClientsRepository _clientsRepository;
        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public ActionResult List()
        {
            IList<Client> clientList;
            using (_clientsRepository)
            {
                try
                {
                    clientList = _clientsRepository.GetClients();
                }
                catch
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }
            return View(clientList);

        }

        public ActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddClient(Client client)
        {
            if (client == null || client.FirstName == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            
            client.LastName = client.LastName ?? "";
            client.Phone = client.Phone ?? "";
            client.Email = client.Email ?? "";
            using (_clientsRepository)
            {
                try
                {
                    client.Id = _clientsRepository.AddClient(client);
                }
                catch
                {
                    return View(client);
                }
            }
            return RedirectToAction(nameof(List));
        }
    }
}
