using System;
using System.Collections.Generic;

namespace Database
{
    public class Advice
    {
        public int Id { get; set; }
        public int? TagId {get; set;}

        public virtual Tag Tag {get;set;}
        public string Description {get; set;}
    }
}