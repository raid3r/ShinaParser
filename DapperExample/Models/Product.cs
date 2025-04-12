using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Models;

public class Product
{
    public int Id { get; set; }
    public string Article { get; set; } = null!;
    public string ProductTitle { get; set; } = null!;
    public string BrandTitle { get; set; } = null!;
    public string? CountryTitle { get; set; } = null!;
}
