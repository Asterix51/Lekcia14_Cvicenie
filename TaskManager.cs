namespace Lekcia14_Cvicenie
{
    public class TaskManager
    {
        private Dictionary<Guid, Task> tasks = new Dictionary<Guid, Task>(); // GUID ako kľúč
        private int nextId = 1;
        private string currentSortOption = "priority"; // Defaultné zoradenie podľa priority

        public void SelectSortOption()
        {
            string[] sortOptions = { "Podľa ID", "Podľa názvu", "Podľa termínu (dátumu splnenia)", "Podľa priority" };
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Zvoľte spôsob hlavného zoradenia (ostatné kritériá budú aplikované v poradí):");

                // Zobrazenie možností s grafickým zvýraznením
                for (int i = 0; i < sortOptions.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.WriteLine(sortOptions[i]);
                }
                Console.ResetColor();

                // Čítanie klávesov
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + sortOptions.Length) % sortOptions.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % sortOptions.Length;
                        break;
                    case ConsoleKey.Enter:
                        Console.ResetColor();
                        switch (selectedIndex)
                        {
                            case 0:
                                currentSortOption = "id";
                                Console.WriteLine("\nHlavné zoradenie nastavené na: Podľa ID.");
                                break;
                            case 1:
                                currentSortOption = "name";
                                Console.WriteLine("\nHlavné zoradenie nastavené na: Podľa názvu.");
                                break;
                            case 2:
                                currentSortOption = "duedate";
                                Console.WriteLine("\nHlavné zoradenie nastavené na: Podľa termínu.");
                                break;
                            case 3:
                                currentSortOption = "priority";
                                Console.WriteLine("\nHlavné zoradenie nastavené na: Podľa priority.");
                                break;
                        }
                        Console.WriteLine("\nStlačte ľubovoľnú klávesu pre návrat do hlavného menu...");
                        Console.ReadKey();
                        return; // Ukončenie metódy po výbere
                }
            }
        }

        public void DisplayInteractiveMenu()
        {
            string[] options = 
                {
                "Pridať úlohu",
                "Vymazať úlohu podľa ID",
                "Označiť úlohu ako splnenú",
                "Upraviť úlohu",
                "Vymazať všetky splnené úlohy",
                "Vymazať všetky úlohy",
                "Zmeniť spôsob zoradenia",
                "Ukončiť aplikáciu"
                };

            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                DisplayTasks();

                Console.WriteLine("\nVyberte akciu:");

                // Zobrazenie možností s grafickým zvýraznením
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.WriteLine(options[i]);
                }
                Console.ResetColor();

                // Čítanie klávesov
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % options.Length;
                        break;
                    case ConsoleKey.Enter:
                        HandleMenuSelection(selectedIndex);
                        return; // Ukončí menu po vykonaní akcie
                }
            }
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: AddTask(); break;
                case 1: DeleteTask(); break;
                case 2: MarkTaskCompleted(); break;
                case 3: EditTask(); break;
                case 4: ClearCompletedTasks(); break;
                case 5: ConfirmAndClearAllTasks(); break;
                case 6: SelectSortOption(); break;
                case 7:
                    SaveTasks();
                    Console.WriteLine("Aplikácia bola ukončená.");
                    Environment.Exit(0);
                    break;
            }
        }

        public void DisplayTasks()
        {
            if (!tasks.Any())
            {
                Console.WriteLine("Žiadne úlohy na zobrazenie.");
                return;
            }

            int idWidth = 5;
            int guidWidth = 10;
            int nameWidth = 50;
            int dateWidth = 12;
            int priorityWidth = 10;
            int completedWidth = 9;

            Console.WriteLine(
                $"{"ID".PadRight(idWidth)} | {"GUID".PadRight(guidWidth)} | {"Názov".PadRight(nameWidth)} | {"Termín".PadRight(dateWidth)} | {"Priorita".PadRight(priorityWidth)} | {"Splnené".PadRight(completedWidth)}");
            Console.WriteLine(new string('-', idWidth + guidWidth + nameWidth + dateWidth + priorityWidth + completedWidth + 17));

            // Zoradenie na základe currentSortOption a ostatných kritérií
            IEnumerable<Task> sortedTasks = tasks.Values;
            switch (currentSortOption)
            {
                case "id":
                    sortedTasks = sortedTasks.OrderBy(t => t.Id).ThenBy(t => t.Name).ThenBy(t => t.DueDate).ThenByDescending(t => t.Priority);
                    break;
                case "name":
                    sortedTasks = sortedTasks.OrderBy(t => t.Name).ThenBy(t => t.Id).ThenBy(t => t.DueDate).ThenByDescending(t => t.Priority);
                    break;
                case "duedate":
                    sortedTasks = sortedTasks.OrderBy(t => t.DueDate).ThenBy(t => t.Id).ThenBy(t => t.Name).ThenByDescending(t => t.Priority);
                    break;
                case "priority":
                    sortedTasks = sortedTasks.OrderByDescending(t => t.Priority).ThenBy(t => t.Id).ThenBy(t => t.Name).ThenBy(t => t.DueDate);
                    break;
            }

            foreach (var task in sortedTasks)
            {
                Console.WriteLine(
                    $"{task.Id.ToString().PadRight(idWidth)} | " +
                    $"{task.UniqueId.ToString().Substring(0, 8).PadRight(guidWidth)} | " +
                    $"{Truncate(task.Name, nameWidth).PadRight(nameWidth)} | " +
                    $"{task.DueDate:dd.MM.yyyy}".PadRight(dateWidth) + " | " +
                    $"{task.Priority.ToString().PadRight(priorityWidth)} | " +
                    $"{(task.IsCompleted ? "Áno" : "Nie").PadRight(completedWidth)}");
            }
        }


        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }


        public void AddTask()
        {
            Console.Write("Zadajte názov úlohy: ");
            string name = Console.ReadLine();

            DateTime dueDate;
            while (true)
            {
                Console.Write("Zadajte dátum splnenia (DD.MM.YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out dueDate))
                    break;
                Console.WriteLine("Neplatný formát dátumu. Skúste znova.");
            }

            TaskPriority priority;
            while (true)
            {
                Console.Write("Zadajte prioritu (1 = Nízka, 2 = Stredná, 3 = Vysoká): ");
                if (Enum.TryParse<TaskPriority>(Console.ReadLine(), out priority) &&
                    Enum.IsDefined(typeof(TaskPriority), priority))
                    break;
                Console.WriteLine("Neplatná hodnota priority. Zadajte 1, 2 alebo 3.");
            }

            var task = new Task
            {
                UniqueId = Guid.NewGuid(), // Generovanie GUID
                Id = nextId++,             // Číselné ID
                Name = name,
                DueDate = dueDate,
                Priority = priority,
                IsCompleted = false
            };

            tasks[task.UniqueId] = task; // GUID ako kľúč
            Console.WriteLine("Úloha bola pridaná.");
        }

        public void DeleteTask()
        {
            Console.Write("Zadajte ID alebo GUID úlohy na vymazanie: ");
            string input = Console.ReadLine();

            // Skúsime podľa číselného ID
            if (int.TryParse(input, out int numericId))
            {
                var task = tasks.Values.FirstOrDefault(t => t.Id == numericId);
                if (task != null)
                {
                    tasks.Remove(task.UniqueId);
                    Console.WriteLine("Úloha bola úspešne vymazaná.");
                    return;
                }
            }

            // Skúsime podľa GUID (alebo jeho časti)
            var guidTask = tasks.Values.FirstOrDefault(t => t.UniqueId.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase));
            if (guidTask != null)
            {
                tasks.Remove(guidTask.UniqueId);
                Console.WriteLine("Úloha bola úspešne vymazaná.");
                return;
            }

            Console.WriteLine("Úloha s daným ID alebo GUID neexistuje.");
        }


        public void MarkTaskCompleted()
        {
            Console.Write("Zadajte ID úlohy na označenie ako splnenej: ");
            string input = Console.ReadLine();

            // Hľadanie úlohy podľa číselného ID
            if (int.TryParse(input, out int id))
            {
                var task = tasks.Values.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.IsCompleted = true;
                    Console.WriteLine("Úloha bola označená ako splnená.");
                    return;
                }
            }

            // Hľadanie úlohy podľa GUID (alebo jeho časti)
            var guidTask = tasks.Values.FirstOrDefault(t => t.UniqueId.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase));
            if (guidTask != null)
            {
                guidTask.IsCompleted = true;
                Console.WriteLine("Úloha bola označená ako splnená.");
                return;
            }

            Console.WriteLine("Úloha s daným ID alebo GUID neexistuje.");
        }

        public void EditTask()
        {
            Console.Write("Zadajte ID alebo GUID úlohy na úpravu: ");
            string input = Console.ReadLine();

            // Hľadanie podľa číselného ID
            Task task = null;
            if (int.TryParse(input, out int id))
            {
                task = tasks.Values.FirstOrDefault(t => t.Id == id);
            }
            else
            {
                // Hľadanie podľa GUID
                task = tasks.Values.FirstOrDefault(t => t.UniqueId.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase));
            }

            if (task == null)
            {
                Console.WriteLine("Úloha s daným ID alebo GUID neexistuje.");
                return;
            }

            Console.WriteLine("Čo chcete upraviť? (1 = Názov, 2 = Termín, 3 = Priorita)");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Zadajte nový názov: ");
                    task.Name = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Zadajte nový termín (DD.MM.YYYY): ");
                    task.DueDate = DateTime.Parse(Console.ReadLine());
                    break;
                case 3:
                    Console.Write("Zadajte novú prioritu (1 = Nízka, 2 = Stredná, 3 = Vysoká): ");
                    task.Priority = (TaskPriority)int.Parse(Console.ReadLine());
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
            FileHandler.SaveToFile("tasks.json", tasks); // Ukladanie pomocou GUID ako kľúča
        }

        public void LoadTasks()
        {
            tasks = FileHandler.LoadFromFile<Dictionary<Guid, Task>>("tasks.json") ?? new Dictionary<Guid, Task>();
            nextId = tasks.Values.Any() ? tasks.Values.Max(t => t.Id) + 1 : 1;
        }
    }
}
