using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField]
    private Player player;

    private JulAnim julAnim;

    private void Awake() {
        julAnim = FindObjectOfType<JulAnim>();
    }

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
            ObjectPooler.current.DestroyObject(other.gameObject);
            //Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == 11) {
            player.DodgeObstacle();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Ground") {
            player.isGrounded = true;
            julAnim.SetFallBool(false);
            if (player.isClaquettes) SoundManager.current.UnPauseSound(SoundType.CLAQUETTES);
            else SoundManager.current.UnPauseSound(SoundType.RUN);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.transform.tag == "Ground") {
            if (!player.isTmaxFlying)
            {
                player.isGrounded = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.transform.tag == "Ground") {
            player.isGrounded = false;
        }
    }
}
