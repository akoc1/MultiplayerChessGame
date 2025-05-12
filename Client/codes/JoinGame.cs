using Godot;
using System;
using System.Threading.Tasks;

public partial class JoinGame : Control
{
    VBoxContainer joiningGameContainer;
    VBoxContainer joinConfigContainer;
    LineEdit codeLineEdit;
    Label infoLabel;

    ServerHandler serverHandler;

    private string previousCode;

    public override void _Ready()
    {
        infoLabel = GetNode<Label>("%InfoLabel");
        codeLineEdit = GetNode<LineEdit>("%CodeLineEdit");
        joiningGameContainer = GetNode<VBoxContainer>("%JoiningGameContainer");
        joinConfigContainer = GetNode<VBoxContainer>("%JoinConfigContainer");

        serverHandler = GetNode<ServerHandler>("/root/ServerHandler");

        serverHandler.JoinGameResponseAction += OnJoinGameResponse;
    }

    private void OnJoinGameResponse(JoinGameResponse response)
    {
        if (response?.Success == true)
        {
            infoLabel.Text = $"Successfully joined the game {codeLineEdit.Text}. Loading game...";
            GetNode<ProgressBar>("%LoadingPb").Set("visible", false);
        }
        else
        {
            joinConfigContainer.Set("visible", true);
            joiningGameContainer.Set("visible", false);

            switch (response?.Result)
            {
                case JoinGameResponse.Types.ResponseResult.GameIsFull:
                    infoLabel.Text = "Requested game is full. Try creating a new game or join an existing one.";

                    break;
                case JoinGameResponse.Types.ResponseResult.DoesNotExist:
                    infoLabel.Text = "Requested game doesn't exist. Try creating a new game or join an existing one.";

                    break;
            }
        }
    }

    private async void _on_join_button_pressed()
    {
        if (codeLineEdit.Text.Length != 6)
        {
            infoLabel.Text = "Please make sure your code is 6 character long.";
            return;
        }

        previousCode = codeLineEdit.Text;

        joiningGameContainer.Set("visible", true);
        joinConfigContainer.Set("visible", false);

        await serverHandler.SendJoinGameRequest(codeLineEdit.Text);
    }

    private void _on_cancel_button_pressed()
    {
        serverHandler.JoinGameResponseAction -= OnJoinGameResponse;
        this.QueueFree();
    }
}
