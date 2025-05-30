using UnityEngine;

public class Destructor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DisparoAmigo"))
        {
            DisparoPlayer poolable = other.GetComponent<DisparoPlayer>();
            if (poolable != null)
            {
                poolable.Release(); // usa método seguro
            }
        }
        else if (!other.CompareTag("Background") && !other.CompareTag("DisparoAmigo"))
        {
            Destroy(other.gameObject);
        }
    }
}
