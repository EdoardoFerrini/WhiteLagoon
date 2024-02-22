using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FinalizeBooking(int villaId, DateOnly CheckInDate, int nights)
        {
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, include:"VillaAmenities"),
                CheckinDate = CheckInDate,
                Nights = nights,
                CheckoutDate = CheckInDate.AddDays(nights),
            };
            return View(booking);
        }
    }
}
