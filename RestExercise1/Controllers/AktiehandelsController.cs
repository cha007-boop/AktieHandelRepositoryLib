using AktieHandelRepositoryLib;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestExercise1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktiehandelsController : ControllerBase
    {
        private readonly IAktieHandelRepositoryAsync _repository;

        public AktiehandelsController(IAktieHandelRepositoryAsync repository)
        {
            _repository = repository;
        }

        // GET: api/<AktiehandelsController>
        [HttpGet]
        public async Task<IEnumerable<AktieHandel>> Get()
        {
            return await _repository.GetAll();
        }

        // GET api/<AktiehandelsController>/<id>
        [HttpGet("{id}")]
        public async Task<AktieHandel?> Get(int id)
        {
            return await _repository.GetById(id);
        }

        // POST api/<AktiehandelsController>
        [HttpPost]
        public async Task<AktieHandel?> Post([FromBody] AktieHandel aktieHandel)
        {
            return await _repository.Add(aktieHandel);
        }

        // PUT api/<AktiehandelsController>/<id>
        [HttpPut("{id}")]
        public async Task<AktieHandel?> Put(int id, [FromBody] AktieHandel aktieHandel)
        {
            return await _repository.Update(id, aktieHandel);
        }

        // DELETE api/<AktiehandelsController>/<id>
        [HttpDelete("{id}")]
        public async Task<AktieHandel?> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
