using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

public partial class Camera2D : Godot.Camera2D
{
    private const float MIN_ZOOM = 0.1f;
    private const float MAX_ZOOM = 30f;
    private const float ZOOM_RATE = 1.2f;
	private const float ZOOM_FACTOR_BASE = 1.0f;
    private float zoomFactor = 1.0f;
    public bool mouseOverUI = false;                // hacer getters de estos dos porque como que me jode que estén en públicos
    public bool inMovement = false;
    private Vector2 mouseStartingPosition = Vector2.Inf;

    //private RitualCreationUI _ritualCreationUI;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // _ritualCreationUI = GetParent().GetNode<RitualCreationUI>("RitualCreationUI");
        /** 
		 * El unico motivo por el que dejo esto aca es para que haya un registro de que 
		 * me daba un error si no usaba el GetParent() para obtener el nodo.
		 * Que si no igual me olvido de que esto paso siquiera.
		 *
		*/
    }

    // ... si... es un getter... lo siento
    public Vector2 getCameraZoom()
    {
        return this.Zoom;
    }

    public void MouseEnteredUI()
    {
        // GD.Print("entro a la cosa bien");
        mouseOverUI = true;
    }
    public void MouseExitedUI()
    {
        // GD.Print("salio de la cosa bien");
        mouseOverUI = false;
    }

    public void zoomOut()
    {
        zoomFactor -= ZOOM_RATE;
        if (zoomFactor < MIN_ZOOM)
        {
            zoomFactor = MIN_ZOOM;
        }
    }
    public void zoomIn()
    {
        zoomFactor += ZOOM_RATE;
        if (zoomFactor > MAX_ZOOM)
        {
            zoomFactor = MAX_ZOOM;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            // checks if mouse position was outside ui at the beginning of the movement if the camera wasn't already moving,
            // then, should be able to move freely even if the mouse is over the UI, if and only if, the position
            // that began the movement was outside of the UI, otherwise the position is set at Vector2.Inf
            if (mouseOverUI == false && inMovement == false)
            {
                mouseStartingPosition = mouseMotionEvent.Position;
            }
            if ((Input.IsActionPressed("ui_move_camera") && Input.IsActionPressed("ui_left_click_select"))
                || (Input.IsActionPressed("ui_middle_click")))
            {
                if (mouseStartingPosition != Vector2.Inf)
                {
                    this.Offset -= (mouseMotionEvent.Relative / this.Zoom);
                    inMovement = true;
                }
                else
                {
                    // para evitar que, si se mantienen los inputs apretados desde la UI, el movimiento se inicie apenas se salga de esta
                    inMovement = true;
                }
            }
            else
            {
                mouseStartingPosition = Vector2.Inf;
                inMovement = false;
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // check if mouse position was outside ui at the beginning of the movement
        // if false let this through
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.IsActionPressed("ui_mouse_wheel_up"))
            {
				// GD.Print("rueda hacia arriba");
                this.zoomIn();
                SetPhysicsProcess(true);
            }
            else if (mouseButtonEvent.IsActionPressed("ui_mouse_wheel_down"))
            {
				// GD.Print("rueda hacia abajo");
                this.zoomOut();
                SetPhysicsProcess(true);
            }
            GetViewport().SetInputAsHandled();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        this.Zoom = this.Zoom.Lerp(zoomFactor * Vector2.One, ZOOM_RATE * (float)delta);
		zoomFactor = ZOOM_FACTOR_BASE;
        SetPhysicsProcess(false);
    }
}
