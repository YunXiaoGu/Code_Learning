/// <summary>
/// 泛型单例类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class, new()
{
    // .NET Framework 4.0+/.NET	Core 1.0+ Lazy<T>
    // private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
    // public static T Instance => lazy.Value;

    // 经典写法
    private static readonly object locker = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }

            return instance;
        }
    }
}