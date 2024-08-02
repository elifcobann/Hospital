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

namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AppointmentController : SaControllerBase
    {
        public AppointmentController(HospitalSchema context) : base(context) { }

        [HttpGet]
        public IEnumerable<AppointmentModel> Get()
        {
            AppointmentModel[] data = new AppointmentModel[0];
            try
            {
                data = _context.Appointment.Select(d => new AppointmentModel
                {
                    Id = d.Id,
                    AppointmentDate = d.AppointmentDate,
                    Status = d.Status,
                    Cancel = d.Cancel,
                    DepartmentId = d.Doctor!.DepartmentId,
                    DoctorId = d.DoctorId,
                    PatientId = d.PatientId
                }).ToArray();
            }
            catch { }
            return data;
        }

        [HttpGet]
        [Route("{id}")]

        public AppointmentModel GetbyId(int id)
        {

            AppointmentModel? data = new AppointmentModel();
            try
            {
                data = _context.Appointment.Where(y => y.Id == id).Select(d => new AppointmentModel
                {
                    Id = d.Id,
                    AppointmentDate = d.AppointmentDate,
                    Status = d.Status,
                    DepartmentId = d.Doctor!.DepartmentId,
                    DoctorId = d.DoctorId,
                    PatientId = d.PatientId


                }).FirstOrDefault();
            }
            catch { }
            return data;

        }

        [HttpPost]
        public BusinessResult Post(AppointmentModel model)
        {
            BusinessResult result = new BusinessResult();

            try
            {
                if (((model.AppointmentDate.Minute == 0 || model.AppointmentDate.Minute == 30) && (model.AppointmentDate.Hour > 09 && model.AppointmentDate.Hour <20)) && model.AppointmentDate > DateTime.Now)
                {
                    var dbObj = _context.Appointment
                                    .Where(d => d.DoctorId == model.DoctorId && model.AppointmentDate == d.AppointmentDate)
                                    .FirstOrDefault();
                    //  var dbObj = _context.Appointment.FirstOrDefault(d => d.Id == model.Id);
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

                    _context.SaveChanges();
                    result.Result = true;
                    result.RecordId = dbObj.Id;
                }
                else{
                    result.Result = false;
                    result.ErrorMessage = "";
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
        public IActionResult GetPatientAppointments(int patientId)
        {
            var appointments = _context.Appointment
           .Where(a => a.PatientId == patientId)
           .Select(a => new
           {
               a.Id,
               a.AppointmentDate,
               a.Status,
               a.Cancel
           })
           .ToList();

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
        public IActionResult CheckAppointment(int PatientId, int AppointmentId)
        {
            try
            {
                var today = DateTime.UtcNow;
                var appointment = _context.Appointment
                 //.Where(a => a.PatientId == PatientId && a.Id == AppointmentId && a.AppointmentDate > today && a.Status == false)
                 .Where(a => a.Id == AppointmentId && a.Status == false && a.AppointmentDate > today)
                 .FirstOrDefault();

                if (appointment != null)
                {
                    appointment.Status = true; // Randevuyu onayla
                    _context.SaveChanges();
                    return Ok(new { success = true });
                }
                return NotFound(new { error = "Appointment not found", message = "null object" });


            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return Ok(new { error = "Internal server error", message = ex.Message });
            }

        }


        [HttpPost("cancel-appointment/{AppointmentId}")]
        public IActionResult CancelAppointment(int appointmentId)
        {
            var appointment = _context.Appointment.Find(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "Appointment not found." });

            }
            else
            {


                appointment.Cancel = true;
                _context.SaveChanges();

            }

            return Ok(new { success = true, message = "Appointment cancelled successfully." });

        }

        [HttpGet("Appointment-Future")]
        public IActionResult GetFutureAppointment()
        {   
            var time = DateTime.UtcNow.AddHours(3);
            var appointment = _context.Appointment
            .Where(a=> a.AppointmentDate > time)
            .Select(a=> new{
                
                a.AppointmentDate,
                a.Status,
                a.Cancel,
                a.Doctor,
                a.Doctor.Department

            }).ToList();

            return Ok(appointment);
        }




    }

}




