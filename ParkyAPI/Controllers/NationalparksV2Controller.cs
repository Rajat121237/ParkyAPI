using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalparksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalparksV2Controller(IMapper mapper, INationalParkRepository npRepo)
        {
            _mapper = mapper;
            _npRepo = npRepo;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }
    }
}

//Note: api/NationalParks will be the default route for all the action methods in the controller.
