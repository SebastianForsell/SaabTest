namespace RentingSystem.Interfaces
{
    public abstract class Vehicle
    {
        public virtual int BaseDayRental
        {
            get
            {
               return 500;
            }
        }
        public virtual int KmPrice
        {
            get
            {
                return 0;
            }
        }
        public abstract string Type {get;}
    }
}
