﻿using System;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public event Action Paused;
        public event Action Unpaused;
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        [Button]
        public void Pause()
        {
            IsPaused = true;
            Paused?.Invoke();
        }
        
        [Button]
        public void Unpause()
        {
            IsPaused = true;
            Unpaused?.Invoke();
        }
    }
}