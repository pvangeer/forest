using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using BlipFill = DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill;
using NonVisualDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties;
using NonVisualPictureDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties;
using NonVisualPictureProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties;
using Position = DocumentFormat.OpenXml.Drawing.Spreadsheet.Position;
using ShapeProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties;

namespace StoryTree.IO
{
    public class ExpertFormWriter
    {
        private int rowNumber = 1;
        private string expertElicitationCodeCellRange = "$C$12:$C$18";

        public void WriteForm(string fileName, string eventName, string eventImageFileName, string expertName, DateTime date, double[] waterLevels, double[] frequencies, string[] eventNodes)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                rowNumber = 1;

                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();

                // Add stylesheet
                WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                workbookStylesPart.Stylesheet = StyleSheetLibrary.StyleSheet;
                workbookStylesPart.Stylesheet.Save(workbookStylesPart);
                
                //Add data to first sheet
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                Worksheet worksheet = new Worksheet();
                worksheetPart.Worksheet = worksheet;
                Columns columns = new Columns();
                columns.Append(new Column { Min = 1, Max = 2, Width = 4, CustomWidth = true });
                columns.Append(new Column { Min = 3, Max = 7, Width = 12, CustomWidth = true });
                columns.Append(new Column { Min = 8, Max = 8, Width = 40, CustomWidth = true });
                columns.Append(new Column { Min = 9, Max = 9, Width = 20, CustomWidth = true });
                columns.Append(new Column { Min = 10, Max = 11, Width = 4, CustomWidth = true });
                worksheetPart.Worksheet.Append(columns);
                
                MergeCells mergeCells = new MergeCells();
                var sheetData = new SheetData();
                var dataValidations = new DataValidations();
                worksheet.Append(sheetData);
                worksheet.Append(dataValidations);
                Sheets sheets = new Sheets();

                Sheet sheet1 = new Sheet
                {
                    Name = "Formulier",
                    SheetId = 1,
                    Id = workbookPart.GetIdOfPart(worksheetPart)
                };

                // Write header
                sheetData.Append(new Row());
                rowNumber++;
                AddRow(sheetData, StyleSheetLibrary.TitleStyleIndex,
                    ConstructCell("DOT Formulier", CellValues.String, StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex));
                mergeCells.Append(new MergeCell{ Reference = new StringValue("C2:I2") });
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell("DOT = Deskundigen Oordeel Toets op Maat", CellValues.String, StyleSheetLibrary.DefaultStyleIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);

                // Write eventname
                AddRow(sheetData,0,
                    ConstructCell(string.Format("Gebeurtenis 3: {0}", eventName), CellValues.String, StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                    EmptyCell(StyleSheetLibrary.TitleStyleIndex), 
                    EmptyCell(StyleSheetLibrary.DefaultStyleIndex));
                mergeCells.Append(new MergeCell { Reference = new StringValue("C5:I5") });
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);

                // Write expert information
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell("Expert:", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell(expertName, CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));

                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell("Datum:", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    new Cell
                    {
                        CellValue = new CellValue(date.ToOADate().ToString(CultureInfo.InvariantCulture)),
                        StyleIndex = StyleSheetLibrary.DateTimeBodyStyle
                    });
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);

