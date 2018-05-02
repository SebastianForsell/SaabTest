using RentingSystem.Interfaces;

namespace RentingSystem.Classes
{
    class SmallCarClass : Vehicle
    {
        public override string Type
        {
            get
            {
                return "Small car";
            }
        }
    }
}
