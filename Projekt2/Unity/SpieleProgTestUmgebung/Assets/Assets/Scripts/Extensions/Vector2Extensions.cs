using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class Vector2Extensions
    {
        
        public static Vector3 ToVector3(this Vector2Int vector2d, float height)
        {
            return new Vector3(vector2d.x, height, vector2d.y);
        }
        
        public static Vector3 ToVector3(this Vector2 vector2d, float height)
        {
            return new Vector3(vector2d.x, height, vector2d.y);
        }

        public static IEnumerable<Vector3> ToVector3(this IEnumerable<Vector2Int> collection2d, float height)
        {
            return collection2d.Select(element => new Vector3(element.x, height, element.y));
        }
        
        public static IEnumerable<Vector3> ToVector3(this IEnumerable<Vector2> collection2d, float height)
        {
            return collection2d.Select(element => new Vector3(element.x, height, element.y));
        }

    }
}