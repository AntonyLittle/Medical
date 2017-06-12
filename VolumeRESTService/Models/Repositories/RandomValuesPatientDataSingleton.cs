using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models.Repositories
{
    public class RandomValuesPatientDataSingleton
    {
        private RandomValuesPatientDataSingleton()
        {
            var rng = new Random();

            var names = new string[] { "Bob Cratchett", "Frank Sidebottom", "Sally Sparrow", "Jane Doe", "Francis Smith" };
            for (var i = 1; i <= 5; i++)
                Patients.Add(
                    new Patient { PatientId = i, Name = names[i - 1], Volumes = GenerateVolumes(i, rng.Next(5)) }
                );
        }

        private static List<Volume> GenerateVolumes(int patientId, int volumeCount)
        {
            const int VOLUME_DIMENSION = 100;
            const int SLICE_HEIGHT = VOLUME_DIMENSION;
            const int SLICE_WIDTH = VOLUME_DIMENSION;
            const int SLICE_COUNT = VOLUME_DIMENSION;

            var rng = new Random();


            var volumes = new List<Volume>();

            for (var i = 1; i <= volumeCount; i++)
            {
                var volume = new Volume { VolumeId = i, PatientId = patientId, Name = $"Volume {i}" };
                switch (rng.Next(3))
                {
                    case 1:
                        volume.Modality = "MR";
                        break;
                    case 2:
                        volume.Modality = "PET";
                        break;
                    default:
                        volume.Modality = "CT";
                        break;
                }

                volume.Slices = new List<Slice>();

                for (var sliceCount = 0; sliceCount < SLICE_COUNT; sliceCount++)
                {
                    var voxelCount = SLICE_HEIGHT * SLICE_WIDTH;

                    var slice = new Slice { PatientId = patientId, VolumeId = volume.VolumeId, Index = sliceCount, Width = SLICE_WIDTH, Height = SLICE_HEIGHT, Radiodensities = new short[voxelCount] };

                    for (var voxelIndex = 0; voxelIndex < voxelCount; voxelIndex++)
                        slice.Radiodensities[voxelIndex] = (short)(rng.Next(2000) - 1000);

                    volume.Slices.Add(slice);
                }

                volumes.Add(volume);
            }

            return volumes;
        }

        public List<Patient> Patients { get; } = new List<Patient>();

        private static RandomValuesPatientDataSingleton s_instance;

        public static RandomValuesPatientDataSingleton Instance {
            get {
                if (s_instance == null)
                    s_instance = new RandomValuesPatientDataSingleton();

                return s_instance;
            }
        }
    }
}
