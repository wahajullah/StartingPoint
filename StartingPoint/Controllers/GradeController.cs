using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.CityViewModel;
using StartingPoint.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public GradeController(ApplicationDbContext context, ICommon iCommon)
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
                    _GetGridItem = _GetGridItem.Where(obj => obj.GradeId.ToString().Contains(searchValue)
                    || obj.GradeId.ToString().ToLower().Contains(searchValue)
                    || obj.GradeName.ToLower().Contains(searchValue)

                    || obj.Section.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                var j = Json(new { data = result });
                return Json(new {  data = result });//draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal,

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
                var _City = await _context.Grades.Where(x => x.GradeId == id).FirstOrDefaultAsync();



                _context.Remove(_City);
                await _context.SaveChangesAsync();
                return new JsonResult(_City);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> GetMaxID()
        {
            int CityID = 0;
            var Id = await _context.Grades.OrderByDescending(x => x.GradeId).FirstOrDefaultAsync();
            if (Id == null)
            {
                CityID = 1;
            }
            else
            {
                CityID = Id.GradeId + 1;// Convert.ToInt32(Id.GradeId.ToString().Remove(0, 3)) + 1;
            }
            return CityID;// "GR-" + CityID.ToString("000");
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            Grade vm = new Grade();
            if (id > 0) vm = await _context.Grades.Where(x => x.GradeId == id).SingleOrDefaultAsync();
            if (id == 0) {
                vm.GradeId = id;//await GetMaxID(); 
            }
            return PartialView("_AddEdit", vm);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            Grade vm = await _context.Grades.FirstOrDefaultAsync(m => m.GradeId == id);
            if (vm == null) return NotFound();
            return PartialView("Details", vm);
        }
        private IQueryable<Grade> GetGridItem()
        {
            try
            {
                return (from _Grade in _context.Grades
                        //where _Grade. == false
                        select new Grade
                        {
                            //Id = _Grade.GradeId,
                            GradeId = _Grade.GradeId,
                            GradeName = _Grade.GradeName,
                            Section = _Grade.Section
                        }).OrderByDescending(x => x.GradeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(Grade vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Grades.Where(x => x.GradeName == vm.GradeName).ToListAsync();
                        if (isCheck.Count() >= 0)
                        {
                            Grade _City = new Grade();
                            //_City = await _context.Grades.FindAsync(vm.GradeId);
                            if (vm.GradeId > 0)
                            {
                                _City = await _context.Grades.FindAsync(vm.GradeId);

                                 _City.Section = vm.Section;
                                _City.GradeName = vm.GradeName;
                                //_context.Entry(_City).CurrentValues.SetValues(vm);
                                _context.Update(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "City Updated Successfully. ID: " + _City.GradeId;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _City = vm;
                                vm.Section = _City.Section;
                                vm.GradeName = _City.GradeName;
                                _context.Add(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Grade Created Successfully. ID: " + _City.GradeId;
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
                    if (!IsExists(vm.GradeId))
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
            return _context.Grades.Any(e => e.GradeId == id);
        }


    }
}
