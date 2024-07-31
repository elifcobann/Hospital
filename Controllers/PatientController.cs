using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Hospital.Models;
using Hospital.Controllers.Base;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Hospital.Helpers;
using Hospital.Models.Operational;

namespace Hospital.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PatientController : SaControllerBase {
        public PatientController(HospitalSchema context): base(context) {}

        [HttpGet]
        public IEnumerable<PatientModel> Get() {
            PatientModel[] data = new PatientModel[0];
            try
            {
                data= _context.Patient.Select(d => new PatientModel{
                    Id=d.Id,
                    PatientName= d.PatientName,            //_contextten sonrasını userdan alsa olur mu
                }).ToArray();
        
            }
            catch{}
            return data;
        }

        [HttpGet]
        [Route("id")]
         
         
        public PatientModel GetbyId(int id) {

            PatientModel? data = new PatientModel();
            try{
                data =_context.Patient.Where(y=>y.Id == id).Select(d=> new PatientModel{
                    Id=d.Id,
                    PatientName=d.PatientName,
                }).FirstOrDefault();
            }
            catch{}
            return data;
        }

          [HttpPost]
        public BusinessResult Post(PatientModel model){
            BusinessResult result = new BusinessResult();

            try{
                
                var dbObj =_context.Patient.FirstOrDefault(d=>d.Id == model.Id);
                if(dbObj == null){
                    dbObj = new Patient();
                    _context.Patient.Add(dbObj);
                }

                model.MapTo(dbObj);

                _context.SaveChanges();
                result.Result=true;
                result.RecordId= dbObj.Id;
            }
            catch(System.Exception ex)
            {
                result.Result= false;
                result.ErrorMessage= ex.Message;
            }

            return result;
        }

          [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeletePatient(int id){

            if(_context.Patient == null){
                return NotFound();
            }

          var Patient = await _context.Patient.FindAsync(id);
          if(Patient == null){
            return NotFound();
          }

          _context.Patient.Remove(Patient);
          await _context.SaveChangesAsync();

          return NoContent();
   
        }

        [HttpPost]
        [Route("Login")]
        public BusinessResult LoginPost(PatientModel model){
            BusinessResult result = new BusinessResult();

            try{
                var dbObj = _context.Patient.FirstOrDefault(d=> d.PatientName == model.PatientName && d.Password==model.Password);
                if(dbObj == null){
                    result.Result=false;
                    result.ErrorMessage= "kullanici bulunamadi";
                }
                else{
                    result.Result=true;
                    result.RecordId = dbObj.Id;
                }
            }
            catch(System.Exception ex){
                result.Result=false;
                result.ErrorMessage= ex.Message;
            }
            return result;
        }
  
      

}
}