using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pixyz.UnitySDK.Components;

public class MetadataFieldSelector : EditorWindow
{
    private string propertyName;

    private bool usePropertyValue = false;
    private string propertyValue;

    List<GameObject> matching = new List<GameObject>();

    [MenuItem("Window/Viroo/Viroo Studio Templates/Metadata Property Selector")]
    public static void ShowWindow()
    {
        GetWindow<MetadataFieldSelector>("Metadata Property Selector");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        propertyName = EditorGUILayout.TextField("Property Name", propertyName);

        usePropertyValue = EditorGUILayout.Toggle("Use Value", usePropertyValue);

        if (usePropertyValue) 
        {
            propertyValue = EditorGUILayout.TextField("Property Value", propertyValue);
        }

        if (GUILayout.Button("Filter Selection"))
        {
            List<Metadata> metadatas = new List<Metadata>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                metadatas.AddRange(gameObject.GetComponentsInChildren<Metadata>());
            }

            matching.Clear();

            foreach (Metadata metadata in metadatas)
            {
                if (metadata.containsProperty(propertyName))
                {
                    string value = metadata.getProperty(propertyName);
                    bool checkValue = !usePropertyValue || (value == propertyValue);
                    if (checkValue)
                    {
                        matching.Add(metadata.gameObject);
                    }
                }
            }
            Selection.objects = matching.ToArray();
        }

        GUILayout.EndVertical();
    }
}

