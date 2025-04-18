using UnityEngine;

public class Parallax : MonoBehaviour
{

[SerializeField] private float velocidadBase;
[SerializeField] private float anchoImagen;
[SerializeField] private Vector3 direccion;

private Vector3 posicionInicial;

private float resto;

    void Start()
    {
        posicionInicial = transform.position;
        anchoImagen = GetComponent<SpriteRenderer>().bounds.size.x;
        resto = 0;
    }

    void Update()
    {
        // resto indica lo que queda de recorrido para alcanzar un nuevo ciclo
        resto = (velocidadBase * Time.time) % anchoImagen;

        // la posicion se va refrescando desde la inicial sumando lo que queda (el resto) que si es 0 vuelve a moverse a la inicial
        transform.position = posicionInicial + direccion * resto;
        
    }
}
