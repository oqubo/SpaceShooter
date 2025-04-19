using UnityEngine;
using UnityEngine.Pool;

public class DisparoPlayer : MonoBehaviour
{

    public ObjectPool<DisparoPlayer> myPool { get; set; }
    private bool isReleased = false;

    private void OnEnable()
    {
        isReleased = false; // se reactiva desde el pool
    }

    public void Release()
    {
        if (!isReleased && myPool != null)
        {
            isReleased = true;
            myPool?.Release(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * GameManager.instancia.velocidadDisparo * Time.deltaTime);
    }


}
