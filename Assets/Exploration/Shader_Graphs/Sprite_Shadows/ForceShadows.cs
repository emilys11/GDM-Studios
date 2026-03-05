using UnityEngine;

public class ForceShadows : MonoBehaviour {

    private void Start()
    {
        GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
    
       


}

