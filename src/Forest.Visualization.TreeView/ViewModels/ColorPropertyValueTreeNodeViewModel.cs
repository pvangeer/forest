﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media;
using Forest.Data;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class ColorPropertyValueTreeNodeViewModel<TContent> : PropertyValueTreeNodeViewModelBase,
        IColorPropertyTreeNodeViewModel where TContent : Entity
    {
        private readonly PropertyInfo propertyInfo;
        private TContent content;

        public ColorPropertyValueTreeNodeViewModel(TContent content, string propertyName, string displayName) : base(
            displayName)
        {
            Content = content;

            propertyInfo = typeof(TContent).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(Color)) throw new ArgumentException();

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
                OnPropertyChanged(nameof(ColorValue));
            }
        }

        public Color ColorValue
        {
            get => Content != null ? (Color)propertyInfo.GetValue(Content) : Colors.Black;
            set
            {
                if (propertyInfo.CanWrite && Content != null)
                {
                    propertyInfo.SetValue(Content, value, null);
                    Content.OnPropertyChanged(propertyInfo.Name);
                }
            }
        }

        public override bool IsViewModelFor(object o)
        {
            return ReferenceEquals(o, Content);
        }

        private void ContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyInfo.Name) OnPropertyChanged(nameof(ColorValue));
        }
    }
}