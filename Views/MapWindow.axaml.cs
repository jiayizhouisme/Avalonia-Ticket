using Avalonia.Controls;
using Avalonia.Interactivity;
using BruTile;
using BruTile.Predefined;
using BruTile.Web;
using DryIoc;
using GetStartedApp.Context.Models;
using GetStartedApp.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers.Wms;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using Mapsui.Widgets.ScaleBar;
using NetTopologySuite.Geometries;
using NetTopologySuite.GeometriesGraph;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace GetStartedApp.Views
{
    public partial class MapPage : UserControl
    {
        private readonly IRegionManager _regionManager;

        public MapPage(IContainerExtension container)
        {
            InitializeComponent();
            _regionManager = container.Resolve<IRegionManager>();
            InitializeMap();
        }

        private void InitializeMap()
        {
            var Map = this.FindControl<Mapsui.UI.Avalonia.MapControl>("map");

            var httpClient = new HttpClient();
            string url = "http://webst01.is.autonavi.com/appmaptile?style=7&x={x}&y={y}&z={z}&lang=zh_cn&size=1&scale=1";
            var osmSource = new HttpClientTileSource(httpClient, new GlobalSphericalMercator(), url, 217615, 106401);
            var osmLayer = new TileLayer(osmSource) { Name = "百度地图" };

            #region 地图标点
            var pointFeature = new PointFeature(SphericalMercator.FromLonLat(119, 32.14).ToMPoint());
            CalloutStyle calloutStyle = new CalloutStyle() { };
            pointFeature.Styles.Add(calloutStyle);
            // 为点要素设置一些样式，例如图标
            var symbolStyle = new SymbolStyle
            {
                SymbolType = SymbolType.Image,
                SymbolScale = 0.15,
                BitmapId = 1
            };
            IFeature[] f = new PointFeature[1] { pointFeature };
            var MarkLayer = new MemoryLayer
            {
                Name = "MarkLayer",
                //这里必须要写上，不然点了地图，触发不了
                IsMapInfoLayer = true,
                Features = f
            };
            #endregion

            Map.Map.Widgets.Enqueue(new ScaleBarWidget(Map.Map)
            {
                TextAlignment = Mapsui.Widgets.Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
            });
            Map.Map.Widgets.Enqueue(new Mapsui.Widgets.Zoom.ZoomInOutWidget { MarginX = 20, MarginY = 40 });

            var smc = SphericalMercator.FromLonLat(119, 32.14); // 经纬度
            Map.Map.Home = n => n.CenterOnAndZoomTo(new MPoint(smc.x, smc.y), Map.Map.Navigator.Resolutions[16]);

            Map.Map.Layers.Add(osmLayer);
            Map.Map.Layers.Add(MarkLayer);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _regionManager.RequestNavigate("Nav_HomeContent", "ButtonPage");
        }

        private static WmsProvider CreateWmsProvider()
        {
            const string wmsUrl = "https://geodata.nationaalgeoregister.nl/windkaart/wms?request=GetCapabilities";
            var provider = WmsProvider.CreateAsync(wmsUrl).GetAwaiter().GetResult();
            return provider;
        }
    }

    internal class HttpClientTileSource : ITileSource
    {
        private readonly HttpClient _HttpClient;
        private readonly HttpTileSource _WrappedSource;
        private readonly int defaultX;
        private readonly int defaultY;
        private readonly string _url;

        public HttpClientTileSource(HttpClient httpClient, ITileSchema tileSchema, string urlFormatter, int defaultX = 0, int defaultY = 0, IEnumerable<string> serverNodes = null, string apiKey = null, string name = null,
            BruTile.Cache.IPersistentCache<byte[]> persistentCache = null, Attribution attribution = null)
        {
            _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _WrappedSource = new HttpTileSource(tileSchema, urlFormatter, serverNodes, apiKey, name, persistentCache, ClientFetch, attribution);
            this.defaultX = defaultX;
            this.defaultY = defaultY;
            this._url = urlFormatter;
        }

        public ITileSchema Schema => _WrappedSource.Schema;
        public string Name => _WrappedSource.Name;
        public Attribution Attribution => _WrappedSource.Attribution;

        public Task<byte[]?> GetTileAsync(TileInfo tileInfo)
        {
            return _WrappedSource.GetTileAsync(tileInfo);
        }

        private Task<byte[]?> ClientFetch(Uri uri)
        {
            return _HttpClient.GetByteArrayAsync(uri);
        }
    }
}