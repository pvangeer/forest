using System;
using System.ComponentModel;
using System.Reflection;
using Forest.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class DoubleUpDownPropertyValueTreeNodeViewModel<TContent> : PropertyValueTreeNodeViewModelBase,
        IDoubleUpDownPropertyTreeNodeViewModel where TContent : Entity
    {
        private readonly PropertyInfo propertyInfo;
        private TContent content;

        public DoubleUpDownPropertyValueTreeNodeViewModel(TContent content, string propertyName, string displayName,
            double minValue, double maxValue, double increment, string stringFormat)
            : base(displayName)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Increment = increment;
            StringFormat = stringFormat;

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
                OnPropertyChanged(nameof(DoubleValue));
            }
        }

        public double DoubleValue
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

        public double Increment { get; }

        public string StringFormat { get; }

        public override bool IsViewModelFor(object o)
        {
            return ReferenceEquals(o, content);
        }

        private void ContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyInfo.Name) OnPropertyChanged(nameof(DoubleValue));
        }
    }
}