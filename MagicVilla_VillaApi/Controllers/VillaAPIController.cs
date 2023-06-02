using AutoMapper;
using MagicVilla_VillaApi.Data;
//using MagicVilla_VillaApi.Logging;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

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
        private readonly Response _response;
       // private readonly IRepository<Villa> _repository;
        private readonly IVillaRepository _villaRepository;
        public VillaAPIController(ILogger<VillaAPIController> logger,ApplicationDBContext db,IMapper mapper,IVillaRepository villaRepository)
        {
            _logger= logger;
            _db= db;
            _mapper= mapper;
            _villaRepository= villaRepository;
            _response = new ();
          //  _repository = repository;
        }

       
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Response>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Getting all Villas", "");
                IEnumerable<Villa> list = await _villaRepository.getAllAsync();
                _response.Result = _mapper.Map<List<VillaDto>>(list);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

       
        [HttpGet("{id:int}", Name ="GetVilla")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(VillaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // return type can be given as typeof(VillaDto)
        //or in method signature as well Like "public ActionResult<VillaDto> getVilla(int id)" also.
        public async Task<ActionResult<Response>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get Villa Error with Id" + id, "Error");
                    return BadRequest();
                }
                var villa = await _villaRepository.getFirstOrDefaultAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<ActionResult<Response>> CreateVilla([FromBody]VillaCreateDto villaCreateDto)
        {
            try
            {
                if (villaCreateDto == null)
                {
                    _response.IsSuccess= false;
                    _response.ErrorMessage=new List<string>() { HttpStatusCode.BadRequest.ToString()};
                    return BadRequest(_response);
                }
                if (await _villaRepository.getFirstOrDefaultAsync(u => u.Name.ToLower().Equals(villaCreateDto.Name.ToLower())) != null)
                {
                   
                    _response.IsSuccess = false;
                    _response.ErrorMessage = new List<string>() { HttpStatusCode.BadRequest.ToString() };
                    return BadRequest(_response);
                }
                Villa villa = _mapper.Map<VillaCreateDto, Villa>(villaCreateDto);
                await _villaRepository.CreateAsync(villa);

                _response.StatusCode = System.Net.HttpStatusCode.Created;
                _response.Result = villa;
                return CreatedAtRoute("GetVilla", new VillaDto { Id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage=new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "CUSTOM")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> DeleteVilla(int id)
        {
            Villa villa=await _villaRepository.getFirstOrDefaultAsync(u=>u.Id==id);
            if (villa == null)
            {
                return NotFound();
            }
            if (id == 0)
            {
                return BadRequest();
            }
             await _villaRepository.RemoveAsync(villa);
            //_response.Result=villa;
            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            // await _db.SaveChangesAsync();
            return Ok(_response);
        }

        [HttpPut("{id:int}" , Name ="UpdateVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> UpdateVilla(int id,[FromBody] VillaUpdateDto villaUpdateDto )
            {
            if(id == 0 || id!= villaUpdateDto.Id || villaUpdateDto == null) 
            {
                return BadRequest();
            }
            await _villaRepository.UpdateAsync(_mapper.Map<VillaUpdateDto, Villa>(villaUpdateDto));
            _response.StatusCode=System.Net.HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id,JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }
            var villaToUpdate = await _villaRepository.getFirstOrDefaultAsync(u => u.Id == id,tracked:false);
            if (villaToUpdate == null)
            {
                return NotFound();
            }
            VillaUpdateDto villaDTO =_mapper.Map<Villa,VillaUpdateDto>(villaToUpdate);
          
            //this is the syntax to apply changes in patch
            // as ref chack https://jsonpatch.com/
            patchDto.ApplyTo(villaDTO, ModelState);

           await _villaRepository.UpdateAsync(_mapper.Map<VillaUpdateDto, Villa>(villaDTO));
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
