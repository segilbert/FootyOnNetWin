using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using Caliburn.Micro;
using FootyOnNet.Core.Features.Main;

namespace FootyOnNetWin
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App 
    {
        private WinRTContainer container;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        protected override void Configure()
        {
            container = new WinRTContainer();
            container.RegisterWinRTServices();

            //TODO: Register your view models at the container
            container.PerRequest<MainPageViewModel>();

            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViewModels = "FootyOnNet.Core.Features",
                DefaultSubNamespaceForViews = "FootyOnNetWin.Windows.Features"
            };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);  
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies().Concat(new[] { typeof(MainPageViewModel).GetTypeInfo().Assembly });
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;
            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootViewFor<MainPageViewModel>();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}