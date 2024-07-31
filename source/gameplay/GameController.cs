using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;

public partial class GameController : MusicModule
{
	public static GameController instance { get; private set; }

	// Essential Components
	public SubViewportContainer gameViewportContainer { get; set; }
	public SubViewport gameViewport { get; set; }
	public SubViewportContainer hudViewportContainer { get; set; }
	public SubViewport hudViewport { get; set; }

	public Camera2D gameCam { get; set; }
	public Camera2D hudCam { get; set; }
	public Vector2 gameCamZoom { get; set; }
	public Vector2 hudCamZoom { get; set; }

	public TextureProgressBar healthBar { get; set; }
	public Label score { get; set; }

	public List<AnimatedSprite2D> strumNotes { get; set; }
	Node notesFolder;

	public AudioStreamPlayer inst { get; set; }
	public  AudioStreamPlayer voices1 { get; set; }
	public  AudioStreamPlayer voices2 { get; set; }

	// Characters
	AnimatedSprite2D player;
	Character playerScript;
	AnimatedSprite2D opponent;
	Character opponentScript;

	// Events
	EventInterface eventHandler;

	public override void _Ready()
	{
		base._Ready();

		instance = this;

		string songLowercase = Name.ToString().ToLower();

		SongUtility.ResetSongMetadata();

		JsonUtility.LoadSongContainer(songLowercase);
		SongUtility.ChangeBpm(SongUtility.SONG.bpm);
		
		NotePool.Init(16);

		gameViewportContainer = GetNode<SubViewportContainer>("GameViewportContainer");
		gameViewport = gameViewportContainer.GetNode<SubViewport>("GameViewport");
		hudViewportContainer = GetNode<SubViewportContainer>("HudViewportContainer");
		hudViewport = hudViewportContainer.GetNode<SubViewport>("HudViewport");

		gameCam = gameViewport.GetNode<Camera2D>("Camera2D");
		hudCam = hudViewport.GetNode<Camera2D>("Camera2D");
		gameCamZoom = gameCam.Zoom;
		hudCamZoom = hudCam.Zoom;

		healthBar = hudViewport.GetNode<TextureProgressBar>("Healthbar");
		score = hudViewport.GetNode<Label>("Score");

		player = gameViewport.GetNode<AnimatedSprite2D>("Player");
		playerScript = (Character)player;
		opponent = gameViewport.GetNode<AnimatedSprite2D>("Opponent");
		opponentScript = (Character)opponent;

		strumNotes = new List<AnimatedSprite2D>(SongUtility.SONG.keys * 2);
		
		foreach (AnimatedSprite2D strumNote in hudViewport.GetNode<Node>("StrumNotes").GetChildren())
		{
			StrumNote strumNoteScript = (StrumNote)strumNote;

			if (strumNote.Name.ToString().Contains("Plr"))
			{
				if (JsonUtility.userData.middleScroll)
				    strumNote.Position -= new Vector2(320f, 0);

			    strumNoteScript.characterScript = playerScript;
			}
			else
			{
				if (JsonUtility.userData.middleScroll)
				    strumNote.Hide();
				strumNoteScript.characterScript = opponentScript;
			}

			if (!JsonUtility.userData.downScroll)
			{
				strumNote.Position = new Vector2(strumNote.Position.X, 100);

				healthBar.Position = new Vector2(healthBar.Position.X, 670);
				score.Position = new Vector2(score.Position.X, 640);
			}

			strumNotes.Add(strumNote);
		}

		notesFolder = hudViewport.GetNode<Node>("Notes");

		eventHandler = GetNode<EventInterface>("EventHandler");

		AudioServer.SetBusVolumeDb(0, JsonUtility.userData.songVolume);

		inst = GetNode<AudioStreamPlayer>("Inst");
		inst.Finished += OnSongEnd;
		voices1 = GetNode<AudioStreamPlayer>("Voices1");
		voices2 = GetNode<AudioStreamPlayer>("Voices2");

		eventHandler.AfterStart();
		
		inst.Play();
		voices1.Play();
		voices2.Play();

		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		SongUtility.songPosition = inst.GetPlaybackPosition() * 1000;

		float zoomWeight = (float)delta / (1f / 60f) * 0.05f;

		gameCam.Zoom = gameCam.Zoom.Lerp(gameCamZoom, zoomWeight);
		hudCam.Zoom = hudCam.Zoom.Lerp(hudCamZoom, zoomWeight);

		ControlNotes();

		eventHandler.AfterUpdate(delta);
	}

    public override void StepHit()
    {
        base.StepHit();

		eventHandler.OnStep();
    }

    public override void BeatHit()
    {
        base.BeatHit();

		if (SongUtility.curBeat % 2 == 0)
		{
			opponentScript.Dance();
			playerScript.Dance();
		}

		eventHandler.OnBeat();
    }

	public override void SectionHit()
    {
        base.SectionHit();

		Vector2 beatScale = new Vector2(0.03f, 0.03f);
		
		gameCam.Zoom += beatScale;
		hudCam.Zoom += beatScale;

		eventHandler.OnSection();
    }

	public void UpdateInfo()
	{
		score.Text = $"Score: {SongUtility.score} | Accuracy: {Mathf.Ceil(SongUtility.accuracy.Average())}% | Misses: {SongUtility.misses}";
		score.ResetSize();
		score.Position = new Vector2((1280 - score.Size.X) / 2, score.Position.Y);

		healthBar.Value = SongUtility.health;
	}

    void ControlNotes()
	{
		for (int i = SongUtility.SONG.notes.Count - 1; i >= 0; i--)
		{
			NoteData noteData = SongUtility.SONG.notes[i];

			if (noteData.strumTime - SongUtility.songPosition < 500)
			{
				Node2D note = NotePool.Get(noteData.id);
				Note noteScript = (Note)note;
				noteScript.noteData = noteData;
				noteScript.SetupStrum(strumNotes[noteData.id]);
				if (!note.IsInsideTree())
				    notesFolder.AddChild(note);

				if (noteData.length > 0)
					noteScript.SetupHold();

				if (JsonUtility.userData.middleScroll)
				{
				    if (noteScript.noteData.id < 4)
						note.Show();
					else
						note.Hide();
				}

				SongUtility.SONG.notes.RemoveAt(i);
			}
		}

		for (int i = NotePool.activeNotes.Count - 1; i >= 0; i--)
		{
			Node2D note = NotePool.activeNotes[i];
			Note noteScript = (Note)note;

			float directionToGo = 0;
			if (!noteScript.pressed)
				directionToGo = (SongUtility.songPosition - noteScript.noteData.strumTime) * SongUtility.SONG.speed;
			if (!JsonUtility.userData.downScroll)
			    directionToGo = -directionToGo;

			note.Scale = noteScript.strumNote.Scale;
			
			note.Position = noteScript.strumNote.Position + new Vector2(0, directionToGo);

			if (noteScript.strumNoteScript.auto)
			{
				if (noteScript.noteData.strumTime <= SongUtility.songPosition)
				    noteScript.strumNoteScript.AssignHitable(note);
			}
			else
			{
				if (noteScript.noteData.strumTime > SongUtility.songPosition - 200)
				{
					if (noteScript.noteData.strumTime < SongUtility.songPosition + 160)
						noteScript.strumNoteScript.AssignHitable(note);
				}
				else
				{
					if (!noteScript.pressed)
						noteScript.strumNoteScript.MissNote();
				}
			}
		}
	}

	// callbacks

	void OnSongEnd()
	{
		AudioServer.SetBusVolumeDb(0, 0);
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}
}
