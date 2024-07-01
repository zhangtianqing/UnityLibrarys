using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Dll.UnityUtils.UnityTimer
{

    /// <summary>
    /// Manages updating all the <see cref="global::UnityTimer"/>s that are running in the application.
    /// This will be instantiated the first time you create a timer -- you do not need to add it into the
    /// scene manually.
    /// </summary>
    public class TimerManager : MonoBehaviour
    {
        private List<global::UnityTimer> _timers = new List<global::UnityTimer>();

        // buffer adding timers so we don't edit a collection during iteration
        private List<global::UnityTimer> _timersToAdd = new List<global::UnityTimer>();

        public void RegisterTimer(global::UnityTimer timer)
        {
            _timersToAdd.Add(timer);
        }

        public void CancelAllTimers()
        {
            foreach (global::UnityTimer timer in _timers)
            {
                timer.Cancel();
            }

            _timers = new List<global::UnityTimer>();
            _timersToAdd = new List<global::UnityTimer>();
        }

        public void PauseAllTimers()
        {
            foreach (global::UnityTimer timer in _timers)
            {
                timer.Pause();
            }
        }

        public void ResumeAllTimers()
        {
            foreach (global::UnityTimer timer in _timers)
            {
                timer.Resume();
            }
        }

        // update all the registered timers on every frame
        [UsedImplicitly]
        private void Update()
        {
            UpdateAllTimers();
        }

        private void UpdateAllTimers()
        {
            if (_timersToAdd.Count > 0)
            {
                _timers.AddRange(_timersToAdd);
                _timersToAdd.Clear();
            }

            foreach (global::UnityTimer timer in _timers)
            {
                timer.Update();
            }

            _timers.RemoveAll(t => t.isDone);
        }
    }
}