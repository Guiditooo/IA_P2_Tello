using System;
using Unity.VisualScripting;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public static Action OnCoinPickUp;
    public Action OnTubeHit;

    const float GRAVITY = 20.0f;
    const float MOVEMENT_SPEED = 3.0f;
    const float FLAP_SPEED = 7.5f;
    public Vector3 speed
    {
        get; private set;
    }

    public void Reset()
    {
        speed = Vector3.zero;
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    public void Flap()
    {
        Vector3 newSpeed = speed;
        newSpeed.y = FLAP_SPEED;
        speed = newSpeed;
    }

    public void UpdateBird(float dt)
    {
        Vector3 newSpeed = speed;
        newSpeed.x = MOVEMENT_SPEED;
        newSpeed.y -= GRAVITY * dt;
        speed = newSpeed;

        this.transform.rotation = Quaternion.AngleAxis(speed.y * 5f, Vector3.forward);

        this.transform.position += speed * dt;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Estoy chocando contra: " + collision.gameObject.ToString());
        OnCoinPickUp?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Tube")
        {
            OnTubeHit?.Invoke();
        }
    }

}