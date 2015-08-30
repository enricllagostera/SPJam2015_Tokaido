using UnityEngine;

public class Encaixe : MonoBehaviour
{
    public TipoProp tipo;

    void Start()
    {
        var possiveis = Resources.LoadAll<Prop>(tipo.ToString().ToUpper());
        if (possiveis.Length > 0)
        {
            var novo = Instantiate(possiveis[Random.Range(0, possiveis.Length)], transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
