using System;
using System.Collections.Generic;

public class Player
{
    public string Name {get; set;}
    public int Health {get; set;}
    public int MaxHealth {get; set;}

    public Player(string name , int maxHealth)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) 
        {
            Console.WriteLine("Invalid Damage");
            return;
        }
        
        Health = Health - amount;
        if (Health <= 0)
        {
            Health = 0;
            
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid heal");
            return;
        }
        
        Health = Health + amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public bool IsDead()
    {
       return Health == 0;
    }

    public void PrintStatus()
    {
         Console.WriteLine($"Name : {Name} | Health : {Health}/{MaxHealth}");
    }
}

class Program
{
    static void Main()
    {
        Player player = new Player("Hero",100);
        player.PrintStatus();
        player.TakeDamage(30);
        player.PrintStatus();
        player.Heal(10);
        player.PrintStatus();
        player.TakeDamage(90);
        player.PrintStatus();
        Console.WriteLine(player.IsDead());
        player.Heal(500);
        player.PrintStatus();
        player.TakeDamage(-5);
        player.PrintStatus();
        player.Heal(0);



      
    }


}


