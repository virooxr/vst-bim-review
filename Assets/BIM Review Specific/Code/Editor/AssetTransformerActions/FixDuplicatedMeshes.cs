#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEngine.UIElements;
using System.Linq;

// Please ensure your class and your file have the same name
public class FixDuplicatedMeshes : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter]
    public bool positionWorld = false;

    [UserParameter]
    public Vector3 positionOffset = Vector3.zero;

    [UserParameter]
    public bool eulerWorld = false;

    [UserParameter]
    public Vector3 eulerOffset = Vector3.zero;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        eulerWorld = false;
        eulerOffset = Vector3.zero;
    }

    public override int Id => 925539005;
    public override string MenuPathRuleEngine => "VirooStudio/Fix Duplicated Meshes";
    public override string MenuPathToolbox => "VirooStudio/Fix Duplicated Meshes";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001;


    public override IList<GameObject> Run(IList<GameObject> input)
    {
        // Create a dictionary by Mesh reference
        // key: Mesh
        // value: List<GameObject>
        Dictionary<Mesh, List<GameObject>> meshReferences = new Dictionary<Mesh, List<GameObject>>();

        // Traverse selection and fill dictionary
        foreach (GameObject gameObject in input)
        {
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf != null)
            {
                Mesh currentMesh = mf.sharedMesh;
                if (!meshReferences.ContainsKey(currentMesh))
                {
                    // Create new entry for currentMesh
                    meshReferences.Add(currentMesh, new List<GameObject>());
                }
                // Add gameObject to contained list
                meshReferences[currentMesh].Add(gameObject);
            }
        }

        // If there are more that one occurrence, replace with first occurrence
        if (meshReferences.Count > 1)
        {
            Mesh replaceMesh = meshReferences.Keys.ToList().First();
            for (int i = 1; i < meshReferences.Count; i++)
            {
                List<GameObject> currentList = meshReferences.Values.ToList()[i];
                foreach (GameObject gameObject in currentList)
                {
                    gameObject.GetComponent<MeshFilter>().sharedMesh = replaceMesh;

                    // Apply offset Position
                    if (positionWorld)
                    {
                        gameObject.transform.position += positionOffset;
                    }
                    else
                    {
                        gameObject.transform.localPosition += positionOffset;
                    }

                    // Apply offset Rotation
                    if (eulerWorld)
                    {
                        gameObject.transform.rotation *= Quaternion.Euler(eulerOffset);
                    }
                    else
                    {
                        gameObject.transform.localRotation *= Quaternion.Euler(eulerOffset);
                    }
                }
            }

        }
        return input;
    }
}
#endif