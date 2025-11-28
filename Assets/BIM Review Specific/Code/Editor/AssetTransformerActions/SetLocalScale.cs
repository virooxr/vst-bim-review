#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEngine.Pixyz.UnitySDK.Components;

public class SetLocalScale : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter]
    public Vector3 scale = Vector3.one;

    [HelperMethod]
    public void ResetParameters()
    {
        scale = Vector3.one;
    }

    private string backScalePropertyName = "VirooStudio/Back/transform.localScale";

    public override int Id => 944641982;
    public override string MenuPathRuleEngine => "VirooStudio/Set Local Scale";
    public override string MenuPathToolbox => "VirooStudio/Set Local Scale";
    public override string Tooltip => "Sets the transform.localScale vector of selected GameObjects";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        foreach (var item in input)
        {
            // Save original scale to metadata if not previously saved
            Metadata metadata = item.GetComponent<Metadata>();
            if (!metadata.containsProperty(backScalePropertyName))
            {
                metadata.addOrSetProperty(backScalePropertyName, item.transform.localScale.ToString());
            }

            // Perform change
            item.transform.localScale = scale;
        }
        return input;
    }
}
#endif