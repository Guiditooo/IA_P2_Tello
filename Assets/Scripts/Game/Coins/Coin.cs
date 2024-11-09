using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float coinSpeed;

    private void Awake()
    {
        BirdBehaviour.OnCoinPickUp += Reset;
        Reset();
    }
    private void OnDestroy()
    {
        BirdBehaviour.OnCoinPickUp -= Reset;
    }

    private void Update()
    {
        transform.position += Vector3.left * coinSpeed * Time.deltaTime;
        if (IsOutOfCameraView())
        {
            Reset();
        }
    }

    private void Reset()
    {
        transform.position = Camera.main.ViewportToWorldPoint(
            new Vector2(1.1f, Random.Range(0.1f, 0.9f))
        );
    }

    private bool IsOutOfCameraView()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        return screenPosition.x < 0;
    }
}
