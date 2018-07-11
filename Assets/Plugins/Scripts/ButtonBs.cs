using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBs : MonoBehaviour {

    // Use this for initialization
    public Scene mainmenu;
    public Scene play;
    public Scene editor;

    private void Start()
    {

        mainmenu = SceneManager.GetSceneByName("mainmenu");
        play = SceneManager.GetSceneByName("play");
        editor = SceneManager.GetSceneByName("editor");
        SceneManager.UnloadSceneAsync(play);
        SceneManager.UnloadSceneAsync(editor);
        SceneManager.SetActiveScene(mainmenu);
    }

    public void StartGame() {
        SceneManager.UnloadSceneAsync(mainmenu);
        SceneManager.LoadScene("play");
        SceneManager.SetActiveScene(play);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MapEdit()
    {
        SceneManager.UnloadSceneAsync(mainmenu);
        SceneManager.LoadScene("editor");
        SceneManager.SetActiveScene(editor);
    }


}
