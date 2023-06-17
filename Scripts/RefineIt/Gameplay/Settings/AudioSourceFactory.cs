using Infrastructure.StaticData;

namespace Gameplay.Settings
{
    public class AudioSourceFactory : IAudioSourceFactory
    {
        private readonly IStaticDataService _staticDataService;
        
        AudioSourceFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
           
        }

        public void SpawnAudioSource()
        {
            
        }
    }

    public interface IAudioSourceFactory
    {
    }
}