using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDrive.Models
{
    public class DriveException : Exception
    {
        public string Response { get; set; }
        public DriveException(string message, string response) : base(message)
        {
            Response = response;
        }

    }
}
