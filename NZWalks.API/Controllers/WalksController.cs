using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //  /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        // GET ALL Walks
        //GET: https://localhost:portnumber/api/walks?filterOn=Name?filterQuery=Track&sortby=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data from Database - Domain models
            var walkDomain = await _walkRepository.GetAllAsync(filterOn, filterQuery,sortBy,isAscending ?? true, pageNumber, pageSize);

            // Map Domain Models to DTOs
            // Return DTOs
            return Ok(_mapper.Map<List<WalkDto>>(walkDomain));
        }

        // GET SINGLE REGION *Get Region By ID)
        //GET: https://localhost:portnumber/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = _dbcontext.Regions.Find(id);
            // Get Region Domain Model From Database
            var walksDomain = await _walkRepository.GetByIdAsync(id);

            if (walksDomain == null)
            {
                return NotFound();
            }
            // Map/ Convert walks Domain Model to walks DTO
            // Return DTO
            return Ok(_mapper.Map<WalkDto>(walksDomain));
        }
        //CREATE Walks
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalksRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(addWalksRequestDto);

            await _walkRepository.CreateAsync(walkDomainModel);

            // Map Domain Model To DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        // Update Walk By Id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

            // Checked if walk exits
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));            
        }

        // Delete Walks
        // DELETE:  https://localhost:portnumber/api/Walks/{id} 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

            // Checked if Walks exits
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // return deleted Walks back
            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
        }

    }
}
