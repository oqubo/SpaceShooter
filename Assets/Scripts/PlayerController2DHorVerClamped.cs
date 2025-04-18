using UnityEngine;
using DG.Tweening;
public class PlayerController2DHorVerClamped : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;

    private float horizontalInput, verticalInput;

    private Vector3 minBounds, maxBounds, clampedPosition;

    private Camera cam;
    private float distanceToCamera;



    void Start()
    {

        CalcularLimites();

    }

    void Update()
    {
        Movimiento();
        DelimitarMovimiento();
        Disparo();
    }

    void CalcularLimites(){
        // Calcula los límites del viewport de la cámara principal en coordenadas del mundo
        cam = Camera.main;

        // El plano de proyección debe tener en cuenta la distancia desde la cámara al objeto
        distanceToCamera = Mathf.Abs(cam.transform.position.z - transform.position.z);

        // Esquina inferior izquierda del viewport (0, 0)
        minBounds = cam.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));

        // Esquina superior derecha del viewport (1, 1)
        maxBounds = cam.ViewportToWorldPoint(new Vector3(1, 1, distanceToCamera));
    }

    void Movimiento(){
        // Obtener las pulsaciones
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Mover el objeto
        transform.Translate(new Vector2(horizontalInput, verticalInput).normalized  * speed * Time.deltaTime);
    }

    void DelimitarMovimiento(){
        // Limitar la posición del objeto dentro de los bordes
        clampedPosition.x = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = clampedPosition;
    }

    void Disparo(){
        // Disparar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("DisparoEnemigo"))
        {
            GameManager.instancia.disminuirVida(1);
            //Parpadeo();
            GetComponent<Animator>().SetTrigger("hitTrigger");
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("PowerUpVelocidad"))
        {
            GameManager.instancia.aumentarVelocidadDisparo(5);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("PowerUpVida"))
        {
            GameManager.instancia.aumentarVida(1);
            Destroy(other.gameObject);
        }
    }

    void Parpadeo(){
    
        int parpadeos = 3;
        float duracionParpadeo = 0.1f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sequence secuencia = DOTween.Sequence();

        for (int i = 0; i < parpadeos; i++)
        {
            secuencia.Append(spriteRenderer.DOColor(Color.red, duracionParpadeo));
            secuencia.Append(spriteRenderer.DOColor(Color.white, duracionParpadeo));
        }
        
    }

}
