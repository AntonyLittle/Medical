using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeRESTService.Models.Repositories
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetPatientSummaries();

        Patient GetPatient(int patientId);

        IEnumerable<Volume> GetVolumeSummaries(int patientId);

        Volume GetVolume(int patientId, int volumeId);
    }
}
