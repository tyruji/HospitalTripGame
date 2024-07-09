using System;
using Godot;
public interface IMoveable
{
    public float Speed { get; }
    public Vector2 Direction { get; }
    public Vector2 PreviousDirection { get; }
    public Vector2 Velocity { set; }
}