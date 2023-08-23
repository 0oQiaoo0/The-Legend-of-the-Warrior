using UnityEngine;

namespace Utilities
{
    public class SerializableVector3
    {
        public float x, y, z;
        
        public SerializableVector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public SerializableVector3(Vector3 vec3)
        {
            x = vec3.x;
            y = vec3.y;
            z = vec3.z;
        }
        
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}