using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DapperExample.Models.Export;

public class CountryExport
{
    [JsonIgnore]
    [XmlIgnore]
    public int Id { get; set; }

    public string Title { get; set; } = null!;
}
