using System.Collections.Generic;
using System;

namespace CheckPropertyAttributeClass.Models.Exceptions
{
    public class MediatorException
    {
        public class ErrorItem
        {
            public string Desc { get; set; }
            public int Code { get; set; }
            public string ParamName { get; set; }
            private string _paramPath;
            public string ParamPath
            {
                get => string.IsNullOrWhiteSpace(_paramPath) ? ParamName : _paramPath;
                set => _paramPath = value;
            }
        }

        public class ErrorItemWithValue : ErrorItem
        {
            public List<object> Values { get; set; }
        }

        //public sealed class MediatorException : Exception
        //{
        //    public int RsCode { get; set; }
        //    public string Message { get; set; }
        //    public bool IsSuccess { get; set; }
        //    public List<ErrorItem> ErrorList { get; set; }

        //    public MediatorException(string message, int rsCode, params object[] data)
        //    {
        //        Message = message;
        //        RsCode = rsCode;
        //        IsSuccess = false;
        //        if (data.Length % 2 != 0) return;
        //        for (var i = 0; i < data.Length; i += 2)
        //            Data.Add(data[i], data[i + 1] ?? string.Empty);
        //    }

        //    public MediatorException(string message, int rsCode, List<ErrorItem> errorList, params object[] data)
        //    {
        //        ErrorList = errorList;
        //        Message = message;
        //        RsCode = rsCode;
        //        IsSuccess = false;
        //        if (data.Length % 2 != 0) return;
        //        for (var i = 0; i < data.Length; i += 2)
        //            Data.Add(data[i], data[i + 1] ?? string.Empty);

        //    }

        //    public MediatorException(string message, int rsCode, List<ErrorItemWithValue> errorList, params object[] data) :
        //        this(message, rsCode, errorList?.Cast<ErrorItem>().ToList(), data)
        //    {

        //    }
        //}
    }
}
