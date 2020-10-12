using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using PartitionedScene;

namespace PartitionedSceneEditor
{
    [CustomEditor(typeof(PartitionManager))]
    public class PartitionManagerEditor : Editor
    {
        int rows = 3;
        int columns = 3;

        bool createScenes = false;

        float side = 1000f;

        float triggerPadding = 200f;

        string tag;
        int layer;

        public override void OnInspectorGUI()
        {
            rows = EditorGUILayout.DelayedIntField("Rows", rows);
            columns = EditorGUILayout.DelayedIntField("Columns", columns);
            createScenes = EditorGUILayout.Toggle("Create scenes", createScenes);
            tag = EditorGUILayout.TagField("Tag", tag);
            layer = EditorGUILayout.LayerField("Layer", layer);
            if (GUILayout.Button("Generate"))
            {
                Scene rootScene = EditorSceneManager.GetActiveScene();
                Component component = this.target as Component;
                Transform transform = component.transform;
                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < columns; y++)
                    {
                        string[] path = rootScene.path.Split(char.Parse("/"));
                        path[path.Length - 1] = "Partition_(" + x + "," + y + ")";
                        string sceneName = string.Join("/", path);

                        GameObject go = new GameObject("PartitionTrigger_(" + x + "," + y + ")");
                        go.transform.position = new Vector3(x * side, 0, y * side);
                        go.transform.parent = transform;
                        if(tag != null && tag != string.Empty) {
                            go.tag = tag;
                        }
                        go.layer = layer;
                        
                        PartitionTrigger pt = go.AddComponent(typeof(PartitionTrigger)) as PartitionTrigger;
                        pt.sceneName = sceneName.Substring("Assets/".Length);
                        pt.side = side;

                        BoxCollider collider = go.AddComponent(typeof(BoxCollider)) as BoxCollider;
                        collider.size = new Vector3(side + 2 * triggerPadding, side + 2 * triggerPadding, side + 2 * triggerPadding);
                        collider.isTrigger = true;

                        if (createScenes)
                        {
                            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                            EditorSceneManager.SaveScene(scene, sceneName + ".unity");
                            EditorSceneManager.SetActiveScene(rootScene);
                        }
                    }
                }
            }
        }
    }
}
