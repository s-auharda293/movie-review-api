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
            if (!reviews.Any()) return Ok("There are no reviews yet!");
            return Ok(reviews);
        }


        [HttpGet]
        [Route("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByMovie(int movieId) { 
            var review = await _service.GetReviewsByMovieIdAsync(movieId);
            if (!review.Any()) return Ok("There is no review for this movie yet!");
            return Ok(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            var review = await _service.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto)
        {
            var created = await _service.CreateReviewAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody] UpdateReviewDto dto)
        {
            var result = await _service.UpdateReviewAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch([FromRoute] int id,[FromBody] PatchReviewDto dto)
        {
            var result = await _service.PatchReviewAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _service.DeleteReviewAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
