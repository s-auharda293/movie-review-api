using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Services;
public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllActorsAsync();
    Task<ActorDto?> GetActorByIdAsync(int id);
    Task<ActorDto> CreateActorAsync(CreateActorDto dto);
    Task<bool> UpdateActorAsync(int id, UpdateActorDto dto);
    Task<bool> PatchActorAsync(int id, PatchActorDto dto);
    Task<bool> DeleteActorAsync(int id);
}

