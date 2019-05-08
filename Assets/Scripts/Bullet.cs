using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    float lifetime = 3;

    Animator anim;
    TimeSince ts;

    void Start () {
        anim = GetComponentInChildren<Animator> ();
        ts = 0;
    }
    
    void Update () {
        if (ts > lifetime) {
            Destroy (gameObject);
        }
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (!collision.transform.CompareTag ("Bullet")) {
            Destroy (gameObject, 0.1f);
        }

        if (anim) {
            anim.SetTrigger ("Slow");
            Destroy (gameObject, 0.5f);
        }
    }
}
