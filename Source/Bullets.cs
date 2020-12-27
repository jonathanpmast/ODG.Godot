using Godot;
using System;

public class Bullets : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export] public PackedScene DartScene = null;
    [Export] public int MaxDartsAllowed = 3;
    int _availableDarts = 3;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Player_Shoot(Vector2 direction, Vector2 location)
    {
        if (_availableDarts > 0)
        {
            var dartInstance = (Dart)DartScene.Instance();
            AddChild(dartInstance);
            if (direction.y > 0)
                dartInstance.RotationDegrees = 90;
            if (direction.y < 0)
                dartInstance.RotationDegrees = 270;
            if (direction.x < 0)
                dartInstance.RotationDegrees = 180;
            dartInstance.Position = location;
            dartInstance.Direction = direction;
            dartInstance.Connect("DartDestroyed", this, "_on_Dart_Destroyed");
            _availableDarts--;
        }
    }

    public void _on_Dart_Destroyed(Dart dartDestroyed)
    {
        _availableDarts++;
    }
}
