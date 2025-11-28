#if PIXYZ_PLUGIN_FOR_UNITY
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.Actions;
using UnityEngine.Pixyz.UnitySDK.Components;
using System.Linq;
using Viroo.Studio.Templates;

// Please ensure your class and your file have the same name
public class FixRevitMirrorUsingCSV : ActionInOut<IList<GameObject>, IList<GameObject>>
{
    [UserParameter(tooltip: "Copy the Revit exported CSV inside Assets and Drag & drop here a reference")]
    public TextAsset csvFile;

    [UserParameter(tooltip: "Metadata property identifying a unique instance (first column of the exported CSV from Revit)")]
    public string idPropertyName;

    [UserParameter(tooltip: "[List of Metadata Property Names] which identifies unique mesh occurrences of a type in the instances: i.e.: [Family and Type]")]
    public string[] instanceClassifierPropertyNames;

    [UserParameter(tooltip: "Correction to be applied to mirrored meshes [1,-1, 1] will fix symmetries over Y axis")]
    public Vector3 reflectionVector;

    [HelperMethod] // Helper methods can be accessed from the action's context menu
    public void ResetParameters()
    {
        csvFile = null;
        idPropertyName = "IFC Parameters/IfcGUID";
        instanceClassifierPropertyNames = new List<string>() { "Family and Type" }.ToArray();
        reflectionVector = new Vector3(1, -1, 1);
    }

    public override int Id => 385238814;
    public override string MenuPathRuleEngine => "VirooStudio/Fix Mirror Meshes Using CSV";
    public override string MenuPathToolbox => "VirooStudio/Fix Mirror Meshes Using CSV";
    public override string Tooltip => "";
    public override string Icon => null;
    public override int Priority => 15001;

    public override IList<GameObject> Run(IList<GameObject> input)
    {
        // First Load CSV into dictionary by ifcGUID
        CSV_Data data = new CSV_Data();
        data.ReadCSV(csvFile.text);

        // Iterate over objects get Metadata and fill in
        foreach (GameObject go in input)
        {
            Metadata metadata = go.GetComponent<Metadata>();

            if (metadata != null)
            {
                if (metadata.containsProperty(idPropertyName))
                {
                    string currentGUID = metadata.getProperty(idPropertyName);
                    if (data.elements.ContainsKey(currentGUID))
                    {
                        bool isMirrored = data.elements[currentGUID].mirror;
                        metadata.addOrSetProperty("Mirrored", isMirrored.ToString());
                    }
                }
            }
        }

        // Once Mirrored is calculated, use it to scale in Y axis those beeing true
        // Then correct pivot using difference in bounds (after being scaled)

        // Create a dictionary by Mesh reference
        // key: string ("Family and Type" from metadata)
        // value: List<GameObject>
        Dictionary<string, List<GameObject>> instanceKinds = new Dictionary<string, List<GameObject>>();

        // Traverse selection and fill dictionary
        foreach (GameObject gameObject in input)
        {
            Metadata metadata = gameObject.GetComponent<Metadata>();
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf != null && metadata != null)
            {
                Mesh currentMesh = mf.sharedMesh;
                if (CheckIfMetadataContainsEveryProperty(metadata, instanceClassifierPropertyNames.ToList()) &&
                    metadata.containsProperty(idPropertyName))
                {
                    string instanceKindKey = GetInstanceKindKey(metadata, instanceClassifierPropertyNames.ToList());

                    if (!instanceKinds.ContainsKey(instanceKindKey))
                    {
                        // Create new entry for currentMesh
                        instanceKinds.Add(instanceKindKey, new List<GameObject>());
                    }
                    // Add gameObject to contained list
                    instanceKinds[instanceKindKey].Add(gameObject);
                }
            }
        }

        foreach (var kvp in instanceKinds)
        {
            string key = kvp.Key;
            List<GameObject> gameObjects = kvp.Value;

            Mesh replaceMesh = gameObjects.First().GetComponent<MeshFilter>().sharedMesh;
            Metadata refMetadata = gameObjects.First().GetComponent<Metadata>();
            bool refMirrored = bool.Parse(refMetadata.getProperty("Mirrored"));

            // SKIP FIRST as it is used as reference
            for (int i = 1; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                MeshFilter currentMeshFilter = gameObject.GetComponent<MeshFilter>();

                if (currentMeshFilter.sharedMesh != replaceMesh)
                {
                    Metadata metadata = gameObject.GetComponent<Metadata>();
                    bool mirrored = bool.Parse(metadata.getProperty("Mirrored"));

                    Vector3 currentBoundsCenter = currentMeshFilter.sharedMesh.bounds.center;
                    if (mirrored != refMirrored)
                    {
                        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, reflectionVector);
                        currentBoundsCenter = Vector3.Scale(currentBoundsCenter, reflectionVector);
                        //gameObject.AddComponent<FixNegativeScale>();

                    }
                    Vector3 refBoundsCenter = replaceMesh.bounds.center;
                    Vector3 meshOffset_M = (refBoundsCenter - currentBoundsCenter) * 0.5f;

                    gameObject.transform.localPosition += meshOffset_M;

                    // Set reference mesh
                    string previousMeshName = currentMeshFilter.sharedMesh.name;
                    string newMeshName = replaceMesh.name;

                    currentMeshFilter.sharedMesh = replaceMesh;
                    metadata.addOrSetProperty("Viroo Fixed Mesh", $"From {previousMeshName} -> To {newMeshName} offset [{meshOffset_M.x:F6},{meshOffset_M.y:F6},{meshOffset_M.z:F6} ]");

                }
            }
        }
        return input;
    }

    string GetInstanceKindKey(Metadata metadata, List<string> propertyNames)
    {
        string key = "";
        foreach (string name in propertyNames)
        {
            string value = metadata.getProperty(name);
            key = key + $"[{value}]";
        }
        return key;
    }

    bool CheckIfMetadataContainsEveryProperty(Metadata metadata, List<string> propertyNames)
    {
        bool result = true;
        foreach (string name in propertyNames)
        {
            if (!metadata.containsProperty(name))
            {
                result = false; 
                break;
            }
        }
        return result;
    }

}

[System.Serializable]
public class CSV_Data
{
    public Dictionary<string, CSV_Entry> elements;

    public CSV_Data()
    {
        elements = new Dictionary<string, CSV_Entry>();
    }

    public void ReadCSV(string csvContentText)
    {
        string[] lines = csvContentText.Split("\n"[0]);

        // Skip header, 0 entry
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] values = line.Split(';');

            if (values.Length == 8)
            {
                string ifcGUID = values[0];
                float posX = float.Parse(values[1]);
                float posY = float.Parse(values[2]);
                float posZ = float.Parse(values[3]);

                float eulerX = float.Parse(values[4]);
                float eulerY = float.Parse(values[5]);
                float eulerZ = float.Parse(values[6]);

                bool mirrored = bool.Parse(values[7]);

                Vector3 pos = new Vector3(posX, posY, posZ);
                Vector3 euler = new Vector3(eulerX, eulerY, eulerZ);

                CSV_Entry entry = new CSV_Entry() { position = pos, eulerRotation = euler, mirror = mirrored };

                elements.Add(ifcGUID, entry);
            }
        }
    }
}

[System.Serializable]
public class CSV_Entry
{
    public Vector3 position;
    public Vector3 eulerRotation;
    public bool mirror;
}
#endif