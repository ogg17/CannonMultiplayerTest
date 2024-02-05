using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

namespace VgGames.Game.Photon
{
    public class PhotonInitialisationHandler : IConnectionCallbacks
    {
        private readonly CancellationTokenSource _disconnectedToken = new();

        public bool IsConnected { get; private set; }

        public Action<DisconnectCause> OnDisconnectedAction;

        public PhotonInitialisationHandler() => PhotonNetwork.AddCallbackTarget(this);

        public async UniTask Init(string nickName)
        {
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.ConnectUsingSettings();
            await UniTask.WaitUntil(() => IsConnected, cancellationToken: _disconnectedToken.Token);
        }

        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            IsConnected = true;
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            _disconnectedToken?.Cancel();
            OnDisconnectedAction?.Invoke(cause);
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }
    }
}