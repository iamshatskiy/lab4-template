using Rating.DTO;
using Rating.Entities;
using Rating.Interfaces;

namespace Rating.Services
{
    public class RatingService : IRatingService
    {
        readonly IRatingRepository _ratingRepository;
        public RatingService(IRatingRepository ratingRepository) 
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<RatingResponse?> GetRatingResponseByUserName(string userName)
        {
            var ratingResponse = await _ratingRepository.GetRatingResponseByUserName(userName);
            if (ratingResponse != null) 
            {
                return ratingResponse;
            }

            var entity = new Ratings
            {
                Stars = 50,
                UserName = userName
            };

            _ratingRepository.CreateRating(entity);
            await _ratingRepository.SaveAsync();

            return await _ratingRepository.GetRatingResponseByUserName(userName);
        }

        public async Task<RatingResponse?> UpdateRating(string userName, bool lessCond, bool laterDate)
        {
            var rating = await _ratingRepository.GetRatingByUserName(userName);
            if (rating == null) 
            {
                await GetRatingResponseByUserName(userName).ConfigureAwait(false);
                rating = await _ratingRepository.GetRatingByUserName(userName);
            }
            if (!lessCond && !laterDate)
            {
                rating.Stars = Math.Min(rating.Stars + 1, 99);
            }

            if (lessCond)
            {
                rating.Stars = Math.Max(rating.Stars - 10, 1);
            }

            if (laterDate)
            {
                rating.Stars = Math.Max(rating.Stars - 10, 1);
            }

            _ratingRepository.UpdateRating(rating);
            await _ratingRepository.SaveAsync();

            return await _ratingRepository.GetRatingResponseByUserName(userName);
        }

        public async Task DeleteRating(string userName)
        {
            var rating = await _ratingRepository.GetRatingByUserName(userName);
            if (rating == null)
            {
                return;
            }
            _ratingRepository.DeleteRating(rating);
            await _ratingRepository.SaveAsync();
        }

    }
}
