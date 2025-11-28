using System.Collections.Generic;
using UnityEngine;

namespace Viroo.Studio.Templates
{

    [System.Serializable]
    public class LayerConfig
    {
        public string layerName;
        public bool active;
        public SmartMetadataFilter smartMetadataFilter;
        public List<GameObject> manualIncludedGameObjects;
        public List<GameObject> gameObjects;

        public void Initialize(GameObject target)
        {
            // Filter possible not assigned gameobjects
            manualIncludedGameObjects.RemoveAll(x => x.gameObject == null);

            gameObjects = new List<GameObject>();
            gameObjects.AddRange(manualIncludedGameObjects);
            if (smartMetadataFilter != null)
            {
                List<GameObject> fromSmartMetadataFilter = smartMetadataFilter.FilterHierarchy(target);
                gameObjects.AddRange(fromSmartMetadataFilter);
            }
        }
    }
}