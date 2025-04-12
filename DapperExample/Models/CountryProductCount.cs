using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Models;

public class CountryProductCount
{
    public string CountryTitle { get; set; } = null!;
    public int ProductCount { get; set; }
}
