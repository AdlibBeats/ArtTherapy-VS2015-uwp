using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using ArtTherapy.Models;
using ArtTherapyCore.BaseModels;
using ArtTherapy.Storage;
using ArtTherapy.Models.ProductModels;

namespace ArtTherapy.AppSettings
{
    public class AppSettings : BaseModel
    {
        private static readonly Lazy<AppSettings> _appSettings =
            new Lazy<AppSettings>(() => new AppSettings(), true);

        public static AppSettings Current => _appSettings.Value;

        private AppSettings()
        {
            LoadingType = LoadingType.FullMode;
        }

        private BaseJsonStorage<AppSettings> _baseJsonStorage =>
            (BaseJsonStorage<AppSettings>)_jsonFactoryStorage.Create();

        private JsonFactoryStorage<AppSettings> _jsonFactoryStorage =>
            new JsonFactoryStorage<AppSettings>();

        public LoadingType LoadingType
        {
            get => _LoadingType;
            set => _LoadingType = value;
        }
        private LoadingType _LoadingType;

        public async Task Get()
        {
            var appSettings = await _baseJsonStorage.GetModel("AppSettings.json");
            if (appSettings == null)
                return;
            //Debug.WriteLine(appSettings.LoadingType);
            LoadingType = appSettings.LoadingType;
        }

        public async Task Set(LoadingType loadingType)
        {
            LoadingType = loadingType;
            var result = await _baseJsonStorage.SetModel("AppSettings.json", this);
        }

        public async Task Delete()
        {
            var result = await _baseJsonStorage.DeleteModel("AppSettings.json");
        }
    }
}
