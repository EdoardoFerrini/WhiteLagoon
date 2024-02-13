using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaList = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been created succesfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(villa);
        }

        public IActionResult Edit(int villaId) 
        {
            Villa villa = _unitOfWork.Villa.Get(u=> u.Id == villaId);
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

                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been edited succesfully.";
                return RedirectToAction("Index");

			}
            return View(villa);
        }

        public IActionResult Delete(int villaId)
        {
			Villa villaFromDb = _unitOfWork.Villa.Get(x => x.Id == villaId);

			if (villaFromDb is null)
			{
				return RedirectToAction("Error", "Home");
			}
            return View(villaFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? villafromDb = _unitOfWork.Villa.Get(x => x.Id == villa.Id);
            if (villafromDb is not null)
            {
				_unitOfWork.Villa.Delete(villafromDb);
				_unitOfWork.Save();
                TempData["success"] = "Villa deleted successfully";
				return RedirectToAction("Index");
			}
            return RedirectToAction("Error","Home");
        }
    }
}
