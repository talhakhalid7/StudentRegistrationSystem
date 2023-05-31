using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;

namespace StudentRegistration.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly SRSDbContext _dbContext;

        public RegistrationController(SRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var registrations = _dbContext.Registrations
                                .Include(r => r.Student)
                                .Include(r => r.Course)
                                .ToList();
            return View(registrations);
        }

        // GET: Registration/Create
        public IActionResult Create()
        {
            // Retrieve students and courses to populate dropdown lists
            var studentsList = new SelectList(_dbContext.Students.OrderBy(l => l.StudentId)
            .ToDictionary(us => us.StudentId, us => us.FirstName + ' ' + us.Surname), "Key", "Value");
            var courseList = new SelectList(_dbContext.Courses.OrderBy(l => l.CourseName)
            .ToDictionary(us => us.CourseId, us => us.CourseName), "Key", "Value");
            ViewBag.Students = studentsList;
            ViewBag.Courses = courseList;

            return View();
        }

        // POST: Registration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the student is already registered for the course
                bool isRegistered = _dbContext.Registrations
                    .Any(r => r.StudentId == model.StudentId && r.CourseId == model.CourseId);

                if (!isRegistered)
                {
                    // Check if the course has available spaces
                    CourseModel course = _dbContext.Courses.Find(model.CourseId);
                    if (course != null && course.AvailableSpaces > 0)
                    {
                        // Register the student for the course
                        RegistrationModel registration = new RegistrationModel
                        {
                            StudentId = model.StudentId,
                            CourseId = model.CourseId
                        };

                        _dbContext.Registrations.Add(registration);
                        _dbContext.SaveChanges();

                        // Decrease the available spaces for the course
                        course.AvailableSpaces--;
                        _dbContext.SaveChanges();

                        return RedirectToAction("Index", "Student");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The selected course is full.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The student is already registered for the selected course.");
                }
            }

            // Retrieve students and courses to populate dropdown lists
            var studentsList = new SelectList(_dbContext.Students.OrderBy(l => l.StudentId)
            .ToDictionary(us => us.StudentId, us => us.FirstName + ' ' + us.Surname), "Key", "Value");
            var courseList = new SelectList(_dbContext.Courses.OrderBy(l => l.CourseName)
            .ToDictionary(us => us.CourseId, us => us.CourseName), "Key", "Value");
            ViewBag.Students = studentsList;
            ViewBag.Courses = courseList;

            return View(model);
        }
        // GET: Registration/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = _dbContext.Registrations
                                .Include(r => r.Student)
                                .Include(r => r.Course)
                                .FirstOrDefault(r => r.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("StudentId,CourseId")] RegistrationModel registration)
        {
            if (id != registration.RegistrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(registration);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.RegistrationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registration);
        }

        // GET: Registration/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = _dbContext.Registrations
                                .Include(r => r.Student)
                                .Include(r => r.Course)
                                .FirstOrDefault(r => r.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var registration = _dbContext.Registrations.Find(id);
            _dbContext.Registrations.Remove(registration);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _dbContext.Registrations.Any(e => e.RegistrationId == id);
        }
    }
}
