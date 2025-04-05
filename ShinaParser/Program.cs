// See https://aka.ms/new-console-template for more information
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using ShinaParser.Migrations;
using ShinaParser.Models;
using System.Text.RegularExpressions;

Console.WriteLine("infoshina.com.ua parser");

int pages = 6000;
var pageParser = new PageParser();
for (int i = 0; i < pages; i++)
{
    Console.WriteLine($"Parsing page {i + 1} of {pages}");
    await pageParser.ParseCatalogPageAsync(i);
}
