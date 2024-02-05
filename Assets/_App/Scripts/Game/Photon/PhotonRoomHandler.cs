using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace VgGames.Game.Photon
{
    public class PhotonRoomHandler : IMatchmakingCallbacks
    {
        private readonly CancellationTokenSource _roomFailed = new();
        private bool _isJoin;

        public PhotonRoomHandler() => PhotonNetwork.AddCallbackTarget(this);

        public async UniTask<bool> Init()
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: CreateRoomOptions());
            await UniTask.WaitUntil(() => _isJoin, cancellationToken: _roomFailed.Token);
            return _isJoin;
        }

        private static RoomOptions CreateRoomOptions()
        {
            return new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 4,
                CustomRoomProperties = new Hashtable { { "levelIndex", 0 } },
                CustomRoomPropertiesForLobby = new[] { "levelIndex" }
            };
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            _roomFailed.Cancel();
        }

        public void OnJoinedRoom()
        {
            _isJoin = true;
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            _roomFailed.Cancel();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            _roomFailed.Cancel();
        }

        public void OnLeftRoom()
        {
            _roomFailed.Cancel();
        }
    }
}