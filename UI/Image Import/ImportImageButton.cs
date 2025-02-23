using Godot;
using System;
using System.Net;
using System.Reflection;

public partial class ImportImageButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _GuiInput(InputEvent @event)
    {
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsActionReleased("ui_left_click_select") && this.IsHovered())
			{
				GetTree().CurrentScene.GetNode<Control>("ImportImages").GetNode<FileDialog>("ImportImagePopup").Popup();
			}
			else
			{
				this.AcceptEvent();
			}
			this.AcceptEvent();
			ReleaseFocus();
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		ReleaseFocus();
	}
}
