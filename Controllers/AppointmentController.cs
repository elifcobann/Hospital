using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Hospital.Models;
using Hospital.Controllers.Base;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Hospital.Helpers;
using Hospital.Models.Operational;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using AutoMapper.Configuration.Annotations;
using System.Globalization;
using Microsoft.OpenApi.Validations;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AppointmentController : SaControllerBase
    {
        public AppointmentController(HospitalSchema context) : base(context) { }

        [HttpGet]
        public async Task<IEnumerable<AppointmentModel>> Get()
        {
            try
            {
                return await _context.Appointment
                    .AsNoTracking()
                    .Select(d => new AppointmentModel
                    {
                        Id = d.Id,
                        AppointmentDate = d.AppointmentDate,
                        Status = d.Status,
                        Cancel = d.Cancel,
                        DepartmentId = d.Doctor!.DepartmentId,
                        DoctorId = d.DoctorId,
                        PatientId = d.PatientId
                    }).ToArrayAsync();
            }
            catch
            {
                return Array.Empty<AppointmentModel>();
            }
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<AppointmentModel?> GetbyId(int id)
        {
            try
            {
                return await _context.Appointment
                    .AsNoTracking()
                    .Where(y => y.Id == id).Select(d => new AppointmentModel
                    {
                        Id = d.Id,
                        AppointmentDate = d.AppointmentDate,
                        Status = d.Status,
                        DepartmentId = d.Doctor!.DepartmentId,
                        DoctorId = d.DoctorId,
                        PatientId = d.PatientId
                    }).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<BusinessResult> Post(AppointmentModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                if (((model.AppointmentDate.Minute == 0 || model.AppointmentDate.Minute == 30) && (model.AppointmentDate.Hour > 09 && model.AppointmentDate.Hour < 20)) && model.AppointmentDate > DateTime.Now)
                {
                    var dbObj = await _context.Appointment
                                    .Where(d => d.DoctorId == model.DoctorId && model.AppointmentDate == d.AppointmentDate)
                                    .FirstOrDefaultAsync();

                    if (dbObj == null)
                    {
                        dbObj = new Appointment();
                        _context.Appointment.Add(dbObj);
                    }
                    else
                    {
                        result.Result = false;
                        return result;
                    }

                    model.MapTo(dbObj);

                    await _context.SaveChangesAsync();
                    result.Result = true;
                    result.RecordId = dbObj.Id;
                }
                else
                {
                    result.Result = false;
                    result.ErrorMessage = "Invalid appointment time.";
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

        public async Task<IActionResult> DeleteAppointment(int id)
        {

            if (_context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();

        }



        [HttpGet("patient-appointments/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var appointments = await _context.Appointment
           .AsNoTracking()
           .Where(a => a.PatientId == patientId)
           .Select(a => new
           {
               a.Id,
               a.AppointmentDate,
               a.Status,
               a.Cancel
           })
           .ToListAsync();

            if (appointments.Any())
            {

                return Ok(new { success = true, appointments });
            }
            else
            {

                return NotFound(new { success = false, message = "No appointments found for this patient." });
            }
        }


        [HttpPut("check-appointment/{PatientId}/{AppointmentId}")]
        public async Task<IActionResult> CheckAppointment(int PatientId, int AppointmentId)
        {
            try
            {
                var today = DateTime.UtcNow;
                var appointment = await _context.Appointment
                 .Where(a => a.Id == AppointmentId && a.Status == false && a.AppointmentDate > today)
                 .FirstOrDefaultAsync();

                if (appointment != null)
                {
                    appointment.Status = true; // Randevuyu onayla
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true });
                }
                return NotFound(new { error = "Appointment not found", message = "null object" });


            }
            catch (Exception ex)
            {
                return Ok(new { error = "Internal server error", message = ex.Message });
            }

        }


        [HttpPost("cancel-appointment/{AppointmentId}")]
        public async Task<IActionResult> CancelAppointment(int AppointmentId)
        {
            var appointment = await _context.Appointment.FindAsync(AppointmentId);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "Appointment not found." });

            }
            else
            {
                appointment.Cancel = true;
                await _context.SaveChangesAsync();
            }

            return Ok(new { success = true, message = "Appointment cancelled successfully." });

        }

        [HttpGet("Appointment-Future")]
        public async Task<IActionResult> GetFutureAppointment()
        {
            var time = DateTime.UtcNow.AddHours(3);
            var appointment = await _context.Appointment
            .AsNoTracking()
            .Where(a => a.AppointmentDate > time)
            .Select(a => new
            {
                a.AppointmentDate,
                a.Status,
                a.Cancel,
                a.Doctor,
                a.Doctor!.Department
            }).ToListAsync();

            return Ok(appointment);
        }

    }

}
