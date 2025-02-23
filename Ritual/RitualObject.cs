using Godot;
using System;
using ritualFlags;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Reflection;

public partial class RitualObject : Node2D
{
    private bool mouseOverObject = false;
    private bool inMovement = false;
    private bool imageSelected = false;
    public ComponentClass componentClass = ComponentClass.NONE;
    private Camera2D camera2D;
    private Ritual ritual;
    private Vector2 movementStartingPosition = Vector2.Inf;
    public LineEdit nameEdit;
    public OptionButton connectionSelector;
    public OptionButton componentClassSelector;
    private ReferenceRect selectedHighlight;
    public String name;

    /**
     * imageRegion: it saves the region that composes the original image so that we can use it for the resize method.
     * resizedImage: The image that's going to be show once the original gets resized.
     * This is a copy, so that we can keep referencing the original image, to prevent it 
     * from getting too distorded, after too many resizes.
     * componentImageTexture: Used to apply the Image as a texture to the ObjectImage.
     * componentImage: what's actually shown... I think.
    */
    private Image image;
    private Rect2I imageRegion; 
    private Image resizedImage; 
    private ImageTexture componentImageTexture;
    private ObjectImage componentImage;

    private Vector2 trueCenter;
    private Vector2 imageCenter;
    private float circleRadius;
    public float circleLineWidth = 25;
    private float inBetweenLineWidth = 100f;
    public Color circleColor = new Color (0,0,0,1);

