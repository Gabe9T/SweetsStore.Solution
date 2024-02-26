using Microsoft.AspNetCore.Mvc;
using SweetStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; 

namespace SweetStore.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly SweetStoreContext _db;

        public FlavorsController(SweetStoreContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Flavors.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Flavor flavor)
        {
            _db.Flavors.Add(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisFlavor = _db.Flavors
                .Include(flavor => flavor.TreatFlavors)
                .ThenInclude(join => join.Treat)
                .FirstOrDefault(flavor => flavor.FlavorId == id);
            return View(thisFlavor);
        }

        public ActionResult Edit(int id)
        {
            var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
            return View(thisFlavor);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Flavor flavor)
        {
            _db.Entry(flavor).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
            return View(thisFlavor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
            _db.Flavors.Remove(thisFlavor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
[HttpGet]
public ActionResult AddFlavor(int id)
{
    var treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
    if (treat == null)
    {
        return NotFound();
    }

    ViewBag.TreatId = treat.TreatId;
    ViewBag.Flavors = _db.Flavors.ToList();
    return View(treat);
}

[HttpPost]
[Authorize]
public ActionResult AddTreat(int flavorId, int treatId)
{
    var flavor = _db.Flavors.Include(f => f.TreatFlavors).FirstOrDefault(f => f.FlavorId == flavorId);
    var treat = _db.Treats.FirstOrDefault(t => t.TreatId == treatId);

    if (flavor != null && treat != null)
    {
        if (flavor.TreatFlavors.Any(ft => ft.TreatId == treatId))
        {
            return RedirectToAction("Details", new { id = flavorId });
        }

        var joinEntity = new TreatFlavor { FlavorId = flavorId, TreatId = treatId };
        _db.TreatFlavors.Add(joinEntity);
        _db.SaveChanges();
    }

    return RedirectToAction("Details", new { id = flavorId });
}
    }
}