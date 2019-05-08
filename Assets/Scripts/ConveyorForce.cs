using UnityEngine;

public class ConveyorForce : MonoBehaviour {

    [SerializeField]
    Vector2 forceDirection;
    [SerializeField]
    float strength = 1;

    Vector2 normForceDir;

    void Start () {
        if (forceDirection.magnitude > 0) {
            normForceDir = forceDirection.normalized;
        } else {
            normForceDir = Vector2.zero;
        }
    }
    
    void OnTriggerStay2D (Collider2D collision) {
        Rigidbody2D body = collision.GetComponent<Rigidbody2D> ();
        if (body && !body.CompareTag ("Bullet")) {
            Vector2 forceToCentre = transform.position - collision.transform.position;
            Vector2 netForce = normForceDir + forceToCentre;
            body.AddForce (netForce.normalized * strength, ForceMode2D.Force);
        }
    }
}
