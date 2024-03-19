using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BowlingBall : MonoBehaviour {
    private bool finished = false;
    public Image ArrowFill;
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

    }
    //Oculta el indicador de flecha
    public void HideArrow() {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    //Muestra el indicador de flecha
    public void ShowArrow() {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    //Rellena la flecha en función de la potencia dada por el usuario.
    public void SetPower(float power) {
        ArrowFill.fillAmount = power;
    }

    //Disparar cuando la pelota pasa por un punto determinado.
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "FinishLine" && !finished) {
            finished = true;
            Invoke("FinishRound", 5f);
        }
    }

    //Termina la ronda cuando la pelota haya pasado por cierto punto.
    private void FinishRound() {
        finished = false;
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().RoundEnd();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pin"))
        {
            Debug.Log("choque");
            PlaySound(soundManager.choqueSound, 1f);
        }
    }

    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip && soundManager.fxEnabled)
        {
            AudioSource.PlayClipAtPoint(
                clip, Camera.main.transform.position,
                Mathf.Clamp(soundManager.fxVolume * volumeMultiplier, 0.05f, 1f));
        }
    }


}
