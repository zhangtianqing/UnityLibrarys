using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Scripts.Class
{
    [Serializable]
    public class V3
    {
        [SerializeField]
        public double x = 0;
        [SerializeField]
        public double y = 0;
        [SerializeField]
        public double z = 0;
        public static V3 Map2V3(Vector3 vector3, int bit = 2)
        {
            return new V3() { x = Math.Round(vector3.x, bit), y = Math.Round(vector3.y, bit), z = Math.Round(vector3.z, bit) };
        }
        public static V3 Map2V3(float x, float y, float z, int bit = 2)
        {
            return new V3() { x = Math.Round(x, bit), y = Math.Round(y, bit), z = Math.Round(z, bit) };
        }
        public static Vector3 Map2Vector3(V3 vector3, int bit = 2)
        {
            return new Vector3() { x = (float)Math.Round(vector3.x, bit), y = (float)Math.Round(vector3.y, bit), z = (float)Math.Round(vector3.z, bit) };
        }
        public static Vector3 Map2Vector3(float x, float y, float z, int bit = 2)
        {
            return new Vector3() { x = (float)Math.Round(x, bit), y = (float)Math.Round(y, bit), z = (float)Math.Round(z, bit) };
        }
        public Vector3 GetVector3(int bit = 2)
        {
            return new Vector3() { x = (float)Math.Round(x, bit), y = (float)Math.Round(y, bit), z = (float)Math.Round(z, bit) };
        }
    }
}