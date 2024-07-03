#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using I2.Loc;
using System.Linq;
using System.Collections.Generic;
using Localization.I2LocalizationModifications;

[CustomPropertyDrawer(typeof(LocalizationPathAttribute))]
public class LocalizationPathDrawer : PropertyDrawer
{
    private Dictionary<string, List<string>> categoryCache = new Dictionary<string, List<string>>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        LocalizationPathAttribute pathAttribute = (LocalizationPathAttribute)attribute;
        string filterPath = pathAttribute.Path;

        // Check if the filterPath starts with '$'
        if (filterPath.StartsWith("$"))
        {
            string memberName = filterPath.Substring(1);
            object targetObject = property.serializedObject.targetObject;
            filterPath = I2LocalizationModificationsUtils.GetMemberValue(targetObject, memberName)?.ToString() ?? string.Empty;
        }

        var keyProperty = property.FindPropertyRelative("mTerm");
        if (keyProperty != null)
        {
            if (!categoryCache.ContainsKey(filterPath))
            {
                List<string> terms = LocalizationManager.GetTermsList().Where(term => term.StartsWith(filterPath)).ToList();
                List<string> categories = ExtractEndCategories(terms, filterPath);

                // Add an empty string to the beginning of the list for default selection
                categories.Insert(0, string.Empty);
                categoryCache[filterPath] = categories;
            }

            List<string> cachedCategories = categoryCache[filterPath];
            int selectedIndex = cachedCategories.IndexOf(keyProperty.stringValue);
            if (selectedIndex == -1) selectedIndex = 0; // Default to the empty string

            // Calculate rects
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var popupRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);

            // Draw the label
            EditorGUI.LabelField(labelRect, label);

            // Determine the display text
            string displayText = string.IsNullOrEmpty(keyProperty.stringValue) ? string.Empty : keyProperty.stringValue;

            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUI.Popup(popupRect, selectedIndex, cachedCategories.Select(c => string.IsNullOrEmpty(c) ? string.Empty : c).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (selectedIndex > 0)
                {
                    keyProperty.stringValue = filterPath + "/" + cachedCategories[selectedIndex];
                }
                else
                {
                    keyProperty.stringValue = string.Empty; // Reset to empty if nothing is selected
                }
                keyProperty.serializedObject.ApplyModifiedProperties(); // Explicitly apply the changes
            }

            // Display the selected value inside the popup button
            if (!string.IsNullOrEmpty(keyProperty.stringValue))
            {
                EditorGUI.LabelField(popupRect, displayText, EditorStyles.popup);
            }
        }

        EditorGUI.EndProperty();
    }

    private List<string> ExtractEndCategories(List<string> terms, string filterPath)
    {
        HashSet<string> categories = new HashSet<string>();
        foreach (string term in terms)
        {
            string relativeTerm = term.Substring(filterPath.Length).TrimStart('/');
            string[] parts = relativeTerm.Split('/');
            for (int i = 1; i <= parts.Length; i++)
            {
                string category = string.Join("/", parts.Take(i));
                if (i == parts.Length || parts.Length == 1)
                {
                    categories.Add(category);
                }
            }
        }
        return categories.OrderBy(c => c).ToList();
    }
}
#endif
