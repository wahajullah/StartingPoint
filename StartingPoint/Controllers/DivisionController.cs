using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.DivisionViewModel;
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
    public class DivisionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DivisionController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int DivisionID = 0;
            var Id = await _context.Divisions.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                DivisionID = 1;
            }
            else
            {
                DivisionID = Convert.ToInt32(Id.DivisionId.Remove(0, 3)) + 1;
            }
            return "DV-" + DivisionID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.Division.RoleName)]
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
                    || obj.DivisionId.ToLower().Contains(searchValue)
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

        private IQueryable<DivisionGridViewModel> GetGridItem()
        {
            try
            {
                return (from _Division in _context.Divisions
                        where _Division.Cancelled == false
                        select new DivisionGridViewModel
                        {
                            Id = _Division.Id,
                            DivisionId = _Division.DivisionId,
                            Description = _Division.Description,
                            //CreatedDate = _Division.CreatedDate,                           
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
            DivisionCRUDViewModel vm = await _context.Divisions.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            DivisionCRUDViewModel vm = new DivisionCRUDViewModel();
            if (id > 0) vm = await _context.Divisions.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.DivisionId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(DivisionCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Divisions.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() >= 0)
                        {
                            Division _Division = new Division();
                            if (vm.Id > 0)
                            {
                                _Division = await _context.Divisions.FindAsync(vm.Id);

                                _Division.Description = vm.Description;
                                //vm.CreatedDate = _Division.CreatedDate;
                                //vm.CreatedBy = _Division.CreatedBy;
                                //vm.ModifiedDate = DateTime.Now;
                                //vm.ModifiedBy = HttpContext.User.Identity.Name;
                                //_context.Entry(_Division).CurrentValues.SetValues(vm);
                                _context.Update(_Division);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Division Updated Successfully. ID: " + _Division.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _Division = vm;
                                _Division.CreatedDate = DateTime.Now;
                                _Division.ModifiedDate = DateTime.Now;
                                _Division.CreatedBy = HttpContext.User.Identity.Name;
                                _Division.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_Division);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Division Created Successfully. ID: " + _Division.Id;
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
                var _Division = await _context.Divisions.FindAsync(id);
                _Division.ModifiedDate = DateTime.Now;
                _Division.ModifiedBy = HttpContext.User.Identity.Name;
                _Division.Cancelled = true;

                _context.Update(_Division);
                await _context.SaveChangesAsync();
                return new JsonResult(_Division);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Divisions.Any(e => e.Id == id);
        }
    }
}
