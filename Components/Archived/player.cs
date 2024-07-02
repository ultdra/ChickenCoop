using Godot;
using System;
using System.Collections.Generic;
public partial class player : CharacterBody2D
{
	[Export]
	public int Speed = 1;

	private Vector2 ScreenSize;

	private AnimatedSprite2D AnimatedSprite;

	// Player state
	private PlayerState playerState = PlayerState.Idle;
	private PlayerState previousPlayerState = PlayerState.None;


	#region Signals

	[Signal]
	public delegate void PlayerIdleEventHandler();

	[Signal]
	public delegate void PlayerRunningEventHandler();

	[Signal]
	public delegate void PlayerDeadEventHandler();

	#endregion


	#region Acessors
	public PlayerState CurrentPlayerState {get { return playerState;}}
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		ScreenSize = GetViewportRect().Size;
		Position = ScreenSize/2;
		GD.Print(Position);
		// Getting the references
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection.Normalized() * Speed;

		if(inputDirection != Vector2.Zero)
		{
			playerState = PlayerState.Running;
		}
		else
		{
			playerState = PlayerState.Idle;
		}
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {

		// We will first get the input from the player
		GetInput();

		if(playerState != previousPlayerState)
		{
			switch(playerState)
			{
				case PlayerState.Idle:
					EmitSignal(nameof(PlayerIdle));
				break;

				case PlayerState.Running:
					EmitSignal(nameof(PlayerRunning));
				break;

				case PlayerState.Dead:
					EmitSignal(nameof(PlayerDead));
				break;
			}
			previousPlayerState = playerState;
		}

		MoveAndSlide();

		// Unused: To limit movement to within the screen.
		// Position = Position.Clamp(Vector2.Zero, ScreenSize);

		// We will get mouse position and make the player look at them.
		// See: https://kidscancode.org/godot_recipes/4.x/2d/8_direction/

		// Vector2 mousePosition = GetLocalMousePosition();
		// var angle = Mathf.Snapped(mousePosition.Angle(), Mathf.Pi/4) / (Mathf.Pi/4);
		// angle = Mathf.Wrap((int)angle, 0 , 4);
		

		// Player animation handler
		if(Velocity.Length() != 0)
		{
			AnimatedSprite.Play();
		}
		else
		{
			AnimatedSprite.Stop();
		}
    }


#region Public Manipulators

#endregion

}
