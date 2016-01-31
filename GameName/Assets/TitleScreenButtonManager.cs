using UnityEngine;
using System.Collections;

public class TitleScreenButtonManager : MonoBehaviour {

    public enum CharacterToPlayAs { Death = 1, Angel = 2};

    public static CharacterToPlayAs playingAs = CharacterToPlayAs.Death;

    void Update ()
    {
        if (Input.GetKey(KeyCode.D))
        {
            playingAs = CharacterToPlayAs.Death;
            Application.LoadLevel(1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playingAs = CharacterToPlayAs.Angel;
            Application.LoadLevel(1);
        }
    }
}