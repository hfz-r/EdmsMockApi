using System.Collections.Generic;

namespace EdmsMockApi.Entities
{
    public class Profile : BaseEntity
    {
        private ICollection<ProfileField> _profileFields;

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public virtual ICollection<ProfileField> ProfileFields
        {
            get => _profileFields ?? (_profileFields = new List<ProfileField>());
            protected set => _profileFields = value;
        }
    }
}