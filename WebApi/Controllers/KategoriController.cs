using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    public class KategoriController : ControllerBase
    {
        private IKategoriService _kategoriService;

        public KategoriController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _kategoriService.Query().SingleOrDefault(k => k.Id == id);
            if (model == null)
                return NotFound(); // 404
            return Ok(model); // 200
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = _kategoriService.Query().ToList();
            if (model.Count == 0)
                return NotFound(); // 404
            return Ok(model); // 200
        }

        [HttpPost("Ekle")]
        public IActionResult Ekle(KategoriModel model)
        {
            var result = _kategoriService.Add(model);
            if (result.IsSuccessful)
            {
                return CreatedAtAction("Get", new { id = model.Id }, model);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Put(KategoriModel model)
        {
            var result = _kategoriService.Update(model);
            if (result.IsSuccessful)
                return Ok(model); // 200
            return BadRequest(result.Message); // 400
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _kategoriService.Delete(id);
            if (result.IsSuccessful)
                return Ok(id); // 200
            return BadRequest(result.Message); // 400
        }

    }
}
