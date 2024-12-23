using System;
using Abp.Application.Services.Dto;
using Abp.UI;

namespace Icon.BaseManagement
{
    public class OpenBaseModalInput
    {
        public BaseModalType ModalType { get; set; }
        public string EntityPath { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ClientId { get; set; }

        public OpenBaseModalInput()
        {

        }

        public OpenBaseModalInput GetOpenBaseModalInput(BaseModalType type, string entityPath = null)
        {
            return new OpenBaseModalInput
            {
                ModalType = type,
                EntityPath = entityPath
            };

        }
    }
}