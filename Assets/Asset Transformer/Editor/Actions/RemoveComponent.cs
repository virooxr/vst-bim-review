#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEditor.PixyzPlugin4Unity;
using static UnityEditor.PixyzPlugin4Unity.Actions.AddComponent;

// Please ensure your class and your file have the same name
public class NewCustomAction : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter(tooltip: "Select Component Type")]
    public UnityComponentType componentType = new UnityComponentType();


    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {

    }

    public override int Id => 21279267;
    public override string MenuPathRuleEngine => "VirooStudio/Remove Component";
    public override string MenuPathToolbox => "VirooStudio/Remove Component";
    public override string Tooltip => "USE WITH CAUTION!. Remove a component given its type to the current selection";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        foreach (var item in input)
        {
            var component = item.gameObject.GetComponent(componentType.ComponentType);

            if (component != null)
            {
                Component.DestroyImmediate(component);
            }
        }

        return input;
    }
}
#endif