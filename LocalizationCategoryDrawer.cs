#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using I2.Loc;
using System.Linq;
using System.Collections.Generic;
using Localization.I2LocalizationModifications;

[CustomPropertyDrawer(typeof(LocalizationCategoryAttribute))]
public class LocalizationCategoryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var categoryProperty = property.FindPropertyRelative("category");

        // Get the attribute
        LocalizationCategoryAttribute categoryAttribute = (LocalizationCategoryAttribute)attribute;
        string filterPath = categoryAttribute?.CategoryPath ?? string.Empty;

        // Check if the filterPath starts with '$'
        if (filterPath.StartsWith("$"))
        {
            string memberName = filterPath.Substring(1);
            object targetObject = property.serializedObject.targetObject;
            filterPath = I2LocalizationModificationsUtils.GetMemberValue(targetObject, memberName)?.ToString() ?? string.Empty;
        }

        // Get the terms and extract unique categories, excluding terms
        List<string> terms = LocalizationManager.GetTermsList().Where(term => term.StartsWith(filterPath)).ToList();
        List<string> categories = ExtractCategories(terms, filterPath);

        // Add "None" option to the beginning of the list for default selection
        categories.Insert(0, "None");

        int selectedIndex = categories.IndexOf(categoryProperty.stringValue);
        if (selectedIndex == -1) selectedIndex = 0; // Default to "None"
        // Calculate rects
        var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        var popupRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);

        // Draw the label
        EditorGUI.LabelField(labelRect, label);
        // Determine the display text
        string displayText = string.IsNullOrEmpty(categoryProperty.stringValue) ? string.Empty : categoryProperty.stringValue;

        EditorGUI.BeginChangeCheck();
        selectedIndex = EditorGUI.Popup(popupRect, label.text, selectedIndex, categories.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            if (filterPath.EndsWith("/")) filterPath = filterPath.Substring(0, filterPath.Length - 1);
            categoryProperty.stringValue = filterPath +"/"+ (selectedIndex == 0 ? string.Empty : categories[selectedIndex]);

            categoryProperty.serializedObject.ApplyModifiedProperties();
        }
        
        // Display the selected value inside the popup button
        if (!string.IsNullOrEmpty(categoryProperty.stringValue))
        {
            EditorGUI.LabelField(popupRect, displayText, EditorStyles.popup);
        }
        EditorGUI.EndProperty();
    }

    private List<string> ExtractCategories(List<string> terms, string filterPath)
    {
        HashSet<string> categories = new HashSet<string>();
        foreach (string term in terms)
        {
            string relativeTerm = term.Substring(filterPath.Length).TrimStart('/');
            string[] parts = relativeTerm.Split('/');
            
            if (parts.Length > 1) // Only consider it a category if it has subcategories
            {
                for (int i = 1; i < parts.Length; i++)
                {
                    string category = string.Join("/", parts.Take(i));
                    categories.Add(category);
                }
            }
            else // If there are no subcategories, add only the first part as category
            {
                categories.Add(parts[0]);
            }
        }

        // Remove terms from the first level
        categories.RemoveWhere(cat => terms.Contains($"{filterPath}{cat}"));

        return categories.OrderBy(c => c).ToList();
    }
}
#endif
