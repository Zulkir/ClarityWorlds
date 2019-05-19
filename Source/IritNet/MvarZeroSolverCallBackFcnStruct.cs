using System;

namespace IritNet
{
    public unsafe struct MvarZeroSolverCallBackFcnStruct
    {
        public IntPtr SolveBoundary;    
        public IntPtr NoZeroCrossTest;
        public IntPtr GuaranteedTopologyTest;
        public IntPtr SubdivProblem;
        public IntPtr NumericImprovement;
        public IntPtr SingularSolution;
        public IntPtr UniteSolutions;
        public IntPtr OrganizeSolution;
        public IntPtr UpdateDmn;
        public IntPtr FirstSmoothUpdates;
        public IntPtr MapPtParamSpace2EuclidSpace;
        public IntPtr MapPtParamSpace2EuclidNormal;
        public IntPtr CriticalPts;
    }
}