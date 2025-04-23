using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UI;
public class PlayerController2DHorVerClamped : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private DisparoPlayer bulletPrefab;
    [SerializeField] private Button fireButton;
    [SerializeField] private Joystick joystick;
    [SerializeField] private bool isMobile;
    private float horizontalInput, verticalInput;
    private Vector3 minBounds, maxBounds, clampedPosition;

    private Camera cam;
    private float distanceToCamera;

    private ObjectPool<DisparoPlayer> bulletPool;


    void Awake()
    {
        bulletPool = new ObjectPool<DisparoPlayer>(
            CreateBullet,
            OnGetBullet,
            OnReleaseBullet,
            OnDestroyBullet
        );
            
    }
    
    void Start()
    {

        ComprobarMovilEscritorio();
        CalcularLimites();

    }

    void Update()
    {
        Movimiento();
        DelimitarMovimiento();
        if(!isMobile)
            if (Input.GetKeyDown(KeyCode.Space))
                Disparo();
    }

    void ComprobarMovilEscritorio(){
        isMobile = Application.isMobilePlatform;
        if (isMobile)
        {
            joystick.gameObject.SetActive(true);
            fireButton.gameObject.SetActive(true);
        }
        else
        {
            joystick.gameObject.SetActive(false);
            fireButton.gameObject.SetActive(false);
        }
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

        if(isMobile){
            // Obtener las pulsaciones del joystick
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
            // Mover el objeto
            transform.Translate(new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime);
        }
        else{
            // Obtener las pulsaciones del teclado
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            // Mover el objeto
            transform.Translate(new Vector2(horizontalInput, verticalInput).normalized  * speed * Time.deltaTime);
        }


    }

    void DelimitarMovimiento(){
        // Limitar la posición del objeto dentro de los bordes
        clampedPosition.x = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = clampedPosition;
    }

    public void Disparo(){
        DisparoPlayer bullet = bulletPool.Get();
        bullet.transform.position = transform.position;            
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


    // Object Pool Methods
    private DisparoPlayer CreateBullet()
    {
        DisparoPlayer bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.myPool = bulletPool;
        return bullet;
    }
    private void OnGetBullet(DisparoPlayer bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(DisparoPlayer bullet)
    {
        bullet.gameObject.SetActive(false);
    } 
    private void OnDestroyBullet(DisparoPlayer bullet)
    {
        Destroy(bullet.gameObject);
    }


}
