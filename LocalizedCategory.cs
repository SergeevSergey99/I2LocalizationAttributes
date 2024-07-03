using System;
using UnityEngine;

[Serializable]
public class LocalizedCategory
{
    [SerializeField]
    private string category;

    public string Category
    {
        get => category;
        set => category = value;
    }

    public override string ToString()
    {
        return category;
    }
}