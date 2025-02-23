using Godot;
using System;

/*
 * Nota: el motivo por el que el MouseFilter está en pass, es porque así el PanelContainer
 * puede recibir la información de que el mouse está por encima, por tanto, para evitar que esa información se pase a 
 * los otros nodos, el MouseFilter del PanelContainer está en Stop
 * 
 * */

public partial class ExitButton_Test : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _GuiInput(InputEvent @event)
    {
        //base._GuiInput(@event);
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsActionReleased("ui_left_click_select") && this.IsHovered())
			{
				GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
				GetTree().Quit();
			}
			else
			{
				// GD.Print("entra");
				this.AcceptEvent();
			}
			this.AcceptEvent();
			ReleaseFocus();
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		_GuiInput(null);
	}
}
