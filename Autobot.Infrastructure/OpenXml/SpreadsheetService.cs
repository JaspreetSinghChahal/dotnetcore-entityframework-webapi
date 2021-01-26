using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Autobot.Infrastructure.OpenXml
{
    public class SpreadsheetService : ISpreadsheetService
    {
        public MemoryStream SpreadsheetStream { get; set; }
        private Worksheet currentWorkSheet { get { return spreadSheet.WorkbookPart.WorksheetParts.First().Worksheet; } }
        private SpreadsheetDocument spreadSheet;
        private Columns _cols;

        /// <summary>
        /// Create a basic spreadsheet template         
        /// </summary>
        /// <returns></returns>
        public bool CreateSpreadsheet()
        {
            try
            {
                SpreadsheetStream = new MemoryStream();

                // Create the spreadsheet on the MemoryStream
                spreadSheet = SpreadsheetDocument.Create(SpreadsheetStream, SpreadsheetDocumentType.Workbook);

                WorkbookPart wbp = spreadSheet.AddWorkbookPart();   // Add workbook part
                WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>(); // Add worksheet part
                Workbook wb = new Workbook(); // Workbook
                FileVersion fv = new FileVersion();
                fv.ApplicationName = "App Name";
                Worksheet ws = new Worksheet(); // Worksheet
                SheetData sd = new SheetData(); // Data on worksheet

                // Add stylesheet
                WorkbookStylesPart stylesPart = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = GenerateStyleSheet();
                stylesPart.Stylesheet.Save();


                _cols = new Columns(); // Created to allow bespoke width columns

                ws.Append(sd); // Add sheet data to worksheet
                wsp.Worksheet = ws; // Add the worksheet to the worksheet part
                wsp.Worksheet.Save();
                // Define the sheets that the workbooks has in it.

                Sheets sheets = new Sheets();
                Sheet sheet = new Sheet();
                sheet.SheetId = 1; // Only one sheet per spreadsheet in this class so call it sheet 1
                sheet.Id = wbp.GetIdOfPart(wsp); // ID of sheet comes from worksheet part
                sheet.Name = "sheetData";
                sheets.Append(sheet);
                wb.Append(fv);
                wb.Append(sheets); // Append sheets to workbook

                spreadSheet.WorkbookPart.Workbook = wb;
                spreadSheet.WorkbookPart.Workbook.Save();
            }
            catch
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// add the bespoke columns for the list spreadsheet
        /// </summary>
        public void CreateColumnWidth(uint startIndex, uint? endIndex = null, double? width = null)
        {
            // Find the columns in the worksheet and remove them all
            if (currentWorkSheet.Where(x => x.LocalName == "cols").Count() > 0)
                currentWorkSheet.RemoveChild<Columns>(_cols);

            // Create the column
            Column column = new Column();
            column.Min = startIndex;
            column.Max = endIndex == null ? startIndex : endIndex;
            column.Width = width == null ? 20 : width;
            column.CustomWidth = true;
            _cols.Append(column); // Add it to the list of columns

            // Make sure that the column info is inserted *before* the sheetdata
            currentWorkSheet.InsertBefore<Columns>(_cols, currentWorkSheet.Where(x => x.LocalName == "sheetData").First());
            currentWorkSheet.Save();
            spreadSheet.WorkbookPart.Workbook.Save();

        }

        /// <summary>
        /// Close the spreadsheet
        /// </summary>
        public void CloseSpreadsheet()
        {
            spreadSheet.Close();
        }

        /// <summary>
        /// Pass a list of column headings to create the header row
        /// </summary>
        /// <param name="headers"></param>
        public void AddHeader(List<string> headers)
        {
            // Find the sheetdata of the worksheet
            SheetData sd = (SheetData)currentWorkSheet.Where(x => x.LocalName == "sheetData").First();
            Row header = new Row();
            // increment the row index to the next row
            header.RowIndex = Convert.ToUInt32(sd.ChildElements.Count()) + 1;
            sd.Append(header); // Add the header row

            foreach (string heading in headers)
            {
                AppendCell(header, header.RowIndex, heading, 1);
            }

            // save worksheet
            currentWorkSheet.Save();
        }


        /// <summary>
        /// Pass a list of data items to create a data row
        /// </summary>
        /// <param name="dataItems"></param>
        public void AddRow(List<string> dataItems)
        {
            // Find the sheetdata of the worksheet
            SheetData sd = (SheetData)currentWorkSheet.Where(x => x.LocalName == "sheetData").First();
            Row header = new Row();
            // increment the row index to the next row
            header.RowIndex = Convert.ToUInt32(sd.ChildElements.Count()) + 1;

            sd.Append(header);
            foreach (string item in dataItems)
            {
                AppendCell(header, header.RowIndex, item, 0);
            }

            // save worksheet
            currentWorkSheet.Save();
        }

        /// <summary>
        /// Add cell into the passed row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="styleIndex"></param>
        private void AppendCell(Row row, uint rowIndex, string value, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.InlineString;
            cell.StyleIndex = styleIndex;  // Style index comes from stylesheet generated in GenerateStyleSheet()
            Text t = new Text();
            t.Text = value;

            // Append Text to InlineString object
            InlineString inlineString = new InlineString();
            inlineString.AppendChild(t);

            // Append InlineString to Cell
            cell.AppendChild(inlineString);

            // Get the last cell's column
            string nextCol = "A";
            Cell c = (Cell)row.LastChild;
            if (c != null) // if there are some cells already there...
            {
                int numIndex = c.CellReference.ToString().IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                // Get the last column reference
                string lastCol = c.CellReference.ToString().Substring(0, numIndex);
                // Increment
                nextCol = IncrementColRef(lastCol);
            }

            cell.CellReference = nextCol + rowIndex;
            row.AppendChild(cell);
        }

        // Increment the column reference in an Excel fashion, i.e. A, B, C...Z, AA, AB etc.
        // Partly stolen from somewhere on the Net and modified for my use.
        private string IncrementColRef(string lastRef)
        {
            char[] characters = lastRef.ToUpperInvariant().ToCharArray();
            int sum = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                sum *= 26;
                sum += (characters[i] - 'A' + 1);
            }

            sum++;

            string columnName = String.Empty;
            int modulo;

            while (sum > 0)
            {
                modulo = (sum - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                sum = (int)((sum - modulo) / 26);
            }

            return columnName;
        }

        /// <summary>
        /// Generate style sheet
        /// </summary>
        /// <returns></returns>
        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Italic font.
                        new Italic(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 - The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 - Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 - Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 - Yellow Fill
                    new CellFormat(                                                                   // Index 5 - Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    )
                    { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 - Border
                )
            );
        }
    }
}

