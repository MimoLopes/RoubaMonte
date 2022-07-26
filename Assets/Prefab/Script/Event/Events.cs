
public class Events
{
    public class Player
    {
        public const string Draw = "Player.Draw";
        public const string Steal = "Player.Steal";
    }

    public class Game
    {
        public const string Draw = "Game.Draw";
        public const string Steal = "Game.Steal";
    }

    public class Connection
    {
        public const string Connect = "Connection.Connected";
        public const string JoinLobby = "Connection.JoinLobby";
        public const string LeaveLobby = "Connection.LeaveLobby";
        public const string SetRoomName = "Connection.SetRoomName";
        public const string CreateRoom = "Connection.CreateRoom";
        public const string JoinRoom = "Connection.JoinRoom";
        public const string LeaveRoom = "Connection.LeaveRoom";
        public const string ReadyToPlay = "Connection.ReadyToPlay";
    }
}
