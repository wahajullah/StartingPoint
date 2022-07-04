using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.CountryViewModel;
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
    public class CountryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CountryController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int CountryID = 0;
            var Id = await _context.Countries.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                CountryID = 1;
            }
            else
            {
                CountryID = Convert.ToInt32(Id.CountryId.Remove(0, 3)) + 1;
            }
            return "CO-" + CountryID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.Country.RoleName)]
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
                    || obj.CountryId.ToLower().Contains(searchValue)
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

        private IQueryable<CountryGridViewModel> GetGridItem()
        {
            try
            {
                return (from _Country in _context.Countries
                        where _Country.Cancelled == false
                        select new CountryGridViewModel
                        {
                            Id = _Country.Id,
                            CountryId = _Country.CountryId,
                            Description = _Country.Description,
                            CreatedDate = _Country.CreatedDate,                           
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
            CountryCRUDViewModel vm = await _context.Countries.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            CountryCRUDViewModel vm = new CountryCRUDViewModel();
            if (id > 0) vm = await _context.Countries.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.CountryId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(CountryCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Countries.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            Country _Country = new Country();
                            if (vm.Id > 0)
                            {
                                _Country = await _context.Countries.FindAsync(vm.Id);

                                vm.CreatedDate = _Country.CreatedDate;
                                vm.CreatedBy = _Country.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_Country).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Country Updated Successfully. ID: " + _Country.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _Country = vm;
                                _Country.CreatedDate = DateTime.Now;
                                _Country.ModifiedDate = DateTime.Now;
                                _Country.CreatedBy = HttpContext.User.Identity.Name;
                                _Country.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_Country);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Country Created Successfully. ID: " + _Country.Id;
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
                var _Country = await _context.Countries.FindAsync(id);
                _Country.ModifiedDate = DateTime.Now;
                _Country.ModifiedBy = HttpContext.User.Identity.Name;
                _Country.Cancelled = true;

                _context.Update(_Country);
                await _context.SaveChangesAsync();
                return new JsonResult(_Country);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
