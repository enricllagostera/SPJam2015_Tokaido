using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PosicaoNuvem
{
    TOPO,
    DIREITA,
    BAIXO,
	ESQUERDA
}


public class NuvemDialogo : MonoBehaviour {

    public PosicaoNuvem posicao;
    public Transform origem;
    public Text txtMsgPrincipal;

    public static NuvemDialogo MostrarMsg(string texto, Transform novaOrigem)
    {

        NuvemDialogo nuvem = Instantiate(Resources.Load<NuvemDialogo>("NuvemDialogo"));
        Go.from(nuvem.transform, 0.3f, new GoTweenConfig()
            .scale(Vector3.zero)
            .setEaseType(GoEaseType.QuartOut));
        nuvem.txtMsgPrincipal.text = texto;
        nuvem.origem = novaOrigem;
        return nuvem;
    }

    public void Destruir()
    {
        print("destruindo");
        Go.to(transform, 0.2f, new GoTweenConfig()
            .scale(Vector3.zero)
            .setEaseType(GoEaseType.QuartIn)
            .onComplete(a =>
            {
                Destroy(gameObject);        
            }));
    }
    
    public IEnumerator DestruirComDelay(float delay)
    {
        print("com timer");
        yield return new WaitForSeconds(delay);
        Destruir();
    }

    void Update()
    {
		switch (posicao)
        {
            case PosicaoNuvem.TOPO:
                transform.position = origem.position + new Vector3(0, 4, 0);
                break;
        }
	}
}
