using Godot;
using System;

public partial class Pause : Node
{
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
