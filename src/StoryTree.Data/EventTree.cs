﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;
using StoryTree.Data.Tree;

namespace StoryTree.Data
{
    public class EventTree : INotifyPropertyChanged
    {
        public string Description { get; set; }

        public TreeEvent MainTreeEvent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}