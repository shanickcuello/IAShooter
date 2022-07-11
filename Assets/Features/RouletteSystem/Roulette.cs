using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Features.RouletteSystem
{
    public class Roulette
    {
        public string Execute(Dictionary<string, int> actions)
        {
            var totalWeight = actions.Sum(item => item.Value);
            var random = Random.Range(0, totalWeight);
            foreach (var item in actions)
            {
                random -= item.Value;
                if (random < 0)
                {
                    return item.Key;
                }
            }
            return actions.Keys.First();
        }
    }
}