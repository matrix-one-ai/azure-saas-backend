using System;
using System.Collections.Generic;
using Abp.Localization;
using Abp.UI;


namespace Icon.BaseManagement
{
    public class BaseFormBuilder
    {
        private readonly ILocalizationManager _localizationManager;

        public BaseFormBuilder()
        {
        }
        public BaseFormBuilder(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }
        public BaseFormDto Build(BaseFormType formType)
        {
            return new BaseFormDto(_localizationManager)
            {
                FormTitle = formType.ToString(),
            };
        }


    }
}