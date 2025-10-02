using System;

namespace SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature
{
    [Serializable]
    public struct WeightedItem<T>
    {
        public T Item;
        public int Weight;
    }
}