namespace Lekcia14_Cvicenie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.LoadTasks();

            while (true)
            {
                taskManager.DisplayMenu();
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": taskManager.AddTask(); break;
                    case "2": taskManager.DeleteTask(); break;
                    case "3": taskManager.MarkTaskCompleted(); break;
                    case "4": taskManager.EditTask(); break;
                    case "5": taskManager.ClearCompletedTasks(); break;
                    case "6": taskManager.ConfirmAndClearAllTasks(); break;
                    case "7":
                        taskManager.SaveTasks();
                        Console.WriteLine("Aplikácia bola ukončená.");
                        return;
                    default:
                        Console.WriteLine("Neplatná voľba. Skúste znova.");
                        break;
                }
            }
        }
    }
}
