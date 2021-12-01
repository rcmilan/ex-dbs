namespace model.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
