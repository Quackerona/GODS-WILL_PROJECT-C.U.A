using Godot;
using System;

public partial class SongButton : Control
{
	Panel songPanel;

	Vector2 originalSongPanelPosition;
	Random randomPosOffset = new Random();

	bool hover;

	bool startShaking;

	ColorRect black;

	AudioStreamPlayer transitionIntoSong;

	HBoxContainer buttonContainer;

	public override void _Ready()
	{
		buttonContainer = GetNode<HBoxContainer>("../../../../NavBarContainer");

		songPanel = GetNode<Panel>("TabPanel");
		songPanel.MouseEntered += () => hover = true;
		songPanel.MouseExited += () => hover= false;

		originalSongPanelPosition = songPanel.Position;

		black = GetNode<ColorRect>("../../../../../../Black");

		transitionIntoSong = GetNode<AudioStreamPlayer>("../../../../../../TransitionIntoSong");
	}

    public override void _Process(double delta)
	{
		if (hover)
		{
			if (Input.IsActionJustPressed("leftClick"))
			{
				transitionIntoSong.Play();

				using Tween tween = CreateTween();
				tween.TweenProperty(black, "modulate:a", 1, 2.5);
				tween.TweenCallback(Callable.From(() => 
					GetTree().ChangeSceneToFile("scenes/songs/" + Name + ".tscn")
				));
				
				startShaking = true;

				foreach (Control button in buttonContainer.GetChildren())
				{
					if (button != this)
						button.ProcessMode = ProcessModeEnum.Disabled;
				}
			}
		}

		if (startShaking)
			songPanel.Position = new Vector2(originalSongPanelPosition.X + GetRandomNumber(-1, 1), originalSongPanelPosition.Y + GetRandomNumber(-1, 1));
	}

	public float GetRandomNumber(double minimum, double maximum)
	{ 
		return (float)(randomPosOffset.NextDouble() * (maximum - minimum) + minimum);
	}
}
