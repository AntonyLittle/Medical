using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VolumeRESTService.Models.Repositories;
using System.Diagnostics;
using VolumeRESTService.Models;

namespace VolumeRESTService.Controllers
{
    [Route("api/Patients/{patientId}/Volumes")]
    public class VolumesController : Controller
    {
        private IPatientRepository m_repo;

        public VolumesController(IPatientRepository repo)
        {
            m_repo = repo;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get(int patientId)
        {
            try
            {
                var list = new List<Volume>(m_repo.GetVolumeSummaries(patientId));

                return new ObjectResult(list);
            }
            catch 
            {
                return BadRequest();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int patientId, int id)
        {
            try
            {
                return new ObjectResult(m_repo.GetVolume(patientId, id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST api/values
        [HttpPost]
        public void Post(int patientId, [FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int patientId, int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int patientId, int id)
        {
        }
    }
}
