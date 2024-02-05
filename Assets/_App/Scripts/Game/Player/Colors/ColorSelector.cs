using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Core.StateEvents.InitState;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;
using VgGames.Game.Signals;
using VgGames.Game.UI.Buttons;

namespace VgGames.Game.Player.Colors
{
    public class ColorSelector : MonoBehaviour, IInit, IPunObservable
    {
        [Inject] private ColorSelectorData _colorSelectorData;
        [Inject] private EventBus _eventBus;

        [SerializeField] private ColorButton[] buttons;

        private SelectColorSignal _signal = new();
        private readonly Dictionary<PlayerColor, ColorButton> _buttons = new();
        private Dictionary<int, bool> _isAvailable = new();
        private PhotonView _view;

        public void Init()
        {
            _view = GetComponent<PhotonView>();
            _colorSelectorData.Selector = this;
            foreach (var button in buttons)
            {
                _buttons.Add(button.Color, button);
                _isAvailable.Add((int)button.Color, button.Button.interactable);
                button.Button.onClick.AddListener(() => Push(button.Color));
            }
        }

        public bool CheckAvailable(PlayerColor color)
        {
            return _isAvailable[(int)color];
        }

        private void Push(PlayerColor color)
        {
            _signal.PlayerColor = color;
            _view.RPC(nameof(DisableColor), RpcTarget.All, (int)color);
            _eventBus.Invoke(_signal);
        }

        public void Pop(PlayerColor color)
        {
            if(PhotonNetwork.IsConnected) _view.RPC(nameof(EnableColor), RpcTarget.All, (int)color);
        }

        [PunRPC]
        private void DisableColor(int color)
        {
            _isAvailable[color] = false;
            SetButtons();
        }
        
        [PunRPC]
        private void EnableColor(int color)
        {
           _isAvailable[color] = true;
           SetButtons();
        }

        private void SetButtons()
        {
            foreach (var b in _isAvailable)
                _buttons[(PlayerColor)b.Key].Button.interactable = b.Value;
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
                stream.SendNext(_isAvailable);
            else
            {
                _isAvailable = (Dictionary<int, bool>)stream.ReceiveNext();
                SetButtons();
            }
        }
    }
}