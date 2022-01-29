using PartTimeKamikaze.KrakJam2022.Utils;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerDataSystem : BaseGameSystem {
        public Observable<int> Sanity;

        // TODO: Sanity & inventory?
        public override void OnCreate() {
            Sanity = new Observable<int>(100);
            //Move this to UI Object
        }

        public override void Initialise() {
            throw new System.NotImplementedException();
        }

    }
}
