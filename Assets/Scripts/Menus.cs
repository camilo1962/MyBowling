using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void IrA(string nombre)
    {

        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
