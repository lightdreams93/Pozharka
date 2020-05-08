using System.Text;
using UnityEngine;
using UnityEditor;

class GenerateGuids
{
    [MenuItem("Tools/GenerateGuids")]
    static void Generate()
    {
        Object[] resourcesObjects = Resources.LoadAll("Items");

        foreach (var obj in resourcesObjects)
        {
            string guid;
            long file;

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out guid, out file))
            {
                ItemConfig objects = obj as ItemConfig;
                if (objects) objects.guid = guid;
                if(objects) EditorUtility.SetDirty(objects);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}