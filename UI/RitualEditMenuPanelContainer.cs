using Godot;
using System;

public partial class RitualEditMenuPanelContainer : PanelContainer
{
	private BringRitualMenuButton bringRitualMenuButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bringRitualMenuButton = GetParent().GetNode<PanelContainer>("BringRitualMenuPanel").GetNode<BringRitualMenuButton>("BringRitualMenuButton");
	}

	public void PlayAnimation (string animationName)
	{
		this.GetNode<AnimationPlayer>("MenuAnimations").Play(animationName);
		if (animationName == "ClosingAnimation")
		{
			bringRitualMenuButton.GetParent<PanelContainer>().Visible = true;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
