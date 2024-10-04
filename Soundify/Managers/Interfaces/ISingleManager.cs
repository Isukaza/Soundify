using Soundify.DAL.PostgreSQL.Models.db;
using Soundify.Models.Request.Create;
using Soundify.Models.Request.Update;

namespace Soundify.Managers.Interfaces;

public interface ISingleManager
{
    Task<SingleTrack> GetSingleTrack(Guid singleId);
    Task<SingleTrack> CreateSingleTrack(SingleCreateRequest trackData);
    Task<bool> UpdateSingleTrack(SingleTrack single, SingleUpdateRequest trackData);
    Task<bool> DeleteSingleTrack(SingleTrack track);
}