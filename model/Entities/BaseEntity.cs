namespace model.Entities
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedAt = CreatedAt == DateTime.MinValue ? DateTime.Now : CreatedAt;
        }

        public virtual T ID { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}