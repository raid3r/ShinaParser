// See https://aka.ms/new-console-template for more information
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using ShinaParser.Migrations;
using ShinaParser.Models;
using System.Text.RegularExpressions;

Console.WriteLine("infoshina.com.ua parser");


var url = "https://infoshina.com.ua/uk/shiny?start=1";

using var context = new ParserContext();
using (var client = new HttpClient())
{
    var response = await client.GetAsync(url);
    var content = await response.Content.ReadAsStringAsync();
    //await File.WriteAllTextAsync("C:\\Users\\kvvkv\\source\\repos\\ShinaParser\\ShinaParser\\infoshina.html", content);

    var htmlDocument = new HtmlDocument();
    htmlDocument.LoadHtml(content);
    var doc = htmlDocument.DocumentNode;

    var productCards = doc.QuerySelectorAll(".product-card-content");

    foreach (var productCard in productCards)
    {
        // title
        var productTitle = productCard.QuerySelector("[class*=ProductCard_name]").InnerText;

        // article

        var aricleRaw = productCard.QuerySelector("[class*=Sku_wrap]").InnerText;
        string article = string.Empty;
        string pattern = @"(?<=<!-- -->: <!-- -->)\d+";
        Match match = Regex.Match(aricleRaw, pattern);
        if (match.Success)
        {
            article = match.Value;
        }

        // price
        var priceRaw = productCard.QuerySelector(".Price-block").InnerText;
        // remove spaces
        priceRaw = priceRaw.Replace(" ", "");
        // replace all not digits with ','
        priceRaw = Regex.Replace(priceRaw, @"[^\d,]", ",");
        var price = priceRaw.Split(",")
            .Select(x => Regex.Replace(x, @"[^\d,]", ""))
            .Where(x => x != "")
            .Select(x => int.Parse(x))
            .Min();

        // Brand
        var properties = productCard.QuerySelectorAll("[class*=ProductCard_property_list] div.ais");

        var brandTitle = string.Empty;

        foreach (var property in properties)
        {
            var firstDiv = property.QuerySelector("div");
            var propertyTitle = firstDiv.InnerText;
            if (propertyTitle == "Бренд")
            {
                var brandLink = property.QuerySelector("a");
                brandTitle = brandLink.GetAttributeValue("title", string.Empty);
            }

        }

        // country
        var countryTitle = productCard.QuerySelector("[class*=ProductionData_country] img").GetAttributeValue("alt", string.Empty);

        // url link
        // season
        // production year
        // size

        Console.WriteLine(productTitle);
        Console.WriteLine(article);
        Console.WriteLine(price);
        Console.WriteLine(brandTitle);
        Console.WriteLine(countryTitle);
        Console.WriteLine("------------------------------------------");

        // brands
        var brand = await context.Brands.FirstOrDefaultAsync(x => x.Title == brandTitle);
        if (brand == null)
        {
            brand = new Brand { Title = brandTitle };
            context.Brands.Add(brand);
        }
        // countries
        var country = await context.Countries.FirstOrDefaultAsync(x => x.Title == countryTitle);
        if (country == null)
        {
            country = new Country { Title = countryTitle };
            context.Countries.Add(country);
        }

        var product = await context.Products.FirstOrDefaultAsync(x => x.Article == article);
        if (product == null)
        {
            product = new Product();
            context.Products.Add(product);
        }
        product.Article = article;
        product.Brand = brand;
        product.Country = country;
        product.Price = price;
        product.Title = productTitle;
        product.IsAvailable = true;

        await context.SaveChangesAsync();
    }

    
}
