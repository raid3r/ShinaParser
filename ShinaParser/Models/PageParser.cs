using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShinaParser.Models;

public class PageParser
{

    private string GetProductTitle(HtmlNode productCard)
    {
        var productTitle = productCard.QuerySelector("[class*=ProductCard_name]").InnerText;
        return productTitle;
    }

    public string GetProductArticle(HtmlNode productCard)
    {
        var aricleRaw = productCard.QuerySelector("[class*=Sku_wrap]").InnerText;
        string article = string.Empty;
        string pattern = @"(?<=<!-- -->: <!-- -->)\d+";
        Match match = Regex.Match(aricleRaw, pattern);
        if (match.Success)
        {
            article = match.Value;
        }
        return article;
    }

    public decimal GetProductPrice(HtmlNode productCard)
    {
        var priceRaw = productCard.QuerySelector(".Price-block")?.InnerText;
        if (string.IsNullOrEmpty(priceRaw))
        {
            return 0;
        }
        // remove spaces
        priceRaw = priceRaw.Replace(" ", "");
        // replace all not digits with ',' and remove all not digits
        priceRaw = Regex.Replace(priceRaw, @"[^\d,]", ",");

        var price = priceRaw.Split(",")
            .Select(x => Regex.Replace(x, @"[^\d,]", ""))
            .Where(x => x != "")
            .Select(x => int.Parse(x))
            .Min();

        return price;
    }

    public string GetPropertyValue(HtmlNode productCard, string propertyName)
    {
        var properties = productCard.QuerySelectorAll("[class*=ProductCard_property_list] div.ais");
        foreach (var property in properties)
        {
            var firstDiv = property.QuerySelector("div");
            var propertyTitle = firstDiv.InnerText;
            if (propertyTitle == propertyName)
            {

                var propLink = property.QuerySelector("a");
                return propLink.GetAttributeValue("title", string.Empty);
            }
        }
        return string.Empty;
    }

    public string? GetCountryTitle(HtmlNode productCard)
    {
        var html = productCard.OuterHtml;
        var countryTitle = productCard.QuerySelector("[class*=ProductionData_country] img")?.GetAttributeValue("alt", string.Empty);
        return countryTitle;
    }

    public bool isProductAvailable(HtmlNode productCard)
    {
        return GetProductPrice(productCard) > 0;
    }

    public int? GetProductYear(HtmlNode productCard)
    {
        var html = productCard.InnerHtml;
        string pattern = @"Рік виробництва:\s*(\d{4})";
        Match match = Regex.Match(html, pattern);
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        return null;
    }

    public string? GetProductImageUrl(HtmlNode productCard)
    {
        var html = productCard.InnerHtml;
        string pattern = @"src=.+(\/media\/images\/.+\.jpg)";
        Match match = Regex.Match(html, pattern);
        if (match.Success)
        {
            return "https://infoshina.com.ua" + match.Groups[1].Value;
        }
        return null;
    }

    public async Task ParseCatalogPageAsync(int page)
    {
        var url = "https://infoshina.com.ua/uk/shiny?start=" + page;

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
                var productTitle = GetProductTitle(productCard);
                var article = GetProductArticle(productCard);
                var price = GetProductPrice(productCard);
                var brandTitle = GetPropertyValue(productCard, "Бренд");
                var seasonTitle = GetPropertyValue(productCard, "Сезон");
                var sizeTitle = GetPropertyValue(productCard, "Типорозмір");
                var countryTitle = GetCountryTitle(productCard);
                var isAvailable = isProductAvailable(productCard);
                var year = GetProductYear(productCard);
                var imageUrl = GetProductImageUrl(productCard);
                
                Console.WriteLine(productTitle);
                //Console.WriteLine(article);
                //Console.WriteLine(price);
                //Console.WriteLine(brandTitle);
                //Console.WriteLine(countryTitle);
                //Console.WriteLine(sizeTitle);
                //Console.WriteLine(seasonTitle);
                //Console.WriteLine(year);
                //Console.WriteLine(imageUrl);
                //Console.WriteLine(isAvailable ? "Available" : "NOT available");
                //Console.WriteLine("------------------------------------------");


                // brands
                var brand = await context.Brands.FirstOrDefaultAsync(x => x.Title == brandTitle);
                if (brand == null)
                {
                    brand = new Brand { Title = brandTitle };
                    context.Brands.Add(brand);
                }
                // countries
                Country? country = null;
                if (!string.IsNullOrEmpty(countryTitle))
                {
                    country = await context.Countries.FirstOrDefaultAsync(x => x.Title == countryTitle);
                    if (country == null)
                    {
                        country = new Country { Title = countryTitle };
                        context.Countries.Add(country);
                    }
                }

                // select
                // UPDATE



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
                product.Size = sizeTitle;
                product.Season = seasonTitle;
                product.Year = year;
                product.IsAvailable = isAvailable;
                product.ImageUrl = imageUrl;

                //var html = productCard.OuterHtml;
                await context.SaveChangesAsync();
            }


        }

    }

}
