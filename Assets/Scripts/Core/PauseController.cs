using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IPauseProvider
    {
        bool IsPaused { get; }
        void Register(IPauseHandler handler);
        void Unregister(IPauseHandler handler);
        void SetPaused(bool isPaused);
    }

    public class PauseController : IPauseProvider
    {
        private readonly LinkedList<IPauseHandler> _pauseHandlers = new LinkedList<IPauseHandler>();
        public bool IsPaused { get; private set; }

        void IPauseProvider.Register(IPauseHandler handler)
        {
            if (!_pauseHandlers.Contains(handler))
            {
                _pauseHandlers.AddLast(handler);
            }
        }
        void IPauseProvider.Unregister(IPauseHandler handler)
        {
            _pauseHandlers.Remove(handler);
        }
        void IPauseProvider.SetPaused(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f; // Temporal
            IsPaused = isPaused;
            foreach (IPauseHandler handler in _pauseHandlers)
            {
                handler.SetPaused(isPaused);
            }
        }    
    }
}
