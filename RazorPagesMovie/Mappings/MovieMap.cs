using CsvHelper.Configuration;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Mappings
{
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Id).Ignore(); // Ignore the 'Id' column
            Map(m => m.Title).Name("title");
            Map(m => m.Rating).Name("rating");
            Map(m => m.Genre).Name("genre");
            Map(m => m.Year).Name("year");
            Map(m => m.Score).Name("score");
            Map(m => m.Votes).Name("votes");
            Map(m => m.Director).Name("director");
            Map(m => m.Star).Name("star");
            Map(m => m.Country).Name("country");
            Map(m => m.Budget).Name("budget");
            Map(m => m.Gross).Name("gross");
            Map(m => m.Company).Name("company");
            Map(m => m.Runtime).Name("runtime");
        }
    }
}
