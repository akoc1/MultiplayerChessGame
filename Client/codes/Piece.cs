using Godot;
using System;

public partial class Piece : Node2D
{
    #region Nodes

    Sprite2D pieceSprite;
    Area2D pieceArea;
    CollisionShape2D pieceAreaCollisionShape;

    #endregion

    #region Variables

    [Export]
    public PieceType pieceType = PieceType.Pawn;
    [Export]
    public PieceColor pieceColor = PieceColor.White;
    private bool OwnPiece = true;
    private bool MouseEntered = false;
    private Resource defaultCursorResource;
    private Resource openHandResource;
    private Resource captureHandResource;
    Vector2 hotSpot = new Vector2(12, 16);
    public Vector2 SquareSize;
    
    #endregion

    public override void _Ready()
    {
        pieceSprite = GetNode<Sprite2D>("%PieceSprite");
        pieceArea = GetNode<Area2D>("%Area");
        pieceAreaCollisionShape = GetNode<CollisionShape2D>("%CollisionShape");

        defaultCursorResource = ResourceLoader.Load("res://assets/sprites/cursors/cursor_none.png");
        openHandResource = ResourceLoader.Load("res://assets/sprites/cursors/hand_small_open.png");
        captureHandResource = ResourceLoader.Load("res://assets/sprites/cursors/hand_small_closed.png");

        RectangleShape2D shape = new RectangleShape2D();
        shape.Size = SquareSize;

        pieceAreaCollisionShape.Shape = shape;

        SetImage();
        SnapToPoint();
    }

    private void SetImage()
    {
        string path = string.Empty;
        string param = pieceColor == PieceColor.White ? "-w" : "-b";

        switch (pieceType)
        {
            case PieceType.Pawn:
                path = $"res://assets/sprites/pieces/pawn{param}.png";

                break;
            case PieceType.Rook:
                path = $"res://assets/sprites/pieces/rook{param}.png";

                break;
            case PieceType.Queen:
                path = $"res://assets/sprites/pieces/queen{param}.png";

                break;
            case PieceType.King:
                path = $"res://assets/sprites/pieces/king{param}.png";

                break;
            case PieceType.Bishop:
                path = $"res://assets/sprites/pieces/bishop{param}.png";

                break;
            case PieceType.Knight:
                path = $"res://assets/sprites/pieces/knight{param}.png";

                break;
        }

        var texture = GD.Load<Texture2D>(path);
        pieceSprite.Texture = texture;
    }

    public override void _Process(double delta)
    {
        bool lmbPressed = Input.IsActionPressed("lmb");
        bool lmbJustReleased = Input.IsActionJustReleased("lmb");

        if (OwnPiece)
        {
            if (MouseEntered)
            {
                if (MouseEntered && DragManager.DraggedPiece == null && lmbPressed)
                {
                    DragManager.DraggedPiece = this;
                }

                if (DragManager.DraggedPiece == this)
                {
                    if (lmbPressed)
                    {
                        GlobalPosition = GetGlobalMousePosition();
                        Input.SetCustomMouseCursor(captureHandResource, Input.CursorShape.PointingHand, hotSpot);
                    }

                    if (lmbJustReleased)
                    {
                        SnapToPoint();
                        DragManager.DraggedPiece = null;
                    }
                }
            }
        }
    }

    public void SnapToPoint()
    {
        GlobalPosition = VectorUtils.SnapToPoint(pieceArea.GlobalPosition, SquareSize);
    }

    private void OnMouseEntered()
    {
        if (OwnPiece)
        {
            MouseEntered = true;

            Input.SetCustomMouseCursor(openHandResource, Input.CursorShape.PointingHand, hotSpot);
        }
    }

    private void OnMouseExited()
    {
        if (OwnPiece)
        {
            MouseEntered = false;

            Input.SetCustomMouseCursor(defaultCursorResource, Input.CursorShape.Arrow, hotSpot);
        }
    }
}
