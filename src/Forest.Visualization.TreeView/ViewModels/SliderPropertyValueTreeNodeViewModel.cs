using System;
using System.ComponentModel;
using System.Reflection;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class SliderPropertyValueTreeNodeViewModel<TContent> : PropertyValueTreeNodeViewModelBase,
        ISliderPropertyTreeNodeViewModel where TContent : INotifyPropertyChangedImplementation
    {
        private readonly PropertyInfo propertyInfo;
        private TContent content;

        public SliderPropertyValueTreeNodeViewModel(TContent content, string propertyName, string displayName,
            double minValue, double maxValue)
            : base(displayName)
        {
            MinValue = minValue;
            MaxValue = maxValue;

            propertyInfo = typeof(TContent)
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(double)) throw new ArgumentException();

            this.content = content;
            if (Content != null) Content.PropertyChanged += ContentPropertyChanged;
        }

        public TContent Content
        {
            get => content;
            set
            {
                if (content != null) content.PropertyChanged -= ContentPropertyChanged;
                content = value;
                if (content != null) content.PropertyChanged += ContentPropertyChanged;
                OnPropertyChanged(nameof(Value));
            }
        }

        public double Value
        {
            get => content != null ? (double)propertyInfo.GetValue(content) : double.NaN;
            set
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(content, value, null);
                    content.OnPropertyChanged(propertyInfo.Name);
                }
            }
        }

        public double MinValue { get; }

        public double MaxValue { get; }

        public override bool IsViewModelFor(object o)
        {
            return ReferenceEquals(o, content);
        }

        private void ContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyInfo.Name) OnPropertyChanged(nameof(Value));
        }
    }
}