using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models.Repositories
{
    public class PatientRepository: IPatientRepository
    {

        public IEnumerable<Patient> GetPatientSummaries()
        {
            return from patient in RandomValuesPatientDataSingleton.Instance.Patients select new Patient { PatientId = patient.PatientId, Name = patient.Name };
        }

        public Patient GetPatient(int patientId)
        {
            var patient = RandomValuesPatientDataSingleton.Instance.Patients.SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            return new Patient { PatientId = patient.PatientId, Name = patient.Name };
        }

        public IEnumerable<Volume> GetVolumeSummaries(int patientId)
        {
            var patient = RandomValuesPatientDataSingleton.Instance.Patients.SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            var list = from volume in patient.Volumes select new Volume {
                PatientId = volume.PatientId, VolumeId = volume.VolumeId, Name = volume.Name, Modality = volume.Modality, Slices = new List<Slice>()
            };

            Debug.WriteLine($"GetVolumeSummaries patientID: {patientId}, list count: {list.ToList().Count}");

            return list;
        }

        public Volume GetVolume(int patientId, int volumeId)
        {
            var patient = RandomValuesPatientDataSingleton.Instance.Patients.SingleOrDefault(p => p.PatientId == patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            var volume = patient.Volumes.SingleOrDefault(v => v.VolumeId == volumeId);
            if (volume == null)
                throw new Exception("Volume not found");

            return volume;
        }
    }
}
