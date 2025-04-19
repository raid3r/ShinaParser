using CsvHelper;
using Dapper;
using DapperExample.Models;
using DapperExample.Models.Export;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

Console.WriteLine("Dapper example");


// ORM
// object-relational mapping

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;


var connectionString = "Data Source=SILVERSTONE\\SQLEXPRESS;Initial Catalog=ShinaParserP33;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Command Timeout=0";

//new App(connectionString).Run();


using (var connection = new SqlConnection(connectionString))
{
    // завантажити усі країни
    var allCountriesSql = "SELECT Id,Title FROM countries";
    var allCountries = connection.Query<CountryExport>(allCountriesSql);
    //allCountries.ToList().ForEach(country => Console.WriteLine(country.Title));

    // завантажити усі бренди
    var allBrandsSql = "SELECT brand_id as Id, brand_name as Title FROM product_brands";
    var allBrands = connection.Query<BrandExport>(allBrandsSql);
    //allBrands.ToList().ForEach(brand => Console.WriteLine(brand.Title));

    // завантажити дані продуктів та додати в них бренди та країни
    var allProductsSql = "SELECT " +
        "Id, Title, Article, Price, ImageUrl, Season, Year, IsAvailable, " +
        "country_model_id AS CountryId, BrandId " +
        "FROM Products";
    var allProducts = connection
        .Query<ProductExport>(allProductsSql)
        .Select(p =>
        {
            p.Brand = allBrands.First(b => b.Id == p.BrandId);
            if (p.CountryId != null)
            {
                p.Country = allCountries.First(c => c.Id == p.CountryId);
            }
            return p;
        }).ToList();

    Console.WriteLine($"Product count {allProducts.Count}");


    //allProducts.ForEach(p =>
    //{
    //    Console.WriteLine($"{p.Title} {p.Brand.Title} {p.Country?.Title}");
    //});


    // перетворити на потрібний формат
    // * - JSON
    using (FileStream fs = File.Create("products.json"))
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };
        JsonSerializer.Serialize(fs, allProducts, options);
    }
    
    // * - XML
    var serializer = new XmlSerializer(typeof(List<ProductExport>));
    using (FileStream fs = File.Create("products.xml"))
    {
        serializer.Serialize(fs, allProducts);
    }

    // * - CSV
    using (FileStream fs = File.Create("products.csv"))
    {
        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
        {
            using (CsvWriter csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords<ProductExport>(allProducts);
            }
        }
    }
    
    // * - XLSX 


    // записати у файл
}


//






//using (var connection = new SqlConnection(connectionString))
//{
//    var sql = connection.CreateCommand();





//}

















// *-JSON




// * Додати в меню можливість вивантаження даних у файл
// * Отримання даних - за допомогою Dapper
// * 
// * Пункт меню
// * Зберегти у файл
// * Назва файла
// * Обреріть формат
// * - JSON
// * - XML
// * - CSV
// * - XLSX 
// *  
// * Завантаження з файлу


//using (var connection = new SqlConnection(connectionString))
//{
//    // 17
//    var sql = "UPDATE product_brands SET brand_name=@Title WHERE brand_id=@Id brand_name";
//    var affectedRows = connection.Execute(sql, new { Id = 17, Title = "Updated test brand"});
//    Console.WriteLine($"Updated {affectedRows} rows");


//    // mapping aggregate data to model

//    //var sql = "SELECT \r\ncountries.Title as CountryTitle,\r\nCOUNT(Products.Id) as ProductCount\r\nFROM countries LEFT JOIN Products ON countries.Id=Products.country_model_id GROUP BY countries.Id, countries.Title ORDER BY CountryTitle;";

//    //var rows = connection.Query<CountryProductCount>(sql);

//    //foreach (var row in rows)
//    //{
//    //    Console.WriteLine($"{row.CountryTitle} \t {row.ProductCount}");
//    //}


//    // insert brand

//    //var brand = new Brand
//    //{
//    //    Title = "Test brand"
//    //};

//    //var sql = "INSERT INTO product_brands (brand_name) VALUES (@Title)";

//    //var rowsAffected = connection.Execute(sql, brand);

//    //Console.WriteLine($"Inserted {rowsAffected} rows");

//    sql = "SELECT brand_id AS Id, brand_name AS Title FROM product_brands";
//    var brands = connection.Query<Brand>(sql);
//    foreach (var b in brands)
//    {
//        Console.WriteLine($"Brand id: {b.Id} {b.Title}");
//    }

//    //// select one product
//    //var sql = "SELECT Products.Id, Products.Title as ProductTitle, Article, product_brands.brand_name as BrandTitle, countries.Title as CountryTitle FROM Products LEFT JOIN product_brands ON product_brands.brand_id=Products.BrandId LEFT JOIN countries ON countries.Id=Products.country_model_id WHERE Products.Id=@Id";
//    //var product = connection.QuerySingle<Product>(sql, new { Id = 1 });

//    //Console.WriteLine($"{product.Id} {product.ProductTitle} {product.Article} {product.CountryTitle} {product.BrandTitle}");


//    // select all products
//    //var sql = "SELECT Products.Id, Products.Title as ProductTitle, Article, product_brands.brand_name as BrandTitle, countries.Title as CountryTitle FROM Products LEFT JOIN product_brands ON product_brands.brand_id=Products.BrandId LEFT JOIN countries ON countries.Id=Products.country_model_id";
//    //var products = connection.Query<Product>(sql);
//    //foreach (var product in products)
//    //{
//    //    Console.WriteLine($"{product.Id} {product.ProductTitle} {product.Article} {product.CountryTitle} {product.BrandTitle}");
//    //}
//}

///*
// * Створити проєкт та підключити бібліотеку Dapper
// * Написати програму , яка підключається до бази даних з товарами
// * Додати меню
// * 1. Перегляд всіх товарів (пошук по назві) - запитує текст для пошуку. якщо пустий то усі.
// * 2. Редагування товару
// * 3. Додавання товару
// * 4. Видалення товару
// * 5. Вивід статистикт по брендам та по країнам
// *          Країна - кількість товарів
// *          Бренд - кількість товарів
// * 6. Вихід
// * 
// * з використанням Dapper
// * 
// */ 


///*
// * Додати в меню можливість вивантаження даних у файл
// * Отримання даних - за допомогою Dapper
// * 
// * Пункт меню
// * Зберегти у файл
// * Назва файла
// * Обреріть формат
// * - JSON
// * - XML
// * - CSV
// * - XLSX 
// *  
// * Завантаження з файлу
// * 
// * 
// * 
// * 
// * 
// * 
// * 
// */ 

