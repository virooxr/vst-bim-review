#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEngine.Pixyz.UnitySDK.Components;

// Please ensure your class and your file have the same name
public class RemoveDuplicatedMetadataInHierarchy : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter(tooltip: "Metadata property identifying a unique instance")]
    public string idPropertyName;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        idPropertyName = "IFC Parameters/IfcGUID";
    }

    public override int Id => 935567099;
    public override string MenuPathRuleEngine => "VirooStudio/Remove Duplicated Metadata In Parent";
    public override string MenuPathToolbox => "VirooStudio/Remove Duplicated Metadata In Parent";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        // If object has metadata, look for the same id in children, if found remove from parent
        foreach (var go in input)
        {
            Metadata metadata = go.GetComponent<Metadata>();

            if (metadata != null)
            {
                if (metadata.containsProperty(idPropertyName))
                {
                    string currentGUID = metadata.getProperty(idPropertyName);

                    Metadata[] childrenMetadatas = go.GetComponentsInChildren<Metadata>();

                    bool found = false;
                    foreach (var childMetadata in childrenMetadatas)
                    {
                        if (childMetadata.containsProperty(idPropertyName))
                        {
                            string childGUID = childMetadata.getProperty(idPropertyName);
                            if (childMetadata != metadata && childGUID == currentGUID)
                            {
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (found)
                    {
                        Component.DestroyImmediate(metadata);
                    }
                }

            }

        }
        return input;
    }
}

#endif