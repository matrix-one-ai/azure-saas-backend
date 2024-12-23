using Icon.Dto;
using System;

namespace Icon.EntityChanges.Dto
{
    public class GetEntityChangesByEntityInput
    {
        public string EntityTypeFullName { get; set; }
        public string EntityId { get; set; }
    }
}
