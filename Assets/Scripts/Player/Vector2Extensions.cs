using UnityEngine;

public static class Vector2Extensions
{
    // Gira un vector cierto numero de grados indicados
    public static Vector2 Rotate(this Vector2 originalVector, float rotateAngleInDegrees)
    {
        Quaternion rotation = Quaternion.AngleAxis(rotateAngleInDegrees, Vector3.forward);
        return rotation * originalVector;
    }
}