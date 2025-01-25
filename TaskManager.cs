
namespace Lekcia14_Cvicenie
{
    public class TaskManager
    {
        private Dictionary<int, Task> tasks = new Dictionary<int, Task>();
        private int nextId = 1;

        public void DisplayMenu()
        {
            Console.Clear();
            DisplayTasks();
            Console.WriteLine("\nVyberte akciu:");
            Console.WriteLine("1. Pridať úlohu");
            Console.WriteLine("2. Vymazať úlohu podľa ID");
            Console.WriteLine("3. Označiť úlohu ako splnenú");
            Console.WriteLine("4. Upraviť úlohu");
            Console.WriteLine("5. Vymazať všetky splnené úlohy");
            Console.WriteLine("6. Vymazať všetky úlohy");
            Console.WriteLine("7. Ukončiť aplikáciu");
        }

        public void DisplayTasks()
        {
            if (!tasks.Any())
            {
                Console.WriteLine("Žiadne úlohy na zobrazenie.");
                return;
            }

            var sortedTasks = tasks.Values
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate);

            Console.WriteLine("ID   | Názov          | Termín       | Priorita   | Splnené");
            Console.WriteLine("-----+----------------+--------------+------------+---------");

            foreach (var task in sortedTasks)
            {
                Console.WriteLine($"{task.Id,-4} | {task.Name,-14} | {task.DueDate:dd.MM.yyyy} | {task.Priority,-10} | {(task.IsCompleted ? "Áno" : "Nie")}");
            }
        }

        public void AddTask()
        {
            Console.Write("Zadajte názov úlohy: ");
            string name = Console.ReadLine();

            Console.Write("Zadajte dátum splnenia (DD.MM.YYYY): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Zadajte prioritu (1 = Nízka, 2 = Stredná, 3 = Vysoká): ");
            TaskPriority priority = (TaskPriority)int.Parse(Console.ReadLine());

            tasks[nextId] = new Task
            {
                Id = nextId,
                Name = name,
                DueDate = dueDate,
                Priority = priority,
                IsCompleted = false
            };

            nextId++;
            Console.WriteLine("Úloha bola pridaná.");
        }

        public void DeleteTask()
        {
            Console.Write("Zadajte ID úlohy na vymazanie: ");
            int id = int.Parse(Console.ReadLine());

            if (tasks.Remove(id))
            {
                Console.WriteLine("Úloha bola úspešne vymazaná.");
            }
            else
            {
                Console.WriteLine("Úloha s daným ID neexistuje.");
            }
        }

        public void MarkTaskCompleted()
        {
            Console.Write("Zadajte ID úlohy na označenie ako splnenej: ");
            int id = int.Parse(Console.ReadLine());

            if (tasks.ContainsKey(id))
            {
                tasks[id].IsCompleted = true;
                Console.WriteLine("Úloha bola označená ako splnená.");
            }
            else
            {
                Console.WriteLine("Úloha s daným ID neexistuje.");
            }
        }

        public void EditTask()
        {
            Console.Write("Zadajte ID úlohy na úpravu: ");
            int id = int.Parse(Console.ReadLine());

            if (!tasks.ContainsKey(id))
            {
                Console.WriteLine("Úloha s daným ID neexistuje.");
                return;
            }

            Console.WriteLine("Čo chcete upraviť? (1 = Názov, 2 = Termín, 3 = Priorita)");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Zadajte nový názov: ");
                    tasks[id].Name = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Zadajte nový termín (YYYY-MM-DD): ");
                    tasks[id].DueDate = DateTime.Parse(Console.ReadLine());
                    break;
                case 3:
                    Console.Write("Zadajte novú prioritu (1 = Nízka, 2 = Stredná, 3 = Vysoká): ");
                    tasks[id].Priority = (TaskPriority)int.Parse(Console.ReadLine());
                    break;
            }

            Console.WriteLine("Úloha bola upravená.");
        }

        public void ClearCompletedTasks()
        {
            tasks = tasks.Where(t => !t.Value.IsCompleted)
                         .ToDictionary(t => t.Key, t => t.Value);
            Console.WriteLine("Všetky splnené úlohy boli vymazané.");
        }

        public void ConfirmAndClearAllTasks()
        {
            Console.WriteLine("Ste si istý, že chcete vymazať všetky úlohy? (y/n)");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y")
            {
                tasks.Clear();
                Console.WriteLine("Všetky úlohy boli vymazané.");
            }
            else
            {
                Console.WriteLine("Akcia bola zrušená.");
            }
        }

        public void SaveTasks()
        {
            FileHandler.SaveToFile("tasks.json", tasks);
        }

        public void LoadTasks()
        {
            tasks = FileHandler.LoadFromFile<Dictionary<int, Task>>("tasks.json") ?? new Dictionary<int, Task>();
            nextId = tasks.Keys.Any() ? tasks.Keys.Max() + 1 : 1;
        }
    }
}
