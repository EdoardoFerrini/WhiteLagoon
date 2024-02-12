using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;
        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }
        public IActionResult Index()
        {
            var villaList = _villaRepo.GetAll();
            return View(villaList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if(ModelState.IsValid)
            {
                if(villa.Name == villa.Description)
                {
                    ModelState.AddModelError("name", "The name cannot be the same as the description");
                    return View(villa);
                }
                _villaRepo.Add(villa);
                _villaRepo.Save();
                TempData["success"] = "The Villa has been created succesfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(villa);
        }

        public IActionResult Edit(int villaId) 
        {
            Villa villa = _villaRepo.Get(u=> u.Id == villaId);
            if (villa == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Edit(Villa villa)
        {
            if(ModelState.IsValid)
            {
				if (villa.Name == villa.Description)
				{
					ModelState.AddModelError("name", "The name cannot be the same as the description");
					return View(villa);
				}

                _villaRepo.Update(villa);
                _villaRepo.Save();
                TempData["success"] = "The Villa has been edited succesfully.";
                return RedirectToAction("Index");

			}
            return View(villa);
        }

        public IActionResult Delete(int villaId)
        {
			Villa villaFromDb = _villaRepo.Get(x => x.Id == villaId);

			if (villaFromDb is null)
			{
				return RedirectToAction("Error", "Home");
			}
            return View(villaFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? villafromDb = _villaRepo.Get(x => x.Id == villa.Id);
            if (villafromDb is not null)
            {
				_villaRepo.Delete(villafromDb);
				_villaRepo.Save();
                TempData["success"] = "Villa deleted successfully";
				return RedirectToAction("Index");
			}
            return RedirectToAction("Error","Home");
        }
    }
}
