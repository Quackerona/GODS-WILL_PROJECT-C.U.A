using Godot;
using System;

public partial class Button : Control
{
	Control tabs;

	bool entered;

	TextureRect textureRect;

	public override void _Ready()
	{
		tabs = GetNode<Control>("../../Tabs");

		textureRect = GetNode<TextureRect>("TextureRect");
		textureRect.MouseEntered += () => {
			entered = true;
			textureRect.Modulate = new Color(1, 1, 1, 1);
		};
		textureRect.MouseExited += () => {
			entered = false;
			textureRect.Modulate = new Color(1, 1, 1, 0);
		};
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("leftClick") && entered)
		{
			foreach(Control tab in tabs.GetChildren())
			{
				if (tab.Name != Name)
				{
				    tab.Visible = false;
					tab.ProcessMode = ProcessModeEnum.Disabled;
				}
				else
				{
					tab.Visible = true;
					tab.ProcessMode = ProcessModeEnum.Inherit;
				}
			}
		}
	}
}
