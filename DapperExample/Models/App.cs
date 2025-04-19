using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Models;

public class App
{
    private readonly string _connectionString;
    private SqlConnection connection;

    public App(string connectionString)
    {
        _connectionString = connectionString;
        connection = new SqlConnection(connectionString);
    }

    private string ShowMenu()
    {
        Console.WriteLine("\n--- МЕНЮ ---");
        Console.WriteLine("1. Перегляд товарів (пошук по назві)");
        Console.WriteLine("2. Редагування товару");
        Console.WriteLine("3. Додавання товару");
        Console.WriteLine("4. Видалення товару");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("6. Експорт в файл");
        Console.WriteLine("0. Вихід");
        Console.Write("Виберіть опцію: ");
        return Console.ReadLine();
    }

    private void ShowProducts()
    {
        Console.Write("Введіть назву для пошуку (або залиште порожнім для всіх): ");

        var search = Console.ReadLine();

        var sql = @"SELECT p.id, p.article, p.title AS ProductTitle, 

                               pb.title AS BrandTitle, c.Title AS CountryTitle

                        FROM Products p

                        LEFT JOIN brands pb ON pb.id = p.BrandId

                        LEFT JOIN countries c ON c.Id = p.country_id

                        WHERE p.title LIKE @search";

        var products = connection.Query<Product>(sql, new { search = $"%{search}%" });

        foreach (var p in products)

        {

            Console.WriteLine($"{p.Id}: {p.ProductTitle} | Арт: {p.Article} | {p.BrandTitle} | {p.CountryTitle}");

        }
    }

    private void EditProduct()
    {
        Console.Write("ID товару для редагування: ");

        int editId = int.Parse(Console.ReadLine()!);

        Console.Write("Нова назва: ");

        var newTitle = Console.ReadLine();

        Console.Write("Новий артикул: ");

        var newArticle = Console.ReadLine();

        var sql = "UPDATE Products SET title=@Title, article=@Article WHERE id=@Id";

        var updated = connection.Execute(sql, new { Id = editId, Title = newTitle, Article = newArticle });

        Console.WriteLine($"Оновлено {updated} рядків.");
    }

    private void AddProduct()
    {
        Console.Write("Назва: ");

        var title = Console.ReadLine();

        Console.Write("Артикул: ");

        var article = Console.ReadLine();

        Console.Write("ID бренду: ");

        int brandId = int.Parse(Console.ReadLine()!);

        Console.Write("ID країни: ");

        int countryId = int.Parse(Console.ReadLine()!);

        var sql = @"INSERT INTO Products (title, article, BrandId, country_id, is_available) 
        VALUES (@Title, @Article, @BrandId, @CountryId, @IsAvailable)";

        var inserted = connection.Execute(sql, new
        {
            Title = title,
            Article = article,
            BrandId = brandId,
            CountryId = countryId,
            IsAvailable = true
        });

        Console.WriteLine($"Додано {inserted} товар(ів).");
    }

    private void DeleteProduct()
    {
        Console.Write("ID товару для видалення: ");

        int deleteId = int.Parse(Console.ReadLine()!);

        var sql = "DELETE FROM Products WHERE id=@Id";

        var deleted = connection.Execute(sql, new { Id = deleteId });

        Console.WriteLine($"Видалено {deleted} товар(ів).");
    }

    private void ShowStatistic()
    {
        Console.WriteLine("Статистика по країнах:");

        var sql = @"SELECT c.Title AS CountryTitle, COUNT(p.id) AS ProductCount

                    FROM countries c

                    LEFT JOIN Products p ON c.Id = p.country_id

                    GROUP BY c.Title";

        var countryStats = connection.Query<CountryProductCount>(sql);

        foreach (var stat in countryStats)

        {

            Console.WriteLine($"{stat.CountryTitle}: {stat.ProductCount}");

        }

        Console.WriteLine("\nСтатистика по брендах:");

        sql = @"SELECT pb.title AS CountryTitle, COUNT(p.id) AS ProductCount

                    FROM brands pb

                    LEFT JOIN Products p ON pb.id = p.BrandId

                    GROUP BY pb.title";

        var brandStats = connection.Query<CountryProductCount>(sql);

        foreach (var stat in brandStats)

        {

            Console.WriteLine($"{stat.CountryTitle}: {stat.ProductCount}");

        }
    }

    private void ExportToFile()
    {

    }

    public void Run()
    {
        while (true)
        {
            var input = ShowMenu();

            switch (input)
            {
                case "1":
                    ShowProducts();
                    break;

                case "2":
                    EditProduct();
                    break;

                case "3":
                    AddProduct();
                    break;

                case "4":
                    DeleteProduct();
                    break;

                case "5":
                    ShowStatistic();
                    break;

                case "6":
                    ExportToFile();
                    break;

                case "0":

                    Console.WriteLine("Вихід...");
                    return;

                default:
                    Console.WriteLine("Невірна опція.");
                    break;

            }

        }
    }

}
