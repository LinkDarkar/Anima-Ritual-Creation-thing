using Godot;
using System;

public partial class main : Node2D
{
	private Ritual ritual;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
}
