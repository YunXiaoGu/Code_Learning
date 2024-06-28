using System;
using System.Collections.Generic;
using Singleton;

namespace Timer
{
    /// <summary>
    /// 定时器
    /// <para>依赖MonoBehaviour Update()来实现定时器更新</para>
    /// </summary>
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private List<Timer> timers = new List<Timer>();

        // buffer adding timers so we don't edit a collection during iteration
        private List<Timer> timersToAdd = new List<Timer>();

        public Timer CreateTimer(float duration, Action onComplete, Action<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            Timer timer = new Timer(duration, onComplete, onUpdate, isLooped, useRealTime);
            RegisterTimer(timer);
            return timer;
        }

        public void RegisterTimer(Timer timer)
        {
            timersToAdd.Add(timer);
        }

        public void CancelAllTimers()
        {
            foreach (Timer timer in timers)
            {
                timer.Cancel();
            }

            timers = new List<Timer>();
            timersToAdd = new List<Timer>();
        }

        public void PauseAllTimers()
        {
            foreach (Timer timer in timers)
            {
                timer.Pause();
            }
        }

        public void ResumeAllTimers()
        {
            foreach (Timer timer in timers)
            {
                timer.Resume();
            }
        }

        private void Update()
        {
            if (timersToAdd.Count > 0)
            {
                timers.AddRange(timersToAdd);
                timersToAdd.Clear();
            }

            foreach (Timer timer in timers)
            {
                timer.Update();
            }

            timers.RemoveAll(t => t.IsDone);
        }
    }
}