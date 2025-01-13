using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionSound;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySFX(explosionSound);
        Destroy(gameObject, 0.5f);
    }
}
