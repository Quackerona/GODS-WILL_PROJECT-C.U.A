using Godot;
using System;

public partial class MainMenuBehavior : Node2D
{
	AnimatedSprite2D bf;

    public override void _EnterTree()
    {
		JsonUtility.LoadSettings();
    }

    public override void _Ready()
	{
		GetNode<DiscordSDK>("/root/DiscordSdk").ChangePresence();

		using Tween tween = CreateTween();
		tween.TweenProperty(GetNode<ColorRect>("CanvasLayer/Bar"), "position:y", -360, 1.2).SetEase(Tween.EaseType.Out);
		tween.Parallel().TweenProperty(GetNode<ColorRect>("CanvasLayer/Bar2"), "position:y", 720, 1.2).SetEase(Tween.EaseType.Out);

		bf = GetNode<AnimatedSprite2D>("PageContainer/Page/Tabs/My Character/TabPanel/CharacterTab/Bf");
	}

    public override void _Process(double delta)
    {
        bf.Play("idle");
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
