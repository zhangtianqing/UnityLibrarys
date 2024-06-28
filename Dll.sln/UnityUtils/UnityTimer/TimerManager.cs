using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dll.UnityUtils.UnityTimer
{

    /// <summary>
    /// Manages updating all the <see cref="Timer"/>s that are running in the application.
    /// This will be instantiated the first time you create a timer -- you do not need to add it into the
    /// scene manually.
    /// </summary>
    public class TimerManager : MonoBehaviour
    {
        private List<Timer> _timers = new List<Timer>();

        // buffer adding timers so we don't edit a collection during iteration
        private List<Timer> _timersToAdd = new List<Timer>();

        public void RegisterTimer(Timer timer)
        {
            _timersToAdd.Add(timer);
        }

        public void CancelAllTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Cancel();
            }

            _timers = new List<Timer>();
            _timersToAdd = new List<Timer>();
        }

        public void PauseAllTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Pause();
            }
        }

        public void ResumeAllTimers()
        {
            foreach (Timer timer in _timers)
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

            foreach (Timer timer in _timers)
            {
                timer.Update();
            }

            _timers.RemoveAll(t => t.isDone);
        }
    }
}