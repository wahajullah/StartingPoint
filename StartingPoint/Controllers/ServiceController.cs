using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.ServiceViewModel;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public ServiceController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int ServiceID = 0;
            var Id = await _context.Services.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                ServiceID = 1;
            }
            else
            {
                ServiceID = Convert.ToInt32(Id.ServiceId.Remove(0, 3)) + 1;
            }
            return "SR-" + ServiceID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.Service.RoleName)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var _GetGridItem = GetGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.ServiceId.ToLower().Contains(searchValue)
                    || obj.ShortText.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)                   

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private IQueryable<Service> GetGridItem()
        {
            try
            {
                return (from _Service in _context.Services
                        where _Service.Cancelled == false
                        select new Service
                        {
                            Id = _Service.Id,
                            ServiceId = _Service.ServiceId,
                            ShortText = _Service.ShortText,
                            Description = _Service.Description,
                            DivisionID = _Service.DivisionID,                           
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            Service vm = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            Service vm = new Service();
            if (id > 0) vm = await _context.Services.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.ServiceId = await GetMaxID(); }
            ViewBag.LoadddlDivision = new SelectList(_iCommon.LoadddlDivision(), "Id", "Name");
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(Service vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Services.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() >= 0)
                        {
                            Service _Service = new Service();
                            if (vm.Id > 0)
                            {
                                _Service = await _context.Services.FindAsync(vm.Id);
                                _Service.ShortText = vm.ShortText;
                                _Service.Description = vm.Description;
                                _Service.DivisionID = vm.DivisionID; 
                                _context.Entry(_Service).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Service Updated Successfully. ID: " + _Service.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _Service = vm;
                                //_Service.CreatedDate = DateTime.Now;
                                //_Service.ModifiedDate = DateTime.Now;
                                //_Service.CreatedBy = HttpContext.User.Identity.Name;
                                //_Service.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_Service);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Service Created Successfully. ID: " + _Service.Id;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData["errorDuplication"] = "Record already Exist.";
                            return View("Index");

                        }
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!IsExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Service = await _context.Services.FindAsync(id);
                //_Service.ModifiedDate = DateTime.Now;
                //_Service.ModifiedBy = HttpContext.User.Identity.Name;
                //_Service.Cancelled = true;

                _context.Remove(_Service);
                await _context.SaveChangesAsync();
                return new JsonResult(_Service);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
