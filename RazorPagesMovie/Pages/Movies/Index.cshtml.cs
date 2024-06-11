using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Ratings { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieRating { get; set; }

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        public List<int>? Years { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MovieMinYear { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? MovieMaxYear { get; set; }

        public SelectList? Countries { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieCountry { get; set; }

        // Filtering happens in OnGet() rather than in OnPost(), because OnGet() needs to be able to filter based on
        // user inputs, since users may filter by directly editing the URL with query strings. (So when page is reloaded, 
        // OnGet() must be able to handle those query strings.) 
        // OnPost() method would be identical to OnGet(), so we omit it. When OnPost() would normally be called (when submit
        // button is pressed on Index page), we look for OnPost() here, don't find one, so default to OnGet().
        public async Task OnGetAsync()
        {
            IQueryable<string> ratingQuery = from m in _context.Movie // LINQ query
                                             orderby m.Rating
                                             select m.Rating;
            Ratings = new SelectList(await ratingQuery.Distinct().ToListAsync());

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            IQueryable<int> yearsQuery = from m in _context.Movie
                                         orderby m.Year
                                         select m.Year;
            Years = new List<int>(await yearsQuery.Distinct().ToListAsync());

            IQueryable<string> countryQuery = from m in _context.Movie
                                            orderby m.Country
                                            select m.Country;
            Countries = new SelectList(await countryQuery.Distinct().ToListAsync());

            var movies = from m in _context.Movie // LINQ query. Defined here, but not yet executed.
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString)); // LINQ query still not yet executed
            }

            if (!string.IsNullOrEmpty(MovieRating))
            {
                movies = movies.Where(x => x.Rating == MovieRating);
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }

            if (MovieMinYear is not null & MovieMinYear != 0)
            {
                Console.WriteLine(MovieMinYear);
                movies = movies.Where(x => x.Year >= MovieMinYear);
            }

            if (MovieMaxYear is not null & MovieMaxYear != 0)
            {
                movies = movies.Where(x => x.Year <= MovieMaxYear);
            }

            if (!string.IsNullOrEmpty(MovieCountry))
            {
                movies = movies.Where(x => x.Country == MovieCountry);
            }

            Movie = await movies.ToListAsync(); // Above LINQ is executed here, since we run ToListAsync() on it
        }
    }
}
