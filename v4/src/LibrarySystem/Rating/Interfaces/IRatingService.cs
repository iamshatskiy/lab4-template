using Rating.DTO;

namespace Rating.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponse?> GetRatingResponseByUserName(string userName);
        Task<RatingResponse?> UpdateRating(string userName, bool lessCond, bool laterDate);
        Task DeleteRating(string userName);
    }
}
