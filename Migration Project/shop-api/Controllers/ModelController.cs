using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    

    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetAll()
        {
            try
            {
                var models = await _modelService.GetAll();
                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<Model>> GetById(int id)
        {
            try
            {
                var model = await _modelService.GetById(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        
        public async Task<ActionResult> Create(ModelRequestDTO modelDto)
        {
            try
            {
                var newModel = await _modelService.Create(modelDto);
                return Ok(newModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit/{id}")]
        
        public async Task<ActionResult> Update(int id, ModelRequestDTO modelDto)
        {
            try
            {
                var updatedModel = await _modelService.UpdateAsync(id, modelDto);
                return Ok(updatedModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deletedModel = await _modelService.DeleteAsync(id);
                return Ok(deletedModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}