#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class OffsetRotation : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter]
    public bool isWorldSpace = false;

    [UserParameter]
    public Vector3 offsetRotationEuler = Vector3.zero;


    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        isWorldSpace = false;
        offsetRotationEuler = Vector3.zero;
    }

    public override int Id => 65146509;
    public override string MenuPathRuleEngine => "VirooStudio/Apply Rotation Offset";
    public override string MenuPathToolbox => "VirooStudio/Apply Rotation Offset";
    public override string Tooltip => "Applies the given rotation, either in [local] or [world] space";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        foreach (var item in input)
        {
            if (isWorldSpace)
            {
                item.transform.rotation = item.transform.rotation * Quaternion.Euler(offsetRotationEuler);
            }
            else
            {
                item.transform.localRotation = item.transform.localRotation * Quaternion.Euler(offsetRotationEuler);
            }
        }

        return input; 
    }
}
#endif