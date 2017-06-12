using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models
{
    public class Slice
    {
        public int SliceId { get; set; }

        public int VolumeId { get; set; }
        public int PatientId { get; set; }

        public Volume Volume { get; set; }
        public Patient Patient { get; set; }

        public int Index { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public short[] Radiodensities { get; set; }
    }
}
