namespace CallGate.Seeders
{
    public abstract class BaseSeeder : ISeeder
    {
        public int CompareTo(ISeeder other)
        {
            if ( GetPriority() > other.GetPriority() ) return 1;
            if ( GetPriority() < other.GetPriority() ) return -1;
            
            return 0;
        }

        public abstract int GetPriority();
        public abstract void Seed();
    }
}
