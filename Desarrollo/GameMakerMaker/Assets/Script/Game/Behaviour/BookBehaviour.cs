using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookBehaviour : Interactuable {

    public GameObject Canvas;
    float t = 0;
    Vector2 screenPoint;
    Button close;
    SoundController anim;
    public GameObject[] pages;

    public static BookBehaviour instance;
    private int pageActual;
    public int Page { get { return pageActual; } set {

            pages[pageActual].SetActive(false);
            pageActual = value;
            pages[pageActual].SetActive(true);



        } }


    public override void PostActionAnim(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override bool PreAction(PlayerController player)
    {
        base.PreAction(player);
        return true;
    }


    public override void PostAction(PlayerController player)
    {
        base.PostAction(player);
        StartCoroutine(PageAnimOpen());
        anim.SetTrigger("Open");
    }

    IEnumerator PageAnimOpen()
    {
        Vector2 realPos = new Vector2((Screen.width / 2), (Screen.height / 2));
        Canvas.SetActive(true);
        while (t < 1)
        {
            Canvas.transform.position = Vector2.Lerp(screenPoint, realPos, t);
            t += Time.deltaTime*4;
            yield return null;
        }
        GameManager.Instance.PauseGame(0);
        t = 0;
    }

    IEnumerator PageAnimClose()
    {
        Vector2 realPos = new Vector2((Screen.width / 2), (Screen.height / 2));
        anim.SetTrigger("Close");
        while (t < 1)
        {
            Canvas.transform.position = Vector2.Lerp(realPos, screenPoint, t);
            t += Time.deltaTime * 4;
            yield return null;
        }
        t = 0;
        Canvas.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        instance = this;
        screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        Canvas = GameObject.FindGameObjectWithTag("Recetas");
        close = GameObject.FindGameObjectWithTag("CerrarRecetas").GetComponent<Button>();
        close.onClick.AddListener(Close);
        anim = Canvas.GetComponent<SoundController>();
        //Canvas.SetActive(false);
    }
	
    public void Close()
    {
        GameManager.Instance.PauseGame(1);
        StartCoroutine(PageAnimClose());
    }

}
