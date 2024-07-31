using Godot;
using System;

public partial class MusicModule : Node2D
{
	int oldStep;
	int oldBeat;
	int oldSection;

	public override void _Ready()
	{
		SongUtility.Init();
	}

	public override void _Process(double delta)
	{
		SongUtility.CalculateSongMetadata();

		if (SongUtility.curStep != oldStep)
		{
			oldStep = SongUtility.curStep;
			StepHit();
		}

		if (SongUtility.curBeat != oldBeat)
		{
			oldBeat = SongUtility.curBeat;
			BeatHit();
		}

		if (SongUtility.curSection != oldSection)
		{
			oldSection = SongUtility.curSection;
			SectionHit();
		}
	}

	public virtual void StepHit() {}

    public virtual void BeatHit() {}

	public virtual void SectionHit() {}

}
