using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //var villaNumberList = _db.VillaNumbers.ToList();
            //var villaNumberList = _db.VillaNumbers.Include(u => u.Villa).ToList();
            var villaNumberList = _unitOfWork.VillaNumber.GetAll(include: "Villa");
            return View(villaNumberList);
        }

        public IActionResult Create()
        {
			//IEnumerable<SelectListItem> dropdownVilla = _db.Villas.ToList().Select(u => new SelectListItem
			//{
			//    Text = u.Name,
			//    Value = u.Id.ToString()
			//});
			VillaNumberVm dropdownVilla = new()
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
        public IActionResult Create(VillaNumberVm villaNumberVm)
        {
            //var villaNumberExist = _db.VillaNumbers.Any(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            var villaNumberExist = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !villaNumberExist && villaNumberVm.VillaNumber.VillaId != 0)
            {
                _unitOfWork.VillaNumber.Add(villaNumberVm.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created succesfully.";
                return RedirectToAction("Index");
            }

			villaNumberVm.DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			if (villaNumberExist)
            {
				TempData["error"] = "This Villa Number already exists.";

				return View(villaNumberVm);
			}

            ModelState.AddModelError("DropdownVilla", "Villa Id is required");
			return View(villaNumberVm);
        }

        public IActionResult Edit(int villaNumberId)
        {
            VillaNumberVm villaNumberVm = new()
            {
                DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId),
            };
        
            if (villaNumberVm == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Edit(VillaNumberVm villaNumberVm)
        {
            var villaNumberExist = _unitOfWork.VillaNumber.Any( u=> u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

			if (ModelState.IsValid && villaNumberExist)
			{
                _unitOfWork.VillaNumber.Update(villaNumberVm.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The VillaNumber has successfully edited.";
				return RedirectToAction("Index");
			}

            if (villaNumberExist)
            {
                villaNumberVm.DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});

				TempData["error"] = "The VillaNumber already exist.";
				return View(villaNumberVm);
			}

            ModelState.AddModelError("DropdownVilla", "The Villa id is not valid");
            return RedirectToAction("Index");
			
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVm villaNumberVm = new()
            {
                VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId),
                DropdownVilla = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };

            if (villaNumberVm.VillaNumber is null)
			{
				return RedirectToAction("Error", "Home");
			}

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVm villaNumberVm)
        {
            
            VillaNumber villaNumberfromDb = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            if (villaNumberfromDb is not null)
            {
				_unitOfWork.VillaNumber.Delete(villaNumberfromDb);
				_unitOfWork.Save();
                TempData["success"] = "the VillaNumber is deleted successfully";
				return RedirectToAction("Index");
			}

            return RedirectToAction("Error","Home");
        }
    }
}
