using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControl : MonoBehaviour {

	Rigidbody2D rigidBody;
    Color initialColor;
    public bool IsInvincible { get; private set; }

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float moveForce;
    [SerializeField] [Range(0,100)] float energy = 100;

    [SerializeField] float invincibleTime;

    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        initialColor = sprite.color;
    }
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.RightArrow)){
            rigidBody.AddForce( Vector2.right * moveForce );
		}

		if(Input.GetKey(KeyCode.LeftArrow)){
            rigidBody.AddForce( Vector2.left * moveForce );
		}

		if(Input.GetKey(KeyCode.UpArrow)){
            rigidBody.AddForce( Vector2.up * moveForce );
		}
	}

    public void UpdateEnergy(float deltaEnergy)
    {
        if ( deltaEnergy < 0 && IsInvincible) { return; }

        energy = Mathf.Clamp(energy + deltaEnergy, 0, 100);

        if (deltaEnergy < 0)
        {
            initialInvinsibleTime = invincibleTime;
            StartCoroutine(InvinsibleTimeCoroutine(sprite));
        }
    }    

    float initialInvinsibleTime;

    IEnumerator InvinsibleTimeCoroutine( SpriteRenderer spriteRenderer )
    {
        IsInvincible = true;        
        var tweener =  spriteRenderer.DOFade(0, 0.5f);        
        tweener.SetLoops(-1, LoopType.Yoyo);

        while (initialInvinsibleTime > 0)
        {
            initialInvinsibleTime -= Time.deltaTime;
            yield return null;
        }

        IsInvincible = false;
        tweener.Kill(true);
        spriteRenderer.DOColor(initialColor, 0.5f);

        yield break;
    }
}
