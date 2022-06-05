using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    //[Route("api/trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailController : ControllerBase
    {

        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objTrailDto = new List<TrailDto>();

            foreach (var obj in objList){
                objTrailDto.Add(_mapper.Map<TrailDto>(obj));
            }
            //return Ok(objTrailDto);
            return StatusCode(StatusCodes.Status200OK, objTrailDto);
        }


        /// <summary>
        /// Get trail based on the id.
        /// </summary>
        /// <param name="trailId">Trail ID</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Trail))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult GetTrail(int trailId)
        {
            var objTrail = _trailRepo.GetTrail(trailId);
            if (objTrail == null)
                return NotFound();

            var objTrailDto = _mapper.Map<TrailDto>(objTrail);
            return Ok(objTrailDto);
        }


        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
           var allTrails = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (allTrails == null)
                return NotFound();

            var listOfDto = new List<TrailDto>();
            foreach (var trail in allTrails)
                listOfDto.Add(_mapper.Map<TrailDto>(trail));

            return Ok(listOfDto);
        }


        /// <summary>
        /// Create a new Trail.
        /// </summary>
        /// <param name="trailDto">Trail Object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
                return BadRequest(ModelState);

            bool isTrailExist = _trailRepo.GetTrailsInNationalPark(trailDto.NationalParkId).Any(t => t.Name.Trim().ToLower().Equals(trailDto.Name.Trim().ToLower()));
            if (isTrailExist)
            {
                ModelState.AddModelError("", "Trail Already Exist");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {trailObj.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }


        /// <summary>
        /// Update the existing trail.
        /// </summary>
        /// <param name="trailId">Trail ID</param>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
                return BadRequest(ModelState);

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record { trailObj.Name }");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete the existing Trail.
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
                return NotFound();

            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the record { trailObj.Name }");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }

    }
}
