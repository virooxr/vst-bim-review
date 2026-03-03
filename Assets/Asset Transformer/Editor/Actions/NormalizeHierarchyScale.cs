#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class NormalizeHierarchyScale : ActionInOut<IList<GameObject>, IList<GameObject>>
{

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
    }

    public override int Id => 51366610;
    public override string MenuPathRuleEngine => "VirooStudio/Normalize Hierarchy Scale";
    public override string MenuPathToolbox => "VirooStudio/Normalize Hierarchy Scale";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        List<Transform> list = new List<Transform>();

        foreach (GameObject currentGO in input)
        {
            Transform current = currentGO.transform;
            NormalizeTransformRecursive(current);
        }
        return input;
    }

    void NormalizeTransformRecursive(Transform current)
    {
        List<Transform> list = new List<Transform>();

        // Create list with contained children
        for (int i = 0; i < current.childCount; i++)
        {
            Transform child = current.GetChild(i); 
            list.Add(child);
        }

        if (list.Count > 0)
        {
            // Unparent children
            foreach (Transform child in list)
            {
                child.SetParent(null, true);
            }

            // Set local scale to 1
            current.localScale = Vector3.one;

            // Reparent children again
            foreach (Transform child in list)
            {
                child.SetParent(current, true);
                NormalizeTransformRecursive(child);
            }
        }
    }
}
#endif