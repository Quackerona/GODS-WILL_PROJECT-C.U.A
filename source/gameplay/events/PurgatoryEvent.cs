using Godot;
using System;

public partial class PurgatoryEvent : Node, EventInterface
{
    ColorRect checker;
    Sprite2D sky;
    Sprite2D bg;
    Node2D ghosts;

    CanvasLayer canvasLayer;
    ColorRect black;
    Sprite2D portrait;

    float bloomValue;
	ShaderMaterial bloom;

	public override void _Ready()
	{
        
	}

	public override void _Process(double delta)
	{
	}

    public void AfterStart()
    {
        canvasLayer = GameController.instance.gameViewport.GetNode<CanvasLayer>("BeginningLayer");
        black = canvasLayer.GetNode<ColorRect>("Black");
        portrait = canvasLayer.GetNode<Sprite2D>("Portrait");

        checker = GameController.instance.gameViewport.GetNode<ColorRect>("Checker");
        sky = GameController.instance.gameViewport.GetNode<Sprite2D>("Sky");
        bg = GameController.instance.gameViewport.GetNode<Sprite2D>("Bg");
        ghosts = GameController.instance.gameViewport.GetNode<Node2D>("Ghosts");


        bloom = (ShaderMaterial)GetNode<ColorRect>("../BloomShader/ColorRect").Material;

        GameController.instance.gameCam.Position = new Vector2(397, 360);
    }

    public void AfterUpdate(double delta)
    {
        bloomValue = Mathf.Lerp(bloomValue, 0, (float)delta / (1f / 60f) * 0.05f);
		bloom.SetShaderParameter("intensity", bloomValue);
    }

    public void OnStep()
    {
        switch (SongUtility.curStep)
        {
            case 12:
                portrait.Visible = false;
			    portrait.Texture = ResourceLoader.Load<Texture2D>("res://assets/images/gameplay/purgatory/oldBuild2.png");
                break;
            case 16:
                portrait.Visible = true;
                break;
            case 28:
                portrait.Visible = false;
                portrait.Texture = ResourceLoader.Load<Texture2D>("res://assets/images/gameplay/purgatory/oldBuild3.png");
                break;
		    case 32:
			    portrait.Visible = true;
                break;
		    case 44:
                portrait.Visible = false;
                portrait.Texture = ResourceLoader.Load<Texture2D>("res://assets/images/gameplay/purgatory/oldBuild4.png");
                break;
		    case 48:
			    portrait.Visible = true;
                break;
		    case 54:
                portrait.Visible = false;
                portrait.Texture = ResourceLoader.Load<Texture2D>("res://assets/images/gameplay/purgatory/face.png");
                break;
            case 56:
			    portrait.Visible = true;
                break;
            case 64:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(portrait, "modulate:a", 0, 5);
                    tween.Parallel().TweenProperty(black, "modulate:a", 0, 11);
                    tween.Parallel().TweenProperty(GameController.instance.gameCam, "zoom", new Vector2(3f, 3f), 11);
                    tween.Parallel().TweenProperty(GameController.instance.hudViewportContainer, "modulate:a", 1, 1);
                }
                break;
            case 188:
                canvasLayer.QueueFree();

                GameController.instance.gameViewportContainer.Visible = false;

                GameController.instance.gameCamZoom = new Vector2(0.9f, 0.9f);
                break;
            case 192:
                bg.Visible = false;
                sky.Visible = false;

                GameController.instance.gameViewportContainer.Visible = true;
                break;
            case 316:
                bg.Visible = true;
                sky.Visible = true;
                
                GameController.instance.gameViewportContainer.Visible = false;
                break;
            case 320:
                ghosts.Visible = true;
                ghosts.ProcessMode = ProcessModeEnum.Inherit;
                
                GameController.instance.gameViewportContainer.Visible = true;
                GameController.instance.gameCamZoom = new Vector2(1.3f, 1.3f);
                break;
            case 448:
                GameController.instance.gameCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 576:
                ghosts.QueueFree();

                GameController.instance.gameViewportContainer.Visible = false;
                break;
            case 592:
                GameController.instance.gameCamZoom = new Vector2(1.3f, 1.3f);
                GameController.instance.gameCam.Zoom += new Vector2(10f, 10f);

                GameController.instance.gameViewportContainer.Visible = true;
                break;
            case 716:
                bg.QueueFree();
                sky.QueueFree();
                
                GameController.instance.gameViewportContainer.Visible = false;
                break;
            case 720:
                GameController.instance.gameViewportContainer.Visible = true;

                GameController.instance.gameCamZoom = new Vector2(0.9f, 0.9f);
                break;
            case 848:
                GameController.instance.gameViewportContainer.Visible = false;
                GameController.instance.hudViewportContainer.Visible = false;
                break;
                
        }
    }

    public void OnBeat()
    {
        if (SongUtility.curBeat >= 48 && SongUtility.curBeat < 80)
		{
			GameController.instance.gameCam.Zoom += new Vector2(0.065f, 0.065f);
			GameController.instance.hudCam.Zoom += new Vector2(0.025f, 0.025f);

			bloomValue += 0.03f;
		}
		else if (SongUtility.curBeat >= 80 && SongUtility.curBeat < 112 && SongUtility.curBeat % 2 == 0)
		{
			GameController.instance.gameCam.Zoom += new Vector2(0.065f, 0.065f);
			GameController.instance.hudCam.Zoom += new Vector2(0.025f, 0.025f);

			bloomValue += 0.03f;
		}
		else if (SongUtility.curBeat >= 112 && SongUtility.curBeat < 144)
		{
			GameController.instance.gameCam.Zoom += new Vector2(0.065f, 0.065f);
			GameController.instance.hudCam.Zoom += new Vector2(0.025f, 0.025f);

			bloomValue += 0.03f;
		}
		else if (SongUtility.curBeat >= 148 && SongUtility.curBeat < 179 && SongUtility.curBeat % 2 == 0)
		{
			GameController.instance.gameCam.Zoom += new Vector2(0.065f, 0.065f);
			GameController.instance.hudCam.Zoom += new Vector2(0.025f, 0.025f);

			bloomValue += 0.03f;
		}
		else if (SongUtility.curBeat >= 180 && SongUtility.curBeat < 212)
		{
			GameController.instance.gameCam.Zoom += new Vector2(0.065f, 0.065f);
			GameController.instance.hudCam.Zoom += new Vector2(0.025f, 0.025f);

			bloomValue += 0.03f;
		}
    }

    public void OnSection()
    {
        switch (SongUtility.curSection)
        {
            case 6:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 8:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 10:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 12:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 16:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 20:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 24:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 28:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 32:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 36:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 39:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 41:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 43:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
            case 45:
                GameController.instance.gameCam.Position = new Vector2(397, 360);
                break;
            case 49:
                GameController.instance.gameCam.Position = new Vector2(842, 360);
                break;
        }
    }

}
