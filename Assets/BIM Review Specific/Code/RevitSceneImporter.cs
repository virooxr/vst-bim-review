using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class RevitJsonImporter : MonoBehaviour
{
    [Header("Archivo JSON exportado desde Revit")]
    public string jsonFilePath = "revit_unity_export.json";

    private Dictionary<string, Mesh> meshCache = new Dictionary<string, Mesh>();
    private Dictionary<string, GameObject> nodeCache = new Dictionary<string, GameObject>();

    void Start()
    {
        LoadSceneFromJson(jsonFilePath);
    }

    public void LoadSceneFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"Archivo no encontrado: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        var exportData = JsonConvert.DeserializeObject<ExportData>(json);

        if (exportData == null)
        {
            Debug.LogError("Error al deserializar JSON");
            return;
        }

        // Crear todos los GameObjects
        foreach (var node in exportData.Scene)
        {
            // Determinar padre
            GameObject parentGO = null;
            if (!string.IsNullOrEmpty(node.ParentUniqueId))
            {
                if (!nodeCache.TryGetValue(node.ParentUniqueId, out parentGO))
                {
                    // Crear un nodo intermedio con nombre legible
                    string readableName = GetReadableName(node.ParentUniqueId);
                    parentGO = new GameObject(readableName);
                    nodeCache[node.ParentUniqueId] = parentGO;
                }
            }

            GameObject go = new GameObject(node.Name);
            nodeCache[node.ElementUniqueId] = go;

            // Transform
            go.transform.localPosition = new Vector3(node.Position[0], node.Position[1], node.Position[2]);
            go.transform.localRotation = new Quaternion(node.Rotation[0], node.Rotation[1], node.Rotation[2], node.Rotation[3]);
            go.transform.localScale = new Vector3(node.Scale[0], node.Scale[1], node.Scale[2]);

            if (parentGO != null)
                go.transform.SetParent(parentGO.transform, false);

            // Mesh
            if (!string.IsNullOrEmpty(node.MeshId) && exportData.Meshes.TryGetValue(node.MeshId, out var meshData))
            {
                if (!meshCache.TryGetValue(node.MeshId, out var mesh))
                {
                    mesh = new Mesh();
                    mesh.name = node.Name + "_Mesh";
                    Vector3[] verts = new Vector3[meshData.Vertices.Count];
                    for (int i = 0; i < verts.Length; i++)
                        verts[i] = new Vector3(meshData.Vertices[i].X, meshData.Vertices[i].Y, meshData.Vertices[i].Z);
                    mesh.vertices = verts;
                    mesh.triangles = meshData.Triangles.ToArray();
                    mesh.RecalculateNormals();
                    meshCache[node.MeshId] = mesh;
                }

                var mf = go.AddComponent<MeshFilter>();
                mf.mesh = mesh;
                var mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterial = new Material(Shader.Find("Standard"));
            }
        }
    }

    // Convierte IDs de nivel o categoría a nombres legibles
    private string GetReadableName(string uniqueId)
    {
        if (uniqueId.StartsWith("Level_"))
            return "Level " + uniqueId.Substring(6, 6); // opcional: puedes ajustar substring según formato de UniqueId
        if (uniqueId.Contains("_Category_"))
        {
            int idx = uniqueId.IndexOf("_Category_");
            return uniqueId.Substring(idx + 10);
        }
        return uniqueId;
    }

    // Estructuras JSON
    [System.Serializable]
    public class ExportData
    {
        public List<SceneNode> Scene;
        public Dictionary<string, MeshData> Meshes;
    }

    [System.Serializable]
    public class SceneNode
    {
        public string Name;
        public string ElementUniqueId;
        public string ParentUniqueId;
        public string MeshId;
        public float[] Position;
        public float[] Rotation;
        public float[] Scale;
    }

    [System.Serializable]
    public class MeshData
    {
        public List<Vertex> Vertices;
        public List<int> Triangles;
    }

    [System.Serializable]
    public class Vertex
    {
        public float X;
        public float Y;
        public float Z;
    }
}
