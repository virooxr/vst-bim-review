#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class SetLayerByName : ActionInOut<IList<GameObject>, IList<GameObject>> 
{
    [UserParameter]
    public string layerName = "";

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters() 
    {
        layerName = "";
    }

    public override int Id => 404620476;
    public override string MenuPathRuleEngine => "VirooStudio/Set Layer By Name";
    public override string MenuPathToolbox => "VirooStudio/Set Layer By Name";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001; 

    public override IList<GameObject> Run(IList<GameObject> input) 
    {
        for (int i = 0; i < input.Count; i++)
        {
            int layer = LayerMask.NameToLayer(layerName);
            input[i].layer = layer;
        }
        return input;
    }
}
#endif