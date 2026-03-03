#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;

// Please ensure your class and your file have the same name
public class SetDefaultMaterial : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter]
    public Material defaultMaterial;

    public override int Id { get { return 548006311; } }
    public override string MenuPathRuleEngine { get { return "VirooStudio/Set Default Material"; } }
    public override string MenuPathToolbox { get { return "VirooStudio/Set Default Material"; } }
    public override string Tooltip { get { return "Assign default material"; } }
    public override string Icon { get { return null; } }
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        foreach (GameObject go in input)
        {
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                continue;
            }

            Material[] materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == null)
                {
                    materials[i] = defaultMaterial;
                }

            }

            renderer.sharedMaterials = materials;
        }
        return input;
    }
}
#endif