using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Collections;
using UnityEditor;




public class Menu : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textoNivel;
    [SerializeField] private TextMeshProUGUI textoPuntuacion;



    void Start()
    {

            // Cargar la partida
        GameManager.instancia.cargarPartida();
        textoNivel.text = "Nivel: " + GameManager.instancia.nivelMax.ToString();
        textoPuntuacion.text = "Puntuacion: " + GameManager.instancia.puntosMax.ToString();
    }


}
