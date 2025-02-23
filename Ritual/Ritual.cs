using Godot;
using System;
using ritualFlags;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class Ritual : Node2D
{
	private String abilityForCreation;
	private int creationDifficulty;
	private int creationCheck;
	private String abilityToPerform;
	private int performingDifficulty;
	private int performingCheck;
	private TypesOfTime typeOfTimeDevoted;
	private int timeSpentCheckModifier;
	public Godot.Collections.Array<Node2D> ritualObjects;
	private CircumstanceComplexityModifiers circumstanceModifiers;
	private float circumstanceLocationLeylineValue;
	private int circumstanceWeatherValue;
	private int circumstanceTimeValue;
	private int circumstanceRitualNatureValue;
	private float circumstanceThreeFoldSymmetryValue;
	private float circumstanceFiveFoldSymmetryValue;
	private int circumstanceLifeOnTheLineValue;
	private float cirumstancePercentageModifiersValue;
	private int circumstanceFlatModifiersValue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.abilityForCreation = "Occult";
        this.creationDifficulty = 0;
        this.creationCheck = 0;

        this.abilityToPerform = "None";
        this.performingDifficulty = 0;
        this.performingCheck = 0;

        this.typeOfTimeDevoted = TypesOfTime.NONE;
        this.timeSpentCheckModifier = 0;
        this.ritualObjects = new Godot.Collections.Array<Node2D>();

        this.circumstanceModifiers = CircumstanceComplexityModifiers.NONE;
        this.circumstanceLocationLeylineValue = 0.0f;
        this.circumstanceWeatherValue = 0;
        this.circumstanceTimeValue = 0;
        this.circumstanceRitualNatureValue = 0;
        this.circumstanceThreeFoldSymmetryValue = 0.0f;
        this.circumstanceFiveFoldSymmetryValue = 0.0f;
        this.circumstanceLifeOnTheLineValue = 0;

        this.cirumstancePercentageModifiersValue = 0.0f;
        this.circumstanceFlatModifiersValue = 0;
	}

	public void InitRitualObject (Image image, ImageTexture imageTexture, String filePath)
	{
		PackedScene newObjectScene = GD.Load<PackedScene>("res://Ritual/ritual_object.tscn");
		RitualObject newObject = (RitualObject) newObjectScene.Instantiate();
		this.ritualObjects.Add(newObject);
		this.AddChild(newObject);
		newObject.InitImage(image, imageTexture);
		newObject.name = filePath;
		newObject.nameEdit.PlaceholderText = filePath;
		this.UpdateConnectionSelectors();
	}

	public RitualObject GetSelectedComponent (String name)
	{
		GD.Print("searching for: " + name);
		foreach (RitualObject ritualObject in this.ritualObjects)
		{
			if (ritualObject.name == name)
			{
				return ritualObject;
			}
		}
		return null;
	}

	public void UpdateConnectionSelectors ()
	{
		foreach (RitualObject ritualObject in this.ritualObjects)
		{
			string previouslySelectedComponent = null;
			if (ritualObject.parent != null)
			{
				previouslySelectedComponent = ritualObject.parent.name;
			}

			int currentIndex = 1;
			ritualObject.connectionSelector.Clear();
			ritualObject.connectionSelector.AddSeparator("Select component");
			ritualObject.connectionSelector.SetItemDisabled(0, true);
			foreach (RitualObject otherRitualObject in this.ritualObjects)
			{
				if (otherRitualObject == ritualObject)
				{
					continue;
				}
				ritualObject.connectionSelector.AddItem(otherRitualObject.name, currentIndex);
				currentIndex += 1;
			}

			// If it had something selected from before, it checks for the previously selected component
			// once it finds it, it reselect it so it appears in the menu
			if (previouslySelectedComponent != null)
			{
				for (int i = 1; i <= currentIndex; i += 1)
				{
					if (ritualObject.connectionSelector.GetItemText(i) == previouslySelectedComponent)
					{
						ritualObject.connectionSelector.Select(i);
						break;
					}
				}
				continue;
			}
			ritualObject.connectionSelector.Select(0);
		}
	}

	public void DeleteComponent (RitualObject ritualObject)
	{
		if (this.ritualObjects.Remove(ritualObject))
		{
			if (ritualObject.parent != null)
			{
				ritualObject.parent.children.Remove(ritualObject);
			}
			if (ritualObject.children.Count > 0)
			{
				foreach (RitualObject childComponent in ritualObject.children)
				{
					childComponent.parent = null;
					// dejo la linea de abajo comentada solo para que quede precedente de que hacer esto aparentement jode la iteración por el arreglo
					// ritualObject.children.Remove(childComponent);
				}
			}
		}
		ritualObject.QueueFree();
		this.UpdateConnectionSelectors();
	}

	// this function is called each time an object's class gets altered, if there is no quintessence it's going to call
	// validity prompt to prompt the message that the current setup is not valid, and that there must be at least
	// one quintessence
	public void CheckIfComponentClassesValid ()
	{
		// as of right now it makes sure that there is at least one quintessence
		int quintessenceCounter = 0;
		foreach (RitualObject currentRitualObject in this.ritualObjects)
		{
			if (currentRitualObject.componentClass == ComponentClass.QUINTESSENCE)
			{
				quintessenceCounter += 1;
			}
		}
		if (quintessenceCounter <= 0)
		{
			GD.Print("Current setup is not valid");
			return;
		}
		GD.Print("Current setup is valid");
	}

	public void TypesOfTimeChanged (int newType)
	{
		this.typeOfTimeDevoted = (TypesOfTime) newType;
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        // base._UnhandledInput(@event);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
