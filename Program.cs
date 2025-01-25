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
                taskManager.DisplayInteractiveMenu();
            }
        }
    }
}
