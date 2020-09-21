using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{

    public void Lose() {
        SceneManager.LoadScene(0);
    }

}
