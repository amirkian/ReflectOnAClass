using Newtonsoft.Json;
using static CheckPropertyAttributeClass.Models.Exceptions.MediatorException;
using System.Collections.Generic;

namespace CheckPropertyAttributeClass.Models.Exceptions
{
    public class JException : JsonSerializationException
    {
        public List<ErrorItem> ErrorItems { get; set; }
        public JException(string message, List<ErrorItem> errorItems) : base(message)
        {
            ErrorItems = errorItems;
        }
    }
}
