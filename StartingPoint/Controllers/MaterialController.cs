using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.MaterialViewModel;
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
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public MaterialController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task<string> GetMaxID()
        {
            int MaterialId = 0;
            var Id = await _context.Materials.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                MaterialId = 1;
            }
            else
            {
                MaterialId = Convert.ToInt32(Id.MaterialId.Remove(0, 6)) + 1;
            }
            return "ME-" + MaterialId.ToString("000000");
        }

        [Authorize(Roles = Pages.MainMenu.Material.RoleName)]
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
                    || obj.MaterialGroupId.ToString().ToLower().Contains(searchValue)
                    ||obj.Brand.ToString().ToLower().Contains(searchValue)
                    || obj.Model.ToString().ToLower().Contains(searchValue)
                    || obj.Manufacturer.ToString().ToLower().Contains(searchValue)
                    || obj.SupplierId.ToString().ToLower().Contains(searchValue)
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

        private IQueryable<MaterialCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _Material in _context.Materials    
                        
                        join _Service in _context.Services on _Material.ServiceId equals _Service.Id
                        join _MaterialGroup in _context.MaterialGroups on _Material.MaterialGroupId equals _MaterialGroup.Id
                        join _AddressSupplier in _context.AddressBooks on _Material.SupplierId equals _AddressSupplier.Id
                        join _Country in _context.Countries on _Material.CountryId equals _Country.Id

                        where _Material.Cancelled == false
                        select new MaterialCRUDViewModel
                        {
                            Id = _Material.Id,
                            MaterialId = _Material.MaterialId,
                            ServiceDisplay = _Service.ShortText,
                            MaterialGroupDisplay = _MaterialGroup.Description,
                            Description = _Material.Description,
                            Unit = _Material.Unit,
                            Rate = _Material.Rate,
                            Brand = _Material.Brand,
                            Model = _Material.Model,
                            Manufacturer = _Material.Manufacturer,
                            SupplierDisplay = _AddressSupplier.Company,
                            Size = _Material.Size,
                            Weight = _Material.Weight,  
                            CountryDisplay = _Country.Description,
                            CreatedDate = _Material.CreatedDate, 
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
            MaterialCRUDViewModel vm = await GetGridItem().Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            MaterialCRUDViewModel vm = new MaterialCRUDViewModel();
            ViewBag._LoadddlService = new SelectList(_iCommon.LoadddlService(), "Id", "Name");
            ViewBag._LoadddlMaterialGroup = new SelectList(_iCommon.LoadddlMaterialGroup(), "Id", "Name");
            ViewBag._LoadddlAddressSupplier = new SelectList(_iCommon.LoadddlAddressSupplier(), "Id", "Name");
            ViewBag._LoadddlCountry = new SelectList(_iCommon.LoadddlCountry(), "Id", "Name");

            if (id > 0) vm = await _context.Materials.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.MaterialId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }
        public void abc()
        {
           // return PartialView("_AddEdit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(MaterialCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Material _Material = new Material();
                        if (vm.Id > 0)
                        {
                            _Material = await _context.Materials.FindAsync(vm.Id);

                            vm.CreatedDate = _Material.CreatedDate;
                            vm.CreatedBy = _Material.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_Material).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Material Updated Successfully. ID: " + _Material.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _Material = vm;
                            _Material.CreatedDate = DateTime.Now;
                            _Material.ModifiedDate = DateTime.Now;
                            _Material.CreatedBy = HttpContext.User.Identity.Name;
                            _Material.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_Material);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Material Created Successfully. ID: " + _Material.Id;
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
                var _Material = await _context.Materials.FindAsync(id);
                _Material.ModifiedDate = DateTime.Now;
                _Material.ModifiedBy = HttpContext.User.Identity.Name;
                _Material.Cancelled = true;

                _context.Update(_Material);
                await _context.SaveChangesAsync();
                return new JsonResult(_Material);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
