using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        MaxLife,
        JumpBoost,
        MorphingBall,
        MissilePickup
    }

    public PickupType currentPickup;
    public AudioClip pickupSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController temp = collision.gameObject.GetComponent<PlayerController>();
            switch (currentPickup)
            {
                case PickupType.Life:
                    GameManager.instance.lives += 5;
                    Debug.Log("Lives: " + GameManager.instance.lives.ToString());
                    break;
                case PickupType.MaxLife:
                    GameManager.instance.maxLives += 100;
                    GameManager.instance.lives += 100;
                    Debug.Log("Max Lives: " + GameManager.instance.maxLives.ToString());
                    break;
                case PickupType.JumpBoost:
                    temp.StartJumpForceChange();
                    break;
                case PickupType.MorphingBall:
                    temp.canCrouch = true;
                    Debug.Log("You can crouch now!");
                    break;
                case PickupType.MissilePickup:
                    GameManager.instance.numMissiles++;
                    Debug.Log("Missiles: " + GameManager.instance.numMissiles.ToString());
                    break;
            }
            if (pickupSound)
            {
                collision.gameObject.GetComponent<AudioSourceManager>().PlayOneShot(pickupSound, false);
            }
            Destroy(gameObject);
        }
    }
}
