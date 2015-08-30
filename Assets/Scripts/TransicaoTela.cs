using UnityEngine;

public class TransicaoTela : MonoBehaviour {

    void Ativar()
    {
        var efeito = Camera.main.GetComponent<CC_FastVignette>();
        Go.from(efeito, 0.3f, new GoTweenConfig()
            .floatProp("pSharpness", -100f)
            .floatProp("pDarkness", 100f)
            .setEaseType(GoEaseType.BackInOut));
    }
}
