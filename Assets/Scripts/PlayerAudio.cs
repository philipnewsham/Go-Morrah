using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public Movement movement;
    public AudioSource audioSource;
    public AudioClip jumpSFX;
    public AudioClip freezeSFX;
    public AudioClip landSFX;
    public AudioClip exitSFX;
    public AudioClip fallSFX;

    private void OnEnable()
    {
        movement.OnFreeze += OnFreeze;
        movement.OnJump += OnJump;
        movement.OnLand += OnLand;
        movement.OnExit += OnExit;
        movement.OnFall += OnFall;
    }

    private void OnFall()
    {
        PlayAudioClip(fallSFX);
    }

    private void OnExit()
    {
        PlayAudioClip(exitSFX);
    }

    private void OnLand()
    {
        PlayAudioClip(landSFX);
    }

    private void OnJump()
    {
        PlayAudioClip(jumpSFX);
    }

    private void OnFreeze()
    {
        PlayAudioClip(freezeSFX);
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnDisable()
    {
        movement.OnFreeze -= OnFreeze;
        movement.OnJump -= OnJump;
        movement.OnLand -= OnLand;
        movement.OnExit -= OnExit;
        movement.OnFall -= OnFall;
    }
}
