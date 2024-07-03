using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class LocalizationCategoryAttribute : PropertyAttribute
{
    public string CategoryPath { get; private set; }

    public LocalizationCategoryAttribute(string categoryPath = "")
    {
        CategoryPath = categoryPath;
    }
}