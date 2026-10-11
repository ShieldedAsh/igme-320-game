using System;
using UnityEngine;

public static class AngleMath
{
    /// <summary>
    /// Converts an angle in Radians to Degrees
    /// </summary>
    /// <param name="radians">The angle to convert</param>
    /// <returns>The angle in degrees</returns>
    public static float ToDegrees(float radians)
    {
        return (radians * (180.0f / (float)Math.PI));
    }

    /// <summary>
    /// Converts an angle in Degrees to Radians
    /// </summary>
    /// <param name="degrees">The angle to convert</param>
    /// <returns>The angle in radians</returns>
    public static float ToRadians(float degrees)
    {
        return (degrees * ((float)Math.PI / 180.0f));
    }

    /// <summary>
    /// Gets the angle of a vector
    /// </summary>
    /// <param name="direction">The vector to get the angle of</param>
    /// <returns>The angle of the vector in degrees</returns>
    public static float GetVectorDirection(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.right, direction.normalized);
        Vector3 cross = Vector3.Cross(Vector2.right, direction.normalized);
        if (cross.z < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }
}
