using System;
using System.Linq;
using UnityEngine;

namespace SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature
{
    public class TemperatureWeightedRandom<T>
    {
        private readonly WeightedItem<T>[] _items;
        private float[] _recalculatedWeights;
        private float _temperature;

        public TemperatureWeightedRandom(WeightedItem<T>[] items, bool initialize = true, float initializeTemperature = 1)
        {
            _items = items;
            if (initialize)
            {
                CalculateProbabilities(initializeTemperature);
                _temperature = initializeTemperature;
            }
        }
        
        public float Temperature
        {
            get => _temperature;
            set => CalculateProbabilities(value);
        }

        public T Next()
        {
            if(_items.Length == 0)
                throw new Exception("No items to sample from.");
            if (_recalculatedWeights == null)
                throw new Exception($"Probabilities not calculated. Call {nameof(CalculateProbabilities)} first.");
            
            var totalWeight = _recalculatedWeights.Sum();
            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            for (var i = 0; i < _items.Length; i++)
            {
                randomValue -= _recalculatedWeights[i];
                if (randomValue <= 0)
                {
                    return _items[i].Item;
                }
            }
            return _items[^1].Item;
        }

        private void CalculateProbabilities(float temperature)
        {
            if (temperature < 0)
                throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature must be non-negative.");
            if (_items.Length == 0)
                throw new Exception("No items to sample from.");
            _temperature = temperature;
            _recalculatedWeights ??= new float[_items.Length];
            var maxWeight = _items.Max(a => a.Weight);
            if (temperature == 0)
            {
                for (var i = 0; i < _items.Length; i++)
                {
                    _recalculatedWeights[i] = _items[i].Weight == maxWeight ? 1 : 0;
                }

                return;
            }

            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                var newWeight = (float)item.Weight / maxWeight;
                newWeight = Mathf.Log(newWeight) / temperature;
                _recalculatedWeights[i] = newWeight;
            }
            var maxRecalculatedWeight = _recalculatedWeights.Max();
            for (var i = 0; i < _items.Length; i++)
            {
                _recalculatedWeights[i] = Mathf.Exp(_recalculatedWeights[i] - maxRecalculatedWeight); 
            }
        }
    }
}