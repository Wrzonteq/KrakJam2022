using System;
using System.Collections.Generic;

namespace PartTimeKamikaze.KrakJam2022 {
    public static class GameSystems {
        static Dictionary<Type, BaseGameSystem> systemsDict;


        public static T GetSystem <T>() where T : BaseGameSystem {
            return (T)systemsDict[typeof(T)];
        }

        public static void Init(List<BaseGameSystem> systems) {
            systemsDict = new Dictionary<Type, BaseGameSystem>();
            foreach (var s in systems)
                systemsDict[s.GetType()] = s;
        }
    }
}
