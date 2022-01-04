using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
    public float rotSpeed = 100f;
    public float flySpeed = 100f;
    public AudioClip flySound;
    public AudioClip boomSound;
    public AudioClip finishSound;
    public ParticleSystem flyEffect;
    public ParticleSystem boomEffect;
    public ParticleSystem finishEffect;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Playing, Dead, NextLvl};
    State state= State.Playing;

    // Start is called before the first frame update
    void Start()
    {
    state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Playing)
    {
        Launch();
        Rotation();
    }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {

            case "friendly":
                break;
            case "finish":
            state = State.NextLvl;
                audioSource.Stop();
                audioSource.PlayOneShot(finishSound);
                finishEffect.Play();
                Invoke("LoadNextLvl", 1f);
                break;
            case "energy":
                print("Battery");
                break;
            default:
            state = State.Dead;
                audioSource.Stop();
                audioSource.PlayOneShot(boomSound);
                boomEffect.Play();
                Invoke("LoadFirstLvl", 1f);
                break;

        }
    }

    void LoadNextLvl ()
{
    SceneManager.LoadScene(2);
}

void LoadFirstLvl()
{
    SceneManager.LoadScene(1);
}

void Launch() {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (audioSource.isPlaying == false)
                audioSource.PlayOneShot(flySound);
                flyEffect.Play();
        }
        else
        {
            audioSource.Pause();
            flyEffect.Stop();
        }
        
    }
    void Rotation() {

        float rotationSpeed = rotSpeed * Time.deltaTime;

        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }
}
