using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbBroker.Model;

internal class ViewSplitOnItem
{
    public Type Type { get; set; }

    public string SplitOnColumn { get; set; }

    public bool IsCollection { get; set; }

    public int Index { get; set; }

    public PropertyInfo PropertyInfo { get; set; }

    public Dictionary<object, object> MapBuffer { get; set; } = new Dictionary<object, object>(comparer: new DataModelKeyComparer());

    public object TransientRef { get; set; }
}
