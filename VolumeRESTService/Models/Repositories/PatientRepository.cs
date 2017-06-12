using Dicom.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models.Repositories
{
    public class PatientRepository: IPatientRepository
    {

        public IEnumerable<Patient> GetPatientSummaries()
        {
            var patients = new List<Patient>();

            foreach(string directory in Directory.EnumerateDirectories("Data"))
            {
                patients.Add(new Patient { PatientId = patients.Count + 1, Name = Path.GetFileName(directory), Volumes = new List<Volume>() });
            }

            return patients;
        }

        public Patient GetPatient(int patientId)
        {
            var patient = GetPatientSummaries().SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            return patient;
        }

        public IEnumerable<Volume> GetVolumeSummaries(int patientId)
        {
            var patient = GetPatientSummaries().SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            var volumes = new List<Volume>();
            foreach (string directory in Directory.EnumerateDirectories("Data" + @"\" + patient.Name))
            {
                var volume = new Volume { PatientId = patientId, Patient = patient, VolumeId = volumes.Count + 1, Name = Path.GetFileName(directory), Modality = "CT", Slices = new List<Slice>() };

                volumes.Add(volume);
            }

            return volumes;
        }

        public Volume GetVolume(int patientId, int volumeId)
        {
            var volumeSummary = GetVolumeSummaries(patientId).SingleOrDefault(v => v.VolumeId == volumeId);

            int sliceIndex = 0;
            foreach (string dicomFile in Directory.EnumerateFiles("Data" + @"\" + volumeSummary.Patient.Name + @"\" + volumeSummary.Name))
            {
                try
                {
                    var image = new DicomImage(dicomFile);
                    var slice = new Slice {
                        PatientId = patientId,
                        VolumeId = volumeId,
                        SliceId = sliceIndex,
                        Index = sliceIndex,
                        Height = image.Height,
                        Width = image.Width,
                        Radiodensities = new short[image.Height * image.Width]
                    };

                    var data = image.PixelData.GetFrame(0).Data;
                    Buffer.BlockCopy(data, 0, slice.Radiodensities, 0, data.Length);

                    volumeSummary.Slices.Add(slice);
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                sliceIndex ++;
            }

            return volumeSummary;
        }
    }
}
