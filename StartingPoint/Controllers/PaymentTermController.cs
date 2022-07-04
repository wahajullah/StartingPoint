using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.PaymentTermViewModel;
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
    public class PaymentTermController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public PaymentTermController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int PaymentTermID = 0;
            var Id = await _context.PaymentTerms.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                PaymentTermID = 1;
            }
            else
            {
                PaymentTermID = Convert.ToInt32(Id.PaymentTermId.Remove(0, 3)) + 1;
            }
            return "PY-" + PaymentTermID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.PaymentTerm.RoleName)]
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
                    || obj.PaymentTermId.ToLower().Contains(searchValue)
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

        private IQueryable<PaymentTermGridViewModel> GetGridItem()
        {
            try
            {
                return (from _PaymentTerm in _context.PaymentTerms
                        where _PaymentTerm.Cancelled == false
                        select new PaymentTermGridViewModel
                        {
                            Id = _PaymentTerm.Id,
                            PaymentTermId = _PaymentTerm.PaymentTermId,
                            Description = _PaymentTerm.Description,
                            CreatedDate = _PaymentTerm.CreatedDate,                           
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
            PaymentTermCRUDViewModel vm = await _context.PaymentTerms.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            PaymentTermCRUDViewModel vm = new PaymentTermCRUDViewModel();
            if (id > 0) vm = await _context.PaymentTerms.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.PaymentTermId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(PaymentTermCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.PaymentTerms.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            PaymentTerm _PaymentTerm = new PaymentTerm();
                            if (vm.Id > 0)
                            {
                                _PaymentTerm = await _context.PaymentTerms.FindAsync(vm.Id);

                                vm.CreatedDate = _PaymentTerm.CreatedDate;
                                vm.CreatedBy = _PaymentTerm.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_PaymentTerm).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "PaymentTerm Updated Successfully. ID: " + _PaymentTerm.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _PaymentTerm = vm;
                                _PaymentTerm.CreatedDate = DateTime.Now;
                                _PaymentTerm.ModifiedDate = DateTime.Now;
                                _PaymentTerm.CreatedBy = HttpContext.User.Identity.Name;
                                _PaymentTerm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_PaymentTerm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "PaymentTerm Created Successfully. ID: " + _PaymentTerm.Id;
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
                var _PaymentTerm = await _context.PaymentTerms.FindAsync(id);
                _PaymentTerm.ModifiedDate = DateTime.Now;
                _PaymentTerm.ModifiedBy = HttpContext.User.Identity.Name;
                _PaymentTerm.Cancelled = true;

                _context.Update(_PaymentTerm);
                await _context.SaveChangesAsync();
                return new JsonResult(_PaymentTerm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.PaymentTerms.Any(e => e.Id == id);
        }
    }
}
