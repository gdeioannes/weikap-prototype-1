using UnityEngine;
using System.Collections;

public class EndPoint : BaseInteractiveElement
{
   protected override void Awake()
    {
        base.Awake();
        // deactivate by default
        // this.gameObject.SetActive(false);
    }

    protected override void OnCharacterEnter(CharacterControl character)
    {
        // just restart the game
        // TODO: ADD GAME END LOGIC HERE
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
