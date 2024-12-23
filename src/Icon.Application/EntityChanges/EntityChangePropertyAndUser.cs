using Abp.EntityHistory;
using Icon.Authorization.Users;
using System.Collections.Generic;

namespace Icon.EntityChanges
{
    public class EntityChangePropertyAndUser
    {
        public EntityChange EntityChange { get; set; }
        public List<EntityPropertyChange> PropertyChanges { get; set; }
        public User User { get; set; }
    }
}