                // Write list of codes
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell("Code", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Faalkans", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Omschrijving", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(1, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell(0.999, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("999/1000", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("Virtually certain", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(2, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell(0.99, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("99/100", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("Very Likely", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(3, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell(0.9, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("9/10", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("Likely", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(StyleSheetLibrary.TableHeaderStyleIndex, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell(0.5, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("5/10", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("Neutral", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(5, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell(0.1, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("1/10", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("Unlikely", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(6, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell(0.01, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("1/100", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("Very Unlikely", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                    ConstructCell(7, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell(0.001, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("1/1000", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("Virtually Impossible", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                    ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
                AddRow(sheetData,0);
                AddRow(sheetData,0);

                //append a MergeCell to the mergeCells for each set of merged cells
                mergeCells.Append(new MergeCell {Reference = new StringValue("F11:G11")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F12:G12")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F13:G13")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F14:G14")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F15:G15")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F16:G16")});
                mergeCells.Append(new MergeCell {Reference = new StringValue("F17:G17")});
                mergeCells.Append(new MergeCell { Reference = new StringValue("F18:G18") });

                worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());

                // Include image
                AddImage(eventImageFileName, worksheetPart);

                // Write table parts
                foreach (var eventNode in eventNodes)
                {
                    AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);
                    // TODO: merge all cells above table
                    AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                        ConstructCell(eventNode, CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));
                    AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                        ConstructCell("Waterstand", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Frequentie", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Onder", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Gemiddeld", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Boven", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Weergave", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                        ConstructCell("Toelichting", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));

                    var styleIndex = StyleSheetLibrary.TableBodyStyleNormalIndex;
                    for (var index = 0; index < frequencies.Length; index++)
                    {
                        var frequency = frequencies[index];
                        var waterLevel = waterLevels[index];

                        AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex,
                            ConstructCell(waterLevel, CellValues.Number, styleIndex),
                            ConstructCell(frequency, CellValues.Number, styleIndex),
                            ConstructCell(double.NaN, CellValues.Number, styleIndex),
                            ConstructCell(double.NaN, CellValues.Number, styleIndex),
                            ConstructCell(double.NaN, CellValues.Number, styleIndex),
                            ConstructCell(double.NaN, CellValues.Number, styleIndex),
                            ConstructCell("", CellValues.String, styleIndex));
                        
                        dataValidations.Append(new DataValidation
                        {
                            AllowBlank = true,
                            Type = DataValidationValues.List,
                            Formula1 = new Formula1(expertElicitationCodeCellRange),
                            SequenceOfReferences = new ListValue<StringValue>
                            {
                                InnerText = "E" + (rowNumber - 1)
                            }
                        });
                        dataValidations.Append(new DataValidation
                        {
                            AllowBlank = true,
                            Type = DataValidationValues.List,
                            Formula1 = new Formula1(expertElicitationCodeCellRange),
                            SequenceOfReferences = new ListValue<StringValue>
                            {
                                InnerText = "F" + (rowNumber - 1)
                            }
                        });
                        dataValidations.Append(new DataValidation
                        {
                            AllowBlank = true,
                            Type = DataValidationValues.List,
                            Formula1 = new Formula1(expertElicitationCodeCellRange),
                            SequenceOfReferences = new ListValue<StringValue>
                            {
                                InnerText = "G" + (rowNumber-1)
                            }
                        });

                        styleIndex = styleIndex == StyleSheetLibrary.TableBodyStyleNormalIndex
                            ? StyleSheetLibrary.TableAlternateBodyStyleIndex
                            : StyleSheetLibrary.TableBodyStyleNormalIndex;
                    }
                }

                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex);

                var row = new Row(EmptyCell(StyleSheetLibrary.DefaultStyleIndex),
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex), 
                    EmptyCell(StyleSheetLibrary.TopBorderStyleIndex));
                sheetData.Append(row);
                rowNumber++;

                var sheet = sheet1;

                sheets.Append(sheet);

                Workbook workbook = new Workbook();
                workbook.Append(new FileVersion { ApplicationName = "Microsoft Office Excel" });
                workbook.Append(sheets);

                // Save document
                worksheetPart.Worksheet.Save();
                workbookPart.Workbook = workbook;
                workbookPart.Workbook.Save();
                spreadsheetDocument.Close();
            }
        }

        private static Cell EmptyCell(uint styleSheetIndex)
        {
            return new Cell {StyleIndex = styleSheetIndex};
        }

        private void AddRow(SheetData sheetData, uint styleIndex, params Cell[] cells)
        {
            while (cells.Length < 8)
            {
                cells = cells.Concat(new[] {EmptyCell(StyleSheetLibrary.DefaultStyleIndex) }).ToArray();
            }
            
            var row = new Row();
            row.Append(new[]{EmptyCell(StyleSheetLibrary.RightBorderStyleIndex), EmptyCell(styleIndex)}.Concat(cells).Concat(new []{EmptyCell(StyleSheetLibrary.LeftBorderStyleIndex) }));
            sheetData.Append(row);
            rowNumber++;
        }

        private static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
        private static Cell ConstructCell(double value, CellValues dataType, uint styleIndex = 0)
        {
            return double.IsNaN(value)
                ? new Cell
                {
                    DataType = dataType,
                    StyleIndex = styleIndex
                }
                : new Cell
                {
                    CellValue = new CellValue(value.ToString(CultureInfo.InvariantCulture)),
                    DataType = dataType,
                    StyleIndex = styleIndex
                };
        }

        private static void AddImage(string imageFileName, WorksheetPart worksheetPart)
        {
            DrawingsPart drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
            ImagePart imagePart = drawingsPart.AddImagePart(ImagePartType.Png, worksheetPart.GetIdOfPart(drawingsPart));
            using (FileStream fileStream = new FileStream(imageFileName, FileMode.Open))
            {
                imagePart.FeedData(fileStream);
            }

            NonVisualDrawingProperties nonVisualDrawingProperties = new NonVisualDrawingProperties
            {
                Id = 1025,
                Name = "Picture 1",
                Description = "polymathlogo"
            };
            PictureLocks pictureLocks =
                new PictureLocks
                {
                    NoChangeAspect = true,
                    NoChangeArrowheads = true
                };
            NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties = new NonVisualPictureDrawingProperties { PictureLocks = pictureLocks };
            NonVisualPictureProperties pictureNonVisualPictureProperties =
                new NonVisualPictureProperties
                {
                    NonVisualDrawingProperties = nonVisualDrawingProperties,
                    NonVisualPictureDrawingProperties = nonVisualPictureDrawingProperties
                };

            Stretch stretch =
                new Stretch
                {
                    FillRectangle = new FillRectangle()
                };

            BlipFill blipFill = new BlipFill
            {
                Blip = new Blip
                {
                    Embed = drawingsPart.GetIdOfPart(imagePart),
                    CompressionState = BlipCompressionValues.Print
                },
                SourceRectangle = new SourceRectangle()
            };
            blipFill.Append(stretch);

            Offset offset = new Offset
            {
                X = 0,
                Y = 0
            };
            Transform2D transform2D =
                new Transform2D { Offset = offset };
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imageFileName);
            //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
            //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
            //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
            Extents extents = new Extents
            {
                Cx = (long) bitmap.Width * (long) ((float) 914400 / bitmap.HorizontalResolution),
                Cy = (long) bitmap.Height * (long) ((float) 914400 / bitmap.VerticalResolution)
            };
            bitmap.Dispose();
            transform2D.Extents = extents;
            ShapeProperties shapeProperties =
                new ShapeProperties
                {
                    BlackWhiteMode = BlackWhiteModeValues.Auto,
                    Transform2D = transform2D
                };
            PresetGeometry prstGeom =
                new PresetGeometry
                {
                    Preset = ShapeTypeValues.Rectangle,
                    AdjustValueList = new AdjustValueList()
                };
            shapeProperties.Append(prstGeom);
            shapeProperties.Append(new NoFill());

            DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture =
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture
                {
                    NonVisualPictureProperties = pictureNonVisualPictureProperties,
                    BlipFill = blipFill,
                    ShapeProperties = shapeProperties
                };

            Position pos = new Position
            {
                X = 7000000,
                Y = 200000
            };
            Extent ext = new Extent
            {
                Cx = extents.Cx,
                Cy = extents.Cy
            };
            var iColumnId = 7;
            var iRowId = 5;
            var lColumnOffset = 0;
            var lRowOffset = 0;
            OneCellAnchor ocanchor = new OneCellAnchor();
            ocanchor.FromMarker = new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker();
            // Subtract 1 because picture goes to bottom right corner
            // Subtracting 1 makes it more intuitive that (1,1) means top-left corner of (1,1)
            ocanchor.FromMarker.ColumnId = new ColumnId() { Text = iColumnId.ToString(CultureInfo.InvariantCulture) };
            ocanchor.FromMarker.ColumnOffset = new ColumnOffset() { Text = lColumnOffset.ToString(CultureInfo.InvariantCulture) };
            ocanchor.FromMarker.RowId = new RowId() { Text = iRowId.ToString(CultureInfo.InvariantCulture) };
            ocanchor.FromMarker.RowOffset = new RowOffset() { Text = lRowOffset.ToString(CultureInfo.InvariantCulture) };

            ocanchor.Extent = ext;

            ocanchor.Append(picture);
            ocanchor.Append(new ClientData());
            

           /* AbsoluteAnchor anchor = new AbsoluteAnchor
            {
                Position = pos,
                Extent = ext
            };
            anchor.Append(picture);
            anchor.Append(new ClientData());*/
            WorksheetDrawing worksheetDrawing = new WorksheetDrawing();
            //worksheetDrawing.Append(anchor);
            worksheetDrawing.Append(ocanchor);
            Drawing drawing = new Drawing { Id = drawingsPart.GetIdOfPart(imagePart) };

            worksheetDrawing.Save(drawingsPart);

            worksheetPart.Worksheet.Append(drawing);
        }
    }
}

