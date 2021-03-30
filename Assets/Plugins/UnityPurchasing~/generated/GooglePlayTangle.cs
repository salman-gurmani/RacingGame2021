#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("N8/KqJl8m3VZcUm7DG4PPmhkgKw8jg0uPAEKBSaKRIr7AQ0NDQkMD7OvfMdbnQE6vu5abs7Oco/MMdPk3oLFj/bP3wDBHvV65vTRbTSfqo/i295S4eoOIhze7f1ADtMFdz2CV53UMuH4tPiH21gnEcLJTa7z3OQ8Umm0rolrakHa9CofHVRTo+6oWZ47weMiSgXm/HbYd5auVOLPUk9LexJ0nmZ9D4hB8tx7XD7QxE7Axt2Njg0DDDyODQYOjg0NDLezbCnVj6o+JDoFm9wGaWudQt8bt1SbQUN2RoXg9F3CpwgLvM/8GopyBvavMJegbEjPV23PM9fnT927Qyt5+OHDOrINKI210844S1BrmQGzELd6kECqCM++6jNNpNofPw4PDQwN");
        private static int[] order = new int[] { 3,3,9,10,5,12,9,7,11,10,13,12,13,13,14 };
        private static int key = 12;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
