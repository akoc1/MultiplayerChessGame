using Godot;
using System;
using System.Threading.Tasks;

public partial class CreateGame : Control
{
    Label infoLabel;
    Label codeLabel;

    ServerHandler serverHandler;

    public override async void _Ready()
    {
        serverHandler = GetNode<ServerHandler>("/root/ServerHandler");
        
        infoLabel = GetNode<Label>("%InfoLabel");
        codeLabel = GetNode<Label>("%CodeLabel");

        await serverHandler.SendCreateGameRequest();

        serverHandler.GameCreateResponseAction += OnGameCreateResponse;
    }

    private void OnGameCreateResponse(CreateGameResponse response)
    {
        if (response.Success)
        {
            infoLabel.Text = "Game successfully created. Share this code with your friends!";
        } else
        {
            if (response.Result == CreateGameResponse.Types.ResponseResult.AlreadyInGame)
            {
                infoLabel.Text = "You have already created a game.";
            }
        }

        codeLabel.Text = response.GameCode;

        GetNode<VBoxContainer>("%CreatingGameContainer").Set("visible", false);
        GetNode<VBoxContainer>("%GameCreatedContainer").Set("visible", true);
    }

    private void _on_copy_button_pressed()
    {
        Label codeLabel = GetNode<Label>("%CodeLabel");
        DisplayServer.ClipboardSet(codeLabel.Text);
    }

    private void _on_cancel_button_pressed()
    {
        serverHandler.GameCreateResponseAction -= OnGameCreateResponse;

        this.QueueFree();
    }
}
