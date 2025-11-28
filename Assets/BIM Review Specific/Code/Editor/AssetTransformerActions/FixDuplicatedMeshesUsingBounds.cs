#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using System.Linq;
using Viroo.Arena.NMerso.Data;

// Please ensure your class and your file have the same name
public class FixDuplicatedMeshesUsingBounds : ActionInOut<IList<GameObject>, IList<GameObject>>
{

    public enum Axis
    {
        X,
        Y,
        Z
    }

    [UserParameter]
    public Axis AxisToCheckInBoundsCenter = Axis.Y;

    [UserParameter]
    public Axis AxisToRotate180 = Axis.Z;

    [UserParameter]
    public bool WorldSpaceRotation = false;

    // Public field to select the axis in the Inspector
    public Axis selectedAxis;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        AxisToCheckInBoundsCenter = Axis.Y;
        AxisToRotate180 = Axis.Z;
        WorldSpaceRotation = false;
    }

    public override int Id => 41840332;
    public override string MenuPathRuleEngine => "VirooStudio/Fix Duplicated Meshes Using Bounds";
    public override string MenuPathToolbox => "VirooStudio/Fix Duplicated Meshes Using Bounds";
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

                Vector3 eulerOffset = Vector3.zero;
                switch (AxisToRotate180)
                {
                    case Axis.X:
                        eulerOffset = Vector3.right * 180;
                        break;
                    case Axis.Y:
                        eulerOffset = Vector3.up * 180;
                        break;
                    case Axis.Z:
                        eulerOffset = Vector3.forward * 180;
                        break;

                }

                foreach (GameObject gameObject in currentList)
                {
                    MeshFilter currentMeshFilter = gameObject.GetComponent<MeshFilter>();

                    if (BoundsMirrored(AxisToCheckInBoundsCenter, replaceMesh, currentMeshFilter.sharedMesh))
                    {
                        if (WorldSpaceRotation)
                        {
                            gameObject.transform.rotation *= Quaternion.Euler(eulerOffset);
                        }
                        else
                        {
                            gameObject.transform.localRotation *= Quaternion.Euler(eulerOffset);
                        }
                    }
                    currentMeshFilter.sharedMesh = replaceMesh;
                }
            }

        }
        return input;
    }

    private bool BoundsMirrored(Axis axisToCheckBoundsCenter, Mesh mRef, Mesh currentMesh)
    {
        bool mirrored = false;

        float refBoundsSign = 0;
        float currentSign = 0;
        switch (axisToCheckBoundsCenter)
        {
            case Axis.X:
                refBoundsSign = Mathf.Sign(mRef.bounds.center.x);
                currentSign = Mathf.Sign(currentMesh.bounds.center.x);
                break;
            case Axis.Y:
                refBoundsSign = Mathf.Sign(mRef.bounds.center.y);
                currentSign = Mathf.Sign(currentMesh.bounds.center.y);
                break;
            case Axis.Z:
                refBoundsSign = Mathf.Sign(mRef.bounds.center.z);
                currentSign = Mathf.Sign(currentMesh.bounds.center.z);
                break;
        }

        mirrored = (refBoundsSign != currentSign);
        return mirrored;
    }
}
#endif