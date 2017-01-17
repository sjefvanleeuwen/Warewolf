﻿using System.Collections.Generic;
using System.Windows.Input;
// ReSharper disable InconsistentNaming

namespace Dev2.Common.Interfaces.ToolBase
{
    public interface IActionToolRegion<T> : IToolRegion
    {
        T SelectedAction { get; set; }
        ICollection<T> Actions { get; set; }
        ICommand RefreshActionsCommand { get; }
        bool IsActionEnabled { get; set; }
        bool IsRefreshing { get; set; }
        event SomethingChanged SomethingChanged;
        double LabelWidth { get; set; }
    }

    public interface IODBCActionToolRegion<T> : IActionToolRegion<T>
    {
        string CommandText { get; set; }
    }
}