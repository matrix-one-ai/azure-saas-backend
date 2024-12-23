using System;
using System.Collections.Generic;
using Abp.UI;


namespace Icon.BaseManagement
{
    public class BaseModalBuilder
    {
        public BaseModalDto Build(BaseModalType modalType, string entityName, string icon, string size)
        {
            return new BaseModalDto
            {
                ModalTitle = "Modal.Title." + modalType.ToString(),
                ModalEntityName = entityName,
                ModalIcon = icon,
                ModalSize = size,
            };
        }
    }
}
