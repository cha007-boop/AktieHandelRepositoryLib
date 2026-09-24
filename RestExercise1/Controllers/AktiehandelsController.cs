using AktieHandelRepositoryLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AktieHandel>>> Get()
        {
            var aktieHandels = await _repository.GetAll();
            if (aktieHandels == null)
            {
                return NotFound();
            }
            if (aktieHandels.Count() == 0)
            {
                return NoContent();
            }

            return Ok(aktieHandels);
        }

        // GET api/<AktieHandelsController>/filter?column=<columnName>&value=<filterValue>&sort=<columnName>&order=<sortOrder>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AktieHandel>>> GetFiltered([FromQuery] string column, 
                                                                              [FromQuery] string value, 
                                                                              [FromQuery] string sort, 
                                                                              [FromQuery] string order)
        {
            try
            {
                var aktieHandels = await _repository.ListFiltered(column, value, sort, order);
                if (aktieHandels == null)
                {
                    return NotFound();
                }
                if (aktieHandels.Count() == 0)
                {
                    return NoContent();
                }

                return Ok(aktieHandels);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        // GET api/<AktiehandelsController>/<id>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<AktieHandel>> Get(int id)
        {
            var aktieHandel = await _repository.GetById(id);
            if (aktieHandel == null)
            {
                return NotFound();
            }

            return Ok(aktieHandel);
        }

        // POST api/<AktiehandelsController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<AktieHandel?>> Post([FromBody] AktieHandelDTO aktieHandel)
        {

            try
            {
                AktieHandel theAddedAktieHandel = await _repository.Add(AktieHandelDTOHelper.DTOtoClass(aktieHandel));
                if (await _repository.GetById(theAddedAktieHandel.Id) != null)
                {
                    return Created($"/api/aktiehandels/{theAddedAktieHandel.Id}", theAddedAktieHandel);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (SqlException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        // PUT api/<AktieHandelsController>/<id>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<ActionResult<AktieHandel>> Put(int id, [FromBody] AktieHandelDTO aktieHandel)
        {
            try
            {
                var updatedAktieHandel = await _repository.Update(id, AktieHandelDTOHelper.DTOtoClass(aktieHandel));
                if (updatedAktieHandel != null)
                {
                    return Ok(updatedAktieHandel);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/<AktiehandelsController>/<id>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<AktieHandel?>> Delete(int id)
        {
            AktieHandel? deletedAktieHandel = await _repository.Delete(id);
            if (deletedAktieHandel != null)
            {
                return Ok(deletedAktieHandel);
            }
            return NotFound();
        }
    }
}
