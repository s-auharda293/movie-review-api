using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieReviewDbContext _context;

    public MoviesController(MovieReviewDbContext context) {
        _context = context;
    }
}
