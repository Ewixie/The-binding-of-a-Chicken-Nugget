using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        private float _pausedTimeScale = 1f;
        public static float UnpausedDeltaTime => GameManager.Instance.IsPaused ? 0 : Time.deltaTime;
        public static float UnpausedUnscaledDeltaTime => GameManager.Instance.IsPaused ? 0 : Time.unscaledDeltaTime;
        
        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.Paused += OnPause;
            GameManager.Instance.Unpaused += OnUnpause;
        }

        private void OnPause()
        {
            _pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnUnpause()
        {
            Time.timeScale = _pausedTimeScale;
        }

        public static IEnumerator WaitGameTime(float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                elapsedTime += UnpausedDeltaTime;
                yield return null;
            }
        }
        
        public static IEnumerator WaitRealGameTime(float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                elapsedTime += UnpausedUnscaledDeltaTime;
                yield return null;
            }
        }

        public static IEnumerator WaitFixedGameTime(float time, float fixedStep = 0.02f)
        {
            if (fixedStep <= 0)
            {
                throw new ArgumentException("IDI NAHUI");
            }

            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                if (GameManager.Instance.IsPaused)
                {
                    yield return null;
                    continue;
                }

                elapsedTime += fixedStep;
                yield return new WaitForSeconds(fixedStep);
            }
        }
    }
}