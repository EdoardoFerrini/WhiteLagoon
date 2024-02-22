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
        public IActionResult FinalizeBooking(int villaId, string checkInDate, int nights)
        {
            DateOnly parsedCheckInDate = DateOnly.Parse(checkInDate);
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, include: "VillaAmenities"),
                CheckinDate = parsedCheckInDate,
                Nights = nights,
                CheckoutDate = parsedCheckInDate.AddDays(nights),
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);

        }
    }
}
