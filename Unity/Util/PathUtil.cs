using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Util
{
    public static class PathUtil
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder(100);

        /// <summary>
        /// 多个路径组合成一个路径，默认以第一个路径为根路径
        /// </summary>
        /// <param name="paths">多个路径</param>
        /// <returns>拼接成的路径或<see cref="string.Empty"/></returns>
        [CanBeNull]
        public static string Combine(params string[] paths)
        {
            if (paths is null)
            {
                return string.Empty;
            }

            List<string> list = new List<string>();
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }
                foreach (var str in path.Split('/'))
                {
                    if ("/".Equals(str) || "\\".Equals(str) || list.Contains(str))
                    {
                        continue;
                    }

                    list.Add(str);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i]);
                if (i < (list.Count - 1))
                {
                    stringBuilder.Append("/");
                }
            }

            string fullPath = stringBuilder.ToString();
            stringBuilder.Clear();
            return fullPath;
        }
    }
}