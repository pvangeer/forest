using System.Windows;
using System.Windows.Controls;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.TreeView.DataTemplates
{
    public class PropertyValueTreeNodeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LineStyleTemplate { get; set; }

        public DataTemplate FontFamilySelectorTemplate { get; set; }

        public DataTemplate StringTemplate { get; set; }

        public DataTemplate CheckBoxTemplate { get; set; }

        public DataTemplate ColorTemplate { get; set; }

        public DataTemplate DoubleUpDownTemplate { get; set; }

        public DataTemplate SliderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IDoubleUpDownPropertyTreeNodeViewModel) return DoubleUpDownTemplate;

            if (item is ISliderPropertyTreeNodeViewModel) return SliderTemplate;

            if (item is IColorPropertyTreeNodeViewModel) return ColorTemplate;

            if (item is IStringPropertyTreeNodeViewModel) return StringTemplate;

            if (item is IBooleanPropertyTreeNodeViewModel) return CheckBoxTemplate;

            if (item is IFontFamilyPropertyTreeNodeViewModel) return FontFamilySelectorTemplate;

            if (item is ILineStylePropertyTreeNodeViewModel) return LineStyleTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}