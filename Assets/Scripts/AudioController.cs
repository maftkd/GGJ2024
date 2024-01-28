using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Transform _audioSourcePrefab;
    public static AudioController Instance;

    void Awake() {
        if(Instance != null){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }

    public void PlayOneShot(AudioClip clip, Vector3 position) {
        Transform audioSourceTransform = Instantiate(_audioSourcePrefab, position, Quaternion.identity);
        AudioSource audioSource = audioSourceTransform.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, clip.length);
    }

}
