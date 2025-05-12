using Godot;
using System;

public static class VectorUtils
{
    public static Vector2 SnapToPoint(Vector2 position, Vector2 step)
    {
        float snappedX = Mathf.Round(position.X / step.X) * step.X;
        float snappedY = Mathf.Round(position.Y / step.Y) * step.Y;
        
        return new Vector2(snappedX, snappedY);
    }
}