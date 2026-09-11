using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HVO.Scripts.UI
{
    public class PointToClick : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Configs")]
        [SerializeField] private float _delayBeforeFade = 0.4f;
        [SerializeField, Min(0f)] private float _fadeDuration = 0.2f;

        private Action<PointToClick> _onCompleted;
        private CancellationTokenSource _cts;
        private Color _initialColor;

        private void Awake()
        {
            _initialColor = _spriteRenderer.color;
        }

        public void Play(Action<PointToClick> onCompleted)
        {
            CancelPendingRelease();

            _onCompleted = onCompleted;
            _spriteRenderer.color = _initialColor;

            _cts = new CancellationTokenSource();
            FadeOutAndRelease(_cts).Forget();
        }

        private async UniTask FadeOutAndRelease(CancellationTokenSource source)
        {
            var token = source.Token;

            try
            {
                var elapsedTime = 0f;
                await UniTask.WaitForSeconds(_delayBeforeFade, cancellationToken: token);

                while (elapsedTime < _fadeDuration)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);

                    elapsedTime += Time.deltaTime;

                    var progress = _fadeDuration > 0f ? Mathf.Clamp01(elapsedTime / _fadeDuration) : 1f;
                    var color = _initialColor;

                    color.a = Mathf.Lerp(_initialColor.a, 0f, progress);
                    _spriteRenderer.color = color;
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (_cts != source) return;

            var callback = _onCompleted;

            _onCompleted = null;
            _cts = null;
            source.Dispose();

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