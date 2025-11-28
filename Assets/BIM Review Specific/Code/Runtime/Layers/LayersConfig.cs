using System.Collections.Generic;
using UnityEngine;

namespace Viroo.Studio.Templates
{
    public class LayersConfig : MonoBehaviour
    {
        public GameObject targetHierarchy;
        public List<LayerConfig> layerConfigList = new List<LayerConfig>();

        private void Start()
        {
            // Auto Initialize on Start
            Init();
        }

        [ContextMenu("Run & Filter GameObjects")]
        private void Init()
        {
            foreach (LayerConfig layerConfig in layerConfigList)
            {
                layerConfig.Initialize(targetHierarchy);
            }
        }
    }
}
