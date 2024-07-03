using UnityEngine;

public class LocalizedTextWithCategory : MonoBehaviour
{
    [LocalizationCategory("UI")]
    public LocalizedCategory category;
    [LocalizationPath("$category")]
    public I2.Loc.LocalizedString localizedString;
    
}