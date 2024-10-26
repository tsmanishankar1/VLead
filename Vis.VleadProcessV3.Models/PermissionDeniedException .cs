using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException() { }
        public PermissionDeniedException(string message) : base(message) { }
        public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
    }

}
