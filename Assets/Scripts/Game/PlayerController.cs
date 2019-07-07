using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Vector3 originalPosition;
    new Collider collider;
    new Rigidbody rigidbody;
    GameDirector gameDirector => DependencyContainer.Instance.gameDirector;

    float jumpForce;
    float maxJumpDuration;
    bool isGrounded = true;
    float jumpDuration = 0;
    bool isJumpInitiated = false;
    KeyCode jumpKeyCode = KeyCode.UpArrow;


    void Awake()
    {
        originalPosition = gameObject.transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        UpdateConfiguration();
    }

    void Update()
    {
        if (!gameDirector.isPlaying) {
            return;
        }

        // Change element
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            ChangeElement();
        }

        // Player stop jumping
        if (Input.GetKeyUp(jumpKeyCode)) {
            isJumpInitiated = false;
        }
        // Jump
        if (isGrounded) {
            if (Input.GetKeyDown(jumpKeyCode)) {
                ApplyJumpForce();
                jumpDuration = 0;
                isJumpInitiated = true;
            }
        } else {
            if (isJumpInitiated && Input.GetKey(jumpKeyCode)) {
                jumpDuration += Time.deltaTime;
                if (jumpDuration < maxJumpDuration) {
                    ApplyJumpForce();
                }
            }
        }
    }

    private void ApplyJumpForce()
    {
        rigidbody.velocity = Vector3.up * jumpForce;
    }

    private void UpdateConfiguration()
    {
        var configuration = DependencyContainer.Instance.gameConfiguration;
        jumpForce = configuration.jumpForce;
        maxJumpDuration = configuration.maxJumpDuration;
    }

    public void ResetState()
    {
        rigidbody.velocity = Vector3.zero;
        gameObject.transform.position = originalPosition;
        UpdateGroundedState();
    }

    private void ChangeElement()
    {
        gameDirector.OnChangePlayerElement();
        var collidingPlatforms = CollidingPlatforms();
        if (collidingPlatforms.Count > 0) {
            gameDirector.OnPlayerHitPlatform(collidingPlatforms[0]);
        }
    }

    #region Collisions

    private void UpdateGroundedState()
    {
        isGrounded = CollidingPlatforms().Count > 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        // Player hit a platform
        if (collision.gameObject.GetComponent<Platform>() is Platform platform) {
            gameDirector.OnPlayerHitPlatform(platform);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public List<Platform> CollidingPlatforms()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        //LayerMask layerMask = LayerMask.NameToLayer("Platform");
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);

        var platforms = new List<Platform>();
        for (int i = 0; i < hitColliders.Length; i++) {
            if (hitColliders[i].gameObject.GetComponent<Platform>() is Platform platform) {
                platforms.Add(platform);
            }
        }
        return platforms;
    }

    #endregion
}
