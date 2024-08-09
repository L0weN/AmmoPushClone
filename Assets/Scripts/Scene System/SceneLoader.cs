using Mert.EventBus;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Mert.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] Image loadingBar;
        [SerializeField] float fillSpeed = 0.5f;
        [SerializeField] Canvas loadingCanvas;
        [SerializeField] Camera loadingCamera;
        [SerializeField] SceneGroup[] sceneGroups;

        float targetProgress;
        bool isLoading;

        private int currentSceneIndex = 0;

        EventBinding<GameStateChanged> gameStateChangedEventBinding;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        private void Awake()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        private void OnEnable()
        {
            gameStateChangedEventBinding = new EventBinding<GameStateChanged>(HandleGameState);
            EventBus<GameStateChanged>.Register(gameStateChangedEventBinding);
        }

        private void OnDisable()
        {
            EventBus<GameStateChanged>.Unregister(gameStateChangedEventBinding);
        }

        private async void HandleGameState(GameStateChanged gameStateChanged)
        {
            switch (gameStateChanged.State)
            {
                case GameState.PRECOLLECT:
                    CheckPlayerLevel();
                    await LoadSceneGroup(currentSceneIndex);
                    break;
                case GameState.PREBOSS:
                    await LoadSceneGroup(3);
                    break;
            }
        }

        private void Update()
        {
            if (!isLoading) return;

            float currentFillAmount = loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - targetProgress);

            float dynamicFillSpeed = progressDifference * fillSpeed;

            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroup(int index)
        {
            loadingBar.fillAmount = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index." + index);
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);

            EnableLoadingCanvas();
            await manager.LoadScenes(sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        void EnableLoadingCanvas(bool enable = true)
        {
            isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            loadingCamera.gameObject.SetActive(enable);
        }

        private void CheckPlayerLevel()
        {
            int playerLevel = GameResources.Instance.GetPlayerLevel();
            currentSceneIndex = playerLevel < 5 ? 0 : (playerLevel >= 5 && playerLevel <= 10) ? 1 : 2;
        }
    }
}