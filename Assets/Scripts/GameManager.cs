using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.Mathematics;
using UnityEditor;
using System.Collections;



#if UNITY_EDITOR
using UnityEditor;
#endif


// ------------------------
// Clase Game Manager
// ------------------------
public class GameManager : MonoBehaviour {

    private static GameManager _instancia;
    public static GameManager instancia
    {
        get { return _instancia; }
    }

    void Awake()
    {
        if (_instancia == null)
        {
            _instancia = this;
            //DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            //Destroy(gameObject); // Elimina duplicados
        }

        // Limitar FPS en el editor
        #if UNITY_EDITOR
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        #else
            Application.targetFrameRate = -1; // Sin límite en compilado
        #endif




    }




    // ------------------------
    // Atributos 
    // ------------------------
    public int nivel, nivelMax;
    public float velocidad
    {
        get
        {
            if(nivel > 45) return 8;
            else return (2 + (nivel / 5f));
        }
    }
    public int puntos, puntosMax;
    public int vida;
    public int velocidadDisparo;

    public TextMeshProUGUI textoPuntos, textoNivel, textoGameOverNivelMax, textoGameOverPuntosMax;

    public UnityEngine.UI.Image imagenVida;

    public GameObject popupGameOver;


    // Control del estado
    public TiposDeEstado estadoActual;

    // Definimos previamente el enumerado
    public enum TiposDeEstado { INTRO, MENU, JUEGO, SALIR }



    // ------------------------
    // Metodos 
    // ------------------------

    public void aumentarPuntos(int puntos) {
        this.puntos += puntos;
        textoPuntos.text = this.puntos.ToString();
    }

    public void aumentarNivel() {
        this.nivel ++;
        textoNivel.text = "Nivel: " + this.nivel.ToString();
    }

    public void aumentarVida(int vida) {
        if (this.vida >= 3) return;
        this.vida += vida;
        imagenVida.fillAmount = (float)this.vida / 3;
    }
    public void disminuirVida(int vida) {
        this.vida -= vida;
        imagenVida.fillAmount = (float)this.vida / 3;
        if (this.vida <= 0) {
            finalizarPartida();
        }
    }

    public void aumentarVelocidadDisparo(int velocidad) {
        if(velocidad < 30) this.velocidadDisparo += velocidad;
    }
    public void cambiarEscena(string escena) { 
        guardarPartida();
        SceneManager.LoadScene(escena); 
        }
    

    public void comenzarPartida(){
        popupGameOver.SetActive(false); // Hide the Game Over popup
        quitarPausa();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        nivel = 1;
        puntos = 0;
        vida = 3;
        velocidadDisparo = 5;
    }

    public void finalizarPartida(){
        guardarPartida();
        textoGameOverNivelMax.text = "Nivel: " + nivel.ToString();
        textoGameOverPuntosMax.text = "Puntos: " + puntos.ToString();
        popupGameOver.SetActive(true); // Show the Game Over popup
        pausar();
    }


    public void guardarPartida(){
        if(PlayerPrefs.GetInt ("nivelDesbloqueado") < nivel)
            PlayerPrefs.SetInt("nivelDesbloqueado", nivel);
        if(PlayerPrefs.GetInt ("puntosMax") < puntos) 
            PlayerPrefs.SetInt("puntosMax", puntos);
        }
    
    public void cargarPartida(){
        nivelMax = PlayerPrefs.GetInt("nivelDesbloqueado");
        puntosMax = PlayerPrefs.GetInt("puntosMax");
        }

    public void pausar(){ Time.timeScale = 0; }
    public void quitarPausa(){ Time.timeScale = 1; }




}
