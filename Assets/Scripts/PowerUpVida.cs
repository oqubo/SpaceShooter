using UnityEngine;

public class PowerUpVida : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left  * GameManager.instancia.velocidad * Time.deltaTime);
    }




}
