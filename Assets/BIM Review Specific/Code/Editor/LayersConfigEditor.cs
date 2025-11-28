using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Viroo.Studio.Templates;


[CustomEditor(typeof(LayersConfig))]
public class LayersConfigEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        SerializedProperty targetHierarchyProperty = serializedObject.FindProperty("targetHierarchy");
        SerializedProperty listProperty = serializedObject.FindProperty("layerConfigList");

        PropertyField targetHierarchyField = new PropertyField(targetHierarchyProperty);
        PropertyField listField = new PropertyField(listProperty);

        root.Add(targetHierarchyField);
        root.Add(listField);

        return root;
    }
}
