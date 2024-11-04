using System;
using System.ComponentModel;
using System.Reflection;
using Forest.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class BooleanPropertyValueTreeNodeViewModel<TContent> : PropertyValueTreeNodeViewModelBase,
        IBooleanPropertyTreeNodeViewModel where TContent : Entity
    {
        private readonly PropertyInfo propertyInfo;
        private TContent content;

        public BooleanPropertyValueTreeNodeViewModel(TContent content, string propertyName, string displayName) : base(
            displayName)
        {
            this.content = content;
            propertyInfo = typeof(TContent).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(bool)) throw new ArgumentException();

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
                OnPropertyChanged(nameof(BooleanValue));
            }
        }

        public bool BooleanValue
        {
            get => content != null && (bool)propertyInfo.GetValue(content);
            set
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(content, value, null);
                    content.OnPropertyChanged(propertyInfo.Name);
                }
            }
        }

        private void ContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyInfo.Name) OnPropertyChanged(nameof(BooleanValue));
        }

        public override bool IsViewModelFor(object o)
        {
            return ReferenceEquals(o, content);
        }
    }
}