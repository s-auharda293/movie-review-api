using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll() { 
            var reviews = await _service.GetAllReviewsAsync();
            return Ok(reviews);
        }


        [HttpGet]
        [Route("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByMovie(Guid movieId) { 
            var review = await _service.GetReviewsByMovieIdAsync(movieId);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(Guid id)
        {
            var review = await _service.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto)
        {
            try
            {
                var created = await _service.CreateReviewAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException e) {
                return BadRequest(new { error = e.Message});
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]Guid id,[FromBody] UpdateReviewDto dto)
        {
            var result = await _service.UpdateReviewAsync(id, dto);
                if (!result) return NotFound();
                return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id,[FromBody] PatchReviewDto dto)
        {
            try { 
            var result = await _service.PatchReviewAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _service.DeleteReviewAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
