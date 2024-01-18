using Rating.DTO;
using Rating.Entities;

namespace Rating.Interfaces
{
    public interface IRatingRepository
    {
        Task<Ratings> GetRatingByUserName(string userName);
        Task<RatingResponse> GetRatingResponseByUserName(string userName);
        void CreateRating(Ratings entity);
        void UpdateRating(Ratings entity);
        void DeleteRating(Ratings entity);
        Task SaveAsync();
    }
}
