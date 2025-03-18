using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using Prism.Ioc;
using Prism.Regions;

namespace GetStartedApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected IRegionManager RegionManager { get; }
    public ViewModelBase(IContainerExtension container)
    {
        this.RegionManager = container.Resolve<IRegionManager>();
    }
}
