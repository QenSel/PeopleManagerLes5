using Microsoft.AspNetCore.Mvc;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PeopleManagerDbContext _dbContext;

        public PeopleController(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var people = _dbContext.People.ToList();
            return View(people);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {

            if (person.FirstName.ToLowerInvariant().Contains("bavo"))
            {
                ModelState.AddModelError("", "Bavo mag hier niet binnen");
            }
            //Als het niet valid is  returned hij naar de Create pagina met de ingegeven informatie die wel correct is maar
            //de velden die niet geldig waren moet je opnieuw invullen
            //if statements zo schrijven voor genestelde if statements te vermijden
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            _dbContext.People.Add(person);

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var person = _dbContext.People.Find(id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, Person person)
        {
            //if statements zo schrijven voor genestelde if statements te vermijden
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            var dbPerson = _dbContext.People.Find(id);
            if (dbPerson is null)
            {
                return RedirectToAction("Index");
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Email = person.Email;
            dbPerson.Description = person.Description;

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
