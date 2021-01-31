#nullable enable
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    sealed class Title : MonoBehaviour
    {
        [SerializeField] private GameObject m_ButtonNew;
        [SerializeField] private GameObject m_ButtonContinue;
        [SerializeField] private GameObject m_ButtonSelect;

        [System.NonSerialized]
        public string? m_NextSceneName;

        private void Awake()
        {
            Resolver.Register(this);
        }

        private void OnDestroy()
        {
            Resolver.Unregister<Title>();
        }

        public async UniTask<string> SetupAndWait(bool isFirst)
        {
            var ct = this.GetCancellationTokenOnDestroy();

            m_NextSceneName = null;
            m_ButtonNew.SetActive(isFirst);
            m_ButtonContinue.SetActive(!isFirst);
            //m_ButtonSelect.SetActive(!isFirst);
            m_ButtonSelect.SetActive(false);

            while (m_NextSceneName == null)
            {
                await UniTask.NextFrame(cancellationToken: ct);
            }

            return m_NextSceneName;
        }

        public void OnClick(string nextSceneName)
        {
            m_NextSceneName = nextSceneName;
        }
    }
}
