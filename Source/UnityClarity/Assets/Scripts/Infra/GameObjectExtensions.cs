using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using UnityEngine;

namespace Assets.Scripts.Infra
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> EnumerateChildren(this GameObject obj)
        {
            foreach (var childTransform in obj.transform)
                yield return ((Transform)childTransform).gameObject;
        }

        public static IEnumerable<GameObject> EnumerateDeep(this GameObject obj)
        {
            return obj.EnumSelf().Concat(obj.EnumerateChildren().SelectMany(x => x.EnumerateDeep()));
        }
    }
}