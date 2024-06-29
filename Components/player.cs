using Godot;
using System;
using System.Collections.Generic;
public partial class player : CharacterBody2D
{
	[Export]
	public int Speed = 1;

	private Vector2 ScreenSize;

	private AnimatedSprite2D AnimatedSprite;

	private List<bird> Birds = new List<bird>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		ScreenSize = GetViewportRect().Size;
		Position = ScreenSize/2;

		// Getting the references
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection.Normalized() * Speed;
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        // We will first get the input from the player
		GetInput();
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

	public void NotifyNewBird(bird bird)
	{
		Birds.Add(bird);
	}

#endregion

}
