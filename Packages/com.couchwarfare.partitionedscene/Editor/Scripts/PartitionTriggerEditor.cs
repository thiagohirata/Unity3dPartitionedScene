using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using PartitionedScene;

namespace PartitionedSceneEditor
{
    [CustomEditor(typeof(PartitionTrigger))]
    [CanEditMultipleObjects]
    public class PartitionTriggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Scene"))
            {
                foreach(Object t in this.targets) {
                    PartitionTrigger target = t as PartitionTrigger;
                    Scene rootScene = EditorSceneManager.GetActiveScene();

                    Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    EditorSceneManager.SaveScene(scene, "Assets/" + target.sceneName + ".unity");
                }
            }
        }
    }
}
