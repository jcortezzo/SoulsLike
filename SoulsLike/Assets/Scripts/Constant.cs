using System.Collections.Generic;

namespace OGS
{
    public class Constant
    {
        public static ISet<PlayerState> interactingPlayerStates = new HashSet<PlayerState>()
        {
            PlayerState.ROLLING
        };
    }
}
