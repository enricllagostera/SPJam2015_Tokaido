using UnityEngine;
using System.Collections.Generic;
using CsvFiles;
using System.Linq;

public enum EstadoInteragivel
{
    PRE,
    DURANTE,
    POS
}

[System.Serializable]
public class InteragivelInfo
{
    public string id_interagivel { get; set; }
    public string id_item_req { get; set; }
    public string id_item_dado { get; set; }
    public string pre_01 { get; set; }
    public string pre_02 { get; set; }
    public string pre_03 { get; set; }
    public string dur_01 { get; set; }
    public string dur_02 { get; set; }
    public string dur_03 { get; set; }
    public string pos_01 { get; set; }
    public string pos_02 { get; set; }
    public string pos_03 { get; set; }
}

public class Interagivel : MonoBehaviour
{
    public string idInteragivel = "";
    public EstadoInteragivel estado = EstadoInteragivel.PRE;
    public bool ativo = false;
    public List<string> trechosPreItem;
    public List<string> trechosDuranteItem;
    public List<string> trechosPosItem;
    public int trechoAtual = 0;
    private Jogador _jogador;
    private NuvemDialogo _balao;
    public InteragivelInfo dados = null;

    public static List<InteragivelInfo> infos = null;

    void Start()
    {
        if (infos == null)
        {
            infos = CsvFile.Read<InteragivelInfo>("Assets\\data.csv").ToList();
        }
        dados = infos.Find(i => i.id_interagivel.ToLower() == idInteragivel.ToLower());
        // carrega listas de mensagens
        trechosPreItem = new List<string>();
        trechosDuranteItem = new List<string>();
        trechosPosItem = new List<string>();
        trechosPreItem.AddRange(new string[3] { dados.pre_01, dados.pre_02, dados.pre_03 });
        trechosDuranteItem.AddRange(new string[3] { dados.dur_01, dados.dur_02, dados.dur_03 });
        trechosPosItem.AddRange(new string[3] { dados.pos_01, dados.pos_02, dados.pos_03 });
        trechosPreItem.RemoveAll(s => s == "");
        trechosDuranteItem.RemoveAll(s => s == "");
        trechosPosItem.RemoveAll(s => s == "");
        // iniciar        
        estado = EstadoInteragivel.PRE;
        _jogador = GameObject.Find("Jogador").GetComponent<Jogador>();
    }

    void Update()
    {
        if (ativo)
        {
            if (Input.GetKeyUp(KeyCode.Space) && _jogador.interlocutor == this)
            {
                _jogador.estado = EstadoPersonagem.FALANDO;
                if (_balao != null) _balao.Destruir();
                if (_jogador.tem.Contains(dados.id_item_req) && estado != EstadoInteragivel.POS)
                {
                    estado = EstadoInteragivel.DURANTE;
                }
                bool estourou = MostrarMsgAtual();
                if (estourou)
                {
                    //if(estado != EstadoInteragivel.POS) trechoAtual = 0;
                    _balao.StartCoroutine("DestruirComDelay", 0.5f);
                    _jogador.estado = EstadoPersonagem.ANDANDO;
                }
            }
        }
    }

    public bool MostrarMsgAtual()
    {
        int numTrechos = 0;
        List<string> trechos = null;
        switch (estado)
        {
            case EstadoInteragivel.PRE:
                numTrechos = trechosPreItem.Count;
                trechos = trechosPreItem;
                break;
            case EstadoInteragivel.DURANTE:
                numTrechos = trechosDuranteItem.Count;
                trechos = trechosDuranteItem;
                break;
            case EstadoInteragivel.POS:
                numTrechos = trechosPosItem.Count;
                trechos = trechosPosItem;
                break;
        }
        // se já estourou o total de msgs deste tipo
        if (trechoAtual >= numTrechos)
        {
            _balao = NuvemDialogo.MostrarMsg("...", transform);
            trechoAtual = 10;
            if (estado != EstadoInteragivel.POS)
            {
                AdicionarAoInventario();
            }
            //estado = proximoEstado;
            return true;
        }
        // se ainda nao estourou
        _balao = NuvemDialogo.MostrarMsg(trechos[trechoAtual], transform);
        trechoAtual++;
        return false;
    }

    void AdicionarAoInventario()
    {
        if (dados.id_item_req.Length == 0 ||
            _jogador.tem.Contains(dados.id_item_req))
        {
            if (dados.id_item_req.Length > 0)
            {
                _jogador.tem.Remove(dados.id_item_req);
                _jogador.naoTemMais.Add(dados.id_item_req);
            }
            _jogador.tem.Add(dados.id_item_dado);
            estado = EstadoInteragivel.POS;
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.gameObject.name == "Jogador" &&
            _jogador.estado == EstadoPersonagem.ANDANDO)
        {
            trechoAtual = 0;
            ativo = true;
            _jogador.interlocutor = this;
        }
    }

    void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.gameObject.name == "Jogador" &&
            _jogador.interlocutor == this)
        {
            ativo = false;
            _jogador.interlocutor = null;
        }
    }
}
