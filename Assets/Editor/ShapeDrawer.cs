using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(BoolArray))]
public class ShapeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect newPosition = position;
        newPosition.y += 30f;

        Rect sizePosition = position;
        sizePosition.height = 20f;

        SerializedProperty sizeProperty = property.FindPropertyRelative("Size");
        int size = sizeProperty.intValue;
        size = EditorGUI.IntField(sizePosition, "Size", size);
        sizeProperty.intValue = size;

        SerializedProperty data = property.FindPropertyRelative("Rows");
        data.arraySize = size;
        
        newPosition.x = position.width / 2 - size / 2;
        for (int j = 0; j < size; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("Row");
            newPosition.height = 18f;
            
            if(row.arraySize != size)
                row.arraySize = size;
            
            newPosition.width = 18f;
            for (int i = 0; i < size; i++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newPosition.x += newPosition.width;
            }

            newPosition.x = position.width / 2 - size / 2;
            newPosition.y += 18f;
        }
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20f + 30f * property.FindPropertyRelative("Size").intValue;
    }
}