using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
namespace RazorPagesMovie.Models;
using RazorPagesMovie.Mappings;
using System.Globalization;

using CsvHelper.Configuration;
using System.IO;



public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new RazorPagesMovieContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<RazorPagesMovieContext>>()))
        {
            if (context == null || context.Movie == null)
            {
                throw new ArgumentNullException("Null RazorPagesMovieContext");
            }

            // Look for any movies.
            if (context.Movie.Any())
            {
                //return;   // DB has been seeded
                context.Movie.RemoveRange(context.Movie);
            }

            string SeedCSVFilePath = "Data/movies.csv"; // TODO Make this a global variable
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(SeedCSVFilePath);
            using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
            {
                csv.Context.RegisterClassMap<MovieMap>();
                var records = csv.GetRecords<Movie>();
                context.Movie.AddRange(records); // AddRange to add all records at once
                context.SaveChanges();
            }
        }
    }
}

