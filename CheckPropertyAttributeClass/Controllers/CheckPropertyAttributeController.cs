using CheckPropertyAttributeClass.Models;
using CheckPropertyAttributeClass.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPropertyAttributeClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckPropertyAttributeController : ControllerBase
    {
        private readonly ILogger<CheckPropertyAttributeController> _logger;

        public CheckPropertyAttributeController(ILogger<CheckPropertyAttributeController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public void GetClassType([FromForm] FileUpload file)
        {
            if (file != null)
            {

                //Set Key Name
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FormFile.FileName);

                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                }
            }
        }
      
    }
}
