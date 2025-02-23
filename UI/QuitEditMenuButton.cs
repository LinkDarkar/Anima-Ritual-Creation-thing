using Godot;
using System;

public partial class QuitEditMenuButton : Button
{
    private RitualEditMenuPanelContainer ritualEditMenuPanelContainer;
    public override void _Ready()
    {
        ritualEditMenuPanelContainer = GetTree().CurrentScene.GetNode<CanvasLayer>("RitualCreationUI").
                                        GetNode<RitualEditMenuPanelContainer>("RitualEditMenuPanelContainer");
    }
    public override void _GuiInput(InputEvent @event)
    {
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.IsActionReleased("ui_left_click_select") && this.IsHovered())
			{
				ritualEditMenuPanelContainer.PlayAnimation("ClosingAnimation");
			}
			else
			{
				this.AcceptEvent();
			}
			this.AcceptEvent();
			ReleaseFocus();
		}
    }
}