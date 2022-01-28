using System;
using System.Collections.Generic;
using UnityEngine;

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


    // nie jest abstract, zeby mozna bylo stworzyc serializowana liste, ale po tej klasie dziedziczymy wszystkie inne systemy
    // ale dziala jak abstract, bo nie mozna podpiac jako komponent, jesli nie jest w pliku o tej samej nazwie, BANG
    public class BaseGameSystem : MonoBehaviour {

        public virtual void OnCreate() {
            // tutaj wstepna logika inicjalizacyjna - przed odwolaniem sie do innych systemow
        }

        public virtual void Initialise() {
            // tutaj glowna logika inicjalizacji - wszystkie systemy sa juz stworzone, wiec mozna sie do nich odwolac
        }
    }
}
