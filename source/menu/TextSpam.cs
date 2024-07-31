using Godot;
using System;

public partial class TextSpam : Label
{
	Random random;
	public override void _Ready()
	{
		random = new Random();
	}

	public override void _Process(double delta)
	{
		Text = Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString() + Convert.ToChar(random.Next(0, 128)).ToString();
	}
}
