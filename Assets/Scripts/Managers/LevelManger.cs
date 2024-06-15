using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Managers
{
    public class LevelManger : MonoBehaviour
    {
        public static LevelManger Instance;

        [SerializeField] private SceneField playerScene;
        [SerializeField] private SceneField startingStageScene;

        private bool _loadingNewStage;
        
        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadNewGame()
        {
            SceneManager.LoadScene(playerScene);
            SceneManager.LoadScene(startingStageScene, LoadSceneMode.Additive);
        }

        public void LoadNewStage()
        {
            if (_loadingNewStage) return;
            StartCoroutine(LoadNewStageRoutine());
        }

        private IEnumerator LoadNewStageRoutine()
        {
            _loadingNewStage = true;
            var ao = SceneManager.UnloadSceneAsync(startingStageScene);
            yield return ao;            
            SceneManager.LoadScene(startingStageScene, LoadSceneMode.Additive);
            _loadingNewStage = false;
        }
        
        public static bool IsSceneLoaded(string sceneName)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name != sceneName) continue;
                return true;
            }
            return false;
        }
        
        public static bool IsSceneLoaded(SceneField scene)
        {
            return IsSceneLoaded(scene.SceneName);
        }
    }
}
