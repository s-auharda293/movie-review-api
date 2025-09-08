using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class PatchReviewHandler:IRequestHandler<PatchReviewCommand,bool>
    {
        private readonly IApplicationDbContext _context;
        public PatchReviewHandler(IApplicationDbContext context)
        {
         _context = context;   
        }

        public async Task<bool> Handle(PatchReviewCommand request, CancellationToken cancellationToken) {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return false;

            if (!string.IsNullOrEmpty(request.dto.Comment)) review.Comment = request.dto.Comment;
            if (request.dto.Rating.HasValue) review.Rating = request.dto.Rating.Value;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
