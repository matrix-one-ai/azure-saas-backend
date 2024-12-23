using System;
using Abp.Application.Services.Dto;

namespace Icon.BaseManagement
{
    public class GetBaseModalInput
    {
        public BaseModalType ModalType { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? LocationId { get; set; }

        public GetBaseModalInput()
        {

        }
    }

}