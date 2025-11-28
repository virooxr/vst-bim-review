using UnityEngine;
using Virtualware.Networking.Client.Variables;
using Virtualware.Networking;
using System.Collections.Generic;
using System.Linq;

namespace Viroo.Studio.Templates
{
    public class LayerStatesNetworkSync : MonoBehaviour
    {
        public LayersConfig LayersConfig;

        public LayerView layerView;

        private IScenePersistenceAwareElementRegistry elementRegistry;

        private NetworkVariable<List<bool>> layerStatesNetworkVariable;

        protected void Inject(IScenePersistenceAwareElementRegistry elementRegistry, NetworkVariableSynchronizer variableSynchronizer)
        {
            // We store the IScenePersistenceAwareElementRegistry to unregister later
            this.elementRegistry = elementRegistry;

            // We create the variable with myNetworkVariable id and 0 as the default value (if it has already been updated, the variable will retrieve its current value from the server instead of setting this default value)
            layerStatesNetworkVariable = new NetworkVariable<List<bool>>(variableSynchronizer, "VIROO_LayerStatesNetVar", new List<bool>());

            layerStatesNetworkVariable.OnInitialized += OnVariableInitialized;
            layerStatesNetworkVariable.OnValueChanged += OnVariableChanged;

            // By registering the variable in the IScenePersistenceAwareElementRegistry what we do is to limit its persistence to the current scene, when the scene is left the variable will be removed from the server. If this code is run again after returning to the scene with this script, the variable will be created once again in the server
            elementRegistry.Register(layerStatesNetworkVariable);
        }

        protected void Awake()
        {
            // We call this method to have the Inject method automatically called by VIROO
            this.QueueForInject();
        }

        protected void OnDestroy()
        {
            // In order not to leave residual code, we will unsubscribe to the events we've previously subscribed to.
            if (layerStatesNetworkVariable != null)
            {
                layerStatesNetworkVariable.OnValueChanged -= OnVariableChanged;
                layerStatesNetworkVariable.OnInitialized -= OnVariableInitialized;
            }

            if (elementRegistry != null)
            {
                elementRegistry.Unregister(layerStatesNetworkVariable);
            }
        }


        // To change the value of our variable we only have to change the value of its `Value` property.
        public void ChangeValue(List<bool> newValue)
        {
            layerStatesNetworkVariable.Value = newValue;
            // At this moment, we can't assure that the value has its value assigned (as we explain later), it can only be assured to have its value in the OnValueChanged event
            // all operations using the variables value should take place in the OnValueChanged event
        }

        // This event is called when the variable's value is set for the first time, or when it's restored after a successful reconnection after a network disconnection
        private void OnVariableInitialized(object sender, List<bool> value)
        {
            // Empty
        }

        // This event is called when the variable's value changes
        private void OnVariableChanged(object sender, List<bool> value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                LayersConfig.layerConfigList[i].active = value[i];
                Debug.Log($"{LayersConfig.layerConfigList[i].layerName} : {LayersConfig.layerConfigList[i].active}");

                foreach (GameObject go in LayersConfig.layerConfigList[i].gameObjects)
                {
                    go.SetActive(LayersConfig.layerConfigList[i].active);
                }
            }

            // Re-initialize to update values if already open while receiving network updates
            if (layerView.isActiveAndEnabled)
            {
                layerView.Initialize();
            }
        }

        public void SyncNetworkLayers()
        {
            List<bool> states = LayersConfig.layerConfigList.Select(l => l.active).ToList();
            ChangeValue(states);
        }
    }
}