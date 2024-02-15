using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            var villaNumberList = _unitOfWork.Amenity.GetAll(include:"Villa");
            return View(villaNumberList);
        }

        public IActionResult Create()
        {

			AmenityVm dropdownVilla = new()
			{
				DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				})
			};
			return View(dropdownVilla);
        }

        [HttpPost]
        public IActionResult Create(AmenityVm amenityVm)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVm.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created succesfully.";
                return RedirectToAction("Index");
            }

			amenityVm.DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});


            ModelState.AddModelError("DropdownVilla", "Villa Id is required");
			return View(amenityVm);
        }

        public IActionResult Edit(int amenityId)
        {
            AmenityVm amenityVm = new()
            {
                DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId),
            };
        
            if (amenityVm == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(amenityVm);
        }

        [HttpPost]
        public IActionResult Edit(AmenityVm amenityVm)
        {
            var amenityExist = _unitOfWork.Amenity.Any( u=> u.Id == amenityVm.Amenity.Id);

			if (ModelState.IsValid && amenityExist)
			{
                _unitOfWork.Amenity.Update(amenityVm.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has successfully edited.";
				return RedirectToAction("Index");
			}

            if (amenityExist)
            {
                amenityVm.DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});

				TempData["error"] = "The Amenity already exist.";
				return View(amenityVm);
			}

            ModelState.AddModelError("DropdownVilla", "The Villa id is not valid");
            return RedirectToAction("Index");
			
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVm amenityVm = new()
            {
                
                DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
				Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenityId)
			};

            if (amenityVm.Amenity is null)
			{
				return RedirectToAction("Error", "Home");
			}

            return View(amenityVm);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVm amenityVm)
        {
            
            Amenity amenityFromDb = _unitOfWork.Amenity.Get(x => x.Id == amenityVm.Amenity.Id);
            if (amenityFromDb is not null)
            {
				_unitOfWork.Amenity.Delete(amenityFromDb);
				_unitOfWork.Save();
                TempData["success"] = "the VillaNumber is deleted successfully";
				return RedirectToAction("Index");
			}

            return RedirectToAction("Error","Home");
        }
    }
}
