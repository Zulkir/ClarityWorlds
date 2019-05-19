namespace IritNet 
{
    public unsafe struct UserCACrvStructPtrArray10
    {
        public  UserCACrvStruct *RefCrvs0;
        public  UserCACrvStruct *RefCrvs1;
        public  UserCACrvStruct *RefCrvs2;
        public  UserCACrvStruct *RefCrvs3;
        public  UserCACrvStruct *RefCrvs4;
        public  UserCACrvStruct *RefCrvs5;
        public  UserCACrvStruct *RefCrvs6;
        public  UserCACrvStruct *RefCrvs7;
        public  UserCACrvStruct *RefCrvs8;
        public  UserCACrvStruct *RefCrvs9;

        public UserCACrvStruct this[int index]
        {
            get
            {
                var loc = this;
                return ((UserCACrvStruct*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((UserCACrvStruct*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}