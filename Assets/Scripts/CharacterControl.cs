using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControl : MonoBehaviour {

    [System.Serializable]
    public struct MovementForce
    {
        public float horizontal;
        public float vertical;
    }

    Rigidbody2D rigidBody;    

    float initialInvinsibleTime;
    
    float lastHDirection = 1;
    bool upButtonLocked;

    [Header("Character and Movement")]
    [SerializeField] Animator animator;
    [SerializeField] MovementForce waterMovement;
    [SerializeField] MovementForce surfaceMovement;

    [Header("Health")]
    [SerializeField] [Range(0,100)] float energy = 100;
    [SerializeField] float invincibleTime;

    public bool IsInvincible { get; private set; }
    public bool IsOnSurface { get; set; }    
    public System.Action<float> OnEnergyUpdated = delegate { };    

    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update () {

        if (IsOnSurface)
        {
            return;
        }

        Vector2 lastMoveVector = Vector2.zero;

        lastMoveVector.x = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Horizontal");        
        lastMoveVector.y = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Vertical") > 0 ? 1 : 0;

        if (lastMoveVector.x != 0)
        {
            lastHDirection = lastMoveVector.x;
        }

        if (lastMoveVector.y != 1 && rigidBody.velocity.y < 0)
        {
            upButtonLocked = false;
        }

        if ((lastMoveVector.y > 0 && upButtonLocked) || rigidBody.velocity.y > 0)
        {
            lastMoveVector.y = 0;
        }        

        if (rigidBody.velocity.y <= 0 && lastMoveVector.y > 0 && !upButtonLocked)
        {
            animator.SetTrigger("Jump");
            upButtonLocked = true;
        }

        animator.SetFloat("Speed", Mathf.Abs(lastMoveVector.x));
        animator.SetFloat("hSpeed", lastHDirection);
        animator.SetFloat("vSpeed", rigidBody.velocity.y);

        if (lastMoveVector == Vector2.zero)
        {
            return;
        }

        if (IsOnSurface)
        {
            rigidBody.AddForce(new Vector2(lastMoveVector.x * surfaceMovement.horizontal, lastMoveVector.y * surfaceMovement.vertical));
        }
        else
        {
            rigidBody.AddForce(new Vector2(lastMoveVector.x * waterMovement.horizontal, lastMoveVector.y * waterMovement.vertical));
        }        
    }

    public void UpdateEnergy(float deltaEnergy)
    {
        if ( deltaEnergy < 0 && IsInvincible) { return; }

        energy = Mathf.Clamp(energy + deltaEnergy, 0, 100);

        OnEnergyUpdated(energy);

        if (deltaEnergy < 0)
        {
            initialInvinsibleTime = invincibleTime;
            StartCoroutine(InvinsibleTimeCoroutine());
        }
    }

    

    IEnumerator InvinsibleTimeCoroutine()
    {
        IsInvincible = true;
        animator.SetBool("Invinsible", true);
        
        while (initialInvinsibleTime > 0)
        {
            initialInvinsibleTime -= Time.deltaTime;
            yield return null;
        }

        IsInvincible = false;
        animator.SetBool("Invinsible", false);        

        yield break;
    }

    public IEnumerator EnergyCountDownCoroutine(float timelapse)
    {
        while (energy > 0)
        {
            yield return null;
            if (!IsOnSurface)
            {
                energy -= (1f/timelapse) * Time.deltaTime;
                OnEnergyUpdated(energy);
            }
        }

		GameController.Instance.UpdateLevelProgress(PlayerData.LevelStatus.Lose);        
    }
}
