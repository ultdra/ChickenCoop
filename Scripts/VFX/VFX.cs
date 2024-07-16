using Godot;
using System;

public partial class VFX : GpuParticles2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        Emitting = false;
    }

	public void _on_baby_chick_start_sleeping_state()
	{
		Emitting = true;
	}
    public void _on_baby_chick_end_sleeping_state()
    {
        Emitting = false;
    }

    public void _on_baby_chick_start_play_state()
    {
        Emitting = true;
    }
    public void _on_baby_chick_end_play_state()
    {
        Emitting = false;
    }

    public void _on_baby_chick_start_thinking_state()
    {
        Emitting = true;
    }
    public void _on_baby_chick_end_thinking_state()
    {
        Emitting = false;
    }

    public void _on_baby_chick_start_wander_state()
    {
        Emitting = true;
    }
    public void _on_baby_chick_end_wander_state()
    {
        Emitting = false;
    }
    public void _on_baby_chick_start_grazing_state()
    {
        Emitting = true;
    }
    public void _on_baby_chick_end_grazing_state()
    {
        Emitting = false;
    }
}
