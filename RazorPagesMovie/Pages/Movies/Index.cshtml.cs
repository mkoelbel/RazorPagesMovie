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
            // Get lists of values to populate the filter options for the various fields
            AllRatings = new SelectList(await _context.Movie
                .Select(m => m.Rating)
                .Distinct()
                .OrderBy(rating => rating)
                .ToListAsync());

            AllGenres = new SelectList(await _context.Movie
                .Select(m => m.Genre)
                .Distinct()
                .OrderBy(genre => genre)
                .ToListAsync());

            AllYears = new List<int>(await _context.Movie
                .Select(m => m.Year)
                .Distinct()
                .OrderBy(year => year)
                .ToListAsync());

            AllCountries = new SelectList(await _context.Movie
                .Select(m => m.Country)
                .Distinct()
                .OrderBy(country => country)
                .ToListAsync());

            // Get list of movies, filtered on the user inputs
            var movies = _context.Movie.Select(m => m); // LINQ, not executed yet

            if (!string.IsNullOrEmpty(TitleSearchString))
            {
                movies = movies.Where(m => m.Title.Contains(TitleSearchString)); // LINQ, still not executed yet
            }

            if (!string.IsNullOrEmpty(SelectedRating))
            {
                movies = movies.Where(m => m.Rating == SelectedRating);
            }

            if (!string.IsNullOrEmpty(SelectedGenre))
            {
                movies = movies.Where(m => m.Genre == SelectedGenre);
            }

            if (SelectedMinYear is not null & SelectedMinYear != 0)
            {
                movies = movies.Where(m => m.Year >= SelectedMinYear);
            }

            if (SelectedMaxYear is not null & SelectedMaxYear != 0)
            {
                movies = movies.Where(m => m.Year <= SelectedMaxYear);
            }

            if (!string.IsNullOrEmpty(SelectedCountry))
            {
                movies = movies.Where(m => m.Country == SelectedCountry);
            }

            Movie = await movies.ToListAsync(); // LINQ executed here, since we run ToListAsync() on it
        }
    }
}
