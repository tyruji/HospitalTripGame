using System;
using Godot;

public interface IAttacker
{
    public bool Attacking { get; set; }
    public bool HoldingGun { get; set; }
    public Vector2 AttackDirection { get; set; }
}
