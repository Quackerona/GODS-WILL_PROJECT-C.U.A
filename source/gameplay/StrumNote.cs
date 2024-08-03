using Godot;
using System;

public partial class StrumNote : AnimatedSprite2D
{
	[Export]
	public string action { get; set; }

	bool _auto;
	[Export]
	public bool auto { get{return _auto;} set {
		if (value)
			AnimationFinished += AssignAutoAnimation;
		else
			AnimationFinished -= AssignAutoAnimation;
		_auto = value;
	}} // Is it botplay?

	Node2D hitable;
	Note hitableScript;

	public Character characterScript { get; set; }

	public override void _Process(double delta)
	{
		if (!auto)
		{
			if (Input.IsActionJustPressed(action))
			{
				if (hitable == null)
				{
					if (!JsonUtility.userData.ghostTapping)
						MissGhostTapping();

					Play("pressed");
				}
				else
				{
					Play("confirm");
					GoodNoteHit();
				}
			}

			if (Input.IsActionPressed(action))
				characterScript.PlayAnimation(action);

			if (Input.IsActionJustReleased(action))
			{
				if (hitable != null)
				    hitableScript.pressed = false;
					
				Play("static");
			}
		}
		else
		{
			if (hitable != null)
			{
				Play("confirm");

				characterScript.PlayAnimation(action);
				GoodNoteHit();
			}
		}
	}

	void GoodNoteHit()
	{
		if (hitableScript.noteData.id < SongUtility.SONG.keys)
		{
		    SongUtility.health += 0.06f;

			GameController.instance.voices1.VolumeDb = 0;

			SetScore(true);
		}
			
		hitableScript.GoodNoteHit();
	}

	public void MissNote()
	{
		if (hitableScript.noteData.id < SongUtility.SONG.keys)
		{
			SongUtility.health -= 0.04f;
			++SongUtility.misses;

			GameController.instance.voices1.VolumeDb = -80;

			SetScore(true);
		}

		characterScript.PlayAnimation(action + "-miss");

		hitableScript.MissNote();
	}

	public void MissGhostTapping()
	{
		SongUtility.score -= 200;
		SongUtility.health -= 0.04f;
		++SongUtility.misses;

		GameController.instance.voices1.VolumeDb = -80;

		SetScore(false);
	}

	void SetScore(bool includeFactor) // Super accurate, but too punishing. I should probably nerf it later.
	{
		if (includeFactor)
		{
			float factor = 100f - Mathf.Abs(SongUtility.songPosition - hitableScript.noteData.strumTime);

			SongUtility.score += (int)factor;
			SongUtility.accuracy.Add(factor);
		}

		GameController.instance.UpdateInfo();

		if (SongUtility.health <= 0)
		{
			AudioServer.SetBusVolumeDb(0, 0);
			GetTree().ChangeSceneToFile("res://scenes/GameOver.tscn");
		}
	}

	public void AssignHitable(Node2D note)
	{
		if (hitable == null)
		{
			hitable = note;
			hitableScript = (Note)hitable;
		}
	}

	public void DisposeHitable()
	{
		hitable = null;
		hitableScript = null;
	}

	void AssignAutoAnimation() // For the "auto" variable. When "auto" is set, the strum animation must be completed automatically. Else, is completed manually.
	{
		Play("static");
	}
}
