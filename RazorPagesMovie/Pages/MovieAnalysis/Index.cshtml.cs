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

        public List<string> AllGenres { get; set; }
        public List<int> MovieCountsByGenre { get; set; }

        public async Task OnGetAsync()
        {
            // Query to get the count of movies grouped by genre
            var genreData = await _context.Movie
                                          .GroupBy(m => m.Genre)
                                          .Select(g => new
                                          {
                                              Genre = g.Key,
                                              Count = g.Count()
                                          })
                                          .ToListAsync();

            // Process data for the chart
            AllGenres = genreData.Select(g => g.Genre).ToList();
            MovieCountsByGenre = genreData.Select(g => g.Count).ToList();
        }
    }
}
