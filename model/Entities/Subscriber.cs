namespace model.Entities
{
    public class Subscriber : User
    {
        public Subscriber()
        {

        }

        public Subscriber(string name, string email)
        {
            ID = Guid.NewGuid();
            Name = name;
            Email = email;
        }

        public Guid SubscriptionId { get; set; } = Guid.NewGuid();
    }
}
