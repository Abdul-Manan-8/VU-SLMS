using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net.Mail;
using VU_SLMS.Models;

namespace VU_SLMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly vu_slms_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public HomeController(ILogger<HomeController> logger, vu_slms_dbContext context, IWebHostEnvironment he)
        {
            _logger = logger;
            _context = context;
            _he = he;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Notfound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region Employee
        [HttpGet]
        public IActionResult AddUpdateEmployee(int? id)
        {
            if (id != null && id != 0)
            {
                var emp = _context.Employees.Find(id);
                return View(emp);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddUpdateEmployee(Employee employee)
        {
            if (employee.Id == 0)
            {
                employee.CreatedDate = DateTime.Now;
                employee.CreatedBy = "Admin";
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("EmployeeDetail", new { id = employee.Id });
            }
            else
            {
                if (employee != null)
                {
                    employee.CreatedDate = DateTime.Now;
                    employee.CreatedBy = "Admin";
                    _context.Update(employee);
                    _context.SaveChanges();
                    return RedirectToAction("EmployeeDetail", new { id = employee.Id });
                }
            }
            return View();
        }
        public IActionResult EmployeeList()
        {
            return View(_context.Employees.ToList());
        }
        public IActionResult DeleteEmployee(int? id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
                return RedirectToAction(nameof(EmployeeList));
            }   
            return View(nameof(Notfound));
        }
        public IActionResult EmployeeDetail(int? id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                return View(emp);
            }
            return View(nameof(EmployeeList));
        }
        #endregion
        #region Benefit
        [HttpGet]
        public IActionResult AddUpdateBenefit(int? id)
        {
            ViewBag.Totalemp = _context.Employees.ToList();
            if (id != null && id != 0)
            {
                var ben = _context.Benefits.Find(id);
                return View(ben);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddUpdateBenefit(Benefit benefit)
        {
            if (benefit.Id == 0)
            {
                benefit.CreatedDate = DateTime.Now;
                benefit.CreatedBy = "Admin";
                _context.Benefits.Add(benefit);
                _context.SaveChanges();
                return RedirectToAction("BenefitDetail", new { id = benefit.Id });
            }
            else
            {
                if (benefit != null)
                {
                    benefit.CreatedDate = DateTime.Now;
                    benefit.CreatedBy = "Admin";
                    _context.Update(benefit);
                    _context.SaveChanges();
                    return RedirectToAction("BenefitDetail", new { id = benefit.Id });
                }
            }
            return View();
        }
        #endregion
    }
}