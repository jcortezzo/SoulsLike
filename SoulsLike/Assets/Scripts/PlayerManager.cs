using UnityEngine;

namespace OGS
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            inputHandler.IsInteracting = anim.GetBool("IsInteracting");
            inputHandler.RollFlag = false;
            inputHandler.SprintFlag = false;
        }
    }
}
