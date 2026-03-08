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

public static void AddQuantity(List<InventoryItem> inventory, int id, int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid Amount");
            return;
        }
        bool found = false;
        foreach (var item in inventory)
        {
            if (item.Id == id)
            {
                item.Quantity = item.Quantity + amount;
                found = true;
                break;
            }
            
        
        }
        
        if (!found) Console.WriteLine("Item not found");
    }

public static void RemoveQuantity(List<InventoryItem> inventory, int id , int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid Amount");
            return;
        }
        bool found = false;
        foreach (var item in inventory)
        {
            if (item.Id == id)
            {
                found = true;
                if (item.Quantity < amount)
                {
                    Console.WriteLine("Not Enough Quantity");
                    break;
                }
                else
                {
                    item.Quantity = item.Quantity - amount;
                    break;
                }
            }
        }
        if (!found) Console.WriteLine("Item not found");
    }
}


class Program
{
    static void Main()
    {
      var inventory = new List<InventoryItem>();

inventory.Add(new InventoryItem(1, "Axe", 2));
inventory.Add(new InventoryItem(2, "Potion", 5));

InventoryItem.ListItems(inventory);

InventoryItem.AddQuantity(inventory,2,3);
Console.WriteLine("--------------Add--------------");
InventoryItem.ListItems(inventory);
Console.WriteLine("----------------------------");
Console.WriteLine("--------------Remove--------------");

InventoryItem.RemoveQuantity(inventory,1,1);

InventoryItem.ListItems(inventory);


      

      
    }


}


