using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Context;
using Microsoft.AspNetCore.SignalR;

namespace Hospital.Context{

    public class Doctor{

        public int Id{get; set;}
        public string? DoctorName{get; set;}
        public string? Password{get; set;}
        
        
        [ForeignKey("Department")]
        public int DepartmentId{get; set;}
        public virtual Department? Department{get; set;}
    }
}