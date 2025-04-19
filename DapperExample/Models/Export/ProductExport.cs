using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DapperExample.Models.Export;

public class ProductExport
{
    [JsonPropertyName("id")]
    [XmlElement("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    [XmlElement("title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("num")]
    [XmlElement("num")]
    public string Article { get; set; } = null!;
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Season { get; set; }
    public string? Size { get; set; }
    public int? Year { get; set; }
    public bool IsAvailable { get; set; }

    [JsonIgnore]
    [XmlIgnore]
    public int? CountryId { get; set; }
    [JsonIgnore]
    [XmlIgnore]
    public int BrandId { get; set; }

    [JsonPropertyName("country")]
    [XmlElement("country")]
    public virtual CountryExport? Country { get; set; } = null!;
    [JsonPropertyName("brand")]
    [XmlElement("brand")]
    public virtual BrandExport Brand { get; set; } = null!;
}
