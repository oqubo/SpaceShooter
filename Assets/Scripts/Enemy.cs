using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private int puntos;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private GameObject disparoPrefab;
    

    [SerializeField] private TipoEnemigo tipoEnemigo;
    private enum TipoEnemigo { Mosca, MoscaGorda, Calavera, Boss, Monster }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(tipoEnemigo == TipoEnemigo.Monster)
        {
            MoverArribaAbajo();
            StartCoroutine(SpawnDisparoCoroutine());
        }
        else if(tipoEnemigo == TipoEnemigo.Boss)
        {
            Aparecer();
            StartCoroutine(SpawnDisparoCoroutine());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tipoEnemigo != TipoEnemigo.Boss)
        {
            transform.Translate(Vector3.left  * GameManager.instancia.velocidad * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DisparoAmigo"))
        {
            vida--;
            Destroy(other.gameObject);
            Parpadeo();
            
            if (vida == 0)
            {
                Desaparecer();
                GameManager.instancia.aumentarPuntos(puntos);

                if(powerUp != null){
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void Parpadeo()
    {
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

    private void Aparecer()
    {
        transform.DOMoveX(transform.position.x - 7, 1f).SetEase(Ease.InOutQuad);
        transform.DOMoveY(1f, 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void Desaparecer()
    {

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
        Destroy(gameObject, 0.3f);
    }

    private void MoverArribaAbajo()
    {
        transform.DOMoveY(transform.position.y + 0.5f, 0.5f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }

    IEnumerator SpawnDisparoCoroutine(){
        while (true)
        {
            Instantiate(disparoPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
