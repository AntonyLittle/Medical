using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }

        public List<Volume> Volumes { get; set; }
    }
}
