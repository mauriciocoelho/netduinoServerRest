using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netduino.Web.Models
{
    public class PostPin
    {        
            public string Pin { get; set; }
            public bool Status { get; set; }

            public PostPin(string pin, bool status)
            {
                this.Pin = pin;
                this.Status = status;
            }     
    }
}