using System;
using Godot;

public interface IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; }
    public Sprite2D Sprite { get; }
}