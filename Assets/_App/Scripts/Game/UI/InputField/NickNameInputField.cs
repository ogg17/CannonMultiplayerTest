using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;

namespace VgGames.Game.UI.InputField
{
    public class NickNameInputField : BaseInputField
    {
        [Inject] private PlayerSessionData _playerSessionData;

        protected override void OnEndEdit(string value) => 
            _playerSessionData.NickName = value;
    }
}