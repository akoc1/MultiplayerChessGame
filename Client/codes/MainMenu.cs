using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{

    #region Nodes

    Label statusLabel;
    Button createGameButton;
    Button joinGameButton;

    #endregion

    ServerHandler serverHandler;

    public override void _Ready()
    {
        statusLabel = GetNode<Label>("%StatusLabel");
        createGameButton = GetNode<Button>("%CreateGameButton");
        joinGameButton = GetNode<Button>("%JoinGameButton");

        serverHandler = GetNode<ServerHandler>("/root/ServerHandler");

        serverHandler.FailedToConnect += _FailedConnect;
        serverHandler.Connected += _Connected;
        serverHandler.GameStartedAction += OnGameStart;

        ConnectToServer();
    }

    private void OnGameStart(StartGame game)
    {
        // Redirect to game page on game start

        GD.Print("start game request");

        PackedScene gameScene = (PackedScene)ResourceLoader.Load("res://scenes/Game.tscn");
        
        GetTree().ChangeSceneToPacked(gameScene);
    }

    private void _Connected()
    {
        statusLabel.Text = "Connected to the server";

        createGameButton.Set("disabled", false);
        joinGameButton.Set("disabled", false);
    }

    private async void _FailedConnect(string message)
    {
        statusLabel.Text = $"Failed to connect: {message} Trying again ...";

        createGameButton.Set("disabled", true);
        joinGameButton.Set("disabled", true);

        await serverHandler.ConnectAsync();
    }

    private async void ConnectToServer()
    {
        statusLabel.Text = "Connecting to the server...";

        await serverHandler.ConnectAsync();
    }

    private void _on_create_game_button_pressed()
    {
        PackedScene createGameScene = (PackedScene)ResourceLoader.Load("res://scenes/CreateGame.tscn");
        
        CreateGame instance = createGameScene.Instantiate<CreateGame>();

        this.AddChild(instance);
    }

    private void _on_join_game_button_pressed()
    {
        PackedScene joinGameScene = (PackedScene)ResourceLoader.Load("res://scenes/JoinGame.tscn");
        
        JoinGame instance = joinGameScene.Instantiate<JoinGame>();

        this.AddChild(instance);
    }
}
