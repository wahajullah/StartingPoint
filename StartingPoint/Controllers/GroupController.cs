﻿using Microsoft.AspNetCore.Authorization;
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
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public GroupController(ApplicationDbContext context, ICommon iCommon)
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
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.ShortText.ToString().ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)

                    || obj.ServicesId.ToString().Contains(searchValue));
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
                var _City = await _context.Groups.Where(x => x.Id == id).FirstOrDefaultAsync();
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
            Groups vm = new Groups();
            if (id > 0) vm = await _context.Groups.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0)
            {
                vm.Id = id;//await GetMaxID(); 
            }
            ViewBag._LoadddlService = new SelectList(_iCommon.LoadddlService(), "Id", "Name");
            return PartialView("_AddEdit", vm);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            Groups vm = await _context.Groups.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("Details", vm);
        }
        private IQueryable<Groups> GetGridItem()
        {
            //var ss = _context.Services.FirstOrDefault(x => x.Id == Convert.ToInt32(_Grade.ServicesId)).Description;
            try
            {
                return (from _Grade in _context.Groups
                            //where _Grade. == false
                        select new Groups
                        {
                            //Id = _Grade.GradeId,
                            Id = _Grade.Id,
                            ShortText = _Grade.ShortText,
                            Description = _Grade.Description,
                            ServicesId = _context.Services.FirstOrDefault(x => x.Id == Convert.ToInt32(_Grade.ServicesId)).Description
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(Groups vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Groups.Where(x => x.Id == vm.Id).ToListAsync();
                        if (isCheck.Count() >= 0)
                        {
                            Groups _City = new Groups();
                            //_City = await _context.Grades.FindAsync(vm.GradeId);
                            if (vm.Id > 0)
                            {
                                _City = await _context.Groups.FindAsync(vm.Id);

                                _City.ServicesId = vm.ServicesId;
                                _City.ShortText = vm.ShortText;
                                _City.Description = vm.Description;
                                _context.Update(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Group Updated Successfully. ID: " + _City.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _City = vm;
                                _City.ServicesId = vm.ServicesId;
                                _City.ShortText = vm.ShortText;
                                _City.Description = "-"+vm.Description;
                                _context.Add(_City);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Group Created Successfully. ID: " + _City.Id;
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
        private bool IsExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
