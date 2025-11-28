using UnityEngine.UIElements;

namespace Viroo.Studio.Templates
{
    public class LayerListEntryController
    {
        Label m_NameLabel;
        Toggle m_Toggle;

        LayerStatesNetworkSync m_LayerStatesSync;

        public LayerListEntryController(LayerStatesNetworkSync layerStatesSync)
        {
            m_LayerStatesSync = layerStatesSync;
        }

        public void SetVisualElement(VisualElement visualElement)
        {
            m_NameLabel = visualElement.Q<Label>("layer-name");
            m_Toggle = visualElement.Q<Toggle>("layer-toggle");
        }

        public void SetLayerConfig(LayerConfig layerConfig)
        {
            m_NameLabel.text = layerConfig.layerName;
            m_Toggle.value = layerConfig.active;

            m_Toggle.RegisterValueChangedCallback(evt => { OnToggleChanged(layerConfig, evt.newValue); });
        }

        // TODO: Move this to Unity Event outside this scope,to allow other callbacks
        private void OnToggleChanged(LayerConfig layerConfig, bool value)
        {
            layerConfig.active = value;
            m_LayerStatesSync.SyncNetworkLayers();
        }
    }
}