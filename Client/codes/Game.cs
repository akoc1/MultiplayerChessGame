using Godot;
using System;

public partial class Game : Control
{
    #region Nodes

    private TextureRect BoardTexture;
    private Board board;
    private GameManager GameManager;

    #endregion

    Vector2 ResizedBoardSize = new Vector2(420, 420);
    Vector2 BoardTextureSize = new Vector2(784, 784);
    Vector2 BorderSize = new Vector2(8, 8);

    public override void _Ready()
    {
        BoardTexture = GetNode<TextureRect>("%BoardTexture");

        LoadBoard();
    }

    public void LoadBoard()
    {
        PackedScene boardScene = (PackedScene)ResourceLoader.Load("res://scenes/Board.tscn");

        board = (Board)boardScene.Instantiate();

        board.Ratio = BoardTextureSize / ResizedBoardSize;
        BorderSize /= board.Ratio;
        GD.Print(BorderSize);
        board.BorderSize = new Vector2(BorderSize.X / 2, BorderSize.Y);
        board.SquareSize = (ResizedBoardSize - board.BorderSize * 2) / 8;
        board.StartingPoint = BorderSize;

        BoardTexture.AddChild(board);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("space"))
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
