/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2016 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Caliburn.Micro;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.ExtMethods;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Help;
using Dev2.Common.Interfaces.PopupController;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.Security;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio;
using Dev2.Common.Interfaces.Threading;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Common.Interfaces.ToolBase.ExchangeEmail;
using Dev2.Common.Interfaces.Versioning;
using Dev2.Data.ServiceModel;
using Dev2.Factory;
using Dev2.Helpers;
using Dev2.Instrumentation;
using Dev2.Interfaces;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Security;
using Dev2.Services.Events;
using Dev2.Services.Security;
using Dev2.Settings;
using Dev2.Settings.Scheduler;
using Dev2.Studio.AppResources.Comparers;
using Dev2.Studio.Controller;
using Dev2.Studio.Core.AppResources.Browsers;
using Dev2.Studio.Core.AppResources.DependencyInjection.EqualityComparers;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Core.Helpers;
using Dev2.Studio.Core.InterfaceImplementors;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.Core.Models;
using Dev2.Studio.Core.Utils;
using Dev2.Studio.Core.ViewModels;
using Dev2.Studio.Core.ViewModels.Base;
using Dev2.Studio.Core.Workspaces;
using Dev2.Studio.Factory;
using Dev2.Studio.ViewModels.DependencyVisualization;
using Dev2.Studio.ViewModels.Help;
using Dev2.Studio.ViewModels.Workflow;
using Dev2.Studio.ViewModels.WorkSurface;
using Dev2.Studio.Views.DependencyVisualization;
using Dev2.Threading;
using Dev2.Utils;
using Dev2.ViewModels;
using Dev2.Views.Dialogs;
using Dev2.Workspaces;
using Infragistics.Windows.DockManager.Events;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;
using Resource = Dev2.Runtime.ServiceModel.Data.Resource;
using Dev2.Studio.Core;
// ReSharper disable CatchAllClause
// ReSharper disable InconsistentNaming

