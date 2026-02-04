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

    public class DoctorController : SaControllerBase
    {
        public int AppointmentDate { get; private set; }

        public DoctorController(HospitalSchema context) : base(context) { }

        [HttpGet]
        public async Task<IEnumerable<DoctorModel>> Get()
        {
            DoctorModel[] data = new DoctorModel[0];
            try
            {
                data = await _context.Doctor
                    .AsNoTracking()
                    .Select(d => new DoctorModel
                    {
                        Id = d.Id,
                        DoctorName = d.DoctorName,
                    }).OrderBy(d => d.Id).ToArrayAsync();
            }
            catch { }
            return data;
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<DoctorModel?> GetbyId(int id)
        {

            DoctorModel? data = new DoctorModel();
            try
            {
                data = await _context.Doctor
                    .AsNoTracking()
                    .Where(y => y.Id == id).Select(d => new DoctorModel
                    {
                        Id = d.Id,
                        DoctorName = d.DoctorName,
                    }).FirstOrDefaultAsync();
            }
            catch { }
            return data;

        }

        [HttpPost]
        public async Task<BusinessResult> Post(DoctorModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {

                var dbObj = await _context.Doctor.FirstOrDefaultAsync(d => d.Id == model.Id);
                if (dbObj == null)
                {
                    dbObj = new Doctor();
                    _context.Doctor.Add(dbObj);
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


        [HttpPost]
        [Route("Login")]
        public async Task<BusinessResult> LoginPost(DoctorModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                var dbObj = await _context.Doctor
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DoctorName == model.DoctorName && d.Password == model.Password);
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

        public async Task<IActionResult> ScoringDoctor(int AppointmentId, int ScoreNum)
        {
            var today = DateTime.Now;
            var app = await _context.Appointment
            .Where(d => d.Id == AppointmentId && d.AppointmentDate < today)
            .FirstOrDefaultAsync();

            if (app == null)
            {
                return NotFound("Appointment not found.");
            }

            var doctor = await _context.Doctor
            .Where(a => a.Id == app!.DoctorId)
            .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            var patient = await _context.Patient
            .Where(d => d.Id == app!.PatientId)
            .FirstOrDefaultAsync();

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

            await _context.Score.AddAsync(score);
            await _context.SaveChangesAsync();

            return Ok("Score başarıyla kaydedildi.");

        }


        [HttpGet("average-doctor/{DoctorId}")]

        public async Task<IActionResult> AverageDoctor(int DoctorId)
        {
            var average = 0.0;
            try
            {
                average = await _context.Score
                    .AsNoTracking()
                    .Where(d => d.DoctorId == DoctorId)
                    .AverageAsync(s => (double?)s.ScoreNum) ?? 0.0;
            }
            catch { }

            return Ok(new { AverageScore = average });
        }




    }

}
