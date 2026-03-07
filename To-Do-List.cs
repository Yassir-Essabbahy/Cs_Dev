using System;
using System.Collections.Generic;

class HelloWorld {
static List<string> tasks = new List<string>();
static bool keepRunning = true;

  static void Main() {
      
      
      while (keepRunning){
          Machine();
        
    }
      }
  
  static void Machine(){
    Console.WriteLine("Choose Operation : ");
    Console.WriteLine("1-----------Add");
    Console.WriteLine("2-----------List");
    Console.WriteLine("3-----------Remove");
    Console.WriteLine("4-----------Edit");
    Console.WriteLine("5-----------Exit");

    
    int option;
    string input = Console.ReadLine();
    while (!int.TryParse(input, out option))
    {
      
      Console.WriteLine("Invalid Option, Re-Try Again : ");
      input = Console.ReadLine();
    
    }
        
    switch (option){
        case 1:
        Add();
        break;
        
        case 2:
        DisplayList();
        break;
        
        case 3:
        Remove();
        break;
        
        case 4:
        edit();
        break;

        case 5:
        Console.WriteLine("Exitting...!");
        keepRunning = false;
        break;

        default :
        Console.Write("Unkown Option");
        break;


  }
  }
  
  static void Add(){
      

      Console.WriteLine("Add Your Tasks Here (type 'Exit' to leave) : ");
      string input = Console.ReadLine();
      while (input != "Exit"){

          tasks.Add(input);
          input = Console.ReadLine();
          
      }
      Console.WriteLine("Back to Main Menu");
  }
  
  static void DisplayList(){
    Console.WriteLine("                       ");
    Console.WriteLine("                       ");
    Console.WriteLine("                       ");
    Console.WriteLine("                       ");
    Console.WriteLine("Your Tasks Down Below :            ");

 if (tasks.Count == 0)
      {
       Console.WriteLine("There is No Task.");
       
      }
    else
    {
      
      for (int i = 0; i < tasks.Count; i++)
      {
        Console.WriteLine($"{i+1}----{tasks[i]}");
      }
    }
  }
  
  static void Remove(){
    if (tasks.Count == 0)
    {
      Console.WriteLine("There is No Tasks.");
      return;
    }
    else
    {
    Console.WriteLine("What task you want to delete : ");
    string input = Console.ReadLine();
    int n;
    while ((!int.TryParse(input, out n)) || ((n <= 0) || (tasks.Count < n)))
    {
      Console.WriteLine("Invalid, Try Again : ");
      input = Console.ReadLine();
    }
    
    

      
      tasks.RemoveAt(n-1);
      Console.WriteLine($"Removed Task Number {n} Seccueffuly");
    }


  }

  static void edit()
  {
    if (tasks.Count == 0)
      {
        Console.WriteLine("Tasks List is Empty");
      }
      else
    {
      
    DisplayList();
    Console.WriteLine("Which Task You Want to Edit : ");
    string input = Console.ReadLine();
    int n;
    while ((!int.TryParse(input, out n)) || ((n > tasks.Count) || (n <= 0)))
    {
      Console.WriteLine("Invalid, Try Again : ");
      input = Console.ReadLine();
      
    }
    
    
    Console.WriteLine("New Task : ");
    string task = Console.ReadLine();

    
    tasks[n-1] = task;
      
    

     


    }
  }

  
  
}