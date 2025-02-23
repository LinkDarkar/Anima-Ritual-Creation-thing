using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ImportImages : Control
{
    private Ritual ritual;
    public override void _Ready()
    {
        ritual = (Ritual) GetTree().CurrentScene.GetNode<Node2D>("Ritual");
        GetViewport().Connect("files_dropped", new Callable(this, "OnFilesDropped"));
    }

    public void OnFilesDropped(string[] files)
    {
        GD.Print("the file dropped on the window was: " + files[0]);
        this.ImportImage(files[0]);
    }

    private static bool IsFileImage(string fileName)
    {
        List<string> imageFileExtensions = new List<string> {".png", ".jpg", ".jpeg", ".bmp", ".svg", ".svgz", ".tga", ".webp"};
        
        foreach (string extension in imageFileExtensions)
        {
            if (Path.GetExtension(fileName).ToLower().Equals(extension))
            {
                return true;
            }
        }
        return false;
    }
    
    public void ImportImage(string filePath)
    {
        Image image = new Image();
        if (image.Load(filePath) == Error.Ok && IsFileImage(filePath) == true)
        {
            ImageTexture imageTexture = new ImageTexture();
            imageTexture.SetImage(image);
            ritual.InitRitualObject(image, imageTexture, filePath);
        }
        else
        {
            GetNode<AcceptDialog>("ErrorPopup").DialogText = "File is not an image.";
            GetNode<AcceptDialog>("ErrorPopup").Show();
        }
    }    

    public override void _Process(double delta)
    {      
    }
}
