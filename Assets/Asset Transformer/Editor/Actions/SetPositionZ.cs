#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class SetPositionZ : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter] // User parameters are displayed in the action's inspector
    public bool inLocalSpace = false;

    [UserParameter] // User parameters are displayed in the action's inspector
    public float zValue = 0f;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        inLocalSpace = false;
        zValue = 0f;
    }

    public override int Id => 119871753;
    public override string MenuPathRuleEngine => "VirooStudio/Set Position Z";
    public override string MenuPathToolbox => "VirooStudio/Set Position Z";
    public override string Tooltip => "Sets transform.positon.z to value";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        if (inLocalSpace)
        {
            foreach (var item in input)
            {
                item.transform.localPosition = new Vector3(
                    item.transform.localPosition.x,
                    item.transform.localPosition.y,
                    zValue);
            }
        }
        else
        {
            foreach (var item in input)
            {
                item.transform.position = new Vector3(
                    item.transform.position.x,
                    item.transform.position.y,
                    zValue);
            }
        }

        return input; 
    }
}
#endif