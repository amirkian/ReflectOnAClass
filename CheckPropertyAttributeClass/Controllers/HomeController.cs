using CheckPropertyAttributeClass.Models;
using CheckPropertyAttributeClass.Models.Helper;
using CheckPropertyAttributeClass.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckPropertyAttributeClass.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment hostingEnvironment;


        public HomeController(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;

        }


        public static List<GetDomainNameViewModel> getDomainNameViewModels = new List<GetDomainNameViewModel>();


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /**************************************************************************************************************/
        public async Task<ActionResult> ReflectOnClass(string filePath)
        {
            //if (!string.IsNullOrEmpty(filePath))
            //{
            //var path=filePath.Replace("\\\\", "\\");

            //  Assembly assembly = Assembly.LoadFrom(@"D:\Repos\UnitTestingDemo\CheckPropertyAttributeClass\CheckPropertyAttributeClass\wwwroot\uploads\Student.dll");
            //  Assembly assembly = Assembly.LoadFrom(path);

            //   Type type = assembly.GetType("Student");

            //Type[] assemblyTypes = assembly.GetTypes();

            /**/
            Assembly myAssembly = Assembly.LoadFrom(@"D:\Repos\UnitTestingDemo\CheckPropertyAttributeClass\CheckPropertyAttributeClass\wwwroot\uploads\Student.dll");

            //Gets all referenced Types of the current Assembly that implement a specific interface
            //IEnumerable<Type> currentAssemblytypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => typeof(IRequestBody).IsAssignableFrom(y) && !y.IsInterface);

            ////Gets all referenced Types of an assembly that implement a specific interface
            //IEnumerable<Type> otherAssemblyTypes = myAssembly.GetTypes().Where(y => typeof(IRequestBody).IsAssignableFrom(y) && !y.IsInterface);

            ////You can also change IsInterface to IsAbstract if you are checking for types that implement an Abstract Class.
            //IEnumerable<Type> otherAssemblyTypesAbstract = myAssembly.GetTypes().Where(y => typeof(IRequestBody).IsAssignableFrom(y) && !y.IsAbstract);

            //omid

            var type = myAssembly.GetTypes().SingleOrDefault(t => !t.IsInterface && t.GetInterfaces().Any(a => a.Name == typeof(IRequestBody).Name));
            /**/


            //      foreach (Type mytype in assembly.GetTypes()){

            //            GetDomainName(mytype);
            //if (mytype.FullName.Contains("IRequestBody")){

            //    GetDomainName(mytype);
            //    break;
            //}
            //  }







            //   }

            // GetDomainName(typeof(Student));
            GetDomainName(type);
            return View("ReflectOnClass", getDomainNameViewModels);
        }

        public void GetDomainName(Type T)
        {

            var propertyInfos = T.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                if (LikeParentType(propertyInfo, T))
                {
                    continue;
                }

                #region younes
                var childType = propertyInfo.PropertyType;

                if (childType.IsGenericType && childType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type itemActualType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    if (itemActualType.IsClass && !propertyInfo.PropertyType.FullName.Contains("System.String"))
                    {
                        /*Recursive mode*/

                        GetDomainName(itemActualType);

                        /*End Recursive mode*/

                    }
                }
                else if (childType.IsClass && !childType.FullName.Contains("System.String"))
                {
                    continue;

                }


                #endregion
                #region mycode


                if (propertyInfo.CustomAttributes.Count() > 0)
                {
                    var attrs = propertyInfo.GetCustomAttributes(true);
                    foreach (var attr in attrs)
                    {
                        JsonConverterAttribute rangeAttribute = attr as JsonConverterAttribute;
                        if (rangeAttribute != null)
                        {
                            var attProp = rangeAttribute.ConverterParameters;
                            var attpropList = attProp.FirstOrDefault().ToString().Split(';');

                            foreach (var item in attpropList)
                            {
                                if (item.Trim().ToLower().StartsWith("range"))
                                {
                                    //  [Newtonsoft.Json.JsonConverter(typeof(ESBJsonConverter), "Range(1::100)=>6; NonEmptyString=>1313;Length(:16)=>1114")]
                                    GetDomainNameViewModel model = new GetDomainNameViewModel();

                                    /*Get error title*/
                                    model.ErrorTitle = GetErrorTitle(item);
                                    /*End Get error title*/

                                    /*get min value*/
                                    int endIndex = item.IndexOf(':'), startIndex = item.IndexOf('(') + 1;
                                    int length = endIndex - startIndex;
                                    string extracted = item.Substring(startIndex, length);
                                    /*Endget min value*/

                                    /*get max value*/
                                    int endMaxIndex = item.IndexOf(')'), startMaxIndex = item.IndexOf(':', item.IndexOf(':')) + 1;
                                    int lengthMax = endMaxIndex - startMaxIndex;
                                    string extractedMax = item.Substring(startMaxIndex + 1, lengthMax - 1);
                                    /*Endget max value*/
                                    /*get error code*/
                                    string errorCode = item.Substring(item.IndexOf('>') + 1, (item.Length - 1) - item.IndexOf('>'));
                                    /*get error code*/

                                    model.FieldName = propertyInfo.Name;
                                    model.MinVal = extracted;
                                    model.MaxVal = extractedMax;
                                    model.ErrorCode = errorCode;
                                    model.ParamPath= GetParamPath(propertyInfo.DeclaringType.FullName.Split('.'), propertyInfo.Name);


                                    /*Set errorMessage*/
                                    StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList[model.ErrorTitle]);
                                    errorMessage.Replace("{0}", model.FieldName).Replace("{1}", model.MinVal).Replace("{2}", model.MaxVal).Replace("{3}", model.ErrorCode).Replace("{4}", model.ErrorCode).Replace("{5}",model.ParamPath);

                                    /*End Set errorMessage*/
                                    model.ErrorMessage = errorMessage;

                                    getDomainNameViewModels.Add(model);

                                }
                                else if (item.Trim().ToLower().StartsWith("nonemptystring"))
                                {

                                    //[JsonConverter(typeof(ESBJsonConverter), "NonEmptyString=>3064")]
                                    GetDomainNameViewModel model = new GetDomainNameViewModel();

                                    /*Get error title*/
                                    model.ErrorTitle = GetErrorTitle(item);
                                    /*End Get error title*/

                                    /*get error code*/
                                    string errorCode = item.Substring(item.IndexOf('>') + 1, (item.Length - 1) - item.IndexOf('>'));
                                    /*get error code*/

                                    model.FieldName = propertyInfo.Name;
                                    model.ErrorCode = errorCode;
                                    model.ParamPath = GetParamPath(propertyInfo.DeclaringType.FullName.Split('.'), propertyInfo.Name);
               
                                    /*Set errorMessage*/
                                    StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList[model.ErrorTitle]);
                                    errorMessage.Replace("{0}", model.FieldName).Replace("{1}", model.ErrorCode).Replace("{2}",model.ParamPath);

                                    /*End Set errorMessage*/
                                    model.ErrorMessage = errorMessage;

                                    getDomainNameViewModels.Add(model);

                                }
                                else if (item.Trim().ToLower().StartsWith("length".ToLower().Trim()))
                                {
                                    // [JsonConverter(typeof(ESBJsonConverter), "Length(1:16)=>1194;")]
                                    GetDomainNameViewModel model = new GetDomainNameViewModel();


                                    /*Get error title*/
                                    model.ErrorTitle = GetErrorTitle(item);
                                    /*End Get error title*/



                                    /*get error code*/
                                    string errorCode = item.Substring(item.IndexOf('>') + 1, (item.Length - 1) - item.IndexOf('>'));
                                    /*get error code*/

                                    /*get min value*/
                                    int startIndex = item.IndexOf('(') + 1;
                                    int endIndex = item.IndexOf(':');
                                    int length = endIndex - startIndex;
                                    string extracted = item.Substring(startIndex, length);
                                    /*Endget min value*/

                                    /*get max value*/
                                    int startMaxIndex = item.IndexOf(':', item.IndexOf(':'));
                                    int endMaxIndex = item.IndexOf(')');
                                    int lengthMax = endMaxIndex - startMaxIndex;
                                    string extractedMax = item.Substring(startMaxIndex + 1, lengthMax - 1);
                                    /*Endget max value*/


                                    model.FieldName = propertyInfo.Name;
                                    model.MinVal = extracted;
                                    model.MaxVal = extractedMax;
                                    model.ErrorCode = errorCode;
                                    model.ParamPath = GetParamPath(propertyInfo.DeclaringType.FullName.Split('.'), propertyInfo.Name) ;



                                    /*Set errorMessage*/
                                    StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList[model.ErrorTitle]);
                                    errorMessage.Replace("{0}", model.FieldName).Replace("{1}", model.MinVal).Replace("{2}", model.MaxVal).Replace("{3}", model.ErrorCode).Replace("{4}", model.ParamPath);

                                    /*End Set errorMessage*/
                                    model.ErrorMessage = errorMessage;

                                    getDomainNameViewModels.Add(model);


                                }

                                else if (item.Trim().ToLower().StartsWith("NumericString"))
                                {
                                    //     [JsonConverter(typeof(ESBJsonConverter), "NumericString=>1194;NonEmptyString=>1313;Length(8:8)=>1114;PDateTime(yyyyMMdd)=>1207")]
                                    GetDomainNameViewModel model = new GetDomainNameViewModel();


                                    /*Get error title*/
                                    model.ErrorTitle = GetErrorTitle(item);
                                    /*End Get error title*/



                                    /*get error code*/
                                    string errorCode = item.Substring(item.IndexOf('>') + 1, (item.Length - 1) - item.IndexOf('>'));
                                    /*get error code*/
                                    model.FieldName = propertyInfo.Name;
                                    model.ErrorCode = errorCode;

                                    /*Set errorMessage*/
                                    StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList[model.ErrorTitle]);
                                    errorMessage.Replace("{0}", model.FieldName).Replace("{1}", model.MinVal).Replace("{2}", model.MaxVal).Replace("{3}", model.ErrorCode);

                                    /*End Set errorMessage*/
                                    model.ErrorMessage = errorMessage;

                                    getDomainNameViewModels.Add(model);


                                }
                                else if (item.Trim().ToLower().StartsWith("PDateTime"))
                                {
                                    //   [JsonConverter(typeof(ESBJsonConverter), "NumericString=>1194;NonEmptyString=>1313;Length(8:8)=>1114;PDateTime(yyyyMMdd)=>1207")]
                                    GetDomainNameViewModel model = new GetDomainNameViewModel();


                                    /*Get error title*/
                                    model.ErrorTitle = GetErrorTitle(item);
                                    /*End Get error title*/



                                    /*get error code*/
                                    string errorCode = item.Substring(item.IndexOf('>') + 1, (item.Length - 1) - item.IndexOf('>'));
                                    /*get error code*/
                                    model.FieldName = propertyInfo.Name;
                                    model.ErrorCode = errorCode;

                                    /*Set errorMessage*/
                                    StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList[model.ErrorTitle]);
                                    errorMessage.Replace("{0}", model.FieldName).Replace("{1}", model.ErrorCode);

                                    /*End Set errorMessage*/
                                    model.ErrorMessage = errorMessage;

                                    getDomainNameViewModels.Add(model);


                                }


                            }//end foreach

                        }

                        else
                        {
                            Type tp = attr.GetType();
                            if(tp.Name== "RequiredAttribute")
                            {
                                GetDomainNameViewModel model = new GetDomainNameViewModel();

                                model.FieldName = propertyInfo.Name;
                                model.ErrorCode = 0.ToString();
                                model.ParamPath = GetParamPath(propertyInfo.DeclaringType.FullName.Split('.'), propertyInfo.Name);

                                /*Set errorMessage*/
                                StringBuilder errorMessage = new StringBuilder(ErrorsDictionary.ErrorList["RequiredAttribute"]);
                                errorMessage.Replace("{0}", model.FieldName);

                                /*End Set errorMessage*/
                                model.ErrorMessage = errorMessage;

                                getDomainNameViewModels.Add(model);
                            }



                        }
                    }
                }
            }//end foreach (var propertyInfo in propertyInfos)
            #endregion
        }

        /// <summary>
        /// چک میشود که تایپ پراپرتی برابر با تایپ والدش هست
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="ParentType"></param>
        /// <returns></returns>
        public bool LikeParentType(PropertyInfo propertyInfo, Type ParentType)
        {
            var childType = propertyInfo.PropertyType;
            var childTypeFullNameArr = childType.FullName.Split(',');
            var childTypeName = childTypeFullNameArr[0].Split('.').Last();
            if (ParentType.Name == childTypeName)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAssembly([FromForm] FileUpload postedFile)
        {
            if (postedFile != null)
            {
                postedFile.FormFile.ContentType.ToLower();

                //var result = new StringBuilder();
                //using (var reader = new StreamReader(postedFile.FormFile.OpenReadStream()))
                //{
                //    while (reader.Peek() >= 0)
                //        result.AppendLine(reader.ReadLine());
                //}

                //                // Create a pattern for a word that starts with letter "M"  
                //                string pattern = @"[class \s*]\w*(?=(:[\s]*IRequestBody))";
                //                // Create a Regex  
                //                Regex rg = new Regex(pattern);

                //MatchCollection matchedAuthors = rg.Matches(result.ToString());
                //               var baseclass= matchedAuthors[0].Value.Trim();
                //                if (string.IsNullOrEmpty(baseclass))
                //                {
                //                    throw new NullReferenceException("dosen't implement IRequestBody interface!");
                //                }



                var uniqueFileName = GetUniqueFileName(postedFile.FormFile.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var filePath = Path.Combine(uploads, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.FormFile.CopyTo(stream);
                }
                //   ReflectOnClass(filePath);
                return RedirectToAction("ReflectOnClass", "Home", new { filePath });


            }

            return RedirectToAction("ReflectOnClass", "Home", new {  });


        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }


        private string GetErrorTitle(string item)
        {
            Regex filter = new Regex("[\\w]*(?==|\\()");

            var match = filter.Match(item.ToString());
            if (match.Success)
            {
               return match.Value.ToLower();
            }
            return "";
        }
        private string GetParamPath(string[] paramPaths,string fieldName)
        {
            if (paramPaths[0].ToString() == paramPaths[1].ToString())
            {
                return fieldName;
            }
            else
            {
                string str = string.Join(".", paramPaths);
                str += "." + fieldName;
                return str;

            }
        }






    }//end controller
}
