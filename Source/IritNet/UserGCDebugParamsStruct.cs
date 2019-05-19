using System;

namespace IritNet
{
    public unsafe struct UserGCDebugParamsStruct
    {
        /* Don't do the set cover, display all the visibility maps. */
        public int DisableSetCover;
        /* The geometric object and the obstacles are going through several      */
        /* transformations such as adapting their UV parametrization and         */
        /* combining all the polygons into one polygon struct. When this flag is */
        /* TRUE, those objects after those transformation are saved to disk as   */
        /* an itd file. The objects doesn't include any scene transformations    */
        /* depended on the observation points and without the projection to the  */
        /* the screen.                                                           */
        /* Effective only if LoadObjectsFromDisk > 0 (The objects are actually   */
        /* saved regardless of this flag. This flag just tells not to erase the  */
        /* objects at the end).                                                  */
        public int StoreObjectsBeforeOPs;
        /* For each observation point save the object after the transformation   */
        /* saved for StoreObjectsBeforeOPs and additional scene transfomration   */
        /* defined by the observation point and the projection to the screen.    */
        public int StoreObjectsForOPs;
        /* Don't delete the visibility map from disk after creation. */
        public int StoreVisMap;
        /* Create imd matrix files with the matrix used in order to calculate    */
        /* each visibility map. This doesn't include the projection              */
        /* transformations.                                                      */
        public int StoreVisMapMatrix;
        /* Don't create visibility maps. Load them from disk. */
        public int LoadVisMaps;
        /* Control saving/loading of geometric objects and obstacles to disk.    */
        /* (The same objects StoreObjectsBeforeOPs relates to).                  */
        /* 0 - Doesn't save the objects to disk. Keeps the originals and creates */
        /*     copies as needed.                                                 */
        /* 1 - Saves the objects. Keeps the originals and creates copies as      */
        /*     needed.                                                           */
        /* 2 - Saves the objects. Doesn't Keep the originals. Load from disk     */
        /*     for each creation of visibility maps. Reduce memory but increase  */
        /*     time (though may reduce time if it reduces swapping).             */
        /* 3 - Saves the objects. Doesn't Keep the originals. Load from disk     */
        /*     for each creation of sub visibility maps (once for each visibility*/
        /*     map in orthographic and up to 3 times in perspective). Reduce     */
        /*     memory but increases time (though may reduce time if it reduces   */
        /*     swapping).                                                        */
        /* The objects if saved, are deleted at the end unless                   */
        /* StoreObjectsBeforeOPs is TRUE.                                        */
        public int LoadObjectsFromDisk;
        /* This is an input and output of UserGCSolveGeoProblem. GeoObj contains */
        /* several polygonal objects connected using Pnext. Those polygonal      */
        /* objects could possibly be originated from surface objects. GeoObjOrig */
        /* when given to UserGCSolveGeoProblem cotnains a list of objects with   */
        /* the original surfaces (or the polygonal objects if no original        */
        /* surface exists) in the same order as in GeoObj. UserGCSolveGeoProblem */
        /* will change the UV parametrization of all the surfaces (or polygonal  */
        /* objects) according to the same UV parametrization changes done to     */
        /* GeoObj.                                                               */
        public IPObjectStruct *GeoObjOrig;
        /* The function to load and save images. Must be compatible to each      */
        /* other. If any of the load or save functions are NULL, they are both   */
        /* replaced by default functions for PPM image.                          */
        /* LoadImageFunc must allocate memory using IritMalloc since the memroy  */
        /* is freed internally by IritFree.                                      */
        public IntPtr LoadImageFunc;
        public IntPtr SaveImageFunc;
        /* The extension of the visibility maps as saved/loaded by               */
        /* SaveImageFunc/LoadImageFunc. If the default PPM functions are used    */
        /* this field will be set to "ppm" by the application.                   */
        public fixed byte VisMapExtension[4];
        /* This is a printf function to write debug messages to. If it's NULL,   */
        /* it will be replaced by inner function which doesn't write anything    */
        /* and always returns 0.                                                 */
        public IntPtr Print;
    }
}