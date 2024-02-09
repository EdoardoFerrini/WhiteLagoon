using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //var villaNumberList = _db.VillaNumbers.ToList();
            var villaNumberList = _db.VillaNumbers.Include(u => u.Villa).ToList();
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
				DropdownVilla = _db.Villas.ToList().Select(u => new SelectListItem
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
            var villaNumberExist = _db.VillaNumbers.Any(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !villaNumberExist && villaNumberVm.VillaNumber.VillaId != 0)
            {
                _db.VillaNumbers.Add(villaNumberVm.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created succesfully.";
                return RedirectToAction("Index");
            }

			villaNumberVm.DropdownVilla = _db.Villas.Select(u => new SelectListItem
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
                DropdownVilla = _db.Villas.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId),
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
            var villaNumberExist = _db.VillaNumbers.Any( u=> u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

			if (ModelState.IsValid && villaNumberExist)
			{
                _db.VillaNumbers.Update(villaNumberVm.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The VillaNumber has successfully edited.";
				return RedirectToAction("Index");
			}

            if (villaNumberExist)
            {
				villaNumberVm.DropdownVilla = _db.Villas.Select(u => new SelectListItem
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

        public IActionResult Delete(int villaId)
        {
			Villa villaFromDb = _db.Villas.SingleOrDefault(x => x.Id == villaId);
			if (villaFromDb is null)
			{
				return RedirectToAction("Error", "Home");
			}
            return View(villaFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? villafromDb = _db.Villas.FirstOrDefault(x => x.Id == villa.Id);
            if (villafromDb is not null)
            {
				_db.Villas.Remove(villafromDb);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
            return RedirectToAction("Error","Home");
        }
    }
}
