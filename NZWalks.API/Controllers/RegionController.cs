using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;       

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;           
        }
        // GET ALL REGIONs
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data from Database - Domain models
            var regionsDomain = await _regionRepository.GetAllAsync();

           // Map Domain Models to DTOs
           // Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // GET SINGLE REGION *Get Region By ID)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = _dbcontext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionsDomain = await _regionRepository.GetByIdAsync(id);

            if (regionsDomain == null)
            {
                return NotFound();
            }
            // Map/ Convert Region Domain Model to Region DTO
            // Return DTO
            return Ok(_mapper.Map<RegionDto>(regionsDomain));
        }

        // POST To Create New Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {            
            // Map or Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
        }

        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id} 
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            
            // Checked if region exits
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO           
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));            
        }

        // Delete Region
        // DELETE:  https://localhost:portnumber/api/regions/{id} 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedRegionDomainModel = await _regionRepository.DeleteAsync(id);

            // Checked if region exits           
            if (deletedRegionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted Region back
            // Map  Domain Model to DTO            
            return Ok(_mapper.Map<RegionDto>(deletedRegionDomainModel));
        }
    }
}
