using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.MaterialGroupViewModel;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class MaterialGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public MaterialGroupController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task<string> GetMaxID()
        {
            int MaterialGroupId = 0;
            var Id = await _context.MaterialGroups.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                MaterialGroupId = 1;
            }
            else
            {
                MaterialGroupId = Convert.ToInt32(Id.MaterialGroupId.Remove(0, 3)) + 1;
            }
            return "MG-" + MaterialGroupId.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.MaterialGroup.RoleName)]
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

                    ||obj.ServiceId.ToString().ToLower().Contains(searchValue)                    
                    || obj.Description.ToLower().Contains(searchValue)
                    //|| obj.Company.ToLower().Contains(searchValue)

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

        private IQueryable<MaterialGroupCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _MaterialGroup in _context.MaterialGroups    
                        
                        join _Service in _context.Services on _MaterialGroup.ServiceId equals _Service.Id

                        

                        where _MaterialGroup.Cancelled == false
                        select new MaterialGroupCRUDViewModel
                        {
                            Id = _MaterialGroup.Id,
                            MaterialGroupId = _MaterialGroup.MaterialGroupId,
                            ServiceDisplay = _Service.ShortText,                           
                            Description = _MaterialGroup.Description,
                            CreatedDate = _MaterialGroup.CreatedDate, 
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
            MaterialGroupCRUDViewModel vm = await GetGridItem().Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            MaterialGroupCRUDViewModel vm = new MaterialGroupCRUDViewModel();
            ViewBag._LoadddlService = new SelectList(_iCommon.LoadddlService(), "Id", "Name");           
            
            if (id > 0) vm = await _context.MaterialGroups.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.MaterialGroupId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(MaterialGroupCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        MaterialGroup _MaterialGroup = new MaterialGroup();
                        if (vm.Id > 0)
                        {
                            _MaterialGroup = await _context.MaterialGroups.FindAsync(vm.Id);

                            vm.CreatedDate = _MaterialGroup.CreatedDate;
                            vm.CreatedBy = _MaterialGroup.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_MaterialGroup).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Group Updated Successfully. ID: " + _MaterialGroup.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _MaterialGroup = vm;
                            _MaterialGroup.CreatedDate = DateTime.Now;
                            _MaterialGroup.ModifiedDate = DateTime.Now;
                            _MaterialGroup.CreatedBy = HttpContext.User.Identity.Name;
                            _MaterialGroup.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_MaterialGroup);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Group Created Successfully. ID: " + _MaterialGroup.Id;
                            return RedirectToAction(nameof(Index));
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
                var _MaterialGroup = await _context.MaterialGroups.FindAsync(id);
                _MaterialGroup.ModifiedDate = DateTime.Now;
                _MaterialGroup.ModifiedBy = HttpContext.User.Identity.Name;
                _MaterialGroup.Cancelled = true;

                _context.Update(_MaterialGroup);
                await _context.SaveChangesAsync();
                return new JsonResult(_MaterialGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.MaterialGroups.Any(e => e.Id == id);
        }
    }
}
