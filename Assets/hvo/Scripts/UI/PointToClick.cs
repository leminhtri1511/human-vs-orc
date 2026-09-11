using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HVO.Scripts.UI
{
    public class PointToClick : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;

        private Action<PointToClick> _onCompleted;
        private CancellationTokenSource _cts;

        public void Play(Action<PointToClick> onCompleted)
        {
            CancelPendingRelease();

            _onCompleted = onCompleted;
            _cts = new CancellationTokenSource();

            ReleaseAfterDelay(_cts.Token).Forget();
        }

        private async UniTask ReleaseAfterDelay(CancellationToken token)
        {
            bool isCancelled = await UniTask
                .WaitForSeconds(_duration, cancellationToken: token)
                .SuppressCancellationThrow();

            if (isCancelled) return;

            var callback = _onCompleted;

            _onCompleted = null;

            _cts?.Dispose();
            _cts = null;

            callback?.Invoke(this);
        }

        private void OnDisable()
        {
            _onCompleted = null;
            CancelPendingRelease();
        }

        private void CancelPendingRelease()
        {
            if (_cts == null) return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}