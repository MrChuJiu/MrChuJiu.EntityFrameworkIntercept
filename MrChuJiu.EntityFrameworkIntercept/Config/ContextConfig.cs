using System;
using System.Collections.Generic;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept.Config
{
    public sealed class ContextConfig
    {
        internal static ContextConfigOptions ContextConfigOptions = new ContextConfigOptions();
        public static void Configure(Action<IContextConfigBuilder> configAction)
        {
            if (null == configAction)
                return;

            var builder = new ContextConfigBuilder();
            configAction.Invoke(builder);
            ContextConfigOptions = builder.Build();
        }
    }
}
