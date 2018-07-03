using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class Token
    {

        public int tokenId { get; set; }
        public int userId { get; set; }
        public string authToken { get; set; }
        public System.DateTime issuedOn { get; set; }
        public System.DateTime expiresOn { get; set; }
    }
    public class TokenResponse
    {


        public string action { get; set; }     
        public string authToken { get; set; }
        public string message { get; set; }   
        public object loginDetails { get; set; }

    }
}