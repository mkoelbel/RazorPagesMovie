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

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        // Filtering happens in OnGet() rather than in OnPost(), because OnGet() needs to be able to filter based on
        // user inputs, since users may filter by directly editing the URL with query strings. (So when page is reloaded, 
        // OnGet() must be able to handle those query strings.) 
        // OnPost() method would be identical to OnGet(), so we omit it. When OnPost() would normally be called (when submit
        // button is pressed on Index page), we look for OnPost() here, don't find one, so default to OnGet().
        public async Task OnGetAsync()
        {
            IQueryable<string> genreQuery = from m in _context.Movie // LINQ query
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie // LINQ query. Defined here, but not yet executed.
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString)); // LINQ query still not yet executed
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            Movie = await movies.ToListAsync(); // Above LINQ is executed here, since we run ToListAsync() on it
        }
    }
}
