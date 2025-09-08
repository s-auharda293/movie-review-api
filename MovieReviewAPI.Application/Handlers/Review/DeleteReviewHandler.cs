using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class DeleteReviewHandler:IRequestHandler<DeleteReviewCommand,bool>
    {
        private readonly IApplicationDbContext _context;
        public DeleteReviewHandler(IApplicationDbContext context) { 
            _context = context;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken) {
            var review = await _context.Reviews.FirstOrDefaultAsync(r=>r.Id == request.Id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
