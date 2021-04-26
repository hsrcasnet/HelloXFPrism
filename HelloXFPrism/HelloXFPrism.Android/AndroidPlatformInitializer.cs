using Prism;
using Prism.Ioc;

namespace HelloXFPrism.Droid
{
    public partial class MainActivity
    {
        public class AndroidPlatformInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
            }
        }
    }
}