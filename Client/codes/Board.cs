using Godot;

public partial class Board : Node2D
{
    public Vector2 StartingPoint;
    public Vector2 Ratio;
    public Vector2 SquareSize;
    public Vector2 BorderSize;

    DragManager dragManager;

    public override void _Ready()
    {
        dragManager = GetNode<DragManager>("DragManager");

        InitializePieces();

        QueueRedraw();
    }

    public void InitializePieces()
    {
        // opponent
        for (int y = 0; y <= 1; y++)
        {
            for (int i = 0; i < 8; i++)
            {
                PackedScene pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/Piece.tscn");

                Piece piece = (Piece)pieceScene.Instantiate();
                piece.SquareSize = SquareSize;

                piece.GlobalPosition = new Vector2(
                    StartingPoint.X + SquareSize.X * i,
                    StartingPoint.Y + SquareSize.Y * y
                );

                if (y == 0)
                {
                    if (i == 0 || i == 7)
                        piece.pieceType = PieceType.Rook;

                    if (i == 1 || i == 6)
                        piece.pieceType = PieceType.Knight;
                    
                    if (i == 2 || i == 5)
                        piece.pieceType = PieceType.Bishop;

                    if (i == 3)
                        piece.pieceType = PieceType.King;

                    if (i == 4)
                        piece.pieceType = PieceType.Queen;
                }
                else
                {
                    piece.pieceType = PieceType.Pawn;
                }

                GD.Print($"x: {piece.GlobalPosition.X}, y: {piece.GlobalPosition.Y}");

                AddChild(piece);
            }
        }

        for (int y = 6; y <= 7; y++)
        {
            for (int i = 0; i < 8; i++)
            {
                PackedScene pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/Piece.tscn");

                Piece piece = (Piece)pieceScene.Instantiate();
                piece.SquareSize = SquareSize;
                piece.pieceColor = PieceColor.Black;

                piece.GlobalPosition = new Vector2(
                    StartingPoint.X + SquareSize.X * i,
                    StartingPoint.Y + SquareSize.Y * y
                );

                if (y == 7)
                {
                    if (i == 0 || i == 7)
                        piece.pieceType = PieceType.Rook;

                    if (i == 1 || i == 6)
                        piece.pieceType = PieceType.Knight;
                    
                    if (i == 2 || i == 5)
                        piece.pieceType = PieceType.Bishop;

                    if (i == 3)
                        piece.pieceType = PieceType.King;

                    if (i == 4)
                        piece.pieceType = PieceType.Queen;
                }
                else
                {
                    piece.pieceType = PieceType.Pawn;
                }

                GD.Print($"x: {piece.GlobalPosition.X}, y: {piece.GlobalPosition.Y}");

                AddChild(piece);
            }
        }
    }


    public override void _Draw()
    {
        DrawCircle(StartingPoint, 1f, Colors.Crimson);

        /*for (int y = 0; y <= 1; y++)
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
        }*/
    }
}
