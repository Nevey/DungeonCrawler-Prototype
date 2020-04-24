using UnityEngine;

namespace CardboardCore.Utilities
{
    public static class RotationUtil
    {
        public static Vector3 GetVectorSimple(float xAngle, float yAngle, float zAngle, Vector3 offset)
        {
            Quaternion xQuaternion = Quaternion.AngleAxis(xAngle, Vector3.left);
            Quaternion yQuaternion = Quaternion.AngleAxis(yAngle, Vector3.up);
            Quaternion zQuaternion = Quaternion.AngleAxis(zAngle, Vector3.forward);

            Quaternion rotation = xQuaternion * yQuaternion * zQuaternion;

            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

            return matrix.MultiplyPoint3x4(offset);
        }

        public static Vector3 GetVector(float xAngle, float yAngle, float zAngle, Vector3 offset, Vector3 position, Vector3 scale)
        {
            Quaternion xQuaternion = Quaternion.AngleAxis(xAngle, Vector3.left);
            Quaternion yQuaternion = Quaternion.AngleAxis(yAngle, Vector3.up);
            Quaternion zQuaternion = Quaternion.AngleAxis(zAngle, Vector3.forward);

            Quaternion rotation = xQuaternion * yQuaternion * zQuaternion;

            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

            return matrix.MultiplyPoint3x4(offset);
        }
    }
}