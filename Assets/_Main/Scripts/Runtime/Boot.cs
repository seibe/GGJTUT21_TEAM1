#nullable enable
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Boot : MonoBehaviour
    {
        /// <summary>
        /// SynchronizationContext の上書き
        /// (<see href="https://github.com/Cysharp/UniTask#unitasksynchronizationcontext">LINK</see>)
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void OnLaunch()
        {
            SynchronizationContext.SetSynchronizationContext(new UniTaskSynchronizationContext());

            Analytics.enabled = false;
            Analytics.deviceStatsEnabled = false;
            Analytics.limitUserTracking = true;
            Analytics.initializeOnStartup = false;
        }

        private void Awake()
        {
            var playerLoop = UnityEngine.LowLevel.PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref playerLoop);

            Application.targetFrameRate = 60;
        }

        private async UniTaskVoid Start()
        {
            const string k_NextLevelIndex = "NEXT_LEVEL_INDEX";
            var ct = this.GetCancellationTokenOnDestroy();

            while (true)
            {
                var levelIndex = PlayerPrefs.GetInt(k_NextLevelIndex, 0);

                await SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive).WithCancellation(ct);

                var nextSceneName = await Resolver.Get<Title>().SetupAndWait(levelIndex == 0).WithCancellation(ct);

                _ = SceneManager.UnloadSceneAsync("Title");

                if (nextSceneName == "LevelSelect")
                {
                    // todo: ステージ選択画面
                }

                while (LevelData.TryGetLevel(levelIndex, out var levelData))
                {
                    await SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive).WithCancellation(ct);

                    Resolver.Get<TileDrawer>().UpdateTile(levelData);

                    await UniTask.WaitUntil(() => levelData.IsSuccess(), cancellationToken: ct);

                    // todo: クリア画面
                    await UniTask.Delay(500, cancellationToken: ct);

                    PlayerPrefs.SetInt(k_NextLevelIndex, ++levelIndex);

                    _ = SceneManager.UnloadSceneAsync("Level");
                }

                PlayerPrefs.DeleteKey(k_NextLevelIndex);
            }
        }
    }
}
