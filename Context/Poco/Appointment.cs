using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Hospital.Context;
using Microsoft.AspNetCore.SignalR;

namespace Hospital.Context{

    public class Appointment{
        public int Id{get; set;}
        public DateTime AppointmentDate{get; set;}
        public bool Status{get; set;} 
        public bool Cancel{get; set;}

        [ForeignKey("Doctor")]
        public int DoctorId{get; set;}
    

        [ForeignKey("Patient")]
        public int PatientId{get; set;}
        
        public virtual Doctor? Doctor{get; set;}
        public virtual Patient? Patient{get; set;}
        //public int AppointmentId { get; internal set; }
    }
}