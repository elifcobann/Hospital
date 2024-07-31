using Microsoft.AspNetCore.Mvc;
using Hospital.Context;
using Microsoft.AspNetCore.Hosting;

namespace Hospital.Controllers.Base{
    public class SaControllerBase : Controller{
        public SaControllerBase(){
        }

        public SaControllerBase(HospitalSchema context){
            _context = context;
        }

        public SaControllerBase(HospitalSchema context, IWebHostEnvironment environment){
            _context =context;
            _environment = environment;
        }

        protected HospitalSchema _context;
        protected IWebHostEnvironment _environment;

    }
}

