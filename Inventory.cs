using System;
using System.Collections.Generic;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }

    public InventoryItem(int id, string name, int quantity)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
    }
    public static void ListItems(List<InventoryItem> inventory)
{
    if (inventory.Count == 0)
    {
        Console.WriteLine("Inventory empty");
        return;
    }

    foreach (var item in inventory)
    {
        Console.WriteLine($"ID: {item.Id} | Name: {item.Name} | Qty: {item.Quantity}");
    }
}
}


class Program
{
    static void Main()
    {
      var inventory = new List<InventoryItem>();

inventory.Add(new InventoryItem(1, "Axe", 1));
inventory.Add(new InventoryItem(2, "Potion", 5));

InventoryItem.ListItems(inventory);
      

      
    }


}


