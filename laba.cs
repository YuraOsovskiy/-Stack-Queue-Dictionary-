using System;
using System.Collections.Generic;

// Інтерфейс

interface IProduct
{
    string Name { get; }
    decimal Price { get; }
    decimal CalculateDiscount();
}

// Абстрактний клас товару
abstract class Product : IProduct
{
    public string Name { get; protected set; }
    public decimal Price { get; protected set; }

    public abstract decimal CalculateCost();
    public abstract decimal CalculateDiscount();
}

// Класи-нащадки для різних видів товарів
class Book : Product
{
    public int PageCount { get; private set; }

    public Book(string name, decimal price, int pageCount)
    {
        Name = name;
        Price = price;
        PageCount = pageCount;
    }

    public override decimal CalculateCost()
    {
        return Price;
    }

    public override decimal CalculateDiscount()
    {
        // Реалізація обчислення знижки для книг
        return Price * 0.1m;
    }
}

class Electronics : Product
{
    public int MemorySize { get; private set; }

    public Electronics(string name, decimal price, int memorySize)
    {
        Name = name;
        Price = price;
        MemorySize = memorySize;
    }

    public override decimal CalculateCost()
    {
        return Price;
    }

    public override decimal CalculateDiscount()
    {
        // Реалізація обчислення знижки для електроніки
        return Price * 0.05m;
    }
}

class Clothing : Product
{
    public string Size { get; private set; }

    public Clothing(string name, decimal price, string size)
    {
        Name = name;
        Price = price;
        Size = size;
    }

    public override decimal CalculateCost()
    {
        return Price;
    }

    public override decimal CalculateDiscount()
    {
        // Реалізація обчислення знижки для одягу
        return Price * 0.15m;
    }
}

// Клас для замовлення
class Order
{
    public int OrderNumber { get; private set; }
    public List<IProduct> Products { get; private set; } = new List<IProduct>();
    public decimal TotalCost { get; private set; }

    public delegate void OrderStatusChangedDelegate(string status);
    public event OrderStatusChangedDelegate OrderStatusChanged;

    public Order(int orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public void AddProduct(IProduct product)
    {
        Products.Add(product);
        TotalCost += product.CalculateCost();
    }

    public void ProcessOrder()
    {
        // Розрахунок знижок
        decimal totalDiscount = 0;
        foreach (var product in Products)
        {
            totalDiscount += product.CalculateDiscount();
        }

        TotalCost -= totalDiscount;

        // Оповіщення про зміну статусу замовлення
        OnOrderStatusChanged("Processed");
    }

    protected virtual void OnOrderStatusChanged(string status)
    {
        OrderStatusChanged?.Invoke($"Order {OrderNumber}: {status}");
    }
}

// Клас для обробки замовлень
class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        order.ProcessOrder();
    }
}

// Клас для сервісу сповіщень
class NotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine($"Notification: {message}");
    }
}

// Точка входу в програму
class Program
{
    static void Main()
    {
        // Створення об'єктів товарів
        Book book = new Book("Book1", 20.0m, 200);
        Electronics electronics = new Electronics("Electronics1", 500.0m, 256);
        Clothing clothing = new Clothing("Clothing1", 50.0m, "Medium");

        // Створення замовлення
        Order order = new Order(1);

        // Додавання товарів до замовлення
        order.AddProduct(book);
        order.AddProduct(electronics);
        order.AddProduct(clothing);

        // Створення об'єктів для обробки та сповіщення замовлення
        OrderProcessor orderProcessor = new OrderProcessor();
        NotificationService notificationService = new NotificationService();

        // Підписка на подію зміни статусу замовлення
        order.OrderStatusChanged += notificationService.SendNotification;

        // Обробка та виведення інформації про замовлення
        orderProcessor.ProcessOrder(order);

        Console.ReadLine();
    }
}
