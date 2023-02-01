namespace ShoppingSystem.Models
{
    public class User
    {
        public enum BuyerType
        {
            none, regular, golden, wholesale
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BuyerType Type { get; set; } = BuyerType.regular;
        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
