using UnityEngine;
using Utils;

namespace Managers
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [Header("Audio Sources")]
        public AudioSource musicSource;
        public AudioSource sfxSource;
        public AudioSource ambienceSource;

        [Header("Ambient Sounds")]
        public AudioClip backgroundMusic;
        public AudioClip freezerAmbience;
        public AudioClip gardenAmbience;
        public AudioClip ratsAmbience;

        [Header("SFX")]
        public AudioClip buyUpgrade;
        public AudioClip collectCandy;
        public AudioClip collectItem;
        public AudioClip footstep;
        public AudioClip hit;
        public AudioClip molePop;
        public AudioClip openContainer;

        [Range(0f, 1f)]
        public float musicVolume = 0.1f;
        public float freezerVolume = 0.5f;
        public float gardenVolume = 0.1f;
        public float ratsVolume = 0.1f;

        private void Start()
        {
            musicSource.volume = musicVolume;
            PlayBackgroundMusic();
        }

        public void PlayBackgroundMusic()
        {
            if (backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        // public void PlaySFX(AudioClip clip)
        // {
        //     if (clip != null)
        //     {
        //         sfxSource.PlayOneShot(clip);
        //     }
        // }
        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip, volume);
            }
        }
        
        // Specific helper methods
        public void PlayFootstep() => PlaySFX(footstep);
        public void PlayCollectCandy() => PlaySFX(collectCandy);
        public void PlayCollectItem() => PlaySFX(collectItem);
        public void PlayBuyUpgrade() => PlaySFX(buyUpgrade);
        public void PlayHit() => PlaySFX(hit, 0.3f);

        // Optional: ambient zone triggers
        // public void PlayFreezerAmbience() => PlaySFX(freezerAmbience);
     
        public void PlayFreezerAmbience()
        {
            ambienceSource.clip = freezerAmbience;
            ambienceSource.loop = true;
            ambienceSource.volume = freezerVolume;
            ambienceSource.Play();
        }
        public void PlayGardenAmbience()
        {
            ambienceSource.Stop();
            ambienceSource.clip = gardenAmbience;
            ambienceSource.volume = gardenVolume;
            ambienceSource.loop = true;
            ambienceSource.Play();
        }

        public void PlayRatsAmbience()
        {
            ambienceSource.Stop();
            ambienceSource.clip = ratsAmbience;
            ambienceSource.volume = ratsVolume;
            ambienceSource.loop = true;
            ambienceSource.Play();
        }


        public void StopAmbience()
        {
            ambienceSource.Stop();
        }
        public void PlayCollectCandy(float volume = 1f)
        {
            if (collectCandy != null)
                sfxSource.PlayOneShot(collectCandy, volume);
        }
        public void PlayMolePop()
        {
            sfxSource.PlayOneShot(molePop, 0.2f);
        }
        public void PlayOpenContainer()
        {
            sfxSource.PlayOneShot(openContainer, 0.5f);
        }
        
        private void StopAmbienceIfPlaying(AudioClip clip)
        {
            if (ambienceSource.isPlaying && ambienceSource.clip == clip)
            {
                ambienceSource.Stop();
            }
        }
        public void StopRatsAmbience() => StopAmbienceIfPlaying(ratsAmbience);
        public void StopGardenAmbience() => StopAmbienceIfPlaying(gardenAmbience);
        public void StopFreezerAmbience() => StopAmbienceIfPlaying(freezerAmbience);


    }
}
