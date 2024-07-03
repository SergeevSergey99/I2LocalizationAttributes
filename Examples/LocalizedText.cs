using I2.Loc;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [LocalizationPath("UI/Text/")]
    public I2.Loc.LocalizedString localizedString;
}