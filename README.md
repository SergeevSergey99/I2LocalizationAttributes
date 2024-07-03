# I2LocalizationModifications

This repository contains custom modifications and utilities for the [I2 Localization plugin](https://assetstore.unity.com/packages/tools/localization/i2-localization-14884) in Unity. It provides attributes and property drawers for easier selection and management of localization terms and categories within the Unity editor.

## Features

- **LocalizationPathAttribute**: An attribute for specifying a localization path.
- **LocalizationCategoryAttribute**: An attribute for specifying a localization category.

## Installation

1. Clone or download this repository.
2. Add the files to your Unity project.

## Usage

### LocalizationPathAttribute

Use the `LocalizationPathAttribute` to specify a localization path for a property.

```csharp
using I2.Loc;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [LocalizationPath("UI/Text")]
    public LocalizedString localizedString;
}
```
My Terms looks like this:
![My Terms](Images/Terms.png)