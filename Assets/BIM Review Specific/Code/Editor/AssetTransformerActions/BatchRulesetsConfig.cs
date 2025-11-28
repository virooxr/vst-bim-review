using System.Collections.Generic;
using UnityEditor.PixyzPlugin4Unity.RuleEngine;
using UnityEngine;

[CreateAssetMenu(menuName = "Viroo Studio/Templates/Batch Rulesets Config")]
public class BatchRulesetsConfig : ScriptableObject
{
    [SerializeField]
    public List<RuleSet> rulesets = new List<RuleSet>(); 
}