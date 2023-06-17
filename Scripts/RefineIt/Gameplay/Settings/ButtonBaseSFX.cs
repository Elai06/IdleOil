using Gameplay.Settings;
using Infrastructure;
using Zenject;

namespace Gameplay.Region
{
    public class ButtonBaseSFX : ButtonBase
    {
        [Inject] private AudioService _audioService;

        public ClickClipId clickClipId;

        public override void OnClick()
        {
            if(_audioService == null)
                InjectService.Instance.Inject(this);
            _audioService.PlaySFX(EAudioSources.SFXSource,clickClipId);
        }
    }
}