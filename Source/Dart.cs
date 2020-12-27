using Godot;
using System;

public class Dart : KinematicBody2D
{
    public Vector2 Direction;
    private Vector2 _velocity;
    [Export] public int Speed = 8;

    [Signal]
    delegate void DartDestroyed(Dart destroyedDart);

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {        
        _velocity = new Vector2();
        _velocity = Direction.Normalized() * Speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        var collision_info = MoveAndCollide(_velocity);
        if (collision_info != null)
        {
            EmitSignal(nameof(DartDestroyed), this);
            QueueFree();
        }
    }    
}
