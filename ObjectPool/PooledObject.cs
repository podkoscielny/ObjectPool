using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AoOkami.ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        public PoolTags PoolTag { get; private set; }

        public void SetPoolTag(PoolTags tag) => PoolTag = tag;
    }
}