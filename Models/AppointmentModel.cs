namespace Hospital.Models{

    public class AppointmentModel{
        public int Id{get; set;}
        public int PatientId{get; set;}
        public int DoctorId{get; set;}
        public int DepartmentId{get; set;}
        public DateTime AppointmentDate{get; set;}
        public bool? Status{get; set;}
        public bool Cancel{get; set;}
    }
}