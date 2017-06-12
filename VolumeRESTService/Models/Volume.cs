using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models
{
    public class Volume
    {
        public int VolumeId { get; set; }
    
        public int PatientId { get; set; }

        public string Name { get; set; }

        public string Modality { get; set; }

        public Patient Patient { get; set; }

        public List<Slice> Slices { get; set; }
    }
}
