using System;
using UnityEngine;

namespace KazatanGames.Framework
{
    public abstract class BaseIntroElement : MonoBehaviour
    {
        public abstract bool DestroyImmediatelyOnComplete { get; }
        public abstract bool IsComplete { get; }

        public event Action<BaseIntroElement> OnIntroElementComplete;

        private void Awake()
        {
            Initialise();
        }

        private void Update()
        {
            if (IsComplete)
            {
                OnIntroElementComplete?.Invoke(this);
                if (DestroyImmediatelyOnComplete) DestroyImmediate(this);
            } else
            {
                DoUpdate();
            }
        }

        protected abstract void Initialise();
        protected abstract void DoUpdate();
    }
}