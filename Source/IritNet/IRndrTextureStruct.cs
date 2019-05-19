using System;

namespace IritNet
{
    public unsafe struct IRndrTextureStruct
    {
        public int Type;                                  /* Procedural/Raster/Surface. */
        public byte *TextureFileName;

        public IPObjectStruct *OrigSrf;      /* Reference to original geometry surface. */
        public IrtPtTypeArray2 OrigSrfParamDomain;   /* Parametric domain of original srf. */

        /* Raster image texture. */
        public IRndrImageStruct *PrmImage; /* Ptr to texture image if exists, NULL else.*/
        public double PrmUMin, PrmUMax, PrmVMin, PrmVMax;       /* Parametric domain. */
        public double PrmUScale, PrmVScale, PrmWScale;    /* Scale in U/V of texture. */
        public double PrmAngle;     /* Angle of rotation of texture around main axis. */
        public int PrmTextureType;  /* Type of param. texture: regular, spherical, etc. */
        public IrtVecType PrmDir;       /* Direction used in some parametric texturing. */
        public IrtVecType PrmOrg;    /* Origin point used in some parametric texturing. */
        public IrtHmgnMatType PrmMat;      /* Parametric texture transformation matrix. */

        /* Surface style texture. */
        public CagdSrfStruct *Srf;         /* A surface to evaluate for texture value.  */
        public IrtPtTypeArray2 SrfParamDomain;                /* Parametric domain of srf. */
        public IRndrImageStruct *SrfScale;     /* To map the value of srf into a color. */
        public fixed double SrfScaleMinMax[2];  /* Values to be mapped to srfScale extrema. */
        public int SrfFunc;     /* If surface value should be piped through a function. */

        /* Curvature style textures. */
        public double CrvtrSrfMax;	         /* Bounding value on the curvature. */

        /* Procedure/volumetric type texture. */
        public IntPtr vTexture;
        public IrtHmgnMatType Mat;                    /* Texture transformation matrix. */
        public double Width;              /* Width used in some volumetric texturing. */
        public double Depth;              /* Width used in some volumetric texturing. */
        public double Brightness;    /* Brightness used in some volumetric texturing. */
        public double Frequency;      /* Frequency used in some volumetric texturing. */
        public IrtImgPixelStruct Color;     /* Color used in some volumetric texturing. */
        public IrtVecType tScale;                          /* Volumetric texture scale. */
        public IrtVecType Dir;          /* Direction used in some volumetric texturing. */
        public IrtVecType Org;       /* Origin point used in some volumetric texturing. */
        /* Parameters specific for "wood" volumetric texture. */
        public double CenterScale;
        public double CenterFactor;
        public double WaveNPoints;
        public double WaveFactor;
        public double FiberScale;
        public double FiberFactor;
        /* Parameters specific for "marble" volumetric texture. */
        public double TurbulenceScale;
        public double TurbulenceFactor;
        /* Parameters specific for "checker" volumetric texture. */
        public IRndrColorTypeArray3 CheckerColor;                     /* Checker's colors. */
        public double CheckerPlane; /* Flag for whether it's a plane checker or a 3D. */
    }
}