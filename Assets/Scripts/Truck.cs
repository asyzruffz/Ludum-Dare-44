using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour {

    public int capacity = 4;
    [SerializeField]
    float loadingTime = 2;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    Transform warehouse, loadingBay;

    [Space]
    [SerializeField]
    CratePile pile;
    
    Animator anim;
    TimeSince ts;
    Vector3 bayPos, warehousePos;
    bool isMoving, isDelivering;
    float freq;
    float t;
    int dir, cargoAmt;

    void Start () {
        anim = GetComponent<Animator> ();

        if (loadingBay) {
            bayPos = loadingBay.position;
        }
        if (warehouse) {
            warehousePos = warehouse.position;
        }

        float travelDistance = Vector3.Distance (warehousePos, bayPos);
        freq = speed / Mathf.Max (0.0001f, travelDistance);

        Vector2 diff = warehousePos - bayPos;
        float angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

        isMoving = false;
        isDelivering = false;
        cargoAmt = 0;
        ts = 0;

        StartDelivery ();
    }

    void Update () {
        if (Input.GetMouseButtonDown(1)) {
            StartDelivery ();
        }

        if (isDelivering) {
            DoDelivery ();
        }
    }
    
    public void StartDelivery () {
        if (!isDelivering) {
            isDelivering = true;
            dir = -1;
            t = 1;
            MoveTruck ();
        }
    }

    void DoDelivery () {
        if (isMoving) {
            t += dir * freq * Time.deltaTime;
            transform.position = Vector3.Lerp (bayPos, warehousePos, t);
            
            if (t <= 0 || t >= 1) {
                StopTruck ();
                dir *= -1;
                if (dir < 0) {
                    UnloadCargo ();
                    isDelivering = false;
                }
            }
        } else {
            if (ts > loadingTime) {
                MoveTruck ();
            } else if (ts > loadingTime / 2 && cargoAmt < capacity) {
                LoadCargo ();
            }
        }
    }

    void MoveTruck () {
        isMoving = true;
        ts = 0;
    }

    void StopTruck () {
        t = (dir + 1) / 2;
        transform.position = Vector3.Lerp (bayPos, warehousePos, t);
        isMoving = false;
        ts = 0;
    }

    public void LoadCargo () { // called multiple frames
        cargoAmt += pile.LoadToTruck (capacity - cargoAmt);
        if (cargoAmt > 0) {
            anim.SetInteger ("Cargo", pile.cargoType);
        }
    }

    void UnloadCargo () {
        if (cargoAmt > 0) {
            Debug.Log ("Delivered " + cargoAmt + " cargo!");
        }
        anim.SetInteger ("Cargo", 0);
        cargoAmt = 0;
    }
}
