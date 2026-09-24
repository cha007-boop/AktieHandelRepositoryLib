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



        // GET api/<AktieHandelsController>?id=<id>&name=<name>&maxExchangePrice=<maxExchangePrice>&minExchangePrice=<minExchangePrice>&maxAmount=<maxAmount>&minAmount=<minAmount>&sort=<sort>&order=<order>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AktieHandel>>> GetAll([FromQuery] int? id = null,
                                                                         [FromQuery] string? name = null,
                                                                         [FromQuery] double? maxExchangePrice = null,
                                                                         [FromQuery] double? minExchangePrice = null,
                                                                         [FromQuery] int? maxAmount = null,
                                                                         [FromQuery] int? minAmount = null,
                                                                         [FromQuery] string? sort = null,
                                                                         [FromQuery] string? order = null)
        {
            try
            {
                var aktieHandels = await _repository.GetAll(id, name, maxExchangePrice, minExchangePrice, maxAmount, minAmount, sort, order);
                if (aktieHandels == null || aktieHandels.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(aktieHandels);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
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
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
