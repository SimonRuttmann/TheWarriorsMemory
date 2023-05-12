using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>( this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source) 
                action(element);
        }

        public static void Add<T>(this IEnumerable<T> source, T element)
        {
            source.ToList().Add(element);
        }
        
        public static void AddAll<T>(this IList<T> source, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                source.Add(element);
            }
        }
        
        public static void AddAll<T>(this ISet<T> source, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                source.Add(element);
            }
        }

        
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
        
        public static void Clear<T>(this IEnumerable<T> source)
        {
            source.ToList().Clear();
        }
    }
}