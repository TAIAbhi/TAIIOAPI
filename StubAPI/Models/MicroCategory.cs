using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class MicroCategory
    {
        public int microId { get; set; }
        public int subCateId { get; set; }
        public string name { get; set; }
        public bool ? isExample { get; set; }
}
}