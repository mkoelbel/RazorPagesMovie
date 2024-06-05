using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models;

public class Movie
{
    public int Id { get; set; } // Required by the database for the primary key.
    public string? Title { get; set; } // ? makes Title field nullable.
    [Display(Name = "Release Date")] // Better display name in app
    [DataType(DataType.Date)] // Only display date, not time, in ReleaseDate field below.
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
}
