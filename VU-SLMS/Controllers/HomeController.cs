using VU_SLMS.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net.Mail;
using VU_SLMS.Models;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;

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
        #region Static
        public IActionResult Index()
        {
            ViewBag.TotalEmp = _context.Employees.Count();
            ViewBag.TotalBen = _context.Benefits.Count();
            List<Employee> employees = new List<Employee>();
            foreach (var item in _context.Employees.ToList())
            {
                var leave = _context.Leaves.Where(l => l.Name == "Regular" && l.EmployeeId == item.Id).OrderByDescending(date => date.DateFrom).FirstOrDefault();
                if (leave != null)
                {
                    var count = (DateTime.Now - leave.DateFrom).Days;
                    if (count > 80)
                    {
                        Employee newemp = new Employee();
                        newemp = _context.Employees.Where(e => e.Id == item.Id).FirstOrDefault();
                        employees.Add(newemp);
                    }
                }
                else
                {
                    var empl = _context.Employees.Where(e => e.Id == item.Id).FirstOrDefault();
                    var count = (DateTime.Now - empl.JoiningDate).Days;
                    if (count > 80)
                    {
                        Employee newemp = new Employee();
                        newemp = _context.Employees.Where(e => e.Id == item.Id).FirstOrDefault();
                        employees.Add(newemp);
                    }
                }
            }
            return View(employees.OrderBy(name => name.Name));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(SystemUser systemuser)
        {
            var User = _context.SystemUsers.Where(a => a.UserName == systemuser.UserName && a.Password == systemuser.Password).FirstOrDefault();
            if (User == null)
            {
                ViewBag.Error = "Invalid user name or password";
                return View();
            }
            //Success Login
            HttpContext.Session.SetInt32("Id", User.Id);
            HttpContext.Session.SetString("UserName", User.UserName);
            HttpContext.Session.SetInt32("Type", User.Type.Value);

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
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
        #endregion
        #region Systemuser
        [HttpGet]
        public IActionResult AddSystemUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSystemUser(SystemUser systemUser)
        {
            systemUser.Type = 2;
            _context.SystemUsers.Add(systemUser);
            _context.SaveChanges();
            return RedirectToAction(nameof(SystemUserList));
        }
        [HttpGet]
        public IActionResult UpdateSystemUser(int? id)
        {
            var U = _context.SystemUsers.Find(id);
            if(U != null)
            {
                return View(U);
            }
            return RedirectToAction(nameof(Notfound));
        }
        [HttpPost]
        public IActionResult UpdateSystemUser(SystemUser systemUser)
        {
            if (systemUser != null)
            {
                _context.SystemUsers.Update(systemUser);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Notfound));
        }
        public IActionResult SystemUserList()
        {
            if(HttpContext.Session.GetInt32("Type") == 2)
            {
                var user = _context.SystemUsers.Where(u => u.Id == HttpContext.Session.GetInt32("Id")).ToList();
                return View(user);
            }
            var lst = _context.SystemUsers.Where(u => u.Type == 1 || u.Type == 2).ToList();
            return View(lst);
        }
        public IActionResult DeleteSystemUser(int? id)
        {
            var U = _context.SystemUsers.Find(id);
            if(U != null)
            {
                _context.SystemUsers.Remove(U);
                _context.SaveChanges();
                return RedirectToAction(nameof(SystemUserList));
            }
            return RedirectToAction(nameof(Notfound));
        }
        #endregion
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
        public IActionResult AddUpdateEmployee(Employee employee, IFormFile? Image)
        {
            if (employee.Id == 0)
            {
                //to add image and its address
                if (Image != null)
                {
                    string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

                    using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                    {
                        Image.CopyTo(FS);
                    }
                    employee.Image = FinalFilePathVirtual;
                }
                employee.CreatedDate = DateTime.Now;
                employee.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Employees.Add(employee);
                _context.SaveChanges();
                TempData["success"] = "Employee added successfully";
                return RedirectToAction("EmployeeDetail", new { id = employee.Id });
            }
            else
            {
                if (employee != null)
                {
                    //to update image and its address
                    if (Image != null)
                    {
                        string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

                        using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                        {
                            Image.CopyTo(FS);
                        }
                        employee.Image = FinalFilePathVirtual;
                    }
                    employee.CreatedDate = DateTime.Now;
                    employee.CreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(employee);
                    _context.SaveChanges();
                    TempData["success"] = "Employee updated successfully";
                    return RedirectToAction("EmployeeDetail", new { id = employee.Id });
                }
            }
            return View();
        }
        public IActionResult EmployeeList()
        {
            return View(_context.Employees.ToList().OrderBy(name => name.Name));
        }
        public IActionResult DeleteEmployee(int? id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                var ben = _context.Benefits.Where(b => b.EmployeeId == emp.Id).ToList();
                foreach(var d in ben)
                {
                    _context.Benefits.Remove(d);
                    _context.SaveChanges();
                }
                var lev = _context.Leaves.Where(b => b.EmployeeId == emp.Id).ToList();
                foreach (var l in lev)
                {
                    _context.Leaves.Remove(l);
                    _context.SaveChanges();
                }
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
            return RedirectToAction(nameof(Notfound));
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
                benefit.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Benefits.Add(benefit);
                _context.SaveChanges();
                TempData["success"] = "Benefit added successfully";
                return RedirectToAction("BenefitDetail", new { id = benefit.Id });
            }
            else
            {
                if (benefit != null)
                {
                    benefit.CreatedDate = DateTime.Now;
                    benefit.CreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(benefit);
                    _context.SaveChanges();
                    TempData["success"] = "Benefit updated successfully";
                    return RedirectToAction("BenefitDetail", new { id = benefit.Id });
                }
            }
            return View();
        }
        public IActionResult BenefitList()
        {
            var list = (from ben in _context.Benefits
                       from emp in _context.Employees.Where(m => m.Id == ben.EmployeeId).DefaultIfEmpty()
                       select new BenefitModel
                       {
                           Id = ben.Id,
                           Name = ben.Name,
                           Description = ben.Description,
                           EmployeeId = ben.EmployeeId,
                           EmployeeName = emp.Name,
                           DateOfIssue = ben.DateOfIssue,
                       }).ToList();
            return View(list.OrderBy(name => name.Name));
        }
        public IActionResult BenefitDetail(int? id)
        {
            var list = (from ben in _context.Benefits.Where(b => b.Id == id)
                       from emp in _context.Employees.Where(m => m.Id == ben.EmployeeId).DefaultIfEmpty()
                       select new BenefitModel
                       {
                           Id = ben.Id,
                           Name = ben.Name,
                           Description = ben.Description,
                           EmployeeId = ben.EmployeeId,
                           EmployeeName = emp.Name,
                           DateOfIssue = ben.DateOfIssue,
                       }).FirstOrDefault();
            return View(list);
        }
        public IActionResult DeleteBenefit(int? id)
        {
            var ben = _context.Benefits.Find(id);
            if (ben != null)
            {
                _context.Benefits.Remove(ben);
                _context.SaveChanges();
                return RedirectToAction(nameof(BenefitList));
            }
            return View(nameof(Notfound));
        }
        #endregion
        #region Leaves
        [HttpGet]
        public IActionResult AddUpdateLeave(int? id)
        {
            ViewBag.Totalemp = _context.Employees.ToList();
            if (id != null && id != 0)
            {
                var lev = _context.Leaves.Find(id);
                return View(lev);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddUpdateleave(Leave leave)
        {
            if (leave.Id == 0)
            {
                leave.CreatedDate = DateTime.Now;
                leave.CreatedBy = HttpContext.Session.GetString("UserName");
                leave.LeaveCount = (leave.DateTo - leave.DateFrom).Days + 1;
                _context.Leaves.Add(leave);
                _context.SaveChanges();
                TempData["success"] = "Leave added successfully";
                return RedirectToAction("LeaveDetail", new { id = leave.Id });
            }
            else
            {   
                if (leave != null)
                {
                    leave.CreatedDate = DateTime.Now;
                    leave.CreatedBy = HttpContext.Session.GetString("UserName");
                    leave.LeaveCount = (leave.DateTo - leave.DateFrom).Days + 1;
                    _context.Update(leave);
                    _context.SaveChanges();
                    TempData["success"] = "Leave updated successfully";
                    return RedirectToAction("LeaveDetail", new { id = leave.Id });
                }
            }
            return RedirectToAction(nameof(LeaveDetail));
        }
        public IActionResult LeaveList(int? id)
        {
            if (id != null)
            {
                var Emp = _context.Employees.Where(e => e.Id == id).FirstOrDefault();
                if(Emp != null)
                {
                    var list = (from lev in _context.Leaves.Where(e => e.EmployeeId == Emp.Id)
                                from emp in _context.Employees.Where(m => m.Id == lev.EmployeeId).DefaultIfEmpty()
                                select new LeaveModel
                                {
                                    Id = lev.Id,
                                    Name = lev.Name,
                                    EmployeeId = lev.EmployeeId,
                                    EmployeeName = emp.Name,
                                    DateFrom = lev.DateFrom,
                                    DateTo = lev.DateTo,
                                    Leavecount = lev.LeaveCount,
                                    Description = lev.Description
                                }).ToList();
                    ViewBag.EmployeName = "for " + Emp.Name;
                    ViewBag.Totalemp = _context.Employees.ToList();
                    return View(list.OrderByDescending(date => date.DateFrom));
                }
                return RedirectToAction(nameof(Notfound));
            }
            else
            {
                var list = (from lev in _context.Leaves
                            from emp in _context.Employees.Where(m => m.Id == lev.EmployeeId).DefaultIfEmpty()
                            select new LeaveModel
                            {
                                Id = lev.Id,
                                Name = lev.Name,
                                EmployeeId = lev.EmployeeId,
                                EmployeeName = emp.Name,
                                DateFrom = lev.DateFrom,
                                DateTo = lev.DateTo,
                                Leavecount = lev.LeaveCount,
                                Description = lev.Description
                            }).ToList();
                ViewBag.Totalemp = _context.Employees.ToList();
                return View(list.OrderByDescending(date => date.DateFrom));
            }
        }
        public IActionResult LeaveDetail(int id)
        {
            var list = (from lev in _context.Leaves.Where(l => l.Id == id)
                        from emp in _context.Employees.Where(m => m.Id == lev.EmployeeId).DefaultIfEmpty()
                        select new LeaveModel
                        {
                            Id = lev.Id,
                            Name = lev.Name,
                            EmployeeId = lev.EmployeeId,
                            EmployeeName = emp.Name,
                            DateFrom = lev.DateFrom,
                            DateTo = lev.DateTo,
                            Leavecount = lev.LeaveCount,
                            Description = lev.Description
                        }).FirstOrDefault();
            return View(list);
        }
        public IActionResult DeleteLeave(int? id)
        {
            var lev = _context.Leaves.Find(id);
            if (lev != null)
            {
                _context.Leaves.Remove(lev);
                _context.SaveChanges();
                return RedirectToAction(nameof(LeaveList));
            }
            return View(nameof(Notfound));
        }
        #endregion
    }
}