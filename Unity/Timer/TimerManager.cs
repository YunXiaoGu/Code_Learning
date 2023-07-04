using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Timer
{
    public class TimerManager : MonoBehaviour
    {
        private List<Timer> _timers = new List<Timer>();

        // buffer adding timers so we don't edit a collection during iteration
        private List<Timer> _timersToAdd = new List<Timer>();

        private static TimerManager _instance;

        public static TimerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject { name = "TimerManager" };
                    _instance = managerObject.AddComponent<TimerManager>();
                }

                return _instance;
            }
        }

        public Timer CreateTimer(float duration, Action onComplete, Action<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            Timer timer = new Timer(duration, onComplete, onUpdate, isLooped, useRealTime);
            RegisterTimer(timer);
            return timer;
        }

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

        private void Update()
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

            _timers.RemoveAll(t => t.IsDone);
        }
    }
}