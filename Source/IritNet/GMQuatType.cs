﻿namespace IritNet
{
    public unsafe struct GMQuatType
    {
        public fixed double Values[4];

        public double this[int index]
        {
            get
            {
                var loc = this;
                return ((double*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((double*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}