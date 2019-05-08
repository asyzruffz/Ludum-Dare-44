using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField]
    float cooldown = 0.2f;
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform muzzle;

    SpriteRenderer rend;
    Collider2D col;
    TimeSince ts;

    public bool equipped { get; private set; }

    void Start () {
        rend = GetComponent<SpriteRenderer> ();
        col = GetComponent<Collider2D> ();
        ts = 0;
    }
    
    public void Fire () {
        if (ts > cooldown) {
            GameObject b = Instantiate (bulletPrefab, muzzle.position, muzzle.rotation);
            Rigidbody2D catridge = b.GetComponent<Rigidbody2D> ();
            if (catridge) {
                catridge.AddForce (muzzle.up * 2, ForceMode2D.Impulse);
            }
            ts = 0;  // Reset the timer
        }
    }

    public void BeEquipped (Transform handTrans) {
        transform.SetParent (handTrans);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rend.sortingOrder = 2;
        col.enabled = false;
        equipped = true;
        // play sound?
    }

    public void BeDropped () {
        transform.localPosition += Vector3.right * 0.8f;
        transform.SetParent (null);
        rend.sortingOrder = 0;
        col.enabled = true;
        equipped = false;
    }
}
