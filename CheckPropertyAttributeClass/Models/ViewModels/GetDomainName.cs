
using System.Text;

namespace CheckPropertyAttributeClass.Models.ViewModels
{
    public class GetDomainNameViewModel
    {
        public string ParamPath { get; set; }
        public string ErrorTitle { get; set; }
        public string FieldName { get; set; }
        public string MinVal { get; set; }
        public string MaxVal { get; set; }
        public string ErrorCode { get; set; }
        public StringBuilder ErrorMessage { get; set; }

    }
}
