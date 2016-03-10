using UnityEngine;
using System.Collections;

public class TitleScreenButtonManager : MonoBehaviour {

    public enum CharacterToPlayAs { Death = 1, Angel = 2};

    public static CharacterToPlayAs playingAs = CharacterToPlayAs.Death;

    public void PlayAsDeath()
    {
        playingAs = CharacterToPlayAs.Death;
        Application.LoadLevel("Main");
    }

    public void PlayAsAngels()
    {
        playingAs = CharacterToPlayAs.Angel;
        Application.LoadLevel("Main");
    }

    void Update ()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.PlayAsDeath();
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.PlayAsAngels();
        }
    }
}