using Godot;
using System;

public partial class GameOverBehavior : Node2D
{
	Random random = new Random();

	Sprite2D gameOver;
	public override void _Ready()
	{
		base._Ready();

		gameOver = GetNode<Sprite2D>("GameOver");

		Timer timer = new Timer();
		timer.WaitTime = 4.5;
		timer.OneShot = true;
		timer.Timeout += () => {
			GetNode<AudioStreamPlayer>("GameOverSong").Play();
			CreateTween().TweenProperty(gameOver, "modulate:a", 1, 1);
		};
		AddChild(timer);
		timer.Start();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
		}
	}

	public override void _Notification(int what)
    {
		switch (what)
		{
			case (int)MainLoop.NotificationApplicationFocusOut:
				GetTree().Paused = true;
				break;

			case (int)MainLoop.NotificationApplicationFocusIn:
				GetTree().Paused = false;
				break;
		}
    }
}
