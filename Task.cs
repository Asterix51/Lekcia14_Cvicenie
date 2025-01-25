namespace Lekcia14_Cvicenie
{
    public class Task
    {
        public Guid UniqueId { get; set; } // GUID ako hlavný identifikátor
        public int Id { get; set; }        // Číselné ID pre používateľské rozhranie
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
