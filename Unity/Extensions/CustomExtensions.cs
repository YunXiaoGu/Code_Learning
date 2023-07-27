namespace CustomExtensions
{
    public static class CustomExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<int, T> callback)
        {
            int index = 0;
            foreach(var value in list)
            {
                action.Invoke(i, value);
                index += 1;
            }
        }
    }
}