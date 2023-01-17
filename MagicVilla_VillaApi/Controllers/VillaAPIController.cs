using AutoMapper;
using MagicVilla_VillaApi.Data;
//using MagicVilla_VillaApi.Logging;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController] //provides built-in support for data annotations i.e validations
    public class VillaAPIController : ControllerBase
    {
        //Implementaion of custom logging
        //private readonly ILogging _logger;
       private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        public VillaAPIController(ILogger<VillaAPIController> logger,ApplicationDBContext db,IMapper mapper,IVillaRepository villaRepository)
        {
            _logger= logger;
            _db= db;
            _mapper= mapper;
            _villaRepository= villaRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Getting all Villas","");
            IEnumerable<Villa> list = await _villaRepository.getAll();
            return Ok(_mapper.Map<List<VillaDto>>(list));
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(VillaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // return type can be given as typeof(VillaDto)
        //or in method signature as well Like "public ActionResult<VillaDto> getVilla(int id)" also.
        public async Task<ActionResult> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Get Villa Error with Id" + id, "Error");
                return BadRequest();
            }
            var villa = await _villaRepository.getFirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<ActionResult<VillaDto>> CreateVilla([FromBody]VillaCreateDto villaCreateDto)
        {
            if (villaCreateDto == null)
            {
                return BadRequest(villaCreateDto);
            }
            if (await _villaRepository.getFirstOrDefault(u=>u.Name.ToLower().Equals(villaCreateDto.Name.ToLower()))!=null)
            {
                ModelState.AddModelError("CustomError", "Name already exists");
                return BadRequest(ModelState);
            }
            Villa villa = _mapper.Map<VillaCreateDto, Villa>(villaCreateDto);
            await _villaRepository.AddVilla(villa);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new VillaDto { Id= villa.Id }, villa);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa=await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id,[FromBody] VillaUpdateDto villaUpdateDto )
            {
            if(id == 0 || id!= villaUpdateDto.Id || villaUpdateDto == null) 
            {
                return BadRequest();
            }
            _db.Villas.Update(_mapper.Map<VillaUpdateDto, Villa>(villaUpdateDto));
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id,JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Id == id) == null)
            {
                return NotFound();
            }
            var villaToUpdate = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            VillaUpdateDto villaDTO =_mapper.Map<VillaUpdateDto>(villaToUpdate);
            
            //this is the syntax to apply changes in patch
            // as ref chack https://jsonpatch.com/
            patchDto.ApplyTo(villaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
