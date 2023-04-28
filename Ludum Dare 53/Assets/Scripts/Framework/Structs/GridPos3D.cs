namespace KazatanGames.Framework
{
    using System;
    // no UnityEngine using directive as it conflicts with System.Object

    public struct GridPos3D : IEquatable<GridPos3D>
    {
        public static implicit operator UnityEngine.Vector3(GridPos3D pos) { return pos.ToVector3(); }
        public static implicit operator UnityEngine.Vector3Int(GridPos3D pos) { return pos.ToVector3Int(); }

        public int x; // corresponds to default unity x axis
        public int y; // corresponds to default unity z axis
        public int z; // corresponds to default unity y axis

        public GridPos3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public GridPos3D(UnityEngine.Vector3Int input)
        {
            x = input.x;
            y = input.z;
            z = input.y;
        }
        public GridPos3D(UnityEngine.Vector3 input)
        {
            x = UnityEngine.Mathf.FloorToInt(input.x);
            y = UnityEngine.Mathf.FloorToInt(input.z);
            z = UnityEngine.Mathf.FloorToInt(input.y);
        }
        public GridPos3D(GridPos3D input)
        {
            x = input.x;
            y = input.y;
            z = input.z;
        }

        public static bool operator ==(GridPos3D gp1, GridPos3D gp2)
        {
            return gp1.Equals(gp2);
        }
        public static bool operator !=(GridPos3D gp1, GridPos3D gp2)
        {
            return !gp1.Equals(gp2);
        }
        public static GridPos3D operator -(GridPos3D gp1, GridPos3D gp2)
        {
            return new GridPos3D(gp1.x - gp2.x, gp1.y - gp2.y, gp1.z - gp2.z);
        }
        public static GridPos3D operator +(GridPos3D gp1, GridPos3D gp2)
        {
            return new GridPos3D(gp1.x + gp2.x, gp1.y + gp2.y, gp1.z + gp2.z);
        }
        public static GridPos3D operator *(GridPos3D gp1, GridPos3D gp2)
        {
            return new GridPos3D(gp1.x * gp2.x, gp1.y * gp2.y, gp1.z * gp2.z);
        }
        public static GridPos3D operator /(GridPos3D gp1, GridPos3D gp2)
        {
            return new GridPos3D(gp1.x / gp2.x, gp1.y / gp2.y, gp1.z / gp2.z);
        }
        public static GridPos3D operator *(GridPos3D gp1, int multiplier)
        {
            return new GridPos3D(gp1.x * multiplier, gp1.y * multiplier, gp1.z * multiplier);
        }
        public static GridPos3D operator /(GridPos3D gp1, int divisor)
        {
            return new GridPos3D(gp1.x / divisor, gp1.y / divisor, gp1.z / divisor);
        }

        public override bool Equals(Object other)
        {
            if (other is GridPos3D otherGridPos3D)
            {
                return Equals(otherGridPos3D);
            }
            return false;
        }
        public bool Equals(GridPos3D other)
        {
            return other.x == x && other.y == y && other.z == z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public GridPos3D[] CardinalNeighbours
        {
            get
            {
                return new GridPos3D[6]
                {
                    NeighbourLeft, // x-
                    NeighbourRight, // x+
                    NeighbourForeground, // y-
                    NeighbourBackground, // y+
                    NeighbourBelow, // z-
                    NeighbourAbove // z+
                };
            }
        }

        public GridPos3D NeighbourLeft => new(x - 1, y, z);

        public GridPos3D NeighbourRight => new(x + 1, y, z);

        public GridPos3D NeighbourForeground => new(x, y - 1, z);

        public GridPos3D NeighbourBackground => new(x, y + 1, z);

        public GridPos3D NeighbourBelow => new(x, y, z - 1);

        public GridPos3D NeighbourAbove => new(x, y, z + 1);

        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3(x, z, y);
        }

        public UnityEngine.Vector3Int ToVector3Int()
        {
            return new UnityEngine.Vector3Int(x, z, y);
        }
    }
 }