using System.ComponentModel.DataAnnotations;

namespace EdmsMockApi.Entities
{
    public abstract class BaseEntity
    {
        [MaxLength(450)]
        public int Id { get; set; }
    }
}