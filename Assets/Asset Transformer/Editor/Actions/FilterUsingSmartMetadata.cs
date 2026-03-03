#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using Viroo.Studio.Templates;
using System.Linq;

public class FilterUsingSmartMetadata : ActionInOut<IList<GameObject>, IList<GameObject>> 
{
    [UserParameter] 
    public SmartMetadataFilter smartMetadataFilter;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters() 
    {
        smartMetadataFilter = null;
    }

    public override int Id => 915430545;
    public override string MenuPathRuleEngine => "VirooStudio/Filter Using Smart Metadata";
    public override string MenuPathToolbox => "VirooStudio/Filter Using Smart Metadata";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001; 

    public override IList<GameObject> Run(IList<GameObject> input) 
    {
        List<GameObject> output = new List<GameObject>();
        
        // Select objects from the input including children with are compliant with the SmartMetadataFilter
        foreach (var gameObject in input) {
            List<GameObject> partialList = smartMetadataFilter.FilterHierarchy(gameObject);
            output.AddRange(partialList.Where(item => !output.Contains(item)));

        }
        return output;
    }
}
#endif