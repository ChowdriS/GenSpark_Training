using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ShopContext _shopContext;
        private readonly INewsService _newsService;

        public NewsController(ShopContext shopContext, INewsService newsService)
        {
            _shopContext = shopContext;
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAll()
        {
            return Ok(await _newsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetById(int id)
        {
            try
            {
                return Ok(await _newsService.GetById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(NewsRequestDTO dto)
        {
            try
            {
                var news = await _newsService.Create(dto);
                return Ok(news);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Update(int id, NewsUpdateRequestDTO dto)
        {
            try
            {
                var updated = await _newsService.Update(id, dto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _newsService.Delete(id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("export/csv")]
        public async Task<FileContentResult> ExportToCsv()
        {
            var csv = await _newsService.ExportToCsv();
            var bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"News_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }

        [HttpGet("export/excel")]
        public async Task<FileContentResult> ExportToExcel()
        {
            var excelHtml = await _newsService.ExportToExcel();
            var bytes = Encoding.UTF8.GetBytes(excelHtml);
            return File(bytes, "application/vnd.ms-excel", $"News_{DateTime.Now:yyyyMMddHHmmss}.xls");
        }
    }
}
