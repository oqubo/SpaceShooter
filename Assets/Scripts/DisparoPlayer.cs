using UnityEngine;

public class DisparoPlayer : MonoBehaviour
{

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
