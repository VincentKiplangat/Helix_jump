using System.Collections.Generic;
using UnityEngine;

public class DeathPart : MonoBehaviour
{
    public AudioSource deathSound;

    private void OnEnable()
    {
        SetColor(Color.red);
    }

    public void HitDeathPart()
    {
        GameManager.singleton.RestartLevel();
        PlayDeathSound();
    }

    private void SetColor(Color color)
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = color;
        }
    }

    private void PlayDeathSound()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();
        }
    }

    private bool TryGetComponent<T>(out T component) where T : Component
    {
        component = GetComponent<T>();
        return component != null;
    }
}
