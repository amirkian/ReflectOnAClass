using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CheckPropertyAttributeClass.Models.ViewModels
{
    public class FileUpload
    {
        public IFormFile FormFile { get; set; }
    }
}
