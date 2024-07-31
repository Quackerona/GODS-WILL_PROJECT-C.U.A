using Godot;
using System;
using System.Collections.Generic;

public partial class Character : AnimatedSprite2D
{
	[Export]
	bool noMisses;

	[Export]
	bool isGf;
	
	float idleTimer;

	bool left;

	public void SetCharacter(string character)
	{
		SpriteFrames = ResourceLoader.Load<SpriteFrames>("res://assets/images/characters/" + character + "/anims.tres");
	}

	public void PlayAnimation(string name, float idleTimer = 0.2f)
	{
		if (noMisses && name.Contains("miss"))
		    return;
		
		this.idleTimer = idleTimer;

		Stop();
		Play(name);

	}

	public void Dance()
	{
		if (!IsPlaying() && idleTimer <= 0)
		{
			if (isGf)
			{
				if (!left) PlayAnimation("danceLeft");
				else PlayAnimation("danceRight");

				left = !left;
			}
			else PlayAnimation("idle");
		}
	}

	public override void _Process(double delta)
	{
		if (!IsPlaying())
			idleTimer -= (float)delta;
	}
}
