using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVm homeVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(include:"VillaAmenities"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };

            return View(homeVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
