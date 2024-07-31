using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Context;

namespace Hospital.Context{

    public class Score{
        public int Id {get; set;}
        public double ScoreNum{get; set;}
        public double Average{get; set;}

        [ForeignKey("Doctor")]
        public int DoctorId{get; set;}

        [ForeignKey("Patient")]
        public int PatientId{get; set;}

        [ForeignKey("Appointment")]
        public int AppointmentId{get; set;}

        public virtual Doctor? Doctor{get; set;}
        public virtual Patient? Patient{get; set;}
        public virtual Appointment? Appointment{get; set;}
    
    }
}