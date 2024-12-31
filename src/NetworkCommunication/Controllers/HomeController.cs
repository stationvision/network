using Microsoft.AspNetCore.Mvc;

namespace NetworkCommunications.Controllers
{
    public class ClientDataViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Duration { get; set; }
        public int Count { get; set; }
        public string MachineName { get; set; }
        public string Status { get; set; }
    }
    public class HomeController : Controller
    {

        public HomeController()
        {
        }
        public async Task<IActionResult> Index()
        {
            //var data = _repository.GetAll().ToList()
            //    .Select(cd => new ClientDataViewModel
            //    {
            //        StartDate = cd.StartDate.Date,
            //        EndDate = cd.EndDate.Value,
            //        Duration = cd.Duration,
            //        Count = cd.Count,
            //        MachineName = cd.MachineName,
            //        Status = cd.Status.ToString()
            //    })
            //    .ToList();
            return View();
        }
    }
}


