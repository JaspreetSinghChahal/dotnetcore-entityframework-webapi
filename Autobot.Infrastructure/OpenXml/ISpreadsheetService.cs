using System.Collections.Generic;
using System.IO;

namespace Autobot.Infrastructure.OpenXml
{
    public interface ISpreadsheetService
    {
        MemoryStream SpreadsheetStream { get; set; }

        void AddHeader(List<string> headers);
        void AddRow(List<string> dataItems);
        void CloseSpreadsheet();
        void CreateColumnWidth(uint startIndex, uint? endIndex = null, double? width = null);
        bool CreateSpreadsheet();
    }
}