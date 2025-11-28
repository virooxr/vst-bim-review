#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class FilterByMeshName : ActionInOut<IList<GameObject>, IList<GameObject>> 
{
    [UserParameter]
    public string meshName = "";

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters() 
    {
        meshName = "";
    }

    public override int Id => 195748726; 
    public override string MenuPathRuleEngine => "VirooStudio/Filter On Mesh Name";
    public override string MenuPathToolbox => "VirooStudio/Filter On Mesh Name";
    public override string Tooltip => "Select objects with containing a MeshFilter with a specified Mesh Name";
    public override string Icon => null;
    public override int Priority => 15001; 

    public override IList<GameObject> Run(IList<GameObject> input) 
    {
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject gameObject in input)
        {
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>(); 

            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                if (meshFilter.sharedMesh.name == meshName)
                {
                    result.Add(gameObject);
                }
            }
        }

        return result;
    }
}
#endif