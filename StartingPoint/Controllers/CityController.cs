using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.CityViewModel;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CityController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int CityID = 0;
            var Id = await _context.Cities.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                CityID = 1;
            }
            else
            {
                CityID = Convert.ToInt32(Id.CityID.Remove(0, 3)) + 1;
            }
            return "CI-" + CityID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.City.RoleName)]
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
                    || obj.CityID.ToLower().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)                   

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

        private IQueryable<CityGridViewModel> GetGridItem()
        {
            try
            {
                return (from _City in _context.Cities
                        where _City.Cancelled == false
                        select new CityGridViewModel
                        {
                            Id = _City.Id,
                            CityID = _City.CityID,
                            Name = _City.Name,
                            CreatedDate = _City.CreatedDate,                           
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
            CityCRUDViewModel vm = await _context.Cities.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            CityCRUDViewModel vm = new CityCRUDViewModel();
            if (id > 0) vm = await _context.Cities.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.CityID = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(CityCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Cities.Where(x => x.Name == vm.Name).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            City _City = new City();
                            if (vm.Id > 0)
                            {
                                _City = await _context.Cities.FindAsync(vm.Id);

                                vm.CreatedDate = _City.CreatedDate;
                                vm.CreatedBy = _City.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_City).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "City Updated Successfully. ID: " + _City.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _City = vm;
                                _City.CreatedDate = DateTime.Now;
                                _City.ModifiedDate = DateTime.Now;
                                _City.CreatedBy = HttpContext.User.Identity.Name;
                                _City.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "City Created Successfully. ID: " + _City.Id;
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
                var _City = await _context.Cities.FindAsync(id);
                _City.ModifiedDate = DateTime.Now;
                _City.ModifiedBy = HttpContext.User.Identity.Name;
                _City.Cancelled = true;

                _context.Update(_City);
                await _context.SaveChangesAsync();
                return new JsonResult(_City);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
