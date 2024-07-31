using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Context;
using Microsoft.AspNetCore.SignalR;

namespace Hospital.Context{
    
    public class Patient{
        public int Id{get; set;}
        public string? PatientName{get; set;}
        public string? Password {get; set;}
        public string? MedicalHistory{get; set;}
        public int Gender{get; set;}

        

    }
}