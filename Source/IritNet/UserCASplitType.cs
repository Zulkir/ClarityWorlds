using System;

namespace IritNet
{
    [Flags]
    public enum UserCASplitType
    {
        USER_CA_SPLIT_NONE = 0x0000,
        USER_CA_SPLIT_INFLECTION_PTS = 0x0001,
        USER_CA_SPLIT_MAX_CRVTR_PTS =  0x0002,
        USER_CA_SPLIT_C1DISCONT_PTS =  0x0004,
        USER_CA_SPLIT_REAL_C1DISCONT_PTS = 0x0008
    }
}