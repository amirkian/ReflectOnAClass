using System.Collections.Generic;

namespace CheckPropertyAttributeClass.Models.Helper
{
    public static class ErrorsDictionary
    {
       public static Dictionary<string, string> ErrorList = new Dictionary<string, string>(){
{"range" , "فیلد {0} در مسیر روت درخواست ورودی به ESB در صورتی که مقدار حداقل {1} و حداکثر {2}   داشته باشد در مسیر روت پاسخ به کلاینت در فیلدهای RsCode  مقدار {1038} ، IsSuccess  مقدار false  و  Message دارای مقداری معادل خطای مربوطه در ESB و درصورت استفاده از اکسپت ورژن 7 یا 8 در هدر ورودی در مسیر ErrorList[0].Code مقدار {4} و ErrorList[0].ِDesc مقداری معادل کد خطای مربوطه در ESB بازگردانده می شود. در مسیر ErrorList[0].ParamName و ErrorList[0].ParamPath نیز مقدار {5} بازگردانده خواهد شد."},
{"nonemptystring", "فیلد {0} در مسیر روت درخواست ورودی به ESB در صورتی که خالی باشد در مسیر روت پاسخ به کلاینت در فیلدهای RsCode  مقدار 1038 ، IsSuccess  مقدار false  و  Message دارای مقداری معادل خطای مربوطه در ESB و درصورت استفاده از اکسپت ورژن 7 یا 8 در هدر ورودی در مسیر ErrorList[0].Code مقدار {1} و ErrorList[0].ِDesc مقداری معادل کد خطای مربوطه در ESB بازگردانده می شود. در مسیر ErrorList[0].ParamName و ErrorList[0].ParamPath نیز مقدار {2} بازگردانده خواهد شد"},
{"length", "فیلد {0} در مسیر روت درخواست ورودی به ESB در صورتی که تعداد کاراکتر های ارسالی این فیلد مخالف {1} یا {2}  باشد در مسیر روت پاسخ به کلاینت در فیلدهای RsCode  مقدار 1038 ، IsSuccess  مقدار false  و  Message دارای مقداری معادل خطای مربوطه در ESB و درصورت استفاده از اکسپت ورژن 7 یا 8 در هدر ورودی در مسیر ErrorList[0].Code مقدار {3} و ErrorList[0].ِDesc مقداری معادل کد خطای مربوطه در ESB بازگردانده می شود. در مسیر ErrorList[0].ParamName و ErrorList[0].ParamPath نیز مقدار {4} بازگردانده خواهد شد."},
{"RequiredAttribute", "وارد کردن فیلد {0} الزامی است."},
};


    }
}
