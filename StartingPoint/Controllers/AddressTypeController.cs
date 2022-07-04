using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.AddressTypeViewModel;
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
    public class AddressTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AddressTypeController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int AddressTypeID = 0;
            var Id = await _context.AddressTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                AddressTypeID = 1;
            }
            else
            {
                AddressTypeID = Convert.ToInt32(Id.AddressTypeId.Remove(0, 3)) + 1;
            }
            return "AT-" + AddressTypeID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.AddressType.RoleName)]
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
                    || obj.AddressTypeId.ToLower().Contains(searchValue)
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

        private IQueryable<AddressTypeGridViewModel> GetGridItem()
        {
            try
            {
                return (from _AddressType in _context.AddressTypes
                        where _AddressType.Cancelled == false
                        select new AddressTypeGridViewModel
                        {
                            Id = _AddressType.Id,
                            AddressTypeId = _AddressType.AddressTypeId,
                            Description = _AddressType.Description,
                            CreatedDate = _AddressType.CreatedDate,                           
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
            AddressTypeCRUDViewModel vm = await _context.AddressTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            AddressTypeCRUDViewModel vm = new AddressTypeCRUDViewModel();
            if (id > 0) vm = await _context.AddressTypes.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.AddressTypeId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(AddressTypeCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.AddressTypes.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            AddressType _AddressType = new AddressType();
                            if (vm.Id > 0)
                            {
                                _AddressType = await _context.AddressTypes.FindAsync(vm.Id);

                                vm.CreatedDate = _AddressType.CreatedDate;
                                vm.CreatedBy = _AddressType.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_AddressType).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "AddressType Updated Successfully. ID: " + _AddressType.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _AddressType = vm;
                                _AddressType.CreatedDate = DateTime.Now;
                                _AddressType.ModifiedDate = DateTime.Now;
                                _AddressType.CreatedBy = HttpContext.User.Identity.Name;
                                _AddressType.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_AddressType);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "AddressType Created Successfully. ID: " + _AddressType.Id;
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
                var _AddressType = await _context.AddressTypes.FindAsync(id);
                _AddressType.ModifiedDate = DateTime.Now;
                _AddressType.ModifiedBy = HttpContext.User.Identity.Name;
                _AddressType.Cancelled = true;

                _context.Update(_AddressType);
                await _context.SaveChangesAsync();
                return new JsonResult(_AddressType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.AddressTypes.Any(e => e.Id == id);
        }
    }
}
