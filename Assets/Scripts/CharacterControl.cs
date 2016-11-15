﻿using UnityEngine;
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
    Dictionary<ConsumableController.ConsumableType,int> consumables;
    float initialInvinsibleTime;
    int answeredQuestions = 0;    

    [Header("Character and Movement")]
    [SerializeField] Animator animator;
    [SerializeField] MovementForce waterMovement;
    [SerializeField] MovementForce surfaceMovement;

    [Header("Health")]
    [SerializeField] [Range(0,100)] float energy = 100;
    [SerializeField] float invincibleTime;

    public bool IsInvincible { get; private set; }
    public bool IsOnSurface { get; set; }
    public System.Action<ConsumableController.ConsumableType, int> OnConsumableAmountUpdated = delegate { };
    public System.Action<float> OnEnergyUpdated = delegate { };
    public System.Action<int> OnQuestionAnswered = delegate { };

    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        consumables = new Dictionary<ConsumableController.ConsumableType, int>();
    }

    float lastHDirection = 0;

    // Update is called once per frame
    void FixedUpdate () {
        
        Vector2 lastMoveVector = Vector2.zero;

        lastMoveVector.x = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
        lastMoveVector.x = lastMoveVector.x > 0 ? 1 : lastMoveVector.x < 0 ? -1 : 0;

        if (lastMoveVector.x != 0)
        {
            lastHDirection = lastMoveVector.x;
        }

        if (rigidBody.velocity.y <= 0)
        {
            lastMoveVector.y = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical") > 0 ? 1 : 0;
            if (lastMoveVector.y > 0)
            {
                animator.SetTrigger("Jump");
            }
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

    public void UpdateConsumable(ConsumableController.ConsumableType type, int amount)
    {
        if (consumables.ContainsKey(type))
        {
            consumables[type] += amount;
        }
        else {
            consumables[type] = amount;
        }

        OnConsumableAmountUpdated(type, consumables[type]);
    }

    public void UpdateQuestion(int amount)
    {
        answeredQuestions += amount;
        OnQuestionAnswered(answeredQuestions);
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

        // Restart current level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
