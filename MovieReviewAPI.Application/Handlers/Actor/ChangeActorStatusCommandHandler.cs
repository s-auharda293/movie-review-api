using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Domain.Common.Actors;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Domain.Enums;

public class ChangeActorStatusCommandHandler
    : IRequestHandler<ChangeActorStatusCommand, Result<ActorStatusResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ChangeActorStatusCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<ActorStatusResponseDto>> Handle(ChangeActorStatusCommand request, CancellationToken cancellationToken)
    {
        var actor = await _context.Actors
            .FirstOrDefaultAsync(a => a.Id == request.ActorId, cancellationToken);

        if (actor == null)
            return Result<ActorStatusResponseDto>.Failure(ActorErrors.NotFound);

        if (!Enum.TryParse<ProposalStatus>(request.Status, true, out var newStatus))
            return Result<ActorStatusResponseDto>.Failure(ActorErrors.InvalidStatus());

        if (actor.Status == newStatus.ToString())
            return Result<ActorStatusResponseDto>.Failure(ActorErrors.StatusAlreadySet());

        var userId = Guid.Parse(_currentUser.GetUserId()!);

        actor.Status = newStatus.ToString();
        actor.StatusChangedBy = userId;
        actor.StatusChangedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<ActorStatusResponseDto>.Success(new ActorStatusResponseDto
        {
            Id = actor.Id,
            Status = actor.Status,
            ProposedBy = actor.ProposedBy,
            ProposedAt = actor.ProposedAt,
            StatusChangedBy = actor.StatusChangedBy,
            StatusChangedAt = actor.StatusChangedAt
        });
    }
}
