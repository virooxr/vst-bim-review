using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Viroo.Studio.Templates
{
    public class LayerListController
    {
        // UXML template for list entries
        VisualTreeAsset listEntryTemplate;

        // UI element references
        ListView layerListView;

        List<LayerConfig> layerConfigList;

        public void InitializeLayerList(VisualElement root, VisualTreeAsset listElementTemplate, LayerStatesNetworkSync layerStatesSync)
        {
            layerConfigList = layerStatesSync.LayersConfig.layerConfigList;

            // Store a reference to the template for the list entries
            listEntryTemplate = listElementTemplate;

            // Store a reference to the character list element
            layerListView = root.Q<ListView>("layer-list");

            FillLayerList(layerStatesSync);

            // Register to get a callback when an item is selected
            layerListView.selectionChanged += OnLayerSelected;
        }

        void FillLayerList(LayerStatesNetworkSync layerStatesSync)
        {
            // Set up a make item function for a list entry
            layerListView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = listEntryTemplate.Instantiate();

                // Instantiate a controller for the data
                var newListEntryLogic = new LayerListEntryController(layerStatesSync);

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific list entry
            layerListView.bindItem = (item, index) =>
            {
                (item.userData as LayerListEntryController)?.SetLayerConfig(layerConfigList[index]);
                //(item.userData as LayerListEntryController)?.SetLayerGroupData(m_LayerGroupDatas[index]);
            };

            // Set a fixed item height matching the height of the item provided in makeItem. 
            // For dynamic height, see the virtualizationMethod property.
            layerListView.fixedItemHeight = 45;

            // Set the actual item's source list/array
            layerListView.itemsSource = layerConfigList;
        }

        void OnLayerSelected(IEnumerable<object> selectedItems)
        {
            // Get the currently selected item directly from the ListView
            var selectedLayer = layerListView.selectedItem as LayerConfig;
        }
    }
}
