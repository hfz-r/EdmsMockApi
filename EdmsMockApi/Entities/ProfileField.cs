namespace EdmsMockApi.Entities
{
    public class ProfileField : BaseEntity
    {
        public int ColId { get; set; }

        public int ProfileId { get; set; }

        public string ColName { get; set; }

        public string ColDesc { get; set; }

        public string ColDataType { get; set; }

        public virtual Profile Profile { get; set; }
    }
}