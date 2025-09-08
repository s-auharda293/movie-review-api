using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class UpdateReviewHandler:IRequestHandler<UpdateReviewCommand,bool>
    {
        private readonly IApplicationDbContext _context;
        public UpdateReviewHandler(IApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken) {

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return false;

            review.UserName = request.dto.UserName;
            review.Comment = request.dto.Comment;
            review.Rating = request.dto.Rating;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        

    }
    }
}
