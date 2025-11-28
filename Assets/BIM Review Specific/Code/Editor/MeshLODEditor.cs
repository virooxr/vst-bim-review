using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshLODEditor : EditorWindow
{
    private int lodLimit;
    private bool discardOddLevels = false;

    List<Mesh> meshes;

    [MenuItem("Window/Viroo/Viroo Studio Templates/Mesh LOD Utility")]
    public static void ShowWindow()
    {
        GetWindow<MeshLODEditor>("Mesh LOD Utility");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        lodLimit = EditorGUILayout.IntSlider(lodLimit, 0, 63);
        discardOddLevels = EditorGUILayout.Toggle("Discard Odd Levels", discardOddLevels);
       

        if (Selection.objects.Length == 0)
        {
            return;
        }

        if (GUILayout.Button("Use Selection"))
        {
            // Generate mesh list
            
            int skipCount = 0;
            int totalMeshCount = 0;
            meshes = new List<Mesh>();
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GameObject go = Selection.objects[i] as GameObject;
                MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    // Skip repeated meshes or already done                
                    //if (!meshes.Contains(meshFilter.sharedMesh) && !(meshFilter.mesh.lodCount > 1))
                    if (!meshes.Contains(meshFilter.sharedMesh))
                    {
                        meshes.Add(meshFilter.sharedMesh);
                    }
                    else
                    {
                        skipCount++;
                    }
                    totalMeshCount++;
                }
            }
            Debug.Log($"Repeated / Total: {skipCount} / {totalMeshCount}");
        }

        if (meshes.Count > 0)
        {
            if (GUILayout.Button("Generate Mesh LODs"))
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Creating LODS", $"Mesh {i} / {meshes.Count}", (float)i / (float)meshes.Count))
                        break;

                    MeshLodUtility.GenerateMeshLods(meshes[i], discardOddLevels ? MeshLodUtility.LodGenerationFlags.DiscardOddLevels : 0, lodLimit);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        EditorGUILayout.EndVertical();
    }
}

