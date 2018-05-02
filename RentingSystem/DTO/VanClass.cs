using RentingSystem.Interfaces;

namespace RentingSystem.Classes
{
    class VanClass : Vehicle
    {
        public override string Type
        {
            get
            {
                return "Van";
            }
        }
    }
}
