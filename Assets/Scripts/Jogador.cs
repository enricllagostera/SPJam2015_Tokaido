using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum EstadoPersonagem
{
    FALANDO,
    ANDANDO
}

public class Jogador : MonoBehaviour
{
    public EstadoPersonagem estado;
    public Vector2 velocidadeMaxima;
    public Transform visual;
    public Interagivel interlocutor = null;
    private Rigidbody2D _rb;
    private Vector2 _input;
    private GameObject _balao;

    public List<string> tem;
    public List<string> naoTemMais;

    public Text txtTem;
    public Text txtNaoTemMais;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        tem = new List<string>();
        naoTemMais = new List<string>();
        //tem.Add("i_nada", "nada\n");
    }

    void Update()
    {
        //print(estado);
        switch (estado)
        {
            case EstadoPersonagem.FALANDO:
                _input = Vector2.zero;
                break;
            case EstadoPersonagem.ANDANDO:
                // movimento
                _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (_input.magnitude > 0.1f)
                {
                    float sinal = 1;
                    if (_input.x < 0)
                    {
                        sinal = -1;
                    }
                    visual.localScale = new Vector3(sinal, visual.localScale.y, 1);
                }
                break;
        }
        ExibirInventarios();
    }

    void FixedUpdate()
    {
        Vector2 deslocamento = new Vector2();
        deslocamento.x = _input.x * velocidadeMaxima.x;
        deslocamento.y = _input.y * velocidadeMaxima.y;
        _rb.MovePosition(_rb.position + deslocamento * Time.fixedDeltaTime);
    }

    void ExibirInventarios()
    {
        txtTem.text = "você tem:";
        if (tem.Count == 0)
        {
            txtTem.text += "\nNada";
        }
        else
        {
            foreach (var coisa in tem)
            {
                txtTem.text += "\n- " + coisa;
            }
        }
        txtNaoTemMais.text = "você tem:";
        if (naoTemMais.Count == 0)
        {
            txtNaoTemMais.text += "\nNada";
        }
        else
        {
            foreach (var coisa2 in naoTemMais)
            {
                txtNaoTemMais.text += "\n- " + coisa2;
            }
        }
    }
}
