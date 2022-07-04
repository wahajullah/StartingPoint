using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.AddressBookViewModel;
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
    public class AddressBookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AddressBookController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task<string> GetMaxID()
        {
            int AddressId = 0;
            var Id = await _context.AddressBooks.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                AddressId = 1;
            }
            else
            {
                AddressId = Convert.ToInt32(Id.AddressId.Remove(0, 5)) + 1;
            }
            return "AD-" + AddressId.ToString("00000");
        }

        [Authorize(Roles = Pages.MainMenu.AddressBook.RoleName)]
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
                    || obj.AddressTypeId.ToString().ToLower().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Company.ToLower().Contains(searchValue)

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

        private IQueryable<AddressBookCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _AddressBook in _context.AddressBooks                       
                        join _City in _context.Cities on _AddressBook.CityId equals _City.Id
                        join _Country in _context.Countries on _AddressBook.CountryId equals _Country.Id
                        join _AddressType in _context.AddressTypes on _AddressBook.AddressTypeId equals _AddressType.Id
                        join _Status in _context.Statuses on _AddressBook.StatusId equals _Status.Id
                        where _AddressBook.Cancelled == false
                        select new AddressBookCRUDViewModel
                        {
                            Id = _AddressBook.Id,
                            AddressId = _AddressBook.AddressId,
                            Name = _AddressBook.Name,
                            JobTitle = _AddressBook.JobTitle,
                            PPhone = _AddressBook.PPhone,
                            Mobile = _AddressBook.Mobile,
                            PEmail = _AddressBook.PEmail,
                            Company = _AddressBook.Company,
                            OEmail = _AddressBook.OEmail,
                            OPhone = _AddressBook.OPhone,
                            OFax = _AddressBook.OFax,
                            Website = _AddressBook.Website,
                            Pobox = _AddressBook.Pobox,
                            AddressTypeDisplay = _AddressType.AddressTypeId,   
                            CityDisplay = _City.Name,
                            CountryDisplay = _Country.CountryId,
                            StatusDisplay = _Status.StatusId,                           
                            Address = _AddressBook.Address,
                            CreatedDate = _AddressBook.CreatedDate, 
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
            AddressBookCRUDViewModel vm = await GetGridItem().Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            AddressBookCRUDViewModel vm = new AddressBookCRUDViewModel();
            ViewBag._LoadddlCity = new SelectList(_iCommon.LoadddlCity(), "Id", "Name");
            ViewBag._LoadddlAddressType = new SelectList(_iCommon.LoadddlAddressType(), "Id", "Name");
            ViewBag._LoadddlStatus = new SelectList(_iCommon.LoadddlStatus(), "Id", "Name");
            ViewBag._LoadddlCountry = new SelectList(_iCommon.LoadddlCountry(), "Id", "Name");
            
            if (id > 0) vm = await _context.AddressBooks.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.AddressId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(AddressBookCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        AddressBook _AddressBook = new AddressBook();
                        if (vm.Id > 0)
                        {
                            _AddressBook = await _context.AddressBooks.FindAsync(vm.Id);

                            vm.CreatedDate = _AddressBook.CreatedDate;
                            vm.CreatedBy = _AddressBook.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_AddressBook).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Address Updated Successfully. ID: " + _AddressBook.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _AddressBook = vm;
                            _AddressBook.CreatedDate = DateTime.Now;
                            _AddressBook.ModifiedDate = DateTime.Now;
                            _AddressBook.CreatedBy = HttpContext.User.Identity.Name;
                            _AddressBook.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_AddressBook);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Address Created Successfully. ID: " + _AddressBook.Id;
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
                var _AddressBook = await _context.AddressBooks.FindAsync(id);
                _AddressBook.ModifiedDate = DateTime.Now;
                _AddressBook.ModifiedBy = HttpContext.User.Identity.Name;
                _AddressBook.Cancelled = true;

                _context.Update(_AddressBook);
                await _context.SaveChangesAsync();
                return new JsonResult(_AddressBook);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.AddressBooks.Any(e => e.Id == id);
        }
    }
}
