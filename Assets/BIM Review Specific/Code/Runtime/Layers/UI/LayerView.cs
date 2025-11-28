using UnityEngine;
using UnityEngine.UIElements;

namespace Viroo.Studio.Templates
{
    public class LayerView : MonoBehaviour
    {
        [SerializeField]
        VisualTreeAsset m_ListEntryTemplate;

        [SerializeField]
        private LayerStatesNetworkSync layerStatesSync;

        void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            // The UXML is already instantiated by the UIDocument component
            var uiDocument = GetComponent<UIDocument>();

            // Initialize
            var layerListController = new LayerListController();
            layerListController.InitializeLayerList(uiDocument.rootVisualElement, m_ListEntryTemplate, layerStatesSync);
        }
    }
}
