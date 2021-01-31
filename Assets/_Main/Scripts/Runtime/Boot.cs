#nullable enable
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

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
    }
}
