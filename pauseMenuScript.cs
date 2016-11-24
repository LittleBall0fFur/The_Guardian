using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour {
    public Canvas pauseMenu;
    public Button continueGameText;
    public Button quitToMenuGameText;
    public Button exitGameText;

    void Start() {

        pauseMenu = pauseMenu.GetComponent<Canvas>();
        continueGameText = continueGameText.GetComponent<Button>();
        quitToMenuGameText = quitToMenuGameText.GetComponent<Button>();
        exitGameText = exitGameText.GetComponent<Button>();
        pauseMenu.enabled = false;

    }
    public void QuitToMenuPress() {

        SceneManager.LoadScene("Menu");

    }
    public void ContinuePress() {
        Cursor.visible = false;
        pauseMenu.enabled = false;

    }
    public void ExitGame() {

        Application.Quit();

    }
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = true;
            pauseMenu.enabled = !pauseMenu.enabled;
        }

    }

}
