using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Context;

namespace Hospital.Context{

    public class Department{
        public int Id{get; set;}
        public string? DepartmentName {get; set;}
    }
}