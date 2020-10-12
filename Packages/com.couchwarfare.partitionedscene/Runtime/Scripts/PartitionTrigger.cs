using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PartitionedScene
{
    public enum LoaderStatus
    {
        NotLoaded,
        Loading,
        Loaded
    }
    public class PartitionTrigger : MonoBehaviour
    {
        public string sceneName;
        public float side;
        public LoaderStatus status = LoaderStatus.NotLoaded;

        void Start()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (status == LoaderStatus.NotLoaded)
            {
                status = LoaderStatus.Loading;
                StartCoroutine(LoadAsyncScene());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (status != LoaderStatus.NotLoaded)
            {
                status = LoaderStatus.NotLoaded;
                StartCoroutine(UnloadAsyncScene());
            }
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(side, side, side));
        }

        IEnumerator LoadAsyncScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(this.sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            status = LoaderStatus.Loaded;
        }

        IEnumerator UnloadAsyncScene()
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(this.sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
