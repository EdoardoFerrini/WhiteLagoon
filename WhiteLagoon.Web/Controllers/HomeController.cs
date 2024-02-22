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

        [HttpPost]
        public IActionResult Index(HomeVm homeVm)
        {
            homeVm.VillaList = _unitOfWork.Villa.GetAll(include: "VillaAmenities");
            foreach (var villa in homeVm.VillaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            return View(homeVm);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(include:"VillaAmenities").ToList();
            foreach(var villa in villaList)
            {
                if(villa.Id %2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            HomeVm homeVm = new()
            {
                CheckInDate = checkInDate,
                VillaList = villaList,
                Nights = nights
            };
            return PartialView("_VillaList",homeVm);
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
