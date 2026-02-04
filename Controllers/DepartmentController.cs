using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Hospital.Models;
using Hospital.Controllers.Base;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Hospital.Helpers;
using Hospital.Models.Operational;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DepartmentController : SaControllerBase
    {
        public DepartmentController(HospitalSchema context) : base(context) { }


        [HttpGet]
        public async Task<IEnumerable<DepartmentModel>> Get()
        {
            DepartmentModel[] data = new DepartmentModel[0];
            try
            {
                data = await _context.Department
                    .AsNoTracking()
                    .Select(d => new DepartmentModel
                    {
                        Id = d.Id,
                        DepartmentName = d.DepartmentName,
                    }).ToArrayAsync();
            }
            catch { }
            return data;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<DepartmentModel?> Getbyd(int id)
        {

            DepartmentModel? data = new DepartmentModel();
            try
            {
                data = await _context.Department
                    .AsNoTracking()
                    .Where(y => y.Id == id).Select(d => new DepartmentModel
                    {
                        Id = d.Id,
                        DepartmentName = d.DepartmentName,
                    }).FirstOrDefaultAsync();
            }
            catch { }
            return data;

        }

        [HttpPost]
        public async Task<BusinessResult> Post(DepartmentModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                var dbObj = await _context.Department.FirstOrDefaultAsync(d => d.Id == model.Id);
                if (dbObj == null)
                {
                    dbObj = new Department();
                    _context.Department.Add(dbObj);
                }

                model.MapTo(dbObj);

                await _context.SaveChangesAsync();
                result.Result = true;
                result.RecordId = dbObj.Id;
            }
            catch (System.Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteDepartment(int id)
        {

            if (_context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
