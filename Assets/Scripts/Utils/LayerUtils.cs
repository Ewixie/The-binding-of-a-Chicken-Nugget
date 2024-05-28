using UnityEngine;

namespace Utils
{
    public class LayerUtils
    {
        
        public static bool IsInLayerMask(int layer, LayerMask mask)
        {
            return (mask & (1 << layer)) != 0;
        }
    }
}