    /**
     * To connect components to each other, a tree will be used.
     * Due to the requirements, the children are connected to their parent, not viceversa
     * so the parent CANNOT be in the children array.
     * If the parent is null, then the component is on the highest level
     * possible on the tree.
     *
    */ 
    public RitualObject parent = null;
    public Godot.Collections.Array<RitualObject> children = new Godot.Collections.Array<RitualObject>();

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        camera2D = GetTree().CurrentScene.GetNode<Camera2D>("Camera2D");
        nameEdit = this.GetNode<Popup>("PopupObjectEditMenu").GetNode<PanelContainer>("PanelContainer").GetNode<MarginContainer>("MarginContainer").
                                    GetNode<TabContainer>("TabContainer").GetNode<VBoxContainer>("ClassAndConnections").GetNode<HBoxContainer>("Name").
                                    GetNode<LineEdit>("NameEdit");
        componentClassSelector = this.GetNode<Popup>("PopupObjectEditMenu").GetNode<PanelContainer>("PanelContainer").GetNode<MarginContainer>("MarginContainer").
                                    GetNode<TabContainer>("TabContainer").GetNode<VBoxContainer>("ClassAndConnections").GetNode<HBoxContainer>("ClassSelection").
                                    GetNode<OptionButton>("ComponentClassSelector");
        connectionSelector = this.GetNode<Popup>("PopupObjectEditMenu").GetNode<PanelContainer>("PanelContainer").GetNode<MarginContainer>("MarginContainer").
                                    GetNode<TabContainer>("TabContainer").GetNode<VBoxContainer>("ClassAndConnections").GetNode<HBoxContainer>("Connections").
                                    GetNode<OptionButton>("ComponentConnectionSelector");
        selectedHighlight = this.GetNode<TextureRect>("ObjectImage").GetNode<ReferenceRect>("SelectedHighlight");
        this.ritual = GetTree().CurrentScene.GetNode<Ritual>("Ritual");
        this.componentImage = this.GetNode<ObjectImage>("ObjectImage");

	}

    public Vector2 getImageCenter()
    {
        return this.imageCenter;
    }
    public Vector2 getTrueCenter()
    {
        return this.trueCenter;
    }

    private void MouseEntered()
    {
        GD.Print("mouse in");
        this.mouseOverObject = true;
    }
    private void MouseExited()
    {
        GD.Print("mouse out");
        this.mouseOverObject = false;
    }
    private void CloseRequest()
    {
        GetNode<Popup>("PopupObjectEditMenu").Visible = false;
    }
    public void InitImage (Image image, ImageTexture imageTexture)
    {
        this.image = image;
        this.componentImageTexture = imageTexture;
        
        this.componentImage.Texture = this.componentImageTexture;
        this.imageCenter = this.componentImage.Texture.GetSize() / 2;
        this.trueCenter = this.Position + this.imageCenter;
        this.circleRadius = this.componentImage.Texture.GetSize().Length() / 2;
        
        this.imageRegion = new Rect2I((Vector2I) this.Position, (Vector2I) this.componentImage.Texture.GetSize());
        this.resizedImage = this.image.GetRegion(imageRegion);
    }
    private void changeTextureSize (int width, int height)
    {
        this.resizedImage = this.image.GetRegion(imageRegion);
        this.resizedImage.Resize(width, height, Image.Interpolation.Lanczos);
        this.componentImageTexture.SetImage(this.resizedImage);
        this.componentImage.Texture = this.componentImageTexture;
        this.imageCenter = this.componentImage.Texture.GetSize() / 2;
        this.trueCenter = this.Position + this.imageCenter;
        this.circleRadius = this.componentImage.Texture.GetSize().Length() / 2;
        this.QueueRedraw();
    }

    private void NameChanged (String newName)
    {
        this.name = newName;
        this.ritual.UpdateConnectionSelectors();
    }
    private void ComponentClassSelected (int selection)
    {
        this.componentClass = (ComponentClass) selection;
        this.ritual.CheckIfComponentClassesValid();
    }
    private void ConnectToParent(int selection)
    {
        if (this.parent != null)
        {
            this.parent.DisconnectChildren(this);
        }
        RitualObject newParent = this.ritual.GetSelectedComponent(this.connectionSelector.GetItemText(selection));
        this.parent = newParent;
        newParent.ConnectToChildren(this);
        this.QueueRedraw();
        newParent.QueueRedraw();
    }
    private void ConnectToChildren(RitualObject newChild)
    {
        this.children.Add(newChild);
    }
    private void DisconnectChildren(RitualObject removedChild)
    {
        this.children.Remove(removedChild);
    }
    private void CircleLineWidthChanged (float newValue)
    {
        this.circleLineWidth = newValue;
        this.QueueRedraw();
    }
    private void InBetweenLineWidthChanged (float newValue)
    {
        this.inBetweenLineWidth = newValue;
        this.QueueRedraw();
    }
    private void CircleColorChanged (Color newColor)
    {
        this.circleColor = newColor;
        this.QueueRedraw();
    }

    public override void _Input(InputEvent @event)
    { 
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            if (camera2D.mouseOverUI == false && this.inMovement == false && this.mouseOverObject == true)
            {
                this.movementStartingPosition = mouseMotionEvent.Position;
            }
            if (Input.IsActionPressed("ui_left_click_select") && camera2D.inMovement == false)
            {
                if (this.movementStartingPosition != Vector2.Inf)
                {
                    this.Position += mouseMotionEvent.Relative / camera2D.Zoom;
                    this.trueCenter += mouseMotionEvent.Relative / camera2D.Zoom;
                    this.inMovement = true;
                }
                else
                {
                    this.inMovement = true;
                }
            }
            else
            {
                movementStartingPosition = Vector2.Inf;
                this.inMovement = false;
            }
        }

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.IsActionReleased("ui_right_click_select") && this.mouseOverObject == true && camera2D.inMovement == false)
            {
                GetNode<Popup>("PopupObjectEditMenu").Visible = true;
            }
            if (mouseButtonEvent.IsActionPressed("ui_left_click_select"))
            {
                if (this.mouseOverObject == true)
                {
                    this.imageSelected = true;
                    selectedHighlight.Visible = true;
                }
                else if (this.mouseOverObject == false && this.imageSelected == true && this.inMovement == false)
                {
                    this.imageSelected = false;
                    selectedHighlight.Visible = false;
                }
            }
        }

        if (Input.IsActionJustPressed("test"))
        {
            this.changeTextureSize(500,500);
        }
        if (Input.IsActionJustPressed("test2"))
        {
            GD.Print("width: ", this.image.GetWidth());
            GD.Print("height: ", this.image.GetHeight());
            this.changeTextureSize(this.image.GetWidth(), this.image.GetHeight());
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("delete") && this.imageSelected == true)
        {
            this.ritual.DeleteComponent(this);
        }
    }

    public override void _Draw()
    {
        this.DrawArc(this.imageCenter, this.circleRadius, 0, 2*Godot.Mathf.Pi, 360, this.circleColor, this.circleLineWidth, true);
        this.DrawArc(this.imageCenter, this.circleRadius + this.inBetweenLineWidth, 0, 2*Godot.Mathf.Pi, 360, this.circleColor, this.circleLineWidth, true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        this.QueueRedraw();
	}
}