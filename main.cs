using Godot;
using System;

public partial class main : Node2D
{
	private Ritual ritual;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// this.ritual = GetNode<Camera2D>("Camera2D").GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport").GetNode<Ritual>("Ritual");
		// SubViewport test = GetNode<Camera2D>("Camera2D").GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport");
		// this.ritual = GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport").GetNode<Ritual>("Ritual");
		this.ritual = GetNode<Ritual>("Ritual");
	}

	/**
	 * the reason why this is the node that draws the lines between parents and childs
	 * is none other that it's at (0,0), this makes it so I can draw the line between them
	 * without having to worry too much about the relative coordinates between
	 * the components, since trying to do this in the components themselves is a pain in the ass
	*/
	public override void _Draw()
	{
		foreach (RitualObject ritualObject in this.ritual.ritualObjects)
		{
			/*
			if (IsInstanceValid(ritualObject) == false)
			{
				GD.Print("objeto no valido");
				continue;
			}
			//*/
	   		if (ritualObject.children.Count > 0)
	   		{
	   			foreach (RitualObject childComponent in ritualObject.children)
	   			{
					/*
					if (IsInstanceValid(childComponent) == false)
					{
						GD.Print("objeto no valido");
						continue;
					}
					//*/
	   				this.DrawLine(ritualObject.getTrueCenter(), childComponent.getTrueCenter(), ritualObject.circleColor, ritualObject.circleLineWidth, true);
	   			}
	   		}
		}
	}

    public override async void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (Input.IsActionJustPressed("save_test"))
		{
			GetViewport().TransparentBg = true;
			GetNode<CanvasLayer>("RitualCreationUI").Visible = false;
			await ToSignal(RenderingServer.Singleton, RenderingServerInstance.SignalName.FramePostDraw);
			GetViewport().GetTexture().GetImage().SavePng("user://Screenshot.png");
			GetViewport().TransparentBg = false;
			GetNode<CanvasLayer>("RitualCreationUI").Visible = true;
		}
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		QueueRedraw();
	}
}
