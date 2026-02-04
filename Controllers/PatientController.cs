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
    public class PatientController : SaControllerBase
    {
        public PatientController(HospitalSchema context) : base(context) { }

        [HttpGet]
        public async Task<IEnumerable<PatientModel>> Get()
        {
            PatientModel[] data = new PatientModel[0];
            try
            {
                data = await _context.Patient
                    .AsNoTracking()
                    .Select(d => new PatientModel
                    {
                        Id = d.Id,
                        PatientName = d.PatientName,            //_contextten sonrasını userdan alsa olur mu
                    }).ToArrayAsync();

            }
            catch { }
            return data;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<PatientModel?> GetbyId(int id)
        {

            PatientModel? data = new PatientModel();
            try
            {
                data = await _context.Patient
                    .AsNoTracking()
                    .Where(y => y.Id == id).Select(d => new PatientModel
                    {
                        Id = d.Id,
                        PatientName = d.PatientName,
                    }).FirstOrDefaultAsync();
            }
            catch { }
            return data;
        }

        [HttpPost]
        public async Task<BusinessResult> Post(PatientModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {

                var dbObj = await _context.Patient.FirstOrDefaultAsync(d => d.Id == model.Id);
                if (dbObj == null)
                {
                    dbObj = new Patient();
                    _context.Patient.Add(dbObj);
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

        public async Task<IActionResult> DeletePatient(int id)
        {

            if (_context.Patient == null)
            {
                return NotFound();
            }

            var Patient = await _context.Patient.FindAsync(id);
            if (Patient == null)
            {
                return NotFound();
            }

            _context.Patient.Remove(Patient);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<BusinessResult> LoginPost(PatientModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                var dbObj = await _context.Patient
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.PatientName == model.PatientName && d.Password == model.Password);
                if (dbObj == null)
                {
                    result.Result = false;
                    result.ErrorMessage = "kullanici bulunamadi";
                }
                else
                {
                    result.Result = true;
                    result.RecordId = dbObj.Id;
                }
            }
            catch (System.Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }



    }
}
