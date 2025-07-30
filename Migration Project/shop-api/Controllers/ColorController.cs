using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetAll()
        {
            try
            {
                var colors = await _colorService.GetAll();
                return Ok(colors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<Color>> GetById(int id)
        {
            try
            {
                var color = await _colorService.GetById(id);
                return Ok(color);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(ColorRequestDTO color)
        {
            try
            {
                var NewColor = await _colorService.Create(color);
                return Ok(NewColor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Update(int id, ColorRequestDTO dto)
        {
            try
            {
                var Color = await _colorService.UpdateAsync(id,dto);
                return Ok(Color);
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
                var Color = await _colorService.DeleteAsync(id);
                return Ok(Color);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
