#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEngine.Pixyz.UnitySDK.Components;
using System.Diagnostics;
using UnityEditor;

// Please ensure your class and your file have the same name
public class ReplaceMultipleWithMetadata : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter] // User parameters are displayed in the action's inspector
    public MultipleReplacementActionConfig replacementsConfig;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        //replacements = new List<ReplacementEntry>();
    }

    public override int Id => 132059166;
    public override string MenuPathRuleEngine => "VirooStudio/Replace Multiple With Metadata";
    public override string MenuPathToolbox => "VirooStudio/Replace Multiple With Metadata";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        // Your code here

        List<GameObject> objectToRemove = new List<GameObject>();

        foreach (GameObject gameObject in input)
        {
            Metadata metadata = gameObject.GetComponent<Metadata>();
            if (metadata != null)
            {
                foreach (ReplacementEntry entry in replacementsConfig.replacements)
                {
                    if (metadata.containsProperty(entry.propertyName))
                    {
                        if (metadata.getProperty(entry.propertyName) == entry.propertyValue)
                        {
                            // Create new instance of replacement at same location / rotation
                            GameObject newInstance = PrefabUtility.InstantiatePrefab(entry.replacement) as GameObject;
                            newInstance.transform.parent = gameObject.transform.parent;
                            newInstance.transform.localPosition = gameObject.transform.localPosition;
                            newInstance.transform.localRotation = gameObject.transform.localRotation;
                            newInstance.transform.localScale = gameObject.transform.localScale;

                            // Mark Original object to be removed
                            objectToRemove.Add(gameObject);
                        }
                    }
                }
            }
        }

        foreach (GameObject gameObject in objectToRemove)
        {
            GameObject.DestroyImmediate(gameObject);
        }

        return input;
    }
}

#endif