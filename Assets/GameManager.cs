using UnityEngine;
using UnityEngine.SceneManagement;
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
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados
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
    public int nivel;
    public int puntos;
    public int vida;

    // Control del estado
    public TiposDeEstado estadoActual;

    // Definimos previamente el enumerado
    public enum TiposDeEstado { INTRO, MENU, JUEGO, SALIR }



    // ------------------------
    // Metodos 
    // ------------------------
    public void cambiarEscena(string escena) { 
        guardarPartida();
        SceneManager.LoadScene(escena); 
        }
    
    public void reiniciar(){
        guardarPartida();
        quitarPausa();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void guardarPartida(){
        if(PlayerPrefs.GetInt ("nivelDesbloqueado") < nivel)
            PlayerPrefs.SetInt("nivelDesbloqueado", nivel);
        if(PlayerPrefs.GetInt ("puntosMax") < puntos) 
            PlayerPrefs.SetInt("puntosMax", puntos);
        }
    
    public void cargarPartida(){
        nivel = PlayerPrefs.GetInt("nivelDesbloqueado");
        puntos = PlayerPrefs.GetInt("puntosMax");
        }

    public void pausar(){ Time.timeScale = 0; }
    public void quitarPausa(){ Time.timeScale = 1; }



}
