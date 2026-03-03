#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEditor.PixyzPlugin4Unity.RuleEngine;
using UnityEngine;

// Please ensure your class and your file have the same name
public class BatchRuleset : ActionIn<IList<GameObject>>
{
    [UserParameter] // User parameters are displayed in the action's inspector
    public BatchRulesetsConfig rulesetConfig;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()  
    {
    }

    public override int Id => 81723;
    public override string MenuPathRuleEngine => "VirooStudio/Batch Rule Set";
    public override string MenuPathToolbox => "VirooStudio/Batch Rule Set";
    public override string Tooltip => "Allows to create batches based in other rule sets";
    public override string Icon => null;
    public override int Priority => 15001;  

    //public override void Run() 
    //{
    //    foreach (RuleSet ruleSet in rulesetConfig.rulesets)
    //    {
    //        ruleSet.Run(true, false);
    //    }
    //}

    public override void Run(IList<GameObject> input)
    {
        foreach (RuleSet ruleSet in rulesetConfig.rulesets)
        {
            ruleSet.Run(true, false);
        }
    }
}
#endif