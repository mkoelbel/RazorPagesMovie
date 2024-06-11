using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models;

public class Movie
{
    public int Id { get; set; } // Required by the database for the primary key.

    [Required]
    [StringLength(60, MinimumLength = 3)]
    [Name("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
    [Name("rating")]
    public string Rating { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [Name("genre")]
    public string Genre { get; set; } = string.Empty;

    // DateTimes are inherently required, so don't need [Required]
    [Display(Name = "Release Year")] // Better display name in app
    [Range(1900, 2030)]
    [Name("year")]
    public int Year { get; set; }

    [Range(0, 10)]
    [Name("score")]
    [Column(TypeName = "decimal(2, 1)")]
    public decimal Score { get; set; }

    [Range(0, 10000000)]
    [Name("votes")]
    public int Votes { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s\.-]*$")]
    [Name("director")]
    public string Director { get; set; } = string.Empty;

    [Required]
    [StringLength(60, MinimumLength = 3)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s\.-]*$")]
    [Name("star")]
    public string Star { get; set; } = string.Empty;

    [Required]
    [StringLength(60, MinimumLength = 2)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [Name("country")]
    public string Country { get; set; } = string.Empty;

    // decimals, ints, and floats are inherently required, so don't need [Required]
    [Range(0, 10000000000)]
    [Name("budget")]
    [DataType(DataType.Currency)]
    public int Budget { get; set; }

    [Range(0, 10000000000)]
    [Name("gross")]
    [DataType(DataType.Currency)]
    public long Gross { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    [Name("company")]
    public string Company { get; set; } = string.Empty;

    [Display(Name = "Runtime (min)")]
    [Range(0, 240)] // Up to 4 hours
    [Name("runtime")]
    public int Runtime { get; set; }
}
