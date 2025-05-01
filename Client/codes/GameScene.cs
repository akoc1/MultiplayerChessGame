using Godot;
using System;

public partial class GameScene : Control
{
    #region Nodes

    private TextureRect Board;
    private Node2D BoardNode2D;

    #endregion

    public override void _Ready()
    {
        Board = GetNode<TextureRect>("%Board");
        BoardNode2D = GetNode<Node2D>("%BoardNode2D");

        GD.Print($"{Board}");
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        Vector2 boardPosition = BoardNode2D.GlobalPosition;
        Vector2 availableBoardSize = Board.Size - new Vector2(16, 16);
        Vector2 squareSize = availableBoardSize / 8.0f;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (y >= 2 && y < 6)
                    continue;

                Vector2 position = boardPosition + new Vector2(8f + x * squareSize.X, 8f + y * squareSize.Y);
                DrawRect(new Rect2(position, squareSize), Colors.Crimson, true, -1, true);
            }
        }
    }
}
