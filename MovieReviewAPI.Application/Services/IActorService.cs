using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Services;
public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllActorsAsync();
    Task<ActorDto?> GetActorByIdAsync(Guid id);
    Task<ActorDto> CreateActorAsync(CreateActorDto dto);
    Task<bool> UpdateActorAsync(Guid id, UpdateActorDto dto);
    Task<bool> PatchActorAsync(Guid id, PatchActorDto dto);
    Task<bool> DeleteActorAsync(Guid id);
}

