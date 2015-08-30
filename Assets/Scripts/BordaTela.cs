using UnityEngine;

public class BordaTela : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.gameObject.name == "Jogador")
        {
            if (transform.localPosition.x > 0)
            {
                Registro.telaAtual++;
                Camera.main.transform.position += new Vector3(20, 0, 0);
                outro.transform.position = GameObject.Find("EncaixeEsquerda").transform.position;
            }
            else
            {
                Registro.telaAtual--;
                Camera.main.transform.position += new Vector3(-20, 0, 0);
                outro.transform.position = GameObject.Find("EncaixeDireita").transform.position;
            }
            Camera.main.SendMessage("Ativar");
        }
	}
}
