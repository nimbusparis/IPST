/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:IPST_GUI.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GalaSoft.MvvmLight;
using IPST_Engine;
using IPST_Engine.Mapping;
using IPST_Engine.Repository;
using Microsoft.Practices.ServiceLocation;
using IPST_GUI.Model;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        private static UnityContainer _unityContainer;
        private static ISessionFactory _sessionFactory;
        private static Configuration _configuration;
        private PortalViewModel _CurrentPortalViewModel;

        static ViewModelLocator()
        {
            _unityContainer = new UnityContainer();
            var unityServiceLocator = new UnityServiceLocator(_unityContainer);
            ServiceLocator.SetLocatorProvider(() =>
            {
                return unityServiceLocator;
            });

            if (ViewModelBase.IsInDesignModeStatic)
            {
                _unityContainer.RegisterType<IDataService, Design.DesignDataService>();
                _unityContainer.RegisterType<IIPSTEngine, Design.DesignIPSTService>();
            }
            else
            {
                _unityContainer.RegisterType<IDataService, DataService>();
                _unityContainer.RegisterType<IIPSTEngine, IPSTEngine>();
                _unityContainer.RegisterType<IPortalSubmissionParser, PortalSubmissionParser>();
                _unityContainer.RegisterType<IPortalSubmissionRepository, PortalSubmissionRepository>();
                _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("IPSTData.db"))
                .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
                .ExposeConfiguration(c => _configuration = c)
                .ExposeConfiguration(c=>new SchemaUpdate(c).Execute(false, true))
                .BuildSessionFactory();

                var session = _sessionFactory.OpenSession();
                _unityContainer.RegisterInstance(session);

            }

            _unityContainer.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<PortalsViewModel>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<ChartViewModel>(new ContainerControlledLifetimeManager());
            
        }

        public ViewModelLocator()
        {
            CurrentPortal = new PortalViewModel();
        }

        public IPortalSubmissionRepository PortalSubmissionRepository
        {
            get { return _unityContainer.Resolve<IPortalSubmissionRepository>(); }
        }
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PortalsViewModel Portals
        {
            get { return ServiceLocator.Current.GetInstance<PortalsViewModel>(); }
        }

        public virtual PortalViewModel CurrentPortal
        {
            get { return _CurrentPortalViewModel; }
            set { _CurrentPortalViewModel = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ChartViewModel Charts
        {
            get { return ServiceLocator.Current.GetInstance<ChartViewModel>(); }
        }
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

