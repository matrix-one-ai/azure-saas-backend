using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.BaseManagement.BasePage.Requests
{
    public class GetBasePageInput
    {
        public BasePageType PageType { get; set; }
        public BaseListType ListType { get; set; }
    }
}