using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaList = _db.Villas.ToList();
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
                _db.Villas.Add(villa);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been created succesfully.";
                return RedirectToAction("Index");
            }
            return View(villa);
        }

        public IActionResult Edit(int villaId) 
        {
            var villa = _db.Villas.SingleOrDefault(x => x.Id == villaId);
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
                _db.Villas.Update(villa);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been edited succesfully.";
                return RedirectToAction("Index");

			}
            return View(villa);
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
