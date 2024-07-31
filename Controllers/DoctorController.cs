using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Hospital.Models;
using Hospital.Controllers.Base;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Hospital.Helpers;
using Hospital.Models.Operational;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DoctorController : SaControllerBase
    {
        public int AppointmentDate { get; private set; }

        public DoctorController(HospitalSchema context) : base(context) { }

        [HttpGet]
        public IEnumerable<DoctorModel> Get()
        {
            DoctorModel[] data = new DoctorModel[0];
            try
            {
                data = _context.Doctor.Select(d => new DoctorModel
                {
                    Id = d.Id,
                    DoctorName = d.DoctorName,
                }).OrderBy(d => d.Id).ToArray();
            }
            catch { }
            return data;
        }

        [HttpGet]
        [Route("{id}")]

        public DoctorModel GetbyId(int id)
        {

            DoctorModel? data = new DoctorModel();
            try
            {
                data = _context.Doctor.Where(y => y.Id == id).Select(d => new DoctorModel
                {
                    Id = d.Id,
                    DoctorName = d.DoctorName,
                }).FirstOrDefault();
            }
            catch { }
            return data;

        }

        [HttpPost]
        public BusinessResult Post(DoctorModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {

                var dbObj = _context.Doctor.FirstOrDefault(d => d.Id == model.Id);
                if (dbObj == null)
                {
                    dbObj = new Doctor();
                    _context.Doctor.Add(dbObj);
                }

                model.MapTo(dbObj);

                _context.SaveChanges();
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


        [HttpPost]
        [Route("Login")]
        public BusinessResult LoginPost(DoctorModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                var dbObj = _context.Doctor.FirstOrDefault(d => d.DoctorName == model.DoctorName && d.Password == model.Password);
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


        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteDoctor(int id)
        {

            if (_context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();

        }


        [HttpPost("scoring-doctor/{AppointmentId}/{ScoreNum}")]

        public IActionResult ScoringDoctor(int AppointmentId, int ScoreNum)
        {   
            var today = DateTime.Now;
            var app = _context.Appointment
            .Where(d => d.Id == AppointmentId && d.AppointmentDate < today)
            .FirstOrDefault();

            if (app == null)
            {
                return NotFound("Appointment not found.");
            }
            
            var doctor = _context.Doctor
            .Where(a => a.Id == app!.DoctorId)
            .FirstOrDefault();

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            var patient = _context.Patient
            .Where(d => d.Id == app!.PatientId)
            .FirstOrDefault();

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var score = new Score
            {
                AppointmentId = app!.Id,
                DoctorId = doctor!.Id,
                PatientId = patient!.Id,
                ScoreNum = ScoreNum

            };

            _context.Score.Add(score);
            _context.SaveChanges();

            return Ok("Score başarıyla kaydedildi.");

        }


        [HttpGet("average-doctor/{DoctorId}")]

        public IActionResult AverageDoctor(int DoctorId)
        {
            var average = 0.0;
            var score = _context.Score
            .Where(d => d.DoctorId == DoctorId)
            .ToArray();
            if (score.Length > 0)
            {
                average = score.Average(s => s.ScoreNum);
            }

            return Ok(new { AverageScore = average });
        }




    }

}
