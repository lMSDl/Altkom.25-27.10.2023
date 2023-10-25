namespace Models
{
    public class ShoppingList : Entity
    {
        public string? Name { get; set; }
        public DateTime DateTime { get; } = DateTime.Now;
    }
}