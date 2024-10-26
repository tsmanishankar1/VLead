using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class CustomException : Exception
    {
        public CustomException()
        {

        }
        public CustomException(string message)
            : base(message)
        {
        }
        public CustomException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
