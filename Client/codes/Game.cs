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
        board.BorderSize /= board.Ratio;
        board.SquareSize = (ResizedBoardSize - board.BorderSize * 2) / 8;
        board.StartingPoint = board.BorderSize;

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
