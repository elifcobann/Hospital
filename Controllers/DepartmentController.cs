using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Hospital.Models;
using Hospital.Controllers.Base;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Hospital.Helpers;
using Hospital.Models.Operational;

namespace Hospital.Controllers{
    [ApiController]
    [Route("[controller]")]

    public class DepartmentController : SaControllerBase {
        public DepartmentController(HospitalSchema context): base(context){}

    
    [HttpGet]
    public IEnumerable<DepartmentModel> Get() {
        DepartmentModel[] data = new DepartmentModel[0];
        try
        {
            data = _context.Department.Select(d => new DepartmentModel{
                Id= d.Id,
                DepartmentName= d.DepartmentName,
            }).ToArray();
        }
        catch{}
        return data;
    }

    [HttpGet]
    [Route("{id}")]
    public DepartmentModel Getbyd(int id) {

        DepartmentModel? data = new DepartmentModel();
        try{
            data = _context.Department.Where(y=> y.Id == id).Select(d=> new DepartmentModel{
                Id =d.Id,
                DepartmentName=d.DepartmentName,
            }).FirstOrDefault();
        }
        catch{}
        return data;

    }

     [HttpPost]
        public BusinessResult Post(DepartmentModel model){
            BusinessResult result = new BusinessResult();

            try
            {
                var dbObj =_context.Department.FirstOrDefault(d => d.Id == model.Id);
                if(dbObj == null){
                    dbObj = new Department();
                    _context.Department.Add(dbObj);
                }

                model.MapTo(dbObj);

                _context.SaveChanges();
                result.Result=true;
                result.RecordId=dbObj.Id;
            }
            catch(System.Exception ex)
            {
                result.Result=false;
                result.ErrorMessage = ex.Message;
            }

            return result;      
        }

     [HttpDelete]
     [Route("{id}")]

     public async Task<IActionResult> DeleteDepartment(int id){

        if(_context.Department == null){
            return NotFound();
        }

        var department = await _context.Department.FindAsync(id);
        if(department == null){
            return NotFound();
        }

        _context.Department.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
     }    

  }
    
}