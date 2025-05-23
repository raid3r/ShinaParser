﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinaParser.Models;

public class Product
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Column("title")] // назва
    public string Title { get; set; } = null!;
    //- артикль
    [MaxLength(20)]
    public string Article { get; set; } = null!;
    
    public decimal? Price { get; set; }
    //- посилання на картинку
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    //- сезон
    [MaxLength(100)]
    public string? Season { get; set; }
    //- типорозмір
    [MaxLength(100)]
    public string? Size { get; set; }
    //- рік виробництва
    public int? Year { get; set; }
    //- в наявності(так / ні)
    public bool IsAvailable { get; set; }

    [NotMapped] // не створювати поле в БД
    public string? Comment { get; set; } = null!;

    public int? CountryId { get; set; }
    public virtual Country? Country { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    /*
     * 
     * 
     * 1000 products
     * 100 / brand
     * 
     * index  brandId
     * 
     * 1 , адреси запису,
     * 2 , адреси запису,
     * 3 , адреси запису,
     * 4 , адреси запису,
     * 
     * 
     * brandId
     * 1 +
     * 2 -
     * 1 +
     * 3 -
     * 5
     * 1
     * 2
     * 3
     * 4
     * 1
     * 2
     * 
     */

}
