using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SweetStore.Models;
using Microsoft.AspNetCore.Authorization; 

namespace SweetStore.Controllers
{
    public class TreatsController : Controller
    {
        private readonly SweetStoreContext _db;

        public TreatsController(SweetStoreContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Treats.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Treat treat)
        {
            _db.Treats.Add(treat);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisTreat = _db.Treats
                .Include(treat => treat.TreatFlavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
        }

        public ActionResult Edit(int id)
        {
            var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Treat treat)
        {
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
            _db.Treats.Remove(thisTreat);
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
public ActionResult AddFlavor(int id, int flavorId)
{
    var treat = _db.Treats.Include(t => t.TreatFlavors).FirstOrDefault(t => t.TreatId == id);
    var flavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == flavorId);

    if (treat != null && flavor != null)
    {
        if (!treat.TreatFlavors.Any(ft => ft.FlavorId == flavorId))
        {
            var joinEntity = new TreatFlavor { TreatId = id, FlavorId = flavorId };
            _db.TreatFlavors.Add(joinEntity);
            _db.SaveChanges();
        }
    }

    return RedirectToAction("Details", new { id = id });
}
    }
}