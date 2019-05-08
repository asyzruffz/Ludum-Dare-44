using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    [SerializeField]
    float speed = 10;
    [Space]
    [SerializeField]
    Vector3 cursor;
    [SerializeField]
    Vector2 moveDir;
    [SerializeField]
    Transform handR;
    [SerializeField]
    Transform handL;

    float angle = 0f;
    Animator anim;
    Rigidbody2D body;
    EquipmentController equi;
    bool strafe = false;

    void Start () {
        anim = GetComponent<Animator> ();
        body = GetComponent<Rigidbody2D> ();
        equi = GetComponent<EquipmentController> ();
    }
    
    void Update () {
        // Update mouse cursor position
        cursor = Camera.main.ScreenToWorldPoint (Input.mousePosition);

        // Update movement direction
        moveDir = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

        /*if (!strafe && Mathf.Abs(moveDir.x) > 0) {
            strafe = true;
        } else if (Mathf.Abs (moveDir.x) < 0.001f) {
            strafe = false;
        }*/

        moveDir = transform.rotation * moveDir;
        moveDir.Normalize ();

        // Update walking animation
        anim.SetBool ("Walking", moveDir.magnitude > 0);

        // Update hand aiming directions
        AimTowards (cursor, handR);
        AimTowards (cursor, handL);
        
        // Update hand animations
        anim.SetBool ("RightHanded", equi.IsEquipped (Hand.Right));
        anim.SetBool ("LeftHanded", equi.IsEquipped (Hand.Left));

        // Update gun firing
        if (Input.GetButton("Fire1")) {
            equi.Fire (Hand.Left);
        }
        if (Input.GetButton ("Fire2")) {
            equi.Fire (Hand.Right);
        }
    }

    void FixedUpdate () {
        // Update body movement and rotation

        MoveTowards (moveDir);
        FaceTowards (cursor);
    }

    void MoveTowards (Vector2 direction) {
        Vector2 correctedDir = new Vector2 (direction.y, -direction.x);
        body.AddForce (correctedDir * speed, ForceMode2D.Force);
    }

    void FaceTowards (Vector3 target) {
        Vector3 facing = target - transform.position;
        angle = Mathf.Atan2 (facing.y, facing.x) * Mathf.Rad2Deg;
        body.MoveRotation (angle);
    }

    void AimTowards (Vector3 target, Transform anchor) {
        Vector3 facing = target - anchor.position;
        angle = Mathf.Atan2 (facing.y, facing.x) * Mathf.Rad2Deg;
        anchor.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
    }
}
