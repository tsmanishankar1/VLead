using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class ClientCommunicationTran
    {
        public int Id { get; set; }

        public int? JobId { get; set; }

        public int CustomerId { get; set; }

        public int Ccid { get; set; }

        public string? Email { get; set; }

        public bool? IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public virtual ClientCommunication ClientCommunication { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;

        public virtual JobOrder? Job { get; set; }

    }
}
