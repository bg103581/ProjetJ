using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField]
    private Player _player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            _player.HitByObstacle();
        }
        else if (other.gameObject.layer == 9) {
            _player.HitByBonus();
        }
    }

}
