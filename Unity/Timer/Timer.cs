using System;
using UnityEngine;

namespace Framework.Timer
{
    public class Timer
    {
        #region Public Properties/Fields

        public float Duration { get; }
        public bool IsLooped { get; }
        public bool IsCompleted { get; private set; }
        public bool UsesRealTime { get; }

        public bool IsPaused => timeElapsedBeforePause.HasValue;
        public bool IsCancelled => timeElapsedBeforeCancel.HasValue;
        public bool IsDone => IsCompleted || IsCancelled;

        #endregion

        #region Private Properties/Fields

        private readonly Action onComplete;
        private readonly Action<float> onUpdate;
        private float startTime;
        private float lastUpdateTime;

        // for pausing, we push the start time forward by the amount of time that has passed.
        // this will mess with the amount of time that elapsed when we're cancelled or paused if we just
        // check the start time versus the current world time, so we need to cache the time that was elapsed
        // before we paused/cancelled
        private float? timeElapsedBeforeCancel;
        private float? timeElapsedBeforePause;

        #endregion

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        /// <param name="duration">定时器持续时间</param>
        /// <param name="onComplete">结束回调</param>
        /// <param name="onUpdate">更新回调</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="usesRealTime">是否受Timescale影响</param>
        public Timer(float duration, Action onComplete, Action<float> onUpdate, bool isLooped, bool usesRealTime)
        {
            Duration = duration;
            this.onComplete = onComplete;
            this.onUpdate = onUpdate;

            IsLooped = isLooped;
            UsesRealTime = usesRealTime;

            startTime = GetWorldTime();
            lastUpdateTime = startTime;
        }

        #region Private Methods

        private float GetWorldTime()
        {
            return UsesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        private float GetFireTime()
        {
            return startTime + Duration;
        }

        private float GetTimeDelta()
        {
            return GetWorldTime() - lastUpdateTime;
        }

        public void Update()
        {
            if (IsDone)
            {
                return;
            }

            if (IsPaused)
            {
                startTime += GetTimeDelta();
                lastUpdateTime = GetWorldTime();
                return;
            }

            lastUpdateTime = GetWorldTime();

            onUpdate?.Invoke(GetTimeElapsed());

            if (!(GetWorldTime() >= GetFireTime()))
            {
                return;
            }

            onComplete?.Invoke();

            if (IsLooped)
            {
                startTime = GetWorldTime();
            }
            else
            {
                IsCompleted = true;
            }
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Stop a timer that is in-progress or paused. The timer's on completion callback will not be called.
        /// </summary>
        public void Cancel()
        {
            if (IsDone)
            {
                return;
            }

            timeElapsedBeforeCancel = GetTimeElapsed();
            timeElapsedBeforePause = null;
        }

        /// <summary>
        /// Pause a running timer. A paused timer can be resumed from the same point it was paused.
        /// </summary>
        public void Pause()
        {
            if (IsPaused || IsDone)
            {
                return;
            }

            timeElapsedBeforePause = GetTimeElapsed();
        }

        /// <summary>
        /// Continue a paused timer. Does nothing if the timer has not been paused.
        /// </summary>
        public void Resume()
        {
            if (!IsPaused || IsDone)
            {
                return;
            }

            timeElapsedBeforePause = null;
        }

        /// <summary>
        /// Get how many seconds have elapsed since the start of this timer's current cycle.
        /// </summary>
        /// <returns>The number of seconds that have elapsed since the start of this timer's current cycle, i.e.
        /// the current loop if the timer is looped, or the start if it isn't.
        ///
        /// If the timer has finished running, this is equal to the duration.
        ///
        /// If the timer was cancelled/paused, this is equal to the number of seconds that passed between the timer
        /// starting and when it was cancelled/paused.</returns>
        public float GetTimeElapsed()
        {
            if (IsCompleted || GetWorldTime() >= GetFireTime())
            {
                return Duration;
            }

            return timeElapsedBeforeCancel ??
                   timeElapsedBeforePause ??
                   GetWorldTime() - startTime;
        }

        /// <summary>
        /// Get how many seconds remain before the timer completes.
        /// </summary>
        /// <returns>The number of seconds that remain to be elapsed until the timer is completed. A timer
        /// is only elapsing time if it is not paused, cancelled, or completed. This will be equal to zero
        /// if the timer completed.</returns>
        public float GetTimeRemaining()
        {
            return Duration - GetTimeElapsed();
        }

        /// <summary>
        /// Get how much progress the timer has made from start to finish as a ratio.
        /// </summary>
        /// <returns>A value from 0 to 1 indicating how much of the timer's duration has been elapsed.</returns>
        public float GetRatioComplete()
        {
            return GetTimeElapsed() / Duration;
        }

        /// <summary>
        /// Get how much progress the timer has left to make as a ratio.
        /// </summary>
        /// <returns>A value from 0 to 1 indicating how much of the timer's duration remains to be elapsed.</returns>
        public float GetRatioRemaining()
        {
            return GetTimeRemaining() / Duration;
        }

        #endregion
        
    }
}