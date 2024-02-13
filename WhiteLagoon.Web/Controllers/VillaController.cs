using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if (ModelState.IsValid)
                {
                    if(villa.Image != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                        using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                        villa.Image.CopyTo(fileStream);
                        villa.ImageUrl = @"\images\VillaImage\" + fileName;
                    }
                    else
                    {
                        villa.ImageUrl = "https://placeholder.co/600x400";
                    }
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

                if(villa.ImageUrl != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if(!string.IsNullOrEmpty(villa.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    villa.Image.CopyTo(fileStream);

                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
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
                if (!string.IsNullOrEmpty(villafromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villafromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.Villa.Delete(villafromDb);
				_unitOfWork.Save();
                TempData["success"] = "Villa deleted successfully";
				return RedirectToAction("Index");
			}
            return RedirectToAction("Error","Home");
        }
    }
}
