﻿using System.Runtime.InteropServices;
using System.Threading;

namespace Example
{
    public static class ExampleApp
    {
        [DllExport]
        public static void DllMain()
        {
            var thread = new Thread(() =>
            {
                var application = new MyApplication();
                application.Run();
            });

            // thread.SetApartmentState(ApartmentState.STA); If you're running a UI app (WPF, WINFORM)
            thread.Start();
        }
    }
}