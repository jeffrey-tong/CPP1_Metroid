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
        MorphingBall
    }

    public PickupType currentPickup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController temp = collision.gameObject.GetComponent<PlayerController>();
            switch (currentPickup)
            {
                case PickupType.Life:
                    temp.lives += 5;
                    Debug.Log("Lives: " + temp.lives.ToString());
                    break;
                case PickupType.MaxLife:
                    temp.maxLives += 100;
                    Debug.Log("Max Lives: " + temp.maxLives.ToString());
                    break;
                case PickupType.JumpBoost:
                    temp.StartJumpForceChange();
                    break;
                case PickupType.MorphingBall:
                    temp.canCrouch = true;
                    Debug.Log("You can crouch now!");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
