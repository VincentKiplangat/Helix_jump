using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision = false;
    private Vector3 startPos;
    private Rigidbody rb;

    public float speed = 5.0f;
    public int perfectPass;
    public bool isSuperSpeedActive;
    public AudioSource landSound;
    public GameObject splatPrefab;
    public AudioSource perfectPassAudio;

    private void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision)
            return;

        if (isSuperSpeedActive && !collision.transform.GetComponent<Goal>())
        {
            Destroy(collision.transform.parent.gameObject);
            Debug.Log("Destroying platform");
        }

        DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
        if (deathPart)
            deathPart.HitDeathPart();

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * speed, ForceMode.Impulse);

        landSound.Play();

        GameObject newSplat = Instantiate(splatPrefab, new Vector3(transform.position.x, collision.transform.position.y, transform.position.z), transform.rotation);

        ignoreNextCollision = true;
        Invoke("AllowCollision", 0.2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
        perfectPassAudio.Stop();
    }

    private void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
            perfectPassAudio.Play();
            Debug.Log("Perfect Pass!");
        }
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}
