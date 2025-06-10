using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    private GameObject _loaderCanvas;
    [SerializeField]
    private GameObject _mainMenuCanvas;
    [SerializeField]
    private Slider _progressBar;
    private float _target;

    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScreen"))
        {
            _loaderCanvas.SetActive(false);
            _mainMenuCanvas.SetActive(true);
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _progressBar.value = Mathf.MoveTowards(_progressBar.value, _target, 3 * Time.deltaTime);

    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _target = 0;

        try
        {
            _mainMenuCanvas.SetActive(false);
        }
        catch (MissingReferenceException) { }
        catch (NullReferenceException) { };

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(300);
            _target = scene.progress;
            //_progressBar.colors.disabledColor = new Color(Mathf.Lerp(0, 1, 1 - scene.progress / 90), Mathf.Lerp(0, 1, scene.progress / 90), 0);

        } while (scene.progress < 0.9f);

        await Task.Delay(1000);
        scene.allowSceneActivation = true;

        while (SceneManager.GetSceneByName(sceneName).isLoaded == false) { await Task.Delay(25); }

        _loaderCanvas.SetActive(false);
    }

    //public void LoadScene(string sceneName)
    //{
    //    _mainMenuCanvas.SetActive(false);
    //    _loaderCanvas.SetActive(true);
    //    StartCoroutine(LoadSceneCoroutine(sceneName));
    //}

    //IEnumerator LoadSceneCoroutine(string sceneName)
    //{
    //    AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
    //    while (!loadOperation.isDone)
    //    {
    //        float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
    //        _progressBar.value = progressValue;
    //        yield return null;
    //    }
    //}
}
