using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using StoryTree.Messaging;

namespace StoryTree.IO.Import
{
    public static class ElicitationFormReader
    {
        private static readonly StoryTreeLog Log = new StoryTreeLog(typeof(ElicitationFormReader));

        public static IEnumerable<DotForm> ReadElicitationForm(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Log.Error($"Bestandsnaam '{fileName}' kon niet worden gevonden. Importeren is afgebroken.");
                return null;
            }

            var forms = new List<DotForm>();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                foreach (var worksheetPart in workbookPart.WorksheetParts)
                {
                    forms.Add(ReadWorkSheet(worksheetPart.Worksheet, workbookPart));
                }
            }

            return forms;
        }

        private static DotForm ReadWorkSheet(Worksheet worksheet, WorkbookPart workbookPart)
        {
            var nodes = new List<DotNode>();
            uint firstNodeRow = 22;
            var maxRow = worksheet.Descendants<Row>().Max(r => r.RowIndex.Value);

            string nodeName = null;
            List<DotEstimate> estimates = new List<DotEstimate>();

            for (uint iRow = firstNodeRow; iRow < maxRow+1; iRow++)
            {

                var row = GetRow(worksheet, iRow);
                if (row == null)
                {
                    nodes.Add(new DotNode
                    {
                        NodeName = nodeName,
                        Estimates = estimates.ToArray()
                    });

                    nodeName = null;
                    estimates = new List<DotEstimate>();
                    continue;
                }

                var Ccell = row.Descendants<Cell>().FirstOrDefault(c => c.CellReference == "C" + iRow.ToString(CultureInfo.InvariantCulture));
                if (Ccell == null)
                {
                    nodes.Add(new DotNode
                    {
                        NodeName = nodeName,
                        Estimates = estimates.ToArray()
                    });

                    nodeName = null;
                    estimates = new List<DotEstimate>();
                    continue;
                }

                if (Ccell.DataType != null && Ccell.DataType == CellValues.SharedString)
                {
                    if (string.IsNullOrWhiteSpace(nodeName))
                    {
                        nodeName = CellValueAsStringFromCell(Ccell, workbookPart);
                    }
                    continue;
                }

                estimates.Add(new DotEstimate
                {
                    WaterLevel = GetCellValueAsDoubleFromCell(Ccell, workbookPart),
                    Frequency = GetCellValueAsDouble(worksheet, "D" + iRow.ToString(CultureInfo.InvariantCulture), workbookPart),
                    LowerEstimate = GetCellValueAsInt(worksheet, "E" + iRow.ToString(CultureInfo.InvariantCulture), workbookPart),
                    BestEstimate = GetCellValueAsInt(worksheet, "F" + iRow.ToString(CultureInfo.InvariantCulture), workbookPart),
                    UpperEstimate = GetCellValueAsInt(worksheet, "G" + iRow.ToString(CultureInfo.InvariantCulture), workbookPart),
                    Comment = GetCellValueAsString(worksheet, "I" + iRow.ToString(CultureInfo.InvariantCulture), workbookPart)
                });
            }

            return new DotForm
            {
                EventTreeName = GetCellValueAsString(worksheet, "C5", workbookPart),
                ExpertName = GetCellValueAsString(worksheet, "D7", workbookPart),
                Date = GetCellValueAsDateTime(worksheet,"D8"),
                Nodes = nodes.ToArray()
            };
        }

        private static double GetCellValueAsDoubleFromCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = CellValueAsStringFromCell(cell, workbookPart);
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return double.NaN;
            }

            if (!double.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var cellValueAsDouble))
            {
                return double.NaN;
            }

            return cellValueAsDouble;
        }

        private static double GetCellValueAsDouble(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return double.NaN;
            }

            if (!double.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var cellValueAsDouble))
            {
                return double.NaN;
            }

            return cellValueAsDouble;
        }

        private static int GetCellValueAsInt(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return default(int);
            }

            if (!int.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var cellValueAsInt))
            {
                return default(int);
            }

            return cellValueAsInt;
        }

        private static Row GetRow(Worksheet worksheet, uint rowNumber)
        {
            return worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex == rowNumber);
        }

        private static Cell GetCell(Worksheet worksheet, string addressName)
        {
            return worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == addressName);
        }

        private static string GetCellValueAsString(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cell = GetCell(worksheet, cellReference);
            if (cell == null)
            {
                return "";
            }

            return CellValueAsStringFromCell(cell, workbookPart);
        }

        private static string CellValueAsStringFromCell(Cell cell, WorkbookPart workbookPart)
        {
            string cellValue = string.Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString && workbookPart != null)
            {
                if (Int32.TryParse(cell.InnerText, out var id))
                {
                    SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                    if (item.Text != null)
                    {
                        cellValue = item.Text.Text;
                    }
                    else if (item.InnerText != null)
                    {
                        cellValue = item.InnerText;
                    }
                    else if (item.InnerXml != null)
                    {
                        cellValue = item.InnerXml;
                    }
                }
            }
            else
            {
                cellValue = cell.InnerText;
            }

            return cellValue;
        }

        private static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        private static DateTime GetCellValueAsDateTime(Worksheet worksheet, string cellReference)
        {
            var cellValueAsString = GetCellValueAsString(worksheet,cellReference, null);
            if (string.IsNullOrWhiteSpace(cellValueAsString))
            {
                return default(DateTime);
            }

            if (!double.TryParse(cellValueAsString, NumberStyles.Any, CultureInfo.InvariantCulture, out double cellValueAsDouble))
            {
                return default(DateTime);
            }

            if (cellValueAsDouble < 0)
            {
                return default(DateTime);
            }

            return DateTime.FromOADate(cellValueAsDouble);
        }

        private static Worksheet GetWorksheet(WorkbookPart wbPart, string sheetName)
        {
            Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

            if (theSheet == null)
            {
                throw new ArgumentException("sheetName");
            }

            var wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);

            return wsPart.Worksheet;
        }
    }
}
