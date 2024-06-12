using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;

namespace RazorPagesMovie.Pages.MovieAnalysis
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovieContext context)
        {
            _context = context;
        }

        public List<string> AllRatings { get; set; }
        public List<int> MovieCountsByRating { get; set; }

        public List<string> AllGenres { get; set; }
        public List<int> MovieCountsByGenre { get; set; }

        public List<string> AllCountries { get; set; }
        public List<int> MovieCountsByCountry { get; set; }

        public async Task<(List<string>, List<int>)> GetMovieCountsByFieldAsync(string fieldName)
        {
            var movieCountsData = await _context.Movie
                .GroupBy(m => EF.Property<object>(m, fieldName)) // Use EF.Property for accessing dynamic property names
                .Select(g => new
                {
                    FieldValue = g.Key.ToString(), // "Key" is the field we GroupedBy above
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var fieldValues = movieCountsData.Select(data => data.FieldValue).ToList();
            var counts = movieCountsData.Select(data => data.Count).ToList();

            return (fieldValues, counts);
        }

        public async Task OnGetAsync()
        {
            // Data to feed graphs
            (AllRatings, MovieCountsByRating) = await GetMovieCountsByFieldAsync("Rating");
            (AllGenres, MovieCountsByGenre) = await GetMovieCountsByFieldAsync("Genre");
            (AllCountries, MovieCountsByCountry) = await GetMovieCountsByFieldAsync("Country");
        }
    }
}
