using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class RequestSuggestions
    {
        public int uID { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int microcategoryId { get; set; }
        public string location { get; set; }
        public int cityId { get; set; }
        public int platform { get; set; }
        public string comments { get; set; }
        public int contactId { get; set; }
        public DateTime addedWhen { get; set; }
    }
}