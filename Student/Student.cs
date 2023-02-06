using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Student
{
    public interface IRequestBody
    {

    }
    public class Student : IRequestBody
    {
        public Student()
        {
        }

        [JsonConverter(typeof(ESBJsonConverter), "NonEmptyString=>1313")]
        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(ESBJsonConverter), "range(1::100)=>6; nonemptystring=>1313;length(:16)=>1114")]
        public int studentid { get; set; }

        [Required]
        [Newtonsoft.Json.JsonConverter(typeof(ESBJsonConverter), "range(18::29)=>6; nonemptystring=>1313;length(:16)=>1114")]
        public string studentname { get; set; }

        [Range(10, 20)]
        public int age { get; set; }

        /// <summary>
        /// شماره شبا پادی
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(ESBJsonConverter), "NonEmptyString=>3064;")]
        public string iban { get; set; }

        public List<long> Amounts { get; set; }

    }
    public class Teacher
    {
        [Required]
        [JsonConverter(typeof(ESBJsonConverter), "range(18::29)=>6; nonemptystring=>1313;length(:16)=>1114")]
        public string Teachername { get; set; }
    }
}
