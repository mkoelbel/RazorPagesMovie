using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models;

public class Movie
{
    public int Id { get; set; } // Required by the database for the primary key.

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Display(Name = "Release Date")] // Better display name in app
    [DataType(DataType.Date)] // Only display date, not time, in ReleaseDate field below.
    // DateTimes are inherently required, so don't need [Required]
    public DateTime ReleaseDate { get; set; }

    [Range(1, 100)]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")] // So EF Core can correctly map Price to currency in the database (?)
    // decimals, ints, and floats are inherently required, so don't need [Required]
    public decimal Price { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [Required]
    [StringLength(30)]
    public string Genre { get; set; } = string.Empty;

    [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
    [Required]
    [StringLength(5)]
    public string Rating { get; set; } = string.Empty;
}