// ReSharper disable CheckNamespace
namespace Dev2.Studio.ViewModels
{
    public class MainViewModel : BaseConductor<WorkSurfaceContextViewModel>, IMainViewModel,
                                        IHandle<DeleteResourcesMessage>,
                                        IHandle<DeleteFolderMessage>,
                                        IHandle<ShowDependenciesMessage>,
                                        IHandle<AddWorkSurfaceMessage>,
                                        IHandle<RemoveResourceAndCloseTabMessage>,
                                        IHandle<SaveAllOpenTabsMessage>,
                                        IHandle<ShowReverseDependencyVisualizer>,
                                        IHandle<FileChooserMessage>,
                                        IHandle<DisplayMessageBoxMessage>, IShellViewModel
    {

        private IEnvironmentModel _activeEnvironment;
        private WorkSurfaceContextViewModel _previousActive;
        private bool _disposed;

        private AuthorizeCommand<string> _newServiceCommand;
        private AuthorizeCommand<string> _newPluginSourceCommand;
        private AuthorizeCommand<string> _newDatabaseSourceCommand;
        private AuthorizeCommand<string> _newWebSourceCommand;
        private AuthorizeCommand<string> _newServerSourceCommand;
        private AuthorizeCommand<string> _newEmailSourceCommand;
        private AuthorizeCommand<string> _newExchangeSourceCommand;
        private AuthorizeCommand<string> _newRabbitMQSourceCommand;
        private AuthorizeCommand<string> _newSharepointSourceCommand;
        private AuthorizeCommand<string> _newDropboxSourceCommand;
        private AuthorizeCommand<string> _newWcfSourceCommand;
        private ICommand _deployCommand;
        private ICommand _exitCommand;
        private AuthorizeCommand _settingsCommand;
        private AuthorizeCommand _schedulerCommand;
        private ICommand _showCommunityPageCommand;
        readonly IAsyncWorker _asyncWorker;
        private readonly bool _createDesigners;
        private ICommand _showStartPageCommand;
        bool _hasActiveConnection;
        bool _canDebug = true;
        bool _menuExpanded;

        public Common.Interfaces.Studio.Controller.IPopupController PopupProvider { private get; set; }

        private IEnvironmentRepository EnvironmentRepository { get; }


        public bool CloseCurrent { get; private set; }

        public IExplorerViewModel ExplorerViewModel
        {
            get { return _explorerViewModel; }
            set
            {
                if (_explorerViewModel == value) return;
                _explorerViewModel = value;
                NotifyOfPropertyChange(() => ExplorerViewModel);
            }
        }

        public IServer ActiveServer
        {
            get { return _activeServer; }
            set
            {
                if (value.EnvironmentID != _activeServer.EnvironmentID)
                {
                    _activeServer = value;
                    ExplorerViewModel.ConnectControlViewModel.SelectedConnection = value;
                    NotifyOfPropertyChange(() => ActiveServer);
                }
            }
        }

        public IEnvironmentModel ActiveEnvironment
        {
            get { return _activeEnvironment; }
            set
            {
                if (!Equals(value, _activeEnvironment))
                {
                    _activeEnvironment = value;
                    if (EnvironmentRepository != null)
                    {
                        EnvironmentRepository.ActiveEnvironment = value;
                    }
                    OnActiveEnvironmentChanged();
                    NotifyOfPropertyChange(() => ActiveEnvironment);

                }
            }
        }



        public IBrowserPopupController BrowserPopupController { get; }



        void OnActiveEnvironmentChanged()
        {
            NewDatabaseSourceCommand.UpdateContext(ActiveEnvironment);
            NewPluginSourceCommand.UpdateContext(ActiveEnvironment);
            NewWebSourceCommand.UpdateContext(ActiveEnvironment);
            NewWcfSourceCommand.UpdateContext(ActiveEnvironment);
            NewServerSourceCommand.UpdateContext(ActiveEnvironment);
            NewSharepointSourceCommand.UpdateContext(ActiveEnvironment);
            NewRabbitMQSourceCommand.UpdateContext(ActiveEnvironment);
            NewDropboxSourceCommand.UpdateContext(ActiveEnvironment);
            NewEmailSourceCommand.UpdateContext(ActiveEnvironment);
            NewExchangeSourceCommand.UpdateContext(ActiveEnvironment);
            SettingsCommand.UpdateContext(ActiveEnvironment);
            SchedulerCommand.UpdateContext(ActiveEnvironment);
            DebugCommand.UpdateContext(ActiveEnvironment);
            SaveCommand.UpdateContext(ActiveEnvironment);
        }

        public AuthorizeCommand SaveCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                if (ActiveItem.WorkSurfaceKey.WorkSurfaceContext != WorkSurfaceContext.Workflow)
                {
                    var vm = ActiveItem.WorkSurfaceViewModel as IStudioTab;
                    if (vm != null)
                    {
                        return new AuthorizeCommand(AuthorizationContext.Any, o => vm.DoDeactivate(false), o => vm.IsDirty);
                    }
                }
                return ActiveItem.SaveCommand;
            }
        }

        public AuthorizeCommand DebugCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.DebugCommand;
            }
        }

        public AuthorizeCommand QuickDebugCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.QuickDebugCommand;
            }
        }

        public AuthorizeCommand QuickViewInBrowserCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.QuickViewInBrowserCommand;
            }
        }
        public AuthorizeCommand ViewInBrowserCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.ViewInBrowserCommand;
            }
        }

        public ICommand ShowStartPageCommand
        {
            get
            {
                return _showStartPageCommand ?? (_showStartPageCommand = new DelegateCommand(param => ShowStartPage()));
            }
        }

        public ICommand ShowCommunityPageCommand
        {
            get { return _showCommunityPageCommand ?? (_showCommunityPageCommand = new DelegateCommand(param => ShowCommunityPage())); }
        }

        public AuthorizeCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand =
                    new AuthorizeCommand(AuthorizationContext.Administrator, param => AddSettingsWorkSurface(), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand SchedulerCommand
        {
            get
            {
                return _schedulerCommand ?? (_schedulerCommand =
                    new AuthorizeCommand(AuthorizationContext.Administrator, param => AddSchedulerWorkSurface(), param => IsActiveEnvironmentConnected()));
            }
        }




        public AuthorizeCommand<string> NewServiceCommand
        {
            get
            {
                return _newServiceCommand ?? (_newServiceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewService(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewPluginSourceCommand
        {
            get
            {
                return _newPluginSourceCommand ?? (_newPluginSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewPluginSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewDatabaseSourceCommand
        {
            get
            {
                return _newDatabaseSourceCommand ?? (_newDatabaseSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewDatabaseSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewWebSourceCommand
        {
            get
            {
                return _newWebSourceCommand ?? (_newWebSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewWebSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewServerSourceCommand
        {
            get
            {
                return _newServerSourceCommand ?? (_newServerSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewServerSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewEmailSourceCommand
        {
            get
            {
                return _newEmailSourceCommand ?? (_newEmailSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewEmailSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }


        public AuthorizeCommand<string> NewExchangeSourceCommand
        {
            get
            {
                return _newExchangeSourceCommand ?? (_newExchangeSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewExchangeSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewRabbitMQSourceCommand
        {
            get
            {
                return _newRabbitMQSourceCommand ?? (_newRabbitMQSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewRabbitMQSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewSharepointSourceCommand
        {
            get
            {
                return _newSharepointSourceCommand ?? (_newSharepointSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewSharepointSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewDropboxSourceCommand
        {
            get
            {
                return _newDropboxSourceCommand ?? (_newDropboxSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewDropboxSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand<string> NewWcfSourceCommand
        {
            get
            {
                return _newWcfSourceCommand ?? (_newWcfSourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, param => NewWcfSource(@""), param => IsActiveEnvironmentConnected()));
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand ??
                       (_exitCommand =
                        new RelayCommand(param =>
                                         Application.Current.Shutdown(), param => true));
            }
        }

        public ICommand DeployCommand
        {
            get
            {
                return _deployCommand ??
                       (_deployCommand = new RelayCommand(param => AddDeploySurface(new List<IExplorerTreeItem>())));
            }
        }




        public IVersionChecker Version { get; }

        public bool HasActiveConnection
        {
            get
            {
                return _hasActiveConnection;
            }
            private set
            {
                _hasActiveConnection = value;
                NotifyOfPropertyChange(() => HasActiveConnection);
            }
        }

        [ExcludeFromCodeCoverage]
        public MainViewModel()
            : this(EventPublishers.Aggregator, new AsyncWorker(), Core.EnvironmentRepository.Instance, new VersionChecker())
        {
        }

        public MainViewModel(IEventAggregator eventPublisher, IAsyncWorker asyncWorker, IEnvironmentRepository environmentRepository,
            IVersionChecker versionChecker, bool createDesigners = true, IBrowserPopupController browserPopupController = null,
            Common.Interfaces.Studio.Controller.IPopupController popupController = null, IExplorerViewModel explorer = null)
            : base(eventPublisher)
        {
            if (environmentRepository == null)
            {
                throw new ArgumentNullException(nameof(environmentRepository));
            }

            if (versionChecker == null)
            {
                throw new ArgumentNullException(nameof(versionChecker));
            }
            Version = versionChecker;

            VerifyArgument.IsNotNull(@"asyncWorker", asyncWorker);
            _asyncWorker = asyncWorker;
            _createDesigners = createDesigners;
            BrowserPopupController = browserPopupController ?? new ExternalBrowserPopupController();
            PopupProvider = popupController ?? new PopupController();
            _activeServer = LocalhostServer;

            EnvironmentRepository = environmentRepository;
            SetActiveEnvironment(_activeServer.EnvironmentID);

            MenuPanelWidth = 60;
            _menuExpanded = false;

            ExplorerViewModel = explorer ?? new ExplorerViewModel(this, CustomContainer.Get<Microsoft.Practices.Prism.PubSubEvents.IEventAggregator>());
            
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            AddWorkspaceItems();
            ShowStartPage();
            DisplayName = @"Warewolf" + $" ({ClaimsPrincipal.Current.Identity.Name})".ToUpperInvariant();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

        }
        
        public void Handle(ShowReverseDependencyVisualizer message)
        {
            Dev2Logger.Debug(message.GetType().Name);
            if (message.Model != null)
            {
                AddReverseDependencyVisualizerWorkSurface(message.Model);
            }
        }

        public void Handle(SaveAllOpenTabsMessage message)
        {
            Dev2Logger.Debug(message.GetType().Name);
            PersistTabs();
        }


        public void Handle(AddWorkSurfaceMessage message)
        {
            Dev2Logger.Info(message.GetType().Name);
            AddWorkSurface(message.WorkSurfaceObject);

            if (message.ShowDebugWindowOnLoad)
            {
                if (ActiveItem != null && _canDebug)
                {
                    ActiveItem.DebugCommand.Execute(null);
                }
            }
        }

        public void Handle(DeleteResourcesMessage message)
        {
            Dev2Logger.Info(message.GetType().Name);
            DeleteResources(message.ResourceModels, message.FolderName, message.ShowDialog, message.ActionToDoOnDelete);
        }

        public void Handle(DeleteFolderMessage message)
        {
            Dev2Logger.Info(message.GetType().Name);
            if (ShowDeleteDialogForFolder(message.FolderName))
            {
                message.ActionToDoOnDelete?.Invoke();
            }
        }


        public void SetActiveEnvironment(IEnvironmentModel activeEnvironment)
        {
            ActiveEnvironment = activeEnvironment;
            EnvironmentRepository.ActiveEnvironment = ActiveEnvironment;
            SetActiveEnvironment(activeEnvironment.ID);
            ActiveEnvironment.AuthorizationServiceSet += (sender, args) => OnActiveEnvironmentChanged();
        }

        public void Handle(ShowDependenciesMessage message)
        {
            Dev2Logger.Info(message.GetType().Name);
            var model = message.ResourceModel;
            var dependsOnMe = message.ShowDependentOnMe;
            ShowDependencies(dependsOnMe, model, ActiveServer);
        }

        public void ShowDependencies(Guid resourceId, IServer server)
        {
            var environmentModel = EnvironmentRepository.Get(server.EnvironmentID);
            if (environmentModel != null)
            {
                environmentModel.ResourceRepository.LoadResourceFromWorkspace(resourceId, Guid.Empty);
                var resource = environmentModel.ResourceRepository.FindSingle(model => model.ID == resourceId, true);
                var contextualResourceModel = new ResourceModel(environmentModel, EventPublisher);
                contextualResourceModel.Update(resource);
                contextualResourceModel.ID = resourceId;
                ShowDependencies(true, contextualResourceModel, server);
            }
        }

        void ShowDependencies(bool dependsOnMe, IContextualResourceModel model, IServer server)
        {
            var vm = new DependencyVisualiserViewModel(new DependencyVisualiserView(), server, dependsOnMe)
            {
                ResourceType = @"DependencyViewer",
                ResourceModel = model
            };

            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DependencyVisualiser);
            workSurfaceKey.EnvironmentID = model.Environment.ID;
            workSurfaceKey.ResourceID = model.ID;
            workSurfaceKey.ServerID = model.ServerID;

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void Handle(RemoveResourceAndCloseTabMessage message)
        {
            Dev2Logger.Debug(message.GetType().Name);
            if (message.ResourceToRemove == null)
            {
                return;
            }

            var wfscvm = FindWorkSurfaceContextViewModel(message.ResourceToRemove);
            if (message.RemoveFromWorkspace)
            {
                DeactivateItem(wfscvm, true);
            }
            else
            {
                base.DeactivateItem(wfscvm, true);
            }

            _previousActive = null;

        }


        public IContextualResourceModel DeployResource { get; set; }
        public void RefreshActiveEnvironment()
        {
            if (ActiveItem?.Environment != null)
            {
               SetActiveEnvironment(ActiveItem.Environment);
            }
        }
        
        private void TempSave(IEnvironmentModel activeEnvironment, string resourcePath)
        {
            string newWorflowName = NewWorkflowNames.Instance.GetNext();

            IContextualResourceModel tempResource = ResourceModelFactory.CreateResourceModel(activeEnvironment, @"WorkflowService",
                                                                                              newWorflowName);
            tempResource.Category = string.IsNullOrEmpty(resourcePath) ? @"Unassigned\\" + newWorflowName : resourcePath + @"\\" + newWorflowName;
            tempResource.ResourceName = newWorflowName;
            tempResource.DisplayName = newWorflowName;
            tempResource.IsNewWorkflow = true;

            AddAndActivateWorkSurface(WorkSurfaceContextFactory.CreateResourceViewModel(tempResource));
            AddWorkspaceItem(tempResource);
        }

        public void ShowAboutBox()
        {
            var splashViewModel = new SplashViewModel(ActiveServer, new ExternalProcessExecutor());

            SplashPage splashPage = new SplashPage { DataContext = splashViewModel };
            ISplashView splashView = splashPage;
            splashViewModel.ShowServerVersion();
            // Show it 
            splashView.Show(true);
        }


        private bool ShowRemovePopup(IWorkflowDesignerViewModel workflowVm)
        {
            var result = PopupProvider.Show(string.Format(StringResources.DialogBody_NotSaved, workflowVm.ResourceModel.ResourceName),
                $"Save {workflowVm.ResourceModel.ResourceName}?",
                                            MessageBoxButton.YesNoCancel,
                                            MessageBoxImage.Information, @"", false, false, true, false);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    workflowVm.ResourceModel.Commit();
                    Dev2Logger.Info(@"Publish message of type - " + typeof(SaveResourceMessage));
                    EventPublisher.Publish(new SaveResourceMessage(workflowVm.ResourceModel, false, false));

                    return !workflowVm.WorkflowName.ToLower().StartsWith(@"unsaved");
                case MessageBoxResult.No:
                    // We need to remove it ;)
                    var model = workflowVm.ResourceModel;
                    try
                    {
                        if (workflowVm.EnvironmentModel.ResourceRepository.DoesResourceExistInRepo(model) && workflowVm.ResourceModel.IsNewWorkflow)
                        {
                            DeleteResources(new List<IContextualResourceModel> { model }, @"", false);
                        }
                        else
                        {
                            model.Rollback();
                        }
                    }
                    catch (Exception e)
                    {
                        Dev2Logger.Info(@"Exception: " + e.Message);
                    }

                    NewWorkflowNames.Instance.Remove(workflowVm.ResourceModel.ResourceName);
                    return true;
                default:
                    return false;
            }
        }

        private void DisplayResourceWizard(IContextualResourceModel resourceModel, bool isedit)
        {
            if (resourceModel == null)
            {
                return;
            }

            if (isedit && resourceModel.ServerResourceType == ResourceType.WorkflowService.ToString())
            {
                PersistTabs();
            }

            if (resourceModel.WorkflowXaml == null)
            {
                resourceModel.Environment.ResourceRepository.ReloadResource(resourceModel.ID, resourceModel.ResourceType, ResourceModelEqualityComparer.Current, true);
            }

            switch (resourceModel.ServerResourceType)
            {
                case "SqlDatabase":
                case "ODBC":
                case "Oracle":
                case "PostgreSql":
                case "MySqlDatabase":
                    EditDbSource(resourceModel);
                    break;
                case "DbSource":
                    EditDbSource(resourceModel);
                    break;
                case "WebSource":
                    EditWebSource(resourceModel);
                    break;
                case "PluginSource":
                    EditPluginSource(resourceModel);
                    break;
                case "WcfSource":
                    EditWcfSource(resourceModel);
                    break;
                case "EmailSource":
                    EditEmailSource(resourceModel);
                    break;
                case "ExchangeSource":
                    EditExchangeSource(resourceModel);
                    break;
                case "DropBoxSource":
                    EditDropBoxSource(resourceModel);
                    break;
                case "SharepointServerSource":
                    EditSharePointSource(resourceModel);
                    break;
                case "OauthSource":
                    EditDropBoxSource(resourceModel);
                    break;
                case "RabbitMQSource":
                    EditRabbitMQSource(resourceModel);
                    break;
                case "Server":
                case "Dev2Server":
                case "ServerSource":                   
                    EditServer(resourceModel);
                    break;
                default:
                    AddWorkSurfaceContext(resourceModel);
                    break;
            }
        }

        void EditDbSource(IContextualResourceModel resourceModel)
        {
            var db = new DbSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new DbSourceDefinition
            {
                AuthenticationType = db.AuthenticationType,
                DbName = db.DatabaseName,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                ServerName = db.Server,
                Type = db.ServerType,
                UserName = db.UserID
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }
        void EditPluginSource(IContextualResourceModel resourceModel)
        {
            var db = new PluginSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new PluginSourceDefinition
            {
                SelectedDll = new DllListing { FullName = db.AssemblyLocation, Name = db.AssemblyName, Children = new Collection<IFileListing>(), IsDirectory = false },
                Id = db.ResourceID,
                Name = db.ResourceName,
                Path = db.ResourcePath
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.PluginSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }
        void EditWebSource(IContextualResourceModel resourceModel)
        {
            var db = new WebSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new WebServiceSourceDefinition()
            {
                AuthenticationType = db.AuthenticationType,
                DefaultQuery = db.DefaultQuery,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                HostName = db.Address,

                UserName = db.UserName
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }
        void EditSharePointSource(IContextualResourceModel resourceModel)
        {
            var db = new SharepointSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new SharePointServiceSourceDefinition
            {
                AuthenticationType = db.AuthenticationType,
                Server = db.Server,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                UserName = db.UserName
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.SharepointServerSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        void EditEmailSource(IContextualResourceModel resourceModel)
        {
            var db = new EmailSource(resourceModel.WorkflowXaml.ToXElement());

            var def = new EmailServiceSourceDefinition
            {
                Id = db.ResourceID,
                HostName = db.Host,
                Password = db.Password,
                UserName = db.UserName,
                Port = db.Port,
                Timeout = db.Timeout,
                ResourceName = db.ResourceName,
                EnableSsl = db.EnableSsl
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.EmailSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        void EditExchangeSource(IContextualResourceModel resourceModel)
        {
            var db = new ExchangeSource(resourceModel.WorkflowXaml.ToXElement());

            var def = new ExchangeSourceDefinition()
            {
                AutoDiscoverUrl = db.AutoDiscoverUrl,
                Id = db.ResourceID,
                Password = db.Password,
                UserName = db.UserName,
                Timeout = db.Timeout,
                ResourceName = db.ResourceName,
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.Exchange);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        void EditDropBoxSource(IContextualResourceModel resourceModel)
        {
            var db = new DropBoxSource(resourceModel.WorkflowXaml.ToXElement());

            var def = new DropBoxSource
            {
                AccessToken = db.AccessToken,
                ResourceID = db.ResourceID,
                AppKey = db.AppKey,
                ResourceName = db.ResourceName,
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.Exchange);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        // ReSharper disable once InconsistentNaming
        private void EditRabbitMQSource(IContextualResourceModel resourceModel)
        {
            var source = new RabbitMQSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new RabbitMQServiceSourceDefinition
            {
                ResourceID = source.ResourceID,
                ResourceName = source.ResourceName,
                ResourcePath = source.ResourcePath,
                HostName = source.HostName,
                Port = source.Port,
                UserName = source.UserName,
                Password = source.Password,
                VirtualHost = source.VirtualHost
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.RabbitMQSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        void EditWcfSource(IContextualResourceModel resourceModel)
        {
            var wcfsource = new WcfSource(resourceModel.WorkflowXaml.ToXElement());

            var def = new WcfServiceSourceDefinition
            {
                Id = wcfsource.Id,
                Name = wcfsource.ResourceName,
                Path = wcfsource.Path,
                ResourceName = wcfsource.Name,
                EndpointUrl = wcfsource.EndpointUrl
            };
            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WcfSource);
            workSurfaceKey.EnvironmentID = resourceModel.Environment.ID;
            workSurfaceKey.ResourceID = resourceModel.ID;
            workSurfaceKey.ServerID = resourceModel.ServerID;
            EditResource(def, workSurfaceKey);
        }

        public string OpenPasteWindow(string current)
        {
            var pasteView = new ManageWebservicePasteView();
            return pasteView.ShowView(current);
        }

        public IServer LocalhostServer => CustomContainer.Get<IServer>();

        public void SetActiveEnvironment(Guid environmentId)
        {
            var environmentModel = EnvironmentRepository.Get(environmentId);
            ActiveEnvironment = environmentModel != null && (environmentModel.IsConnected || environmentModel.IsLocalHost) ? environmentModel : EnvironmentRepository.Get(Guid.Empty);
            var server = ExplorerViewModel?.ConnectControlViewModel?.Servers?.FirstOrDefault(a => a.EnvironmentID == environmentId);
            if (server != null)
            {
                SetActiveServer(server);
            }
        }

        public void SetActiveServer(IServer server)
        {
            if (server.IsConnected)
            {
                ActiveServer = server;
            }
        }

        public void Debug()
        {
            ActiveItem.DebugCommand.Execute(null);
        }

        public void OpenResource(Guid resourceId, IServer server)
        {
            OpenResource(resourceId,server.EnvironmentID);
        }
        public void OpenResource(Guid resourceId, Guid environmentId)
        {
            var environmentModel = EnvironmentRepository.Get(environmentId);
            if (environmentModel != null)
            {
                var contextualResourceModel = environmentModel.ResourceRepository.LoadContextualResourceModel(resourceId);
                DisplayResourceWizard(contextualResourceModel, true);
            }
        }

        public void CloseResource(Guid resourceId, Guid environmentId)
        {
            var environmentModel = EnvironmentRepository.Get(environmentId);
            var contextualResourceModel = environmentModel?.ResourceRepository.LoadContextualResourceModel(resourceId);
            if (contextualResourceModel != null)
            {
                var wfscvm = FindWorkSurfaceContextViewModel(contextualResourceModel);
                DeactivateItem(wfscvm, true);
            }
        }

        public async void OpenResourceAsync(Guid resourceId, IServer server)
        {
            var environmentModel = EnvironmentRepository.Get(server.EnvironmentID);
            if (environmentModel != null)
            {
                var contextualResourceModel = await environmentModel.ResourceRepository.LoadContextualResourceModelAsync(resourceId);
                var wfscvm = FindWorkSurfaceContextViewModel(contextualResourceModel);
                DeactivateItem(wfscvm, true);
            }
        }

        public void DeployResources(Guid sourceEnvironmentId, Guid destinationEnvironmentId, IList<Guid> resources)
        {
            var environmentModel = EnvironmentRepository.Get(destinationEnvironmentId);
            var sourceEnvironmentModel = EnvironmentRepository.Get(sourceEnvironmentId);
            var dto = new DeployDto { ResourceModels = resources.Select(a => sourceEnvironmentModel.ResourceRepository.LoadContextualResourceModel(a) as IResourceModel).ToList() };
            environmentModel.ResourceRepository.DeployResources(sourceEnvironmentModel, environmentModel, dto);
            ExplorerViewModel.RefreshEnvironment(destinationEnvironmentId);
        }

        public void ShowPopup(IPopupMessage popupMessage)
        {
            PopupProvider.Show(popupMessage.Description, popupMessage.Header, popupMessage.Buttons, MessageBoxImage.Error, @"", false, true, false, false);
        }

        public void EditServer(IContextualResourceModel resourceModel)
        {
            var connection = new Connection(resourceModel.WorkflowXaml.ToXElement());
            string address = null;
            Uri uri;
            if (Uri.TryCreate(connection.Address, UriKind.RelativeOrAbsolute, out uri))
            {
                address = uri.Host;
            }

            var selectedServer = new ServerSource
            {
                Address = connection.Address,
                ID = connection.ResourceID,
                AuthenticationType = connection.AuthenticationType,
                UserName = connection.UserName,
                Password = connection.Password,
                ServerName = address,
                Name = connection.ResourceName,
                ResourcePath = connection.ResourcePath
            };

            EditServer(selectedServer);
        }

        public void EditServer(IServerSource selectedServer)
        {
            var viewModel = new ManageNewServerViewModel(new ManageNewServerSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, @""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedServer, _asyncWorker, new ExternalProcessExecutor());
            var vm = new SourceViewModel<IServerSource>(EventPublisher, viewModel, PopupProvider, new ManageServerControl());

            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource);
            workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
            workSurfaceKey.ResourceID = selectedServer.ID;
            workSurfaceKey.ServerID = ActiveServer.ServerID;
            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);

            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IDbSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var dbSourceViewModel = new ManageDatabaseSourceViewModel(new ManageDatabaseSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker);
            var vm = new SourceViewModel<IDbSource>(EventPublisher, dbSourceViewModel, PopupProvider, new ManageDatabaseSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.Id;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IPluginSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var pluginSourceViewModel = new ManagePluginSourceViewModel(new ManagePluginSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker);
            var vm = new SourceViewModel<IPluginSource>(EventPublisher, pluginSourceViewModel, PopupProvider, new ManagePluginSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.Id;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IWebServiceSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var viewModel = new ManageWebserviceSourceViewModel(new ManageWebServiceSourceModel(ActiveServer.UpdateRepository, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker, new ExternalProcessExecutor());
            var vm = new SourceViewModel<IWebServiceSource>(EventPublisher, viewModel, PopupProvider, new ManageWebserviceSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.Id;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IEmailServiceSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var emailSourceViewModel = new ManageEmailSourceViewModel(new ManageEmailSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource);
            var vm = new SourceViewModel<IEmailServiceSource>(EventPublisher, emailSourceViewModel, PopupProvider, new ManageEmailSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.Id;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IExchangeSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var emailSourceViewModel = new ManageExchangeSourceViewModel(new ManageExchangeSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource);
            var vm = new SourceViewModel<IExchangeSource>(EventPublisher, emailSourceViewModel, PopupProvider, new ManageExchangeSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.ResourceID;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        private void EditResource(IOAuthSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var oauthSourceViewModel = new ManageOAuthSourceViewModel(new ManageOAuthSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ""), selectedSource);
            var vm = new SourceViewModel<IOAuthSource>(EventPublisher, oauthSourceViewModel, PopupProvider, new ManageOAuthSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.ResourceID;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IRabbitMQServiceSourceDefinition selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var viewModel = new ManageRabbitMQSourceViewModel(new ManageRabbitMQSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, this), selectedSource);
            var vm = new SourceViewModel<IRabbitMQServiceSourceDefinition>(EventPublisher, viewModel, PopupProvider, new ManageRabbitMQSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.ResourceID;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(ISharepointServerSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var viewModel = new SharepointServerSourceViewModel(new SharepointServerSourceModel(ActiveServer.UpdateRepository, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker, null);
            var vm = new SourceViewModel<ISharepointServerSource>(EventPublisher, viewModel, PopupProvider, new SharepointServerSource());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.Id;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IWcfServerSource selectedSource, IWorkSurfaceKey workSurfaceKey = null)
        {
            var wcfSourceViewModel = new ManageWcfSourceViewModel(new ManageWcfSourceModel(ActiveServer.UpdateRepository, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker, _activeEnvironment);
            var vm = new SourceViewModel<IWcfServerSource>(EventPublisher, wcfSourceViewModel, PopupProvider, new ManageWcfSourceControl());

            if (workSurfaceKey == null)
            {
                workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
                workSurfaceKey.EnvironmentID = ActiveServer.EnvironmentID;
                workSurfaceKey.ResourceID = selectedSource.ResourceID;
                workSurfaceKey.ServerID = ActiveServer.ServerID;
            }

            var key = workSurfaceKey as WorkSurfaceKey;
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, vm);
            OpeningWorkflowsHelper.AddWorkflow(key);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }


        public void NewService(string resourcePath)
        {
             TempSave(ActiveEnvironment, resourcePath);               
        }

        private async Task<IRequestServiceNameViewModel> GetSaveViewModel(string resourcePath, string header)
        {
            return await RequestServiceNameViewModel.CreateAsync(new EnvironmentViewModel(ActiveServer, this), resourcePath, header);
        }

        public void NewServerSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.ServerSourceNewHeaderLabel);
            var key = (WorkSurfaceKey)WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource);
            key.ServerID = ActiveServer.ServerID;
            // ReSharper disable once PossibleInvalidOperationException
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, new SourceViewModel<IServerSource>(EventPublisher, new ManageNewServerViewModel(new ManageNewServerSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, new ExternalProcessExecutor()) { SelectedGuid = key.ResourceID.Value }, PopupProvider, new ManageServerControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewDatabaseSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.DatabaseSourceServerNewHeaderLabel);
            var key = (WorkSurfaceKey)WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource);
            key.ServerID = ActiveServer.ServerID;
            // ReSharper disable once PossibleInvalidOperationException
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, new SourceViewModel<IDbSource>(EventPublisher, new ManageDatabaseSourceViewModel(new ManageDatabaseSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker) { SelectedGuid = key.ResourceID.Value }, PopupProvider, new ManageDatabaseSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewWebSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.WebserviceNewHeaderLabel);
            var key = (WorkSurfaceKey)WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource);
            key.ServerID = ActiveServer.ServerID;
            // ReSharper disable once PossibleInvalidOperationException
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, new SourceViewModel<IWebServiceSource>(EventPublisher, new ManageWebserviceSourceViewModel(new ManageWebServiceSourceModel(ActiveServer.UpdateRepository, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, new ExternalProcessExecutor()) { SelectedGuid = key.ResourceID.Value }, PopupProvider, new ManageWebserviceSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewPluginSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.PluginSourceNewHeaderLabel);
            var key = (WorkSurfaceKey)WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.PluginSource);
            key.ServerID = ActiveServer.ServerID;
            // ReSharper disable once PossibleInvalidOperationException
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, new SourceViewModel<IPluginSource>(EventPublisher, new ManagePluginSourceViewModel(new ManagePluginSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker) { SelectedGuid = key.ResourceID.Value }, PopupProvider, new ManagePluginSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewWcfSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.WcfServiceNewHeaderLabel);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WcfSource) as WorkSurfaceKey, new SourceViewModel<IWcfServerSource>(EventPublisher, new ManageWcfSourceViewModel(new ManageWcfSourceModel(ActiveServer.UpdateRepository, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, ActiveEnvironment), PopupProvider, new ManageWcfSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewDropboxSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.DropboxSourceNewHeaderLabel);
            var key = (WorkSurfaceKey)WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.OAuthSource);
            key.ServerID = ActiveServer.ServerID;

            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(key, new SourceViewModel<IOAuthSource>(EventPublisher, new ManageOAuthSourceViewModel(new ManageOAuthSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel), PopupProvider, new ManageOAuthSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewRabbitMQSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.RabbitMQSourceNewHeaderLabel);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.RabbitMQSource) as WorkSurfaceKey, new SourceViewModel<IRabbitMQServiceSourceDefinition>(EventPublisher, new ManageRabbitMQSourceViewModel(new ManageRabbitMQSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, this), saveViewModel), PopupProvider, new ManageRabbitMQSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewSharepointSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.SharePointServiceNewHeaderLabel);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.SharepointServerSource) as WorkSurfaceKey, new SourceViewModel<ISharepointServerSource>(EventPublisher, new SharepointServerSourceViewModel(new SharepointServerSourceModel(ActiveServer.UpdateRepository, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, ActiveEnvironment), PopupProvider, new SharepointServerSource()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void AddDeploySurface(IEnumerable<IExplorerTreeItem> items)
        {
            var dep = ActivateOrCreateUniqueWorkSurface<DeployWorksurfaceViewModel>(WorkSurfaceContext.DeployViewer);
            if (dep != null)
            {

                dep.ViewModel.Source.Preselected = new ObservableCollection<IExplorerTreeItem>(items);

            }
        }

        public void OpenVersion(Guid resourceId, IVersionInfo versionInfo)
        {
            var workflowXaml = ActiveServer?.ProxyLayer?.GetVersion(versionInfo);
            if (workflowXaml != null)
            {
                IResourceModel resourceModel =
                    ActiveEnvironment.ResourceRepository.LoadContextualResourceModel(resourceId);

                var xamlElement = XElement.Parse(workflowXaml.ToString());
                var dataList = xamlElement.Element(@"DataList");
                var dataListString = string.Empty;
                if (dataList != null)
                {
                    dataListString = dataList.ToString();
                }
                var action = xamlElement.Element(@"Action");
                var xamlString = string.Empty;
                var xaml = action?.Element(@"XamlDefinition");
                if (xaml != null)
                {
                    xamlString = xaml.Value;
                }

                var resource = new Resource(workflowXaml.ToXElement());
                if (resource.VersionInfo != null)
                {
                    versionInfo.User = resource.VersionInfo.User;
                }

                var resourceVersion = new ResourceModel(ActiveEnvironment, EventPublishers.Aggregator)
                {
                    ResourceType = resourceModel.ResourceType,
                    ResourceName = $"{resource.ResourceName} (v.{versionInfo.VersionNumber})",
                    WorkflowXaml = new StringBuilder(xamlString),
                    UserPermissions = Permissions.View,
                    DataList = dataListString,
                    IsVersionResource = true,
                    ID = Guid.NewGuid()
                };

                DisplayResourceWizard(resourceVersion, true);
            }
        }

        public async void ShowStartPage()
        {

            ActivateOrCreateUniqueWorkSurface<HelpViewModel>(WorkSurfaceContext.StartPage);
            WorkSurfaceContextViewModel workSurfaceContextViewModel = Items.FirstOrDefault(c => c.WorkSurfaceViewModel.DisplayName == "Start Page" && c.WorkSurfaceViewModel.GetType() == typeof(HelpViewModel));
            if (workSurfaceContextViewModel != null)
            {
                await ((HelpViewModel)workSurfaceContextViewModel.WorkSurfaceViewModel).LoadBrowserUri(Version.CommunityPageUri);
            }
        }

        public void ShowCommunityPage()
        {
            BrowserPopupController.ShowPopup(StringResources.Uri_Community_HomePage);
        }
      
        public bool IsActiveEnvironmentConnected()
        {
            if (ActiveEnvironment == null)
            {
                return false;
            }

            HasActiveConnection = ActiveItem != null && ActiveItem.IsEnvironmentConnected();
            return ActiveEnvironment != null && ActiveEnvironment.IsConnected && ActiveEnvironment.CanStudioExecute;
        }

        void AddReverseDependencyVisualizerWorkSurface(IContextualResourceModel resource)
        {
            if (resource == null)
                return;
            ShowDependencies(true, resource, ActiveServer);
        }

        void AddSettingsWorkSurface()
        {
            ActivateOrCreateUniqueWorkSurface<SettingsViewModel>(WorkSurfaceContext.Settings);
        }

        void AddSchedulerWorkSurface()
        {
            ActivateOrCreateUniqueWorkSurface<SchedulerViewModel>(WorkSurfaceContext.Scheduler);
        }

        public void NewEmailSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.EmailSourceNewHeaderLabel);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.EmailSource) as WorkSurfaceKey, new SourceViewModel<IEmailServiceSource>(EventPublisher, new ManageEmailSourceViewModel(new ManageEmailSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator()), PopupProvider, new ManageEmailSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);

        }

        public void NewExchangeSource(string resourcePath)
        {
            Task<IRequestServiceNameViewModel> saveViewModel = GetSaveViewModel(resourcePath, Warewolf.Studio.Resources.Languages.Core.EmailSourceNewHeaderLabel);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.Exchange) as WorkSurfaceKey, new SourceViewModel<IExchangeSource>(EventPublisher, new ManageExchangeSourceViewModel(new ManageExchangeSourceModel(ActiveServer.UpdateRepository, ActiveServer.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator()), PopupProvider, new ManageExchangeSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }


        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    OnDeactivate(true);
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }


        protected override void ChangeActiveItem(WorkSurfaceContextViewModel newItem, bool closePrevious)
        {
            if (_previousActive != null)
            {
                if (newItem?.DataListViewModel != null)
                {
                    string errors;
                    newItem.DataListViewModel.ClearCollections();
                    newItem.DataListViewModel.CreateListsOfIDataListItemModelToBindTo(out errors);
                }
            }
            base.ChangeActiveItem(newItem, closePrevious);
            RefreshActiveEnvironment();
        }


        public override void DeactivateItem(WorkSurfaceContextViewModel item, bool close)
        {
            if (item == null)
            {
                return;
            }

            bool success = true;
            if (close)
            {
                success = CloseWorkSurfaceContext(item, null);
            }

            if (success)
            {
                if (_previousActive != item && Items.Contains(_previousActive))
                {
                    ActivateItem(_previousActive);
                }

                base.DeactivateItem(item, close);
                item.Dispose();
                CloseCurrent = true;
            }
            else
            {
                CloseCurrent = false;
            }
        }


        // Process saving tabs and such when exiting ;)
        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                PersistTabs();
            }

            base.OnDeactivate(close);
        }

        protected override void OnActivationProcessed(WorkSurfaceContextViewModel item, bool success)
        {
            if (success)
            {
                var wfItem = item?.WorkSurfaceViewModel as IWorkflowDesignerViewModel;
                if (wfItem != null)
                {
                    AddWorkspaceItem(wfItem.ResourceModel);
                }
                NotifyOfPropertyChange(() => SaveCommand);
                NotifyOfPropertyChange(() => DebugCommand);
                NotifyOfPropertyChange(() => QuickDebugCommand);
                NotifyOfPropertyChange(() => QuickViewInBrowserCommand);
                NotifyOfPropertyChange(() => ViewInBrowserCommand);
                if (MenuViewModel != null)
                {
                    MenuViewModel.SaveCommand = SaveCommand;
                    MenuViewModel.ExecuteServiceCommand = DebugCommand;
                }
            }
            base.OnActivationProcessed(item, success);
        }

        public ICommand SaveAllCommand => new DelegateCommand(SaveAll);

        void SaveAll(object obj)
        {
            for (int index = Items.Count - 1; index >= 0; index--)
            {
                var workSurfaceContextViewModel = Items[index];
                ActivateItem(workSurfaceContextViewModel);
                var workSurfaceContext = workSurfaceContextViewModel.WorkSurfaceKey.WorkSurfaceContext;
                if (workSurfaceContext == WorkSurfaceContext.Workflow)
                {
                    if (workSurfaceContextViewModel.CanSave())
                    {
                        workSurfaceContextViewModel.Save();
                    }
                }
                else
                {
                    var vm = workSurfaceContextViewModel.WorkSurfaceViewModel;
                    var viewModel = vm as IStudioTab;
                    viewModel?.DoDeactivate(true);
                }
            }
        }
        
        public void UpdateCurrentDataListWithObjectFromJson(string parentObjectName,string json)
        {
            ActiveItem?.DataListViewModel?.GenerateComplexObjectFromJson(parentObjectName, json);
        }

        public override void ActivateItem(WorkSurfaceContextViewModel item)
        {
            _previousActive = ActiveItem;
            if (_previousActive?.DebugOutputViewModel != null)
            {
                _previousActive.DebugOutputViewModel.PropertyChanged -= DebugOutputViewModelOnPropertyChanged;
            }
            base.ActivateItem(item);
            if (item?.ContextualResourceModel == null) return;
            if (item.DebugOutputViewModel != null)
            {
                item.DebugOutputViewModel.PropertyChanged += DebugOutputViewModelOnPropertyChanged;
            }
            SetActiveEnvironment(item.Environment);            
        }

        void DebugOutputViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsProcessing")
            {
                if (MenuViewModel != null)
                {
                    if (ActiveItem.DebugOutputViewModel != null)
                    {
                        MenuViewModel.IsProcessing = ActiveItem.DebugOutputViewModel.IsProcessing;
                    }
                }
            }
        }

        private bool ConfirmDeleteAfterDependencies(ICollection<IContextualResourceModel> models)
        {
            if (!models.Any(model => model.Environment.ResourceRepository.HasDependencies(model)))
            {
                return true;
            }

            if (models.Count >= 1)
            {
                var model = models.FirstOrDefault();
                if (model != null)
                {
                    var result = PopupProvider.Show(string.Format(StringResources.DialogBody_HasDependencies, model.ResourceName, model.ResourceType.GetDescription()),
                                                    string.Format(StringResources.DialogTitle_HasDependencies, model.ResourceType.GetDescription()),
                                                    MessageBoxButton.OK, MessageBoxImage.Error, "", true, true, false, false);

                    if (result != MessageBoxResult.OK)
                    {
                        ShowDependencies(false, model, ActiveServer);
                    }
                }
                return false;
            }
            return true;
        }

        private bool ConfirmDelete(ICollection<IContextualResourceModel> models, string folderName)
        {
            bool confirmDeleteAfterDependencies = ConfirmDeleteAfterDependencies(models);
            if (confirmDeleteAfterDependencies)
            {
                Common.Interfaces.Studio.Controller.IPopupController result = new PopupController();
                if (models.Count > 1)
                {
                    var contextualResourceModel = models.FirstOrDefault();
                    if (contextualResourceModel != null)
                    {
                        var folderBeingDeleted = folderName;
                        return ShowDeleteDialogForFolder(folderBeingDeleted);
                    }
                }
                if (models.Count == 1)
                {
                    var contextualResourceModel = models.FirstOrDefault();
                    if (contextualResourceModel != null)
                    {
                        var deletionName = folderName;
                        var description = "";
                        if (string.IsNullOrEmpty(deletionName))
                        {
                            deletionName = contextualResourceModel.ResourceName;
                            description = contextualResourceModel.ResourceType.GetDescription();
                        }

                        var shouldDelete = PopupProvider.Show(string.Format(StringResources.DialogBody_ConfirmDelete, deletionName, description),
                                                              StringResources.DialogTitle_ConfirmDelete,
                                                              MessageBoxButton.YesNo, MessageBoxImage.Information, @"", false, false, true, false) == MessageBoxResult.Yes;

                        return shouldDelete;
                    }
                }
            }
            return false;
        }

        public bool ShowDeleteDialogForFolder(string folderBeingDeleted)
        {
            var popupResult = PopupProvider.Show(string.Format(StringResources.DialogBody_ConfirmFolderDelete, folderBeingDeleted),
                               StringResources.DialogTitle_ConfirmDelete,
                               MessageBoxButton.YesNo, MessageBoxImage.Information, @"", false, false, true, false);

            var confirmDelete = popupResult == MessageBoxResult.Yes;

            return confirmDelete;
        }
        

        private void DeleteResources(ICollection<IContextualResourceModel> models, string folderName, bool showConfirm = true, System.Action actionToDoOnDelete = null)
        {
            if (models == null || showConfirm && !ConfirmDelete(models, folderName))
            {
                return;
            }

            foreach (var contextualModel in models)
            {
                if (contextualModel == null)
                {
                    continue;
                }

                DeleteContext(contextualModel);

                actionToDoOnDelete?.Invoke();
            }
        }



        public double MenuPanelWidth { get; set; }

        private void SaveWorkspaceItems()
        {
            _getWorkspaceItemRepository().Write();
        }

        private void AddWorkspaceItem(IContextualResourceModel model)
        {
            _getWorkspaceItemRepository().AddWorkspaceItem(model);
        }

        readonly Func<IWorkspaceItemRepository> _getWorkspaceItemRepository = () => WorkspaceItemRepository.Instance;

        private void RemoveWorkspaceItem(IDesignerViewModel viewModel)
        {
            _getWorkspaceItemRepository().Remove(viewModel.ResourceModel);
        }

        protected virtual void AddWorkspaceItems()
        {
            if (EnvironmentRepository == null) return;

            HashSet<IWorkspaceItem> workspaceItemsToRemove = new HashSet<IWorkspaceItem>();
            // ReSharper disable ForCanBeConvertedToForeach
            for (int i = 0; i < _getWorkspaceItemRepository().WorkspaceItems.Count; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                //
                // Get the environment for the workspace item
                //
                IWorkspaceItem item = _getWorkspaceItemRepository().WorkspaceItems[i];
                Dev2Logger.Info($"Start Proccessing WorkspaceItem: {item.ServiceName}");
                IEnvironmentModel environment = EnvironmentRepository.All().Where(env => env.IsConnected).TakeWhile(env => env.Connection != null).FirstOrDefault(env => env.ID == item.EnvironmentID);

                if (environment?.ResourceRepository == null)
                {
                    Dev2Logger.Info(@"Environment Not Found");
                    if (environment != null && item.EnvironmentID == environment.ID)
                    {
                        workspaceItemsToRemove.Add(item);
                    }
                }
                if (environment != null)
                {
                    Dev2Logger.Info($"Proccessing WorkspaceItem: {item.ServiceName} for Environment: {environment.DisplayName}");
                    if (environment.ResourceRepository != null)
                    {
                        environment.ResourceRepository.LoadResourceFromWorkspace(item.ID, item.WorkspaceID);
                        var resource = environment.ResourceRepository.All().FirstOrDefault(rm =>
                        {
                            var sameEnv = true;
                            if (item.EnvironmentID != Guid.Empty)
                            {
                                sameEnv = item.EnvironmentID == environment.ID;
                            }
                            return rm.ID == item.ID && sameEnv;
                        }) as IContextualResourceModel;

                        if (resource == null)
                        {
                            workspaceItemsToRemove.Add(item);
                        }
                        else
                        {
                            Dev2Logger.Info($"Got Resource Model: {resource.DisplayName} ");
                            var fetchResourceDefinition = environment.ResourceRepository.FetchResourceDefinition(environment, item.WorkspaceID, resource.ID, false);
                            resource.WorkflowXaml = fetchResourceDefinition.Message;
                            resource.IsWorkflowSaved = item.IsWorkflowSaved;
                            resource.OnResourceSaved += model => _getWorkspaceItemRepository().UpdateWorkspaceItemIsWorkflowSaved(model);
                            AddWorkSurfaceContextFromWorkspace(resource);
                        }
                    }
                }
                else
                {
                    workspaceItemsToRemove.Add(item);
                }
            }

            foreach (IWorkspaceItem workspaceItem in workspaceItemsToRemove)
            {
                _getWorkspaceItemRepository().WorkspaceItems.Remove(workspaceItem);
            }
        }





        private void DeleteContext(IContextualResourceModel model)
        {
            var context = FindWorkSurfaceContextViewModel(model);
            if (context == null)
            {
                return;
            }

            context.DeleteRequested = true;
            base.DeactivateItem(context, true);
        }

        private T CreateAndActivateUniqueWorkSurface<T>
            (WorkSurfaceContext context, Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            T vmr;
            WorkSurfaceContextViewModel ctx = WorkSurfaceContextFactory.Create(context, initParms, out vmr);
            AddAndActivateWorkSurface(ctx);
            return vmr;
        }

        private T ActivateOrCreateUniqueWorkSurface<T>(WorkSurfaceContext context,
                                                          Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateEnvKey(context, ActiveServer.EnvironmentID) as WorkSurfaceKey;
            var exists = ActivateAndReturnWorkSurfaceIfPresent(key, initParms);

            if (exists == null)
            {
                if (typeof(T) == typeof(SettingsViewModel))
                {
                    Tracker.TrackEvent(TrackerEventGroup.Settings, TrackerEventName.Opened);
                }

                return CreateAndActivateUniqueWorkSurface<T>(context, initParms);
            }
            try
            {
                return (T)exists.WorkSurfaceViewModel;
            }
            catch
            {
                return default(T);
            }
        }

        private bool ActivateWorkSurfaceIfPresent(IContextualResourceModel resource,
                                                  Tuple<string, object>[] initParms = null)
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(resource);
            return ActivateWorkSurfaceIfPresent(key, initParms);
        }

        bool ActivateWorkSurfaceIfPresent(WorkSurfaceKey key, Tuple<string, object>[] initParms = null)
        {
            WorkSurfaceContextViewModel currentContext = FindWorkSurfaceContextViewModel(key);

            if (currentContext != null)
            {
                if (initParms != null)
                    PropertyHelper.SetValues(
                        currentContext.WorkSurfaceViewModel, initParms);

                ActivateItem(currentContext);
                return true;
            }
            return false;
        }

        WorkSurfaceContextViewModel ActivateAndReturnWorkSurfaceIfPresent(WorkSurfaceKey key, Tuple<string, object>[] initParms = null)
        {
            WorkSurfaceContextViewModel currentContext = FindWorkSurfaceContextViewModel(key);

            if (currentContext != null)
            {
                if (initParms != null)
                    PropertyHelper.SetValues(
                        currentContext.WorkSurfaceViewModel, initParms);

                ActivateItem(currentContext);
                return currentContext;
            }
            return null;
        }

        public bool IsWorkFlowOpened(IContextualResourceModel resource)
        {
            return FindWorkSurfaceContextViewModel(resource) != null;
        }
        public void UpdateWorkflowLink(IContextualResourceModel resource, string newPath, string oldPath)
        {
            var x = FindWorkSurfaceContextViewModel(resource);
            if (x != null)
            {


                var path = oldPath.Replace('\\', '/');
                var b = x.WorkSurfaceViewModel as WorkflowDesignerViewModel;
                b?.UpdateWorkflowLink(b.DisplayWorkflowLink.Replace(path, newPath.Replace('\\', '/')));
            }
        }

        WorkSurfaceContextViewModel FindWorkSurfaceContextViewModel(WorkSurfaceKey key)
        {
            return Items.FirstOrDefault(
                c => WorkSurfaceKeyEqualityComparerWithContextKey.Current.Equals(key, c.WorkSurfaceKey));
        }

        public WorkSurfaceContextViewModel FindWorkSurfaceContextViewModel(IContextualResourceModel resource)
        {
            var key = WorkSurfaceKeyFactory.CreateKey(resource);
            return FindWorkSurfaceContextViewModel(key);
        }

        void AddWorkSurfaceContextFromWorkspace(IContextualResourceModel resourceModel)
        {
            AddWorkSurfaceContextImpl(resourceModel, true);
        }

        public void AddWorkSurfaceContext(IContextualResourceModel resourceModel)
        {
            AddWorkSurfaceContextImpl(resourceModel, false);
        }

        private void AddWorkSurfaceContextImpl(IContextualResourceModel resourceModel, bool isLoadingWorkspace)
        {
            if (resourceModel == null)
            {
                return;
            }

            var exists = IsInOpeningState(resourceModel) || ActivateWorkSurfaceIfPresent(resourceModel);

            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(resourceModel);

            if (exists)
            {
                return;
            }

            _canDebug = false;

            if (!isLoadingWorkspace)
            {
                OpeningWorkflowsHelper.AddWorkflow(workSurfaceKey);
            }

            if (!isLoadingWorkspace)
            {
                resourceModel.IsWorkflowSaved = true;
            }

            AddWorkspaceItem(resourceModel);
            AddAndActivateWorkSurface(_getWorkSurfaceContextViewModel(resourceModel, _createDesigners) as WorkSurfaceContextViewModel);

            OpeningWorkflowsHelper.RemoveWorkflow(workSurfaceKey);
            _canDebug = true;
        }

        readonly Func<IContextualResourceModel, bool, IWorkSurfaceContextViewModel> _getWorkSurfaceContextViewModel = (resourceModel, createDesigner) => WorkSurfaceContextFactory.CreateResourceViewModel(resourceModel, createDesigner);

        private bool IsInOpeningState(IContextualResourceModel resource)
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(resource);
            return OpeningWorkflowsHelper.FetchOpeningKeys().Any(c => WorkSurfaceKeyEqualityComparer.Current.Equals(key, c));
        }

        private void AddAndActivateWorkSurface(WorkSurfaceContextViewModel context)
        {
            if (context != null)
            {
                var found = FindWorkSurfaceContextViewModel(context.WorkSurfaceKey);
                if (found == null)
                {
                    found = context;
                    Items.Add(context);
                }
                ActivateItem(found);
            }
        }

        private void AddWorkSurface(IWorkSurfaceObject obj)
        {
            TypeSwitch.Do(obj, TypeSwitch.Case<IContextualResourceModel>(AddWorkSurfaceContext));
        }

        public void TryRemoveContext(IContextualResourceModel model)
        {
            WorkSurfaceContextViewModel context = FindWorkSurfaceContextViewModel(model);
            if (context != null)
            {
                context.DeleteRequested = true;
                DeactivateItem(context, true);
            }
        }

        /// <summary>
        ///     Saves all open tabs locally and writes the open tabs the to collection of workspace items
        /// </summary>
        public void PersistTabs(bool isStudioShutdown = false)
        {
            if (isStudioShutdown)
                SaveAndShutdown(true);
            else
            {
                SaveOnBackgroundTask(false);
            }
        }
        private readonly object _locker = new object();
        void SaveOnBackgroundTask(bool isStudioShutdown)
        {

            SaveWorkspaceItems();
            Task t = new Task(() =>
            {

                lock (_locker)
                {
                    foreach (var ctx in Items.Where(a => true).ToList())
                    {
                        if (!ctx.WorkSurfaceViewModel.DisplayName.ToLower().Contains("version") && ctx.IsEnvironmentConnected())
                        {
                            ctx.Save(true, isStudioShutdown);
                        }
                    }
                }
            });
            t.Start();

        }

        void SaveAndShutdown(bool isStudioShutdown)
        {
            SaveWorkspaceItems();
            foreach (var ctx in Items)
            {
                if (ctx.IsEnvironmentConnected())
                {
                    ctx.Save(true, isStudioShutdown);
                }
            }
        }

        public bool CloseWorkSurfaceContext(WorkSurfaceContextViewModel context, PaneClosingEventArgs e, bool dontPrompt = false)
        {
            bool remove = true;
            if (context != null)
            {
                if (!context.DeleteRequested)
                {
                    var vm = context.WorkSurfaceViewModel;
                    if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Workflow)
                    {
                        var workflowVm = vm as IWorkflowDesignerViewModel;
                        IContextualResourceModel resource = workflowVm?.ResourceModel;
                        if (resource != null)
                        {
                            remove = !resource.IsAuthorized(AuthorizationContext.Contribute) || resource.IsWorkflowSaved;

                            var connection = workflowVm.ResourceModel.Environment.Connection;

                            if (connection != null && !connection.IsConnected)
                            {
                                var result = PopupProvider.Show(string.Format(StringResources.DialogBody_DisconnectedItemNotSaved, workflowVm.ResourceModel.ResourceName),
                                    $"Save not allowed {workflowVm.ResourceModel.ResourceName}?",
                                    MessageBoxButton.OKCancel, MessageBoxImage.Information, "", false, false, true, false);

                                switch (result)
                                {
                                    case MessageBoxResult.OK:
                                        remove = true;
                                        break;
                                    case MessageBoxResult.Cancel:
                                        return false;
                                    default:
                                        return false;
                                }
                            }
                            if (dontPrompt)
                                remove = true;
                            if (!remove)
                            {
                                remove = ShowRemovePopup(workflowVm);
                            }

                            if (remove)
                            {
                                if (resource.IsNewWorkflow)
                                {
                                    NewWorkflowNames.Instance.Remove(resource.ResourceName);
                                }
                                RemoveWorkspaceItem(workflowVm);
                                Items.Remove(context);
                                workflowVm.Dispose();
                                if (_previousActive != null && _previousActive.WorkSurfaceViewModel == vm)
                                    _previousActive = null;
                                Dev2Logger.Info("Publish message of type - " + typeof(TabClosedMessage));
                                EventPublisher.Publish(new TabClosedMessage(context));
                                if (e != null)
                                {
                                    e.Cancel = true;
                                }
                            }
                            else if (e != null)
                            {
                                e.Handled = true;
                                e.Cancel = false;
                            }
                        }
                    }
                    else if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Settings)
                    {
                        var settingsViewModel = vm as SettingsViewModel;
                        if (settingsViewModel != null)
                        {
                            remove = settingsViewModel.DoDeactivate(true);
                            if (remove)
                            {
                                settingsViewModel.Dispose();
                            }
                        }
                    }
                    else if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Scheduler)
                    {
                        var schedulerViewModel = vm as SchedulerViewModel;
                        if (schedulerViewModel != null)
                        {
                            remove = schedulerViewModel.DoDeactivate(true);
                            if (remove)
                            {
                                schedulerViewModel.Dispose();
                            }
                        }
                    }
                    else
                    {
                        var tab = vm as IStudioTab;
                        if (tab != null)
                        {
                            remove = tab.DoDeactivate(true);
                            if (remove)
                            {
                                tab.Dispose();
                            }
                        }
                    }
                }
            }

            return remove;
        }

        public bool OnStudioClosing()
        {
            List<WorkSurfaceContextViewModel> workSurfaceContextViewModels = Items.ToList();
            foreach (WorkSurfaceContextViewModel workSurfaceContextViewModel in workSurfaceContextViewModels)
            {
                var vm = workSurfaceContextViewModel.WorkSurfaceViewModel;
                if (vm != null)
                {
                    if (vm.WorkSurfaceContext == WorkSurfaceContext.Settings)
                    {
                        var settingsViewModel = vm as SettingsViewModel;
                        if (settingsViewModel != null && settingsViewModel.IsDirty)
                        {
                            ActivateItem(workSurfaceContextViewModel);
                            bool remove = settingsViewModel.DoDeactivate(true);
                            if (!remove)
                            {
                                return false;
                            }
                        }
                    }
                    else if (vm.WorkSurfaceContext == WorkSurfaceContext.Scheduler)
                    {
                        var schedulerViewModel = vm as SchedulerViewModel;
                        if (schedulerViewModel?.SelectedTask != null && schedulerViewModel.SelectedTask.IsDirty)
                        {
                            ActivateItem(workSurfaceContextViewModel);
                            bool remove = schedulerViewModel.DoDeactivate(true);
                            if (!remove)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            CloseRemoteConnections();
            return true;
        }

        private void CloseRemoteConnections()
        {
            var connected = EnvironmentRepository.All().Where(a => a.IsConnected);
            foreach (var environmentModel in connected)
            {
                environmentModel.Disconnect();
            }
        }



        public Func<bool> _isBusyDownloadingInstaller;
        IMenuViewModel _menuViewModel;
        IServer _activeServer;
        private IExplorerViewModel _explorerViewModel;

        public bool IsDownloading()
        {
            return _isBusyDownloadingInstaller != null && _isBusyDownloadingInstaller();
        }


        public void Handle(DisplayMessageBoxMessage message)
        {
            PopupProvider.Show(message.Message, message.Heading, MessageBoxButton.OK, message.MessageBoxImage, "", false, false, true, false);
        }


        public async Task<bool> CheckForNewVersion()
        {
            var hasNewVersion = await Version.GetNewerVersionAsync();
            return hasNewVersion;
        }

        public async void DisplayDialogForNewVersion()
        {
            var hasNewVersion = await CheckForNewVersion();
            if (hasNewVersion)
            {
                var dialog = new WebLatestVersionDialog();
                dialog.ShowDialog();
            }
        }


        public bool MenuExpanded
        {
            get
            {
                return _menuExpanded;
            }
            set
            {
                _menuExpanded = value;
                NotifyOfPropertyChange(() => MenuExpanded);
            }
        }
        public IMenuViewModel MenuViewModel => _menuViewModel ?? (_menuViewModel = new MenuViewModel(this));

        public IToolboxViewModel ToolboxViewModel
        {
            get
            {
                var toolboxViewModel = CustomContainer.Get<IToolboxViewModel>();
                return toolboxViewModel;
            }
        }
        public IHelpWindowViewModel HelpViewModel
        {
            get
            {
                var helpViewModel = CustomContainer.Get<IHelpWindowViewModel>();
                return helpViewModel;
            }
        }


        public void Handle(FileChooserMessage message)
        {
            var emailAttachmentView = new ManageEmailAttachmentView();

            emailAttachmentView.ShowView(message.SelectedFiles.ToList());
            var emailAttachmentVm = emailAttachmentView.DataContext as EmailAttachmentVm;
            if (emailAttachmentVm != null && emailAttachmentVm.Result == MessageBoxResult.OK)
            {
                message.SelectedFiles = emailAttachmentVm.GetAttachments();
            }
        }

    }
}
