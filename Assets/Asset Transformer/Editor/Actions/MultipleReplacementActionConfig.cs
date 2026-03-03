using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Viroo Studio/Templates/Multiple Replacement Action Config")]
public class MultipleReplacementActionConfig : ScriptableObject
{
    [SerializeField]
    public List<ReplacementEntry> replacements = new List<ReplacementEntry>();

    private void OnValidate()
    {
        var duplicates = replacements
             .GroupBy(p => new { p.propertyName, p.propertyValue})
             .Where(g => g.Count() > 1)
             .Select(g => new { ReplacementEntry = g.Key, Count = g.Count() })
             .ToList();

        if (duplicates.Count > 0)
        {
            Debug.LogError("Repeated Replacement config found");
            foreach (var dup in duplicates)
            {
                Debug.LogError($"Duplicated Entry: {dup.ReplacementEntry.propertyName} {dup.ReplacementEntry.propertyValue}");
            }
        }
    }
}

[System.Serializable]
public class ReplacementEntry
{
    public string description;
    public string propertyName;
    public string propertyValue;
    public GameObject replacement;
}