﻿using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Forest.Data.Properties;
using Forest.IO.Import;
using BlipFill = DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill;
using Color = System.Drawing.Color;
using FromMarker = DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker;
using NonVisualDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties;
using NonVisualPictureDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties;
using NonVisualPictureProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties;
using Outline = DocumentFormat.OpenXml.Drawing.Outline;
using Picture = DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture;
using ShapeProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties;

namespace Forest.IO.Export
{
    public class ElicitationFormWriter
    {
        private readonly char[] columnHeaders = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly string elicitationCodeCellRange = "$C$12:$C$18";

        //public void WriteForm(string fileName, string eventName, string eventImageFileName, string expertName, DateTime date, double[] waterLevels, double[] frequencies, string[] eventNodes)
        public void WriteForm(string fileName, DotForm dotForm)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Append(new FileVersion { ApplicationName = "Microsoft Office Excel" });

                // Add stylesheet
                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                workbookStylesPart.Stylesheet = StyleSheetLibrary.StyleSheet;
                workbookStylesPart.Stylesheet.Save(workbookStylesPart);

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                WriteWorksheet(workbookPart, sheets, dotForm);

                // Save document
                workbookPart.Workbook.Save();
                spreadsheetDocument.Close();
            }
        }

        private void WriteWorksheet(WorkbookPart workbookPart, Sheets sheets, DotForm form)
        {
            //Add data to first sheet
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var worksheet = new Worksheet();
            worksheetPart.Worksheet = worksheet;
            var columns = new Columns();
            columns.Append(new Column { Min = 1, Max = 2, Width = 4, CustomWidth = true });
            columns.Append(new Column { Min = 3, Max = 7, Width = 12, CustomWidth = true });
            columns.Append(new Column { Min = 8, Max = 8, Width = 40, CustomWidth = true });
            columns.Append(new Column { Min = 9, Max = 9, Width = 20, CustomWidth = true });
            columns.Append(new Column { Min = 10, Max = 11, Width = 4, CustomWidth = true });

            var sheetData = new SheetData();
            var dataValidations = new DataValidations();

            worksheet.Append(columns);
            worksheet.Append(sheetData);
            var mergeCells = CreateOrGetMergeCells(worksheet);
            worksheet.Append(dataValidations);

            var sheet = new Sheet
            {
                Name = "Faalpad",
                SheetId = (uint)sheets.Count() + 1,
                Id = workbookPart.GetIdOfPart(worksheetPart)
            };
            sheets.Append(sheet);

            WriteHeader(sheetData, mergeCells);
            WriteEventHeader(form, sheetData, mergeCells);
            WriteExpertInformation(form, sheetData);
            WriteElicitationCodeInformation(sheetData, mergeCells);
            AddImage(form.GetFileStream, worksheetPart);
            var rowNumber = WriteNodes(form, sheetData, dataValidations);
            WriteSheetBottom(sheetData, rowNumber);


            worksheetPart.Worksheet.Save();
        }

        private void WriteSheetBottom(SheetData sheetData, uint rowNumber)
        {
            AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, rowNumber++);

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
        }

        private uint WriteNodes(DotForm form, SheetData sheetData, DataValidations dataValidations)
        {
            uint rowNumber = 21;
            // Write table parts
            foreach (var node in form.Nodes)
            {
                var estimates = node.Estimates.OrderBy(n => n.WaterLevel).ToArray();

                AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, rowNumber++);
                // TODO: merge all cells above table
                AddRow(sheetData,
                    StyleSheetLibrary.DefaultStyleIndex,
                    rowNumber++,
                    ConstructCell(node.NodeName, CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));
                AddRow(sheetData,
                    StyleSheetLibrary.DefaultStyleIndex,
                    rowNumber++,
                    ConstructCell("Waterstand", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Frequentie", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Onder", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Gemiddeld", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Boven", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Weergave", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                    ConstructCell("Toelichting", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));

                var styleIndex = StyleSheetLibrary.TableBodyStyleNormalIndex;
                foreach (var estimate in estimates)
                {
                    AddRow(sheetData,
                        StyleSheetLibrary.DefaultStyleIndex,
                        rowNumber++,
                        ConstructCell(estimate.WaterLevel, CellValues.Number, styleIndex),
                        ConstructCell(estimate.Frequency, CellValues.Number, styleIndex),
                        ConstructCell(estimate.LowerEstimate == 0 ? double.NaN : estimate.LowerEstimate,
                            CellValues.Number,
                            styleIndex),
                        ConstructCell(estimate.BestEstimate == 0 ? double.NaN : estimate.BestEstimate,
                            CellValues.Number,
                            styleIndex),
                        ConstructCell(estimate.UpperEstimate == 0 ? double.NaN : estimate.UpperEstimate,
                            CellValues.Number,
                            styleIndex),
                        ConstructCell(double.NaN, CellValues.Number, styleIndex),
                        ConstructCell("", CellValues.String, styleIndex));

                    dataValidations.Append(new DataValidation
                    {
                        AllowBlank = true,
                        Type = DataValidationValues.List,
                        Formula1 = new Formula1(elicitationCodeCellRange),
                        SequenceOfReferences = new ListValue<StringValue>
                        {
                            InnerText = "E" + (rowNumber - 1)
                        }
                    });
                    dataValidations.Append(new DataValidation
                    {
                        AllowBlank = true,
                        Type = DataValidationValues.List,
                        Formula1 = new Formula1(elicitationCodeCellRange),
                        SequenceOfReferences = new ListValue<StringValue>
                        {
                            InnerText = "F" + (rowNumber - 1)
                        }
                    });
                    dataValidations.Append(new DataValidation
                    {
                        AllowBlank = true,
                        Type = DataValidationValues.List,
                        Formula1 = new Formula1(elicitationCodeCellRange),
                        SequenceOfReferences = new ListValue<StringValue>
                        {
                            InnerText = "G" + (rowNumber - 1)
                        }
                    });

                    styleIndex = styleIndex == StyleSheetLibrary.TableBodyStyleNormalIndex
                        ? StyleSheetLibrary.TableAlternateBodyStyleIndex
                        : StyleSheetLibrary.TableBodyStyleNormalIndex;
                }
            }

            return rowNumber;
        }

        private void WriteElicitationCodeInformation(SheetData sheetData, MergeCells mergeCells)
        {
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                11,
                ConstructCell("Code", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                ConstructCell("Faalkans", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                ConstructCell("Omschrijving", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                12,
                ConstructCell(1, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell(0.999, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("999/1000", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("Virtually certain", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                13,
                ConstructCell(2, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell(0.99, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("99/100", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("Very Likely", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                14,
                ConstructCell(3, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell(0.9, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("9/10", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("Likely", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                15,
                ConstructCell(StyleSheetLibrary.TableHeaderStyleIndex,
                    CellValues.Number,
                    StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell(0.5, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("5/10", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("Neutral", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                16,
                ConstructCell(5, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell(0.1, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("1/10", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("Unlikely", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                17,
                ConstructCell(6, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell(0.01, CellValues.Number, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("1/100", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("Very Unlikely", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableAlternateBodyStyleIndex));
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                18,
                ConstructCell(7, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell(0.001, CellValues.Number, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("1/1000", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("Virtually Impossible", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex),
                ConstructCell("", CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));
            AddRow(sheetData, 0, 19);
            AddRow(sheetData, 0, 20);

            //append a MergeCell to the mergeCells for each set of merged cells
            mergeCells.Append(new MergeCell { Reference = new StringValue("F11:G11") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F12:G12") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F13:G13") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F14:G14") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F15:G15") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F16:G16") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F17:G17") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("F18:G18") });
        }

        private void WriteExpertInformation(DotForm form, SheetData sheetData)
        {
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                7,
                ConstructCell("Expert:", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                ConstructCell(form.ExpertName, CellValues.String, StyleSheetLibrary.TableBodyStyleNormalIndex));

            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                8,
                ConstructCell("Datum:", CellValues.String, StyleSheetLibrary.TableHeaderStyleIndex),
                new Cell
                {
                    CellValue = new CellValue(form.Date.ToOADate().ToString(CultureInfo.InvariantCulture)),
                    StyleIndex = StyleSheetLibrary.DateTimeBodyStyle
                });
            AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, 9);
            AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, 10);
        }

        private void WriteEventHeader(DotForm form, SheetData sheetData, MergeCells mergeCells)
        {
            AddRow(sheetData,
                0,
                5,
                ConstructCell("Gebeurtenis:", CellValues.String, StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                ConstructCell("Faalpad", CellValues.String, StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.DefaultStyleIndex));
            mergeCells.Append(new MergeCell { Reference = new StringValue("C5:D5") });
            mergeCells.Append(new MergeCell { Reference = new StringValue("E5:I5") });
            AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, 6);
        }

        private void WriteHeader(SheetData sheetData, MergeCells mergeCells)
        {
            sheetData.Append(new Row());
            AddRow(sheetData,
                StyleSheetLibrary.TitleStyleIndex,
                2,
                ConstructCell("DOT Formulier", CellValues.String, StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex),
                EmptyCell(StyleSheetLibrary.TitleStyleIndex));
            mergeCells.Append(new MergeCell { Reference = new StringValue("C2:I2") });
            AddRow(sheetData,
                StyleSheetLibrary.DefaultStyleIndex,
                3,
                ConstructCell("DOT = Deskundigen Oordeel Toets op Maat", CellValues.String));
            AddRow(sheetData, StyleSheetLibrary.DefaultStyleIndex, 4);
        }

        private static MergeCells CreateOrGetMergeCells([NotNull] Worksheet worksheet)
        {
            MergeCells mergeCells;
            if (worksheet.Elements<MergeCells>().Any())
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                else if (worksheet.Elements<DataConsolidate>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                else if (worksheet.Elements<SortState>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                else if (worksheet.Elements<AutoFilter>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                else if (worksheet.Elements<Scenarios>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                else if (worksheet.Elements<ProtectedRanges>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                else if (worksheet.Elements<SheetProtection>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                else if (worksheet.Elements<SheetCalculationProperties>().Any())
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                else
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
            }

            return mergeCells;
        }

        private static Cell EmptyCell(uint styleSheetIndex)
        {
            return new Cell { StyleIndex = styleSheetIndex };
        }

        private void AddRow(SheetData sheetData, uint styleIndex, uint rowNumber, params Cell[] cells)
        {
            while (cells.Length < 8)
            {
                cells = cells.Concat(new[] { EmptyCell(StyleSheetLibrary.DefaultStyleIndex) }).ToArray();
            }

            var cellsToWrite = new[] { EmptyCell(StyleSheetLibrary.RightBorderStyleIndex), EmptyCell(styleIndex) }
                .Concat(cells)
                .Concat(new[] { EmptyCell(StyleSheetLibrary.LeftBorderStyleIndex) })
                .ToArray();

            for (var i = 0; i < cellsToWrite.Length; i++)
                cellsToWrite[i].CellReference = columnHeaders[i] + rowNumber.ToString(CultureInfo.InvariantCulture);
            var row = new Row { RowIndex = rowNumber };
            row.Append(cellsToWrite);
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

        private static void AddImage(Func<FileStream> fileStreamFunc, WorksheetPart worksheetPart)
        {
            if (fileStreamFunc == null)
                return;

            DrawingsPart drawingsPart;
            ImagePart imagePart;
            Extents extents;
            using (var fileStream = fileStreamFunc())
            {
                if (fileStream == null)
                    return;

                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
                imagePart = drawingsPart.AddImagePart(ImagePartType.Png, worksheetPart.GetIdOfPart(drawingsPart));
                imagePart.FeedData(fileStream);

                var image = Image.FromStream(fileStream);
                //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
                //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
                //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
                extents = new Extents
                {
                    Cx = image.Width * (long)(914400 / image.HorizontalResolution),
                    Cy = image.Height * (long)(914400 / image.VerticalResolution)
                };
                image.Dispose();
            }

            var pictureNonVisualPictureProperties =
                new NonVisualPictureProperties
                {
                    NonVisualDrawingProperties = new NonVisualDrawingProperties
                    {
                        Id = 1025,
                        Name = "Picture 1",
                        Description = "eventtree"
                    },
                    NonVisualPictureDrawingProperties = new NonVisualPictureDrawingProperties
                    {
                        PictureLocks = new PictureLocks
                        {
                            NoChangeAspect = true,
                            NoChangeArrowheads = true
                        }
                    }
                };

            var stretch =
                new Stretch
                {
                    FillRectangle = new FillRectangle()
                };

            var blipFill = new BlipFill
            {
                Blip = new Blip
                {
                    Embed = drawingsPart.GetIdOfPart(imagePart),
                    CompressionState = BlipCompressionValues.Print
                },
                SourceRectangle = new SourceRectangle()
            };
            blipFill.Append(stretch);

            var shapeProperties =
                new ShapeProperties
                {
                    BlackWhiteMode = BlackWhiteModeValues.Auto,
                    Transform2D = new Transform2D
                    {
                        Offset = new Offset
                        {
                            X = 0,
                            Y = 0
                        },
                        Extents = extents
                    }
                };
            var prstGeom =
                new PresetGeometry
                {
                    Preset = ShapeTypeValues.Rectangle,
                    AdjustValueList = new AdjustValueList()
                };
            shapeProperties.Append(prstGeom);
            shapeProperties.Append(new SolidFill
            {
                RgbColorModelHex = new RgbColorModelHex
                {
                    Val = Color.White.ToSimpleHexValue()
                }
            });
            var outline = new Outline
            {
                Width = 25400
            };

            var solidFill1 = new SolidFill
            {
                RgbColorModelHex = new RgbColorModelHex
                {
                    Val = StyleSheetLibrary.BorderColor.ToSimpleHexValue()
                }
            };
            outline.Append(solidFill1);
            shapeProperties.Append(outline);

            var picture =
                new Picture
                {
                    NonVisualPictureProperties = pictureNonVisualPictureProperties,
                    BlipFill = blipFill,
                    ShapeProperties = shapeProperties
                };

            var iColumnId = 11;
            var iRowId = 1;
            var lColumnOffset = 0;
            var lRowOffset = 0;
            var ocanchor = new OneCellAnchor
            {
                FromMarker = new FromMarker
                {
                    ColumnId = new ColumnId { Text = iColumnId.ToString(CultureInfo.InvariantCulture) },
                    ColumnOffset = new ColumnOffset { Text = lColumnOffset.ToString(CultureInfo.InvariantCulture) },
                    RowId = new RowId { Text = iRowId.ToString(CultureInfo.InvariantCulture) },
                    RowOffset = new RowOffset { Text = lRowOffset.ToString(CultureInfo.InvariantCulture) }
                },
                Extent = new Extent { Cx = extents.Cx, Cy = extents.Cy }
            };

            ocanchor.Append(picture);
            ocanchor.Append(new ClientData());


            var worksheetDrawing = new WorksheetDrawing();
            worksheetDrawing.Append(ocanchor);
            var drawing = new Drawing { Id = drawingsPart.GetIdOfPart(imagePart) };

            worksheetDrawing.Save(drawingsPart);

            worksheetPart.Worksheet.Append(drawing);
        }
    }
}