using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = System.Drawing.Color;

namespace Forest.IO
{
    public static class StyleSheetLibrary
    {
        public const uint DefaultStyleIndex = 0;
        public const uint TitleStyleIndex = 1;
        public const uint TableBodyStyleNormalIndex = 2;
        public const uint TableAlternateBodyStyleIndex = 3;
        public const uint TableHeaderStyleIndex = 4;
        public const uint DateTimeBodyStyle = 5;
        public const uint RightBorderStyleIndex = 6;
        public const uint LeftBorderStyleIndex = 7;
        public const uint TopBorderStyleIndex = 8;
        public static readonly Color RowColor = Color.AliceBlue;
        public static readonly Color AlternatingRowColor = Color.LightSteelBlue;
        public static readonly Color TitleColor = Color.LightSteelBlue;
        public static readonly Color BorderColor = Color.LightGray;

        public static Stylesheet StyleSheet
        {
            get
            {
                var fonts = new Fonts(
                    CreateFont(10), // 0 - default
                    CreateFont(10, true), // 1 - header
                    CreateFont(16, true) // 2 - title
                );

                var fills = new Fills(
                    new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }, // required, reserved by Excel
                    new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }, // required, reserved by Excel
                    new Fill(new PatternFill(CreateForegroundColor(AlternatingRowColor))
                        { PatternType = PatternValues.Solid }), // 2 - alternating row
                    new Fill(new PatternFill(CreateForegroundColor(RowColor))
                        { PatternType = PatternValues.Solid }), // Index 3 - header row and main row
                    new Fill(new PatternFill(CreateForegroundColor(TitleColor)) { PatternType = PatternValues.Solid }) // Index 4 - Title
                );

                var borders = new Borders(
                    new Border(), // index 0 default - no border
                    new Border( // index 1 black border
                        new LeftBorder(new DocumentFormat.OpenXml.Spreadsheet.Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new DocumentFormat.OpenXml.Spreadsheet.Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new DocumentFormat.OpenXml.Spreadsheet.Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new DocumentFormat.OpenXml.Spreadsheet.Color { Auto = true }) { Style = BorderStyleValues.Thin }),
                    new Border( // index 2 header border
                        new LeftBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
                            { Rgb = new HexBinaryValue { Value = BorderColor.ToHexValue() } }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
                            { Rgb = new HexBinaryValue { Value = BorderColor.ToHexValue() } }) { Style = BorderStyleValues.Thin }),
                    new Border( // index 3 Rightborder
                        new RightBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
                            { Rgb = new HexBinaryValue { Value = BorderColor.ToHexValue() } }) { Style = BorderStyleValues.Thick }),
                    new Border( // index 4 Leftborder
                        new LeftBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
                            { Rgb = new HexBinaryValue { Value = BorderColor.ToHexValue() } }) { Style = BorderStyleValues.Thick }),
                    new Border( // index 5 Topborder
                        new TopBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
                            { Rgb = new HexBinaryValue { Value = BorderColor.ToHexValue() } }) { Style = BorderStyleValues.Thick })
                );

                var cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 2, FillId = 4, BorderId = 5, ApplyFont = true }, // title
                    new CellFormat { FontId = 0, FillId = 3, BorderId = 2, ApplyBorder = true }, // body
                    new CellFormat { FontId = 0, FillId = 2, BorderId = 2, ApplyBorder = true }, // alternate body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 2, ApplyBorder = true }, // header
                    new CellFormat(new Alignment { Horizontal = HorizontalAlignmentValues.Left })
                    {
                        BorderId = 2,
                        FillId = 3,
                        FontId = 0,
                        NumberFormatId = 14,
                        FormatId = 0,
                        ApplyNumberFormat = true
                    }, // Date in header style
                    new CellFormat { FillId = 0, BorderId = 3 }, // right border id
                    new CellFormat { FillId = 0, BorderId = 4 }, // left border id
                    new CellFormat { FillId = 0, BorderId = 5 } // top border id
                );

                return new Stylesheet(fonts, fills, borders, cellFormats);
            }
        }

        private static ForegroundColor CreateForegroundColor(Color color)
        {
            var ligthGrayForegroundColor = new ForegroundColor
            {
                Rgb = new HexBinaryValue { Value = color.ToHexValue() }
            };
            return ligthGrayForegroundColor;
        }

        private static Font CreateFont(DoubleValue fontSize, bool bold = false)
        {
            return bold
                ? new Font(new FontSize { Val = fontSize }, new FontName { Val = "Verdana" }, new Bold())
                : new Font(new FontSize { Val = fontSize }, new FontName { Val = "Verdana" });
        }
    }
}