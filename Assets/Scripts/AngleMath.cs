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
        return radians * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Converts an angle in Degrees to Radians
    /// </summary>
    /// <param name="degrees">The angle to convert</param>
    /// <returns>The angle in radians</returns>
    public static float ToRadians(float degrees)
    {
        return degrees * Mathf.Deg2Rad;
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

    /// <summary>
    /// Gets the overshoot amount for a target
    /// </summary>
    /// <param name="_start">The start point</param>
    /// <param name="_end">The initial end target</param>
    /// <param name="_overshootDistance">How far to move past</param>
    /// <returns>The amount of overshoot</returns>
    public static Vector2 GetOvershoot(Vector2 _start, Vector2 _end, float _overshootDistance)
    {
        float angle = ToRadians(GetVectorDirection(_end - _start));
        if (_overshootDistance > 0)
        {
            return new Vector2(_overshootDistance * Mathf.Cos(angle), _overshootDistance * Mathf.Sin(angle));
        }
        return Vector2.zero;
    }
}
