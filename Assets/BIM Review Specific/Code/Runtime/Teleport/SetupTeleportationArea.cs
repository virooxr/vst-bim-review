using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace Viroo.Studio.Templates
{
    public class SetupTeleportationArea : MonoBehaviour
    {
        public string tagName;

        void Start()
        {
            List<TagScript> tags = GameObject.FindObjectsByType<TagScript>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            tags = tags.Where(t => t.tagName == tagName).ToList();

            List<Collider> colliders = new List<Collider>();
            foreach (TagScript tag in tags)
            {
                Collider collider = tag.GetComponent<Collider>();
                if (collider != null)
                {
                    colliders.Add(collider);
                }
            }

            TeleportationArea teleportationArea = GetComponent<TeleportationArea>();
            teleportationArea.colliders.AddRange(colliders);

            teleportationArea.interactionManager.UnregisterInteractable(teleportationArea as IXRInteractable);
            teleportationArea.interactionManager.RegisterInteractable(teleportationArea as IXRInteractable);
        }
    }
}
