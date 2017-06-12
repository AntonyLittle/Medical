using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VolumeRESTService.Models.Repositories;

namespace VolumeRESTService.Controllers
{
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private IPatientRepository m_repo;

        public PatientsController(IPatientRepository repo)
        {
            m_repo = repo;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return new ObjectResult(m_repo.GetPatientSummaries());
            }
            catch 
            {
                return BadRequest();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(m_repo.GetPatient(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
