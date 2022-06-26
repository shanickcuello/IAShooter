namespace Features.Soldier.Scripts.FSM.States
{
    public class SoldierDeath<T> : States<T>
    {
        private readonly SoldierController _soldierController;

        public SoldierDeath(SoldierController soldierController)
        {
            _soldierController = soldierController;
        }

        public override void Awake()
        {
            _soldierController.UnableColliders();
        }
    }
}