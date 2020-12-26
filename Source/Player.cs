using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int speed = 200;

    public Vector2 _velocity = new Vector2();    
    private AnimatedSprite _animatedSprite;
    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public void GetInput()
    {
        _velocity = new Vector2();

        if (Input.IsActionPressed("right"))
        {
            _velocity.x += 1;
        }
        else if (Input.IsActionPressed("left"))
        {
            _velocity.x -= 1;            
        }

        if (Input.IsActionPressed("down"))
        {
            _velocity.y += 1;
        }
        else if (Input.IsActionPressed("up"))
        {
            _velocity.y -= 1;            
        }
        _velocity = _velocity.Normalized() * speed;
    }

    private void Animate()
    {
        string animation = "";
        if (_velocity.x > 0)
            animation = "w_right";
        else if (_velocity.x < 0)
            animation = "w_left";
        if (_velocity.y > 0)
            animation = "w_down";
        else if (_velocity.y < 0)
            animation = "w_up";

        if (_velocity == Vector2.Zero)
            _animatedSprite.Stop();
        else if (!_animatedSprite.IsPlaying() ||
                 _animatedSprite.Animation != animation)
            _animatedSprite.Play(animation);

    }

    public override void _Process(float delta)
    {
        GetInput();
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity = MoveAndSlide(_velocity);
        Animate();
    }
}
