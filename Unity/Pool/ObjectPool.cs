using System;
using System.Collections.Generic;

namespace Pool
{
    /// <summary>
    /// 泛型对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : class, new()
    {
        private readonly List<T> list;
        private int maxSize;
        private Func<T> onElementCreate;
        private Action<T> onElementGet;
        private Action<T> onElementRelease;
        private Action<T> onElementDestroy;

        public ObjectPool(Func<T> onElementCreate, Action<T> onElementGet, Action<T> onElementRelease, Action<T> onElementDestroy, int defaultCapacity = 10, int maxSize = 1000)
        {
            list = new List<T>(defaultCapacity);
            this.onElementCreate = onElementCreate;
            this.onElementGet = onElementGet;
            this.onElementRelease = onElementRelease;
            this.onElementDestroy = onElementDestroy;
            this.maxSize = maxSize;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns>T对象</returns>
        public T Get()
        {
            T obj;
            if (list.Count == 0)
            {
                obj = onElementCreate();
            }
            else
            {
                int index = list.Count - 1;
                obj = list[index];
                list.RemoveAt(index);
            }
            onElementGet?.Invoke(obj);
            return obj;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj"></param>
        public void Release(T obj)
        {
            onElementRelease?.Invoke(obj);
            if (list.Count < maxSize)
            {
                list.Add(obj);
            }
            else
            {
                onElementDestroy?.Invoke(obj);
            }
        }

        /// <summary>
        /// 清除对象池
        /// </summary>
        public void Clear()
        {
            if (onElementDestroy != null)
            {
                foreach (T obj in list)
                    onElementDestroy(obj);
            }

            list.Clear();
        }
    }
}