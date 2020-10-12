using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField]
    private Player player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            player.HitByObstacle(other);
        }
        else if (other.gameObject.layer == 9) {
            player.HitByBonus(other.tag);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == 10) {
            player.HitByDisc(other.tag == "Gold");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == 11) {
            player.DodgeObstacle();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Ground") {
            player.isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.transform.tag == "Ground") {
            player.isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.transform.tag == "Ground") {
            player.isGrounded = false;
        }
    }
}
