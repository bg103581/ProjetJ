using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField]
    private Player player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            player.HitByObstacle();
        }
        else if (other.gameObject.layer == 9) {
            player.HitByBonus();
        }
        else if (other.gameObject.layer == 10) {
            player.HitByDisc(other.tag == "Gold");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == 11) {
            player.DodgeObstacle();
        }
    }

}
