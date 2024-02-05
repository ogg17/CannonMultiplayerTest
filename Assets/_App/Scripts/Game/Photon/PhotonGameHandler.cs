using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using VgGames.Core.Config;
using VgGames.Game.Player;
using VgGames.Game.RuntimeData.PlayerColorModule;

namespace VgGames.Game.Photon
{
    public class PhotonGameHandler : IOnEventCallback
    {
        public PhotonGameHandler() => PhotonNetwork.AddCallbackTarget(this);

        public void Spawn(int color)
        {
            var player = Object.Instantiate(Resources.Load<GameObject>(GameConfig.PlayerPrefabName));
            var goal = Object.Instantiate(Resources.Load<GameObject>(GameConfig.GoalPrefabName));

            var photonPlayerView = player.GetComponent<PhotonView>();
            var photonGoalView = goal.GetComponent<PhotonView>();

            Allocate(color, photonPlayerView, photonGoalView, player, goal);
        }

        private void Allocate(int color, PhotonView photonPlayerView, PhotonView photonGoalView, GameObject player,
            GameObject goal)
        {
            if (PhotonNetwork.AllocateViewID(photonPlayerView) && PhotonNetwork.AllocateViewID(photonGoalView))
            {
                var controller = player.GetComponent<PlayerController>();
                controller.SetColor(color);
                controller.Activate();
                goal.tag = ((PlayerColor)color).ToTag();

                object[] data =
                {
                    player.transform.position, player.transform.rotation, photonPlayerView.ViewID,
                    goal.transform.position, goal.transform.rotation, photonGoalView.ViewID, color
                };

                var raiseEventOptions = new RaiseEventOptions
                    { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };

                var sendOptions = new SendOptions { Reliability = true };

                PhotonNetwork.RaiseEvent(GameConfig.InstantiationEventCode, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate a Player or Goal ViewId.");
                Object.Destroy(player);
                Object.Destroy(goal);
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != GameConfig.InstantiationEventCode) return;

            var data = (object[])photonEvent.CustomData;

            var player = Object.Instantiate(Resources.Load<GameObject>("Player"), (Vector3)data[0],
                (Quaternion)data[1]);
            var gaol = Object.Instantiate(Resources.Load<GameObject>("Goal"), (Vector3)data[3],
                (Quaternion)data[4]);

            var photonView = player.GetComponent<PhotonView>();
            photonView.ViewID = (int)data[2];
            var photonGoalView = gaol.GetComponent<PhotonView>();
            photonGoalView.ViewID = (int)data[5];

            var controller = player.GetComponent<PlayerController>();
            controller.SetColor((int)data[6]);
            controller.Activate();
            gaol.tag = ((PlayerColor)(int)data[6]).ToTag();
        }
    }
}