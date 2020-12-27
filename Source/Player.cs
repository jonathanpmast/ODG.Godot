using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Export] public int speed = 200;

    [Signal]
    delegate void Shoot(Vector2 direction, Vector2 location);

    public Vector2 _velocity = new Vector2();
    private Vector2 _direction = new Vector2();
    private Vector2 _facingDirection = new Vector2();
    private Vector2 _shootFromPosition = new Vector2();
    private AnimatedSprite _animatedSprite;
    private Dictionary<string, Node2D> _offsetNodes = new Dictionary<string, Node2D>();

    private bool _isFiring = false;
    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _offsetNodes.Add("up", GetNode<Node2D>("FireOffsets/Up"));
        _offsetNodes.Add("down", GetNode<Node2D>("FireOffsets/Down"));
        _offsetNodes.Add("right", GetNode<Node2D>("FireOffsets/Right"));
        _offsetNodes.Add("left",GetNode<Node2D>("FireOffsets/Left"));
    }

    public void GetInput()
    {
        _direction = new Vector2();

        if (Input.IsActionPressed("right"))
        {
            _direction.x += 1;
        }
        else if (Input.IsActionPressed("left"))
        {
            _direction.x -= 1;            
        }

        if (Input.IsActionPressed("down"))
        {
            _direction.y += 1;
        }
        else if (Input.IsActionPressed("up"))
        {
            _direction.y -= 1;            
        }
        _velocity = _direction.Normalized() * speed;
    }

    private void Animate()
    {
        string animation = "";
        if (_direction.x > 0)
        {
            animation = "w_right";
            _facingDirection = new Vector2(1, 0);
            _shootFromPosition = ToGlobal(_offsetNodes["right"].Position);
        }
        else if (_direction.x < 0)
        {
            animation = "w_left";
            _facingDirection = new Vector2(-1, 0);
            _shootFromPosition = ToGlobal(_offsetNodes["left"].Position);
        }
        if (_direction.y > 0)
        {
            animation = "w_down";
            _facingDirection = new Vector2(0, 1);
            _shootFromPosition = ToGlobal(_offsetNodes["down"].Position);
        }
        else if (_direction.y < 0)
        {
            _facingDirection = new Vector2(0, -1);
            animation = "w_up";
            _shootFromPosition = ToGlobal(_offsetNodes["up"].Position);
        }

        if (_direction == Vector2.Zero)
            _animatedSprite.Stop();
        else if (!_animatedSprite.IsPlaying() ||
                 _animatedSprite.Animation != animation)
            _animatedSprite.Play(animation);

    }

    private void Fire()
    {
        if (Input.IsActionJustPressed("Fire"))
            EmitSignal(nameof(Shoot), _facingDirection, _shootFromPosition);
    }

    public override void _Process(float delta)
    {
        GetInput();
        Fire();
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity = MoveAndSlide(_velocity);
        Animate();
    }
}
