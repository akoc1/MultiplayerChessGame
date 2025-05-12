using Godot;

public partial class Board : Node2D
{
    public Vector2 StartingPoint;
    public Vector2 Ratio;
    public Vector2 SquareSize;
    public Vector2 BorderSize = new Vector2(8, 8);

    DragManager dragManager;

    public override void _Ready()
    {
        dragManager = GetNode<DragManager>("DragManager");

        InitializePieces();
    }

    public void InitializePieces()
    {
        for (int y = 0; y <= 1; y++)
        {
            for (int i = 0; i < 8; i++)
            {
                PackedScene pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/Piece.tscn");

                Piece piece = (Piece)pieceScene.Instantiate();

                piece.SquareSize = SquareSize;
                
                if (i == 0)
                {
                    piece.GlobalPosition = new Vector2(StartingPoint.X, StartingPoint.Y + SquareSize.Y * y);
                }
                else
                {
                    piece.GlobalPosition = new Vector2(BorderSize.X + ((SquareSize.X + SquareSize.X) * i), BorderSize.Y + SquareSize.Y * y + 25);
                }

                GD.Print(piece.GlobalPosition);

                AddChild(piece);
            }
        }
    }

    /*public override void _Draw()
    {
        DrawCircle(StartingPoint, 4f, Colors.Crimson);

        for (int y = 0; y <= 1; y++)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    DrawRect(new Rect2(new Vector2(StartingPoint.X, StartingPoint.Y + SquareSize.Y * y), SquareSize), Colors.Crimson);
                }
                else
                {
                    DrawRect(new Rect2(new Vector2(BorderSize.X + SquareSize.X * i, BorderSize.Y + SquareSize.Y * y), SquareSize), Colors.Crimson);
                }
            }
        }

        for (int y = 6; y <= 7; y++)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    DrawRect(new Rect2(new Vector2(StartingPoint.X, StartingPoint.Y + SquareSize.Y * y), SquareSize), Colors.SeaGreen);
                }
                else
                {
                    DrawRect(new Rect2(new Vector2(BorderSize.X + SquareSize.X * i, BorderSize.Y + SquareSize.Y * y), SquareSize), Colors.SeaGreen);
                }
            }
        }
    }*/
}
