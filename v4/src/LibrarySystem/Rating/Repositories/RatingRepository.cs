using Microsoft.EntityFrameworkCore;
using Rating.DTO;
using Rating.Entities;
using Rating.Interfaces;

namespace Rating.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        readonly RatingDbContext _context;

        public RatingRepository(RatingDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void CreateRating(Ratings entity)
        {
            _context.Ratings.Add(entity);
        }
        public void UpdateRating(Ratings entity)
        {
            _context.Ratings.Update(entity);
        }

        public void DeleteRating(Ratings entity)
        {
            _context.Ratings.Remove(entity);
        }

        public async Task<RatingResponse> GetRatingResponseByUserName(string userName)
        {
            var query = await _context.Ratings.Where(r => r.UserName == userName).FirstOrDefaultAsync();

            if (query != null)
            {
                return new RatingResponse { Stars = query.Stars };
            }
            else
            {
                return null;
            }
        }

        public async Task<Ratings> GetRatingByUserName(string userName)
        {
            var rating = await _context.Ratings.Where(r => r.UserName == userName).FirstOrDefaultAsync();

            return rating == null ? null : rating;
        }
    }
}
