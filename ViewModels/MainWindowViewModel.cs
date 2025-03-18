using Avalonia.Controls;
using Avalonia.Interactivity;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using GetStartedApp.Command;
using GetStartedApp.Context;
using GetStartedApp.Context.Models;
using GetStartedApp.Services;
using GetStartedApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tmds.DBus.Protocol;

namespace GetStartedApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IBlogService blogService,IContainerExtension container) : base(container)
    {

        RegionManager.RegisterViewWithRegion("Nav_MainContent", "LoginPage");
    }


}
