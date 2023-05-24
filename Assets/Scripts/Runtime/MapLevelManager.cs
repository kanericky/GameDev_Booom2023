using DG.Tweening;
using Runtime.Menu;
using UnityEngine;

public class MapLevelManager : MonoBehaviour
{
    public Canvas dropItemCanvas;
    public static MapLevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MenuUIManager.instance.TransitionIntro();
    }


    public void CloseDropItemMenu()
    {
        dropItemCanvas.gameObject.SetActive(false);
    }
    
}
