using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Structs
{
    /// <summary>
    /// Vector of three floats
    /// </summary>
    public struct Vec3f
    {
        public float x;
        public float y;
        public float z;

        public Vec3f(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Clones this vector
        /// </summary>
        /// <returns>A clone of this vector</returns>
        public Vec3f Clone()
        {
            var clone = new Vec3f();
            clone.x = x;
            clone.y = y;
            clone.z = z;
            return clone;
        }

        /// <summary>
        /// Adds a vector to this one
        /// </summary>
        /// <param name="other">Vector to add</param>
        public void Add(Vec3f other)
        {
            x += other.x;
            y += other.y;
            z += other.z;
        }

        /// <summary>
        /// Subtracts a vector from this one
        /// </summary>
        /// <param name="other">Vector to subtract</param>
        public void Subtract(Vec3f other)
        {
            x -= other.x;
            y -= other.y;
            z -= other.z;
        }

        /// <summary>
        /// Normalizes this vector
        /// </summary>
        public void Normalize()
        {
            float len = (float)Math.Sqrt(x * x + y * y + z * z);
            x /= len;
            y /= len;
            z /= len;
        }

        /// <summary>
        /// Returns the cross product of this and the other vector
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vec3f Crossed(Vec3f other)
        {
            var cross = new Vec3f();
            cross.x = y * other.z - z * other.y;
            cross.y = z * other.x - x * other.z;
            cross.z = x * other.y - y * other.x;
            return cross;
        }

        public Vec3f Multiplied(float v)
        {
            Vec3f res = new Vec3f();

            res.x = x * v;
            res.y = y * v;
            res.z = z * v;

            return res;
        }

        public void Divide(float v)
        {
            x /= v;
            y /= v;
            z /= v;
        }

        public Vec3f Subtracted(Vec3f v)
        {
            Vec3f res = new Vec3f();

            res.x = x - v.x;
            res.y = y - v.y;
            res.z = z - v.z;

            return res;
        }
    }
}
