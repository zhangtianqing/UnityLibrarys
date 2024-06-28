

using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using Dll.UnityUtils;
using System.Linq;
using UnityEngine;

namespace UnityUtils
{
    public static class RandomUtils
    {
        public static bool RandomHit(float percent = 0.5f)
        {
            return UnityEngine.Random.Range(0f, 1f) < percent;
        }

        public static string RandomStr(int size)
        {
            var random = new System.Random((int)DateTime.Now.Ticks);

            var builder = new System.Text.StringBuilder();

            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        public static int RandomInt(int min, int max)
        {
            var random = new  System.Random((int)DateTime.Now.Ticks);
            return random.Next(min, max);
        }

        public static T RandomItem<T>(this List<T> list)
        {
            var i = UnityEngine.Random.Range(0, list.Count);

            if (i < 0 || i >= list.Count)
                return default(T);

            return list[i];
        }
        #region 数组
        /// <summary>
        /// 数组的2个元素位置调换
        /// </summary>
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index2];
            array[index2] = array[index1];
            array[index1] = temp;
        }
        /// <summary>
        /// 列表的2个元素位置调换
        /// </summary>
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index2];
            list[index2] = list[index1];
            list[index1] = temp;
        }

        /// <summary>
        /// 乱序排序数组
        /// </summary>
        public static void SortRandom<T>(this T[] array)
        {
            int randomIndex;
            for (int i = array.Length - 1; i > 0; i--)
            {
                randomIndex = Random.Range(0, i);
                array.Swap(randomIndex, i);
            }
        }
        /// <summary>
        /// 乱序排序列表
        /// </summary>
        public static void SortRandom<T>(this List<T> list)
        {
            int randomIndex;
            for (int i = list.Count - 1; i > 0; i--)
            {
                randomIndex = Random.Range(0, i);
                list.Swap(randomIndex, i);
            }
        }
        public static List<int> RandomSortListIndex(int maxIndex, int takeCount = -1) { 
            List<int> list = new List<int>();
            for (int i = 0; i < maxIndex; i++) { 
                list.Add(i);
            }
            list.SortRandom();
            if (takeCount>=1)
            {
                return list.Take(takeCount).ToList();
            }
            return list;
        }
        #endregion
    }
}
