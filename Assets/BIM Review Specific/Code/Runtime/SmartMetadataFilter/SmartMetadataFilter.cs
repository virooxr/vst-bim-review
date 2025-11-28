using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Viroo.Studio.Templates
{
    [CreateAssetMenu(fileName = "Smart Metadata Filter", menuName = "Viroo Studio/Templates/SmartMetadataFilter", order = 1)]
    public class SmartMetadataFilter : ScriptableObject
    {
        public string label;

        public MetadataInclusionSet[] metaDataInclusionSets; 

        public List<GameObject> FilterHierarchy(GameObject target)
        {
            List<GameObject> result = new List<GameObject>();
            // Process filters and calculate objects
            UnityEngine.Pixyz.UnitySDK.Components.Metadata[] metadatas = target.GetComponentsInChildren<UnityEngine.Pixyz.UnitySDK.Components.Metadata>();
            foreach (var metadata in metadatas)
            {
                foreach (MetadataInclusionSet layerMetadataInclusionSet in metaDataInclusionSets)
                {
                    bool checkMustHave = layerMetadataInclusionSet.mustHave.All(i => HasFieldWithValue(metadata, i.PropertyName, i.PropertyValue));
                    bool excludeThoseHaving = layerMetadataInclusionSet.excludeThoseHaving.Any(i => HasFieldWithValue(metadata, i.PropertyName, i.PropertyValue));
                    if (checkMustHave && !excludeThoseHaving)
                    {
                        result.Add(metadata.gameObject);
                    }
                }
            }
            return result;
        }

        private bool HasFieldWithValue(UnityEngine.Pixyz.UnitySDK.Components.Metadata metadata, string propertyName, string propertyValue)
        {
            if (metadata.containsProperty(propertyName))
            {
                string value = metadata.getProperty(propertyName);
                return (value == propertyValue);
            }
            else
            {
                return false;
            }
        }
    }
}
