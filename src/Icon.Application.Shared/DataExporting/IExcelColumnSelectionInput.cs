using System.Collections.Generic;

namespace Icon.DataExporting
{
    public interface IExcelColumnSelectionInput
    {
        List<string> SelectedColumns { get; set; }
    }
}