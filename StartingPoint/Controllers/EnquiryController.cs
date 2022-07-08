using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class EnquiryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public EnquiryController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
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
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                //{
                //    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                //}

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.id.ToString().Contains(searchValue)
                    || obj.Project.ToString().ToLower().Contains(searchValue)
                    || obj.Code.ToLower().Contains(searchValue)
                    || obj.Location.ToLower().Contains(searchValue)
                    || obj.MainContract.ToLower().Contains(searchValue)
                    || obj.Consultant.ToLower().Contains(searchValue)
                    || obj.Client.ToLower().Contains(searchValue)
                    || obj.Status.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                var j = Json(new { data = result });
                return Json(new { data = result });//draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal,

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _City = await _context.Enquiry.Where(x => x.id == id).FirstOrDefaultAsync();
                _context.Remove(_City);
                await _context.SaveChangesAsync();
                return new JsonResult(_City);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetMaxID()
        {
            int enq = 0;
            Enquiry Id =  _context.Enquiry.OrderByDescending(x => x.id).FirstOrDefault();
            if (Id == null)
            {
                enq = 1;
            }
            else
            {
                enq = Id.id + 1;// Convert.ToInt32(Id.GradeId.ToString().Remove(0, 3)) + 1;
            }
            return "EQ-" + enq.ToString("00000");
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            Enquiry vm = new Enquiry();
            if (id > 0) vm = await _context.Enquiry.Where(x => x.id == id).SingleOrDefaultAsync();
            if (id == 0)
            {
                vm.Code = GetMaxID(); 
            }
            //var grpList = await _context.Enquiry.Where(x => x.Id == id).SingleOrDefaultAsync();
            //ViewBag._LoadddlService = new SelectList(_iCommon.LoadddlGroup(), "Id", "Name");
            return PartialView("_AddEdit", vm);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            Enquiry vm = await _context.Enquiry.FirstOrDefaultAsync(m => m.id == id);
            if (vm == null) return NotFound();
            ViewBag.detail = "True";
            return PartialView("Details", vm);
        }
        private IQueryable<Enquiry> GetGridItem()
        {
            //var ss = _context.Services.FirstOrDefault(x => x.Id == Convert.ToInt32(_Grade.ServicesId)).Description;
            try
            {
                return (from enq in _context.Enquiry
                            //where _Grade. == false
                        select new Enquiry
                        {
                            //Id = _Grade.GradeId,
                            id = enq.id,
                            Code = enq.Code,//GetMaxID(),
                            Client = enq.Client,
                            Project = enq.Project,
                            Location = enq.Location,
                            MainContract = enq.MainContract,
                            Consultant = enq.Consultant,
                            Service = enq.Service,
                            Status = enq.Status
                        }) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(Enquiry vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Enquiry.Where(x => x.id == vm.id).ToListAsync();
                        if (isCheck.Count() >= 0)
                        {
                            Enquiry _City = new Enquiry();
                            //_City = await _context.Grades.FindAsync(vm.GradeId);
                            if (vm.id > 0)
                            {
                                _City = await _context.Enquiry.FindAsync(vm.id);
                                _City.Code = vm.Code;
                                _City.Service = vm.Service;
                                //_City.Status = vm.Status;
                                _City.Consultant = vm.Consultant;
                                _City.Client = vm.Client;
                                _City.MainContract = vm.MainContract;
                                _City.Location = vm.Location;
                                _City.Project = vm.Project;
                                _City.Remarks = vm.Remarks;
                                _context.Update(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlertModal"] = "Enquiry Updated Successfully. ID: " + _City.id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                vm.Status = "Pending";
                                _context.Add(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlertModal"] = "Enquiry Created Successfully. ID: " + _City.id+1;
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
                    if (!IsExists(vm.id))
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
        private bool IsExists(long id)
        {
            return _context.Enquiry.Any(e => e.id == id);
        }
    }
}
