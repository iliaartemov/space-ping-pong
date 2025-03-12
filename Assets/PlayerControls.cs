using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public float speed = 10.0f;
    public float boundY = 2.25f;
    private Rigidbody2D rb2d;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    void Update () {
        HandleMovement();
        ClampPosition();
    }

    void HandleMovement() {
        Vector2 vel = rb2d.linearVelocity;

        if (Input.GetKey(moveUp)) {
            vel.y = speed;
            if (Input.GetKeyDown(moveUp)) {
                PlayReverseSound();
            }
        } else if (Input.GetKey(moveDown)) {
            vel.y = -speed;
            if (Input.GetKeyDown(moveDown)) {
                PlayReverseSound();
            }
        } else {
            vel.y = 0;
        }

        rb2d.linearVelocity = vel;
    }

    void ClampPosition() {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -boundY, boundY);
        transform.position = pos;
    }

    void PlayReverseSound() {
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (emitter != null) {
            emitter.Play();
        } else {
            Debug.LogWarning("FMOD Studio Event Emitter не найден!");
        }
    }
}