using System;
using System.Globalization;
using UnityEngine;

/**
 * Pew Pew Pew 2023
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.PewPewPew2023
{
    [Serializable]
    public class SerializableVector2 : IEquatable<SerializableVector2>
    {
        public static SerializableVector2 zero = new(0f, 0f);

        public static implicit operator Vector2(SerializableVector2 sv) { return new(sv.x, sv.y); }
        public static implicit operator Vector3(SerializableVector2 sv) { return new(sv.x, sv.y); }
        public static implicit operator SerializableVector2(Vector2 v) { return new(v.x, v.y); }
        public static implicit operator SerializableVector2(Vector3 v) { return new(v.x, v.y); }

        public float x;
        public float y;

        /**
         * CONSTRUCTORS
         */
        public SerializableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public SerializableVector2(Vector2 input)
        {
            x = input.x;
            y = input.y;
        }
        public SerializableVector2(Vector3 input, bool useZ = false)
        {
            x = input.x;
            y = useZ ? input.z : input.y;
        }

        /**
         * IEquatable
         */
        public override bool Equals(object other)
        {
            if (other is SerializableVector2)
            {
                return Equals((SerializableVector2)other);
            }
            return false;
        }
        public bool Equals(SerializableVector2 other)
        {
            return other.x == x && other.y == y;
        }

        /// *listonly*
        public override string ToString()
        {
            return ToString(null, null);
        }

        // Returns a nicely formatted string for this vector.
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return string.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        // used to allow SerializableVector2s to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}