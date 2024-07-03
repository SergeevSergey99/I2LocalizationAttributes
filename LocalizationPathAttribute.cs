using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class LocalizationPathAttribute : PropertyAttribute
{
    public string Path { get; private set; }

    public LocalizationPathAttribute(string path = "")
    {
        Path = path;
    }
}