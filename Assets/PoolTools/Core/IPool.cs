using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolTools
{
    public interface IPool
    {
        bool RecyclePoolable(int atIndex);
    }
}