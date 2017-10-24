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

namespace ArtTherapy.AppSettings
{
    public class AppSettings : BaseModel
    {
        private static readonly Lazy<AppSettings> _appSettings =
            new Lazy<AppSettings>(() => new AppSettings(), true);

        public static AppSettings Current => _appSettings.Value;

        private AppSettings()
        {
            LoadingType = Models.ProductModels.LoadingType.FullMode;
        }

        private BaseJsonStorage<AppSettings> _baseJsonStorage =>
            (BaseJsonStorage<AppSettings>)_jsonFactoryStorage.Create();

        private JsonFactoryStorage<AppSettings> _jsonFactoryStorage =>
            new JsonFactoryStorage<AppSettings>();

        public ArtTherapy.Models.ProductModels.LoadingType LoadingType
        {
            get => _LoadingType;
            set => SetValue(ref _LoadingType, value);
        }
        private ArtTherapy.Models.ProductModels.LoadingType _LoadingType;

        public async void Get()
        {
            var appSettings = await _baseJsonStorage.GetModel("AppSettings.json");
            if (appSettings == null)
                return;

            LoadingType = appSettings.LoadingType;
        }

        public async void Set(ArtTherapy.Models.ProductModels.LoadingType loadingType)
        {
            LoadingType = loadingType;
            var result = await _baseJsonStorage.SetModel("AppSettings.json", this);
        }

        public async void Delete()
        {
            var result = await _baseJsonStorage.DeleteModel("AppSettings");
        }
    }
}
