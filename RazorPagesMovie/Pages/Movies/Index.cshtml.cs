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
        public string? TitleSearchString { get; set; }

        public SelectList? AllRatings { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedRating { get; set; }

        public SelectList? AllGenres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedGenre { get; set; }

        public List<int>? AllYears { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedMinYear { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SelectedMaxYear { get; set; }

        public SelectList? AllCountries { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedCountry { get; set; }

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
            AllRatings = new SelectList(await ratingQuery.Distinct().ToListAsync());

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            AllGenres = new SelectList(await genreQuery.Distinct().ToListAsync());

            IQueryable<int> yearsQuery = from m in _context.Movie
                                         orderby m.Year
                                         select m.Year;
            AllYears = new List<int>(await yearsQuery.Distinct().ToListAsync());

            IQueryable<string> countryQuery = from m in _context.Movie
                                            orderby m.Country
                                            select m.Country;
            AllCountries = new SelectList(await countryQuery.Distinct().ToListAsync());

            var movies = from m in _context.Movie // LINQ query. Defined here, but not yet executed.
                         select m;
            if (!string.IsNullOrEmpty(TitleSearchString))
            {
                movies = movies.Where(s => s.Title.Contains(TitleSearchString)); // LINQ query still not yet executed
            }

            if (!string.IsNullOrEmpty(SelectedRating))
            {
                movies = movies.Where(x => x.Rating == SelectedRating);
            }

            if (!string.IsNullOrEmpty(SelectedGenre))
            {
                movies = movies.Where(x => x.Genre == SelectedGenre);
            }

            if (SelectedMinYear is not null & SelectedMinYear != 0)
            {
                movies = movies.Where(x => x.Year >= SelectedMinYear);
            }

            if (SelectedMaxYear is not null & SelectedMaxYear != 0)
            {
                movies = movies.Where(x => x.Year <= SelectedMaxYear);
            }

            if (!string.IsNullOrEmpty(SelectedCountry))
            {
                movies = movies.Where(x => x.Country == SelectedCountry);
            }

            Movie = await movies.ToListAsync(); // Above LINQ is executed here, since we run ToListAsync() on it
        }
    }
}
