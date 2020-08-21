// using System;
//
// namespace Plugin.Angpysha.LeftTabbedPage
// {
//     /// <summary>
//     /// Cross Angpysha.LeftTabbedPage
//     /// </summary>
//     public static class CrossAngpysha.LeftTabbedPage
//     {
//         static Lazy<IAngpysha.LeftTabbedPage> implementation = new Lazy<IAngpysha.LeftTabbedPage>(() => CreateAngpysha.LeftTabbedPage(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
//
//     /// <summary>
//     /// Gets if the plugin is supported on the current platform.
//     /// </summary>
//     public static bool IsSupported => implementation.Value == null ? false : true;
//
//     /// <summary>
//     /// Current plugin implementation to use
//     /// </summary>
//     public static IAngpysha.LeftTabbedPage Current
//     {
//         get
//         {
//             IAngpysha.LeftTabbedPage ret = implementation.Value;
//             if (ret == null)
//             {
//                 throw NotImplementedInReferenceAssembly();
//             }
//             return ret;
//         }
//     }
//
//     static IAngpysha.LeftTabbedPage CreateAngpysha.LeftTabbedPage()
//     {
// #if NETSTANDARD1_0 || NETSTANDARD2_0
//             return null;
// #else
// #pragma warning disable IDE0022 // Use expression body for methods
//         return new Angpysha.LeftTabbedPageImplementation();
// #pragma warning restore IDE0022 // Use expression body for methods
// #endif
//     }
//
//     internal static Exception NotImplementedInReferenceAssembly() =>
//         new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
//
// }
// }
