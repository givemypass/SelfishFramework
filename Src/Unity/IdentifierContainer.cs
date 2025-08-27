using System;
using SelfishFramework.Src.Core;
using TriInspector;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    [CreateAssetMenu(fileName = "identifier", menuName = "Identifiers/Identifier")]
    public class IdentifierContainer : ScriptableObject, IEquatable<IdentifierContainer>
    {
        [SerializeField, ReadOnly] private int _id;

        public static implicit operator int(IdentifierContainer lhs) => lhs.Id;

        public int Id
        {
            get
            {
                if (_id == 0)
                    _id = name.GenerateIndex();
                return _id;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is IdentifierContainer container &&
                   Id == container.Id;
        }

        public bool Equals(IdentifierContainer other)
        {
            if (other == null)
                return false;

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }

        [Button]
        private void OnValidate()
        {
            _id = name.GenerateIndex();
        }
    }
}