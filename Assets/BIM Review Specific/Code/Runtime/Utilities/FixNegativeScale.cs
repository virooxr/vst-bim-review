using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace Viroo.Studio.Templates
{
    /// <summary>
    /// Checks if any component of the localscale is negative
    /// if so, normailize it creating a child to hold geometry and negative scales
    /// </summary>
    public class FixNegativeScale : MonoBehaviour
    {
        bool fixDone = false;

        //private void Update()
        //{
        //    if (!fixDone)
        //    {
        //        FixNegativeScaleUsingChild();
        //    }
        //    else
        //    {
        //        // Do not continue trying to fix
        //        Component.Destroy(this, 1f);
        //    }
        //}

        private void Awake()
        {
            if (fixDone) return;

            fixDone = true;
            FixNegativeScaleUsingChild();
        }


        [ContextMenu("Perform Fix")]
        public void FixNegativeScaleUsingChild()
        {
            //gameObject.SetActive(false);

            fixDone = true;
            if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0)
            {
                // Create child object in the same position 
                GameObject childGo = new GameObject(this.name);
                childGo.transform.parent = transform.parent;
                childGo.transform.localPosition = transform.localPosition;
                childGo.transform.localRotation = transform.localRotation;
                childGo.transform.localScale = transform.localScale;

                // Transfer components from parent to child and remove them from parent
                MeshFilter mf = GetComponent<MeshFilter>();
                MeshRenderer mr = GetComponent<MeshRenderer>();
                MeshCollider mc = GetComponent<MeshCollider>();
                BoxCollider bc = GetComponent<BoxCollider>();
                SphereCollider sc = GetComponent<SphereCollider>();
                CapsuleCollider cc = GetComponent<CapsuleCollider>();

                if (mf != null)
                {
                    MeshFilter childMf = childGo.AddComponent<MeshFilter>();
                    childMf.sharedMesh = mf.sharedMesh;

                    GameObject.DestroyImmediate(mf);
                }

                if (mr != null)
                {
                    MeshRenderer childMr = childGo.AddComponent<MeshRenderer>();
                    List<Material> mats = new List<Material>();
                    mr.GetSharedMaterials(mats);
                    childMr.SetSharedMaterials(mats);

                    GameObject.DestroyImmediate(mr);
                }

                if (mc != null)
                {
                    MeshCollider childMc = childGo.AddComponent<MeshCollider>();
                    childMc.sharedMesh = mc.sharedMesh;
                    childMc.convex = mc.convex;
                    childMc.sharedMaterial = mc.sharedMaterial;

                    GameObject.DestroyImmediate(mc);
                }

                if (bc != null)
                {
                    BoxCollider childBc = childGo.AddComponent<BoxCollider>();
                    childBc.isTrigger = bc.isTrigger;
                    childBc.sharedMaterial = bc.sharedMaterial;

                    GameObject.DestroyImmediate(bc);
                }

                if (sc != null)
                {
                    SphereCollider childSc = childGo.AddComponent<SphereCollider>();
                    childSc.isTrigger = sc.isTrigger;
                    childSc.sharedMaterial = sc.sharedMaterial;

                    GameObject.DestroyImmediate(sc);
                }

                if (cc != null)
                {
                    CapsuleCollider childCc = childGo.AddComponent<CapsuleCollider>();
                    childCc.isTrigger = cc.isTrigger;
                    childCc.sharedMaterial = cc.sharedMaterial;

                    GameObject.DestroyImmediate(cc);
                }

                // Set unitary scale on parent
                this.transform.localScale = Vector3.one;

                // Set child in parent's hierarchy
                childGo.transform.parent = this.transform;

                //gameObject.SetActive(true);


                //// Fix missing removed colliders from interactables
                //XRBaseInteractable[] xRBaseInteractables = GetComponentsInChildren<XRBaseInteractable>();
                //foreach (XRBaseInteractable x in xRBaseInteractables)
                //{
                //    x.colliders.Clear();
                //    x.colliders.AddRange(GetComponentsInChildren<Collider>());
                //    //x.colliders.RemoveAll(x => x == null);    
                //}
            }
        }
    }

    //public static class ExtensionMethods
    //{
    //    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    //    {
    //        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    //    }

    //    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    //    {
    //        Type type = comp.GetType();
    //        if (type != other.GetType()) return null; // type mis-match
    //        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
    //        PropertyInfo[] pinfos = type.GetProperties(flags);
    //        foreach (var pinfo in pinfos)
    //        {
    //            if (pinfo.CanWrite)
    //            {
    //                try
    //                {
    //                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
    //                }
    //                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
    //            }
    //        }
    //        FieldInfo[] finfos = type.GetFields(flags);
    //        foreach (var finfo in finfos)
    //        {
    //            finfo.SetValue(comp, finfo.GetValue(other));
    //        }
    //        return comp as T;
    //    }
    //}

}

