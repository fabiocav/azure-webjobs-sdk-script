﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Azure.WebJobs.Script.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Azure.WebJobs.Script.Tests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;status&quot;: &quot;Resolved&quot;,
        ///  &quot;context&quot;: {
        ///    &quot;id&quot;: &quot;/subscriptions/&lt;your-subscriptionId&gt;/resourceGroups/functionsWebhooks/providers/microsoft.insights/alertrules/webhooksdemo&quot;,
        ///    &quot;name&quot;: &quot;webhooksdemo&quot;,
        ///    &quot;description&quot;: &quot;webhooksdemo&quot;,
        ///    &quot;conditionType&quot;: &quot;Metric&quot;,
        ///    &quot;condition&quot;: {
        ///      &quot;metricName&quot;: &quot;Requests&quot;,
        ///      &quot;metricUnit&quot;: &quot;Count&quot;,
        ///      &quot;metricValue&quot;: &quot;1&quot;,
        ///      &quot;threshold&quot;: &quot;1&quot;,
        ///      &quot;windowSize&quot;: &quot;5&quot;,
        ///      &quot;timeAggregation&quot;: &quot;Total&quot;,
        ///      &quot;operator&quot;: &quot;GreaterThan&quot;
        ///    } [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AzureWebHookEventRequest {
            get {
                return ResourceManager.GetString("AzureWebHookEventRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///    &quot;scriptFile&quot;:&quot;DotNetFunctionAssembly.dll&quot;,
        ///    &quot;entryPoint&quot;: &quot;TestFunction.Function.Run&quot;,
        ///    &quot;bindings&quot;: [
        ///        {
        ///            &quot;type&quot;: &quot;httpTrigger&quot;,
        ///            &quot;name&quot;: &quot;req&quot;,
        ///            &quot;direction&quot;: &quot;in&quot;,
        ///            &quot;methods&quot;: [ &quot;get&quot; ]
        ///        },
        ///        {
        ///            &quot;type&quot;: &quot;http&quot;,
        ///            &quot;name&quot;: &quot;$return&quot;,
        ///            &quot;direction&quot;: &quot;out&quot;
        ///        }
        ///    ]
        ///}.
        /// </summary>
        internal static string DotNetFunctionJson {
            get {
                return ResourceManager.GetString("DotNetFunctionJson", resourceCulture);
            }
        }
    }
}
