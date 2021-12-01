namespace model.Entities
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedAt = CreatedAt == DateTime.MinValue ? DateTime.Now : CreatedAt;
        }

        public T ID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
