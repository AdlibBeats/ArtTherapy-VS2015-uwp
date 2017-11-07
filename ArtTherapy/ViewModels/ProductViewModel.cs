using ArtTherapy.Models.ProductModels;
using ArtTherapy.Storage;
using ArtTherapyCore.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArtTherapyCore.BaseModels;

namespace ArtTherapy.ViewModels
{
    public class PostEventArgs : EventArgs
    {
        public bool IsFullInitialized { get; }

        public PostEventArgs(bool isFullInitialized)
        {
            IsFullInitialized = isFullInitialized;
        }
    }

    /// <summary>
    /// ViewModel
    /// </summary>
    /// <typeparam name="T">BaseModel</typeparam>
    public class ProductViewModel<T> : BaseViewModel<T> where T : ProductModel, new()
    {
        private List<TaskCompletionSource<bool>> _IsLoadedList = new List<TaskCompletionSource<bool>>();
        private JsonFactoryStorage<T> _JsonFactoryStorage = new JsonFactoryStorage<T>();

        public BaseJsonStorage<T> Storage { get; private set; }
        public event EventHandler<PostEventArgs> Loaded;

        public ProductViewModel(LoadingType loadingType)
        {
            ProductModel = new T()
            {
                Items = new ObservableCollection<CurrentProductModel>()
            };

            Storage = (BaseJsonStorage<T>)_JsonFactoryStorage.Create();
            LoadingType = loadingType;
        }

        public LoadingType LoadingType { get; private set; }

        //public async Task SetLoadingType(LoadingType loadingType)
        //{
        //    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //    {
        //        this.LoadingType = loadingType;
        //        ProductModel.Items.ToList().ForEach((CurrentProductModel x) => x.LoadingType = this.LoadingType);
        //    });
        //}

        /// <summary>
        /// Полное количество, полученное через запрос.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Получает текущее количество, доступное в GridView.
        /// </summary>
        public int CurrentCount { get => ProductModel.Items.Count; }

        /// <summary>
        /// Загрузка данных из json файлов.
        /// </summary>
        /// <param name="scrollViewerProgress">Результат скроллинга.</param>
        /// <param name="scrollCheck">Значение для проверки скролла.</param>
        /// <param name="startCountLoad">Количество подгружаемых элементов.</param>
        /// <param name="fullDataTimeLoading">Время загрузки полной информации (Название и Sku).</param>
        /// <param name="smallDataTimeLoading">Время загрузки изображений, ценников и остатков.</param>
        public async Task LoadData(double scrollViewerProgress, double scrollCheck = 0.9999, int startCountLoad = 10, int fullDataTimeLoading = 1000, int smallDataTimeLoading = 500)
        {
            if (scrollViewerProgress > scrollCheck)
            {
                _IsLoadedList.Add(new TaskCompletionSource<bool>(false));

                JsonLoadingHelper loadingHelper = new JsonLoadingHelper(JsonLoadingType.GetCount);
                //Получаем количество
                var postModel = await Storage.GetModel(loadingHelper.Path) as T;
                if (postModel != null)
                {
                    Count = postModel.Count;
                    if (Count > 0 && CurrentCount < Count)
                    {
                        //Defalut
                        await AddDefaultProducts(startCountLoad);

                        //Идекс последнего добавленного элемента
                        int startIndex = ProductModel.Items.IndexOf(ProductModel.Items.LastOrDefault());
                        int startCountLoadIndex = startCountLoad - 1;
                        if (startIndex >= startCountLoadIndex)
                        {
                            startIndex -= startCountLoadIndex;

                            //Назвние и Sku
                            await GetNamesSkuTask(loadingHelper, startIndex, startCountLoad, fullDataTimeLoading);

                            //Картинки
                            Task getImagesTask =
                                GetImagesTask(loadingHelper, startIndex, startCountLoad, smallDataTimeLoading);

                            //Цена
                            Task getPricesTask =
                                GetPrices(loadingHelper, startIndex, startCountLoad, smallDataTimeLoading);

                            //Остатки
                            Task getRemainsTask =
                                GetRemains(loadingHelper, startIndex, startCountLoad, smallDataTimeLoading);

                            //Запустить параллельно 3 потока
                            await Task.WhenAll(getImagesTask, getPricesTask, getRemainsTask);

                            await _IsLoadedList.LastOrDefault().Task;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет пустой продукт.
        /// </summary>
        /// <param name="startCountLoad"></param>
        /// <returns></returns>
        private async Task AddDefaultProducts(int startCountLoad)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < Count; i++)
                {
                    if (i == startCountLoad || CurrentCount == Count) break;
                    ProductModel.Items.Add(new CurrentProductModel()
                    {
                        IsEnabledBuy = false,
                        IsLoading = true,
                        //LoadingType = this.LoadingType
                    });
                    Debug.WriteLine($"Добавлено {CurrentCount} из {Count}");
                }
            });
        }

        /// <summary>
        /// Получает название и Sku.
        /// </summary>
        /// <param name="loadingHelper"></param>
        /// <param name="index"></param>
        /// <param name="startCountLoad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task GetNamesSkuTask(JsonLoadingHelper loadingHelper, int index, int startCountLoad, int time)
        {
            loadingHelper.LoadingType = JsonLoadingType.GetFullData;
            var fullPostModel = await Storage.GetModel(loadingHelper.Path) as T;
            await Task.Delay(time);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (fullPostModel?.Items != null && fullPostModel.Items.Any())
                {
                    for (int i = index, k = 0; k < startCountLoad && i < Count; i++, k++)
                    {
                        //fullPostModel.Items[i].LoadingType = this.LoadingType;
                        ProductModel.Items[i] = fullPostModel.Items[i];
                        ProductModel.Items[i].IsLoading = false;

                        ProductModel.Items[i].ImageUrl = "None";
                        ProductModel.Items[i].IsLoadingImage = true;
                        ProductModel.Items[i].IsLoadingPrice = true;
                        ProductModel.Items[i].IsLoadingRemains = true;
                    }
                }
                Loaded?.Invoke(this, new PostEventArgs(Count.Equals(CurrentCount)));
            });
        }

        /// <summary>
        /// Получает картинки.
        /// </summary>
        /// <param name="loadingHelper"></param>
        /// <param name="index"></param>
        /// <param name="startCountLoad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task GetImagesTask(JsonLoadingHelper loadingHelper, int index, int startCountLoad, int time)
        {
            loadingHelper.LoadingType = JsonLoadingType.GetImagesData;
            var images = await Storage.GetModel(loadingHelper.Path) as T;
            await Task.Delay(time);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                for (int i = index, k = 0; k < startCountLoad && i < Count; i++, k++)
                {
                    if (images?.Items != null && images.Items.Any())
                        ProductModel.Items[i].ImageUrl = images.Items[i].ImageUrl;
                    else
                        ProductModel.Items[i].ImageUrl = null;

                    ProductModel.Items[i].IsLoadingImage = false;
                }
            });
        }

        /// <summary>
        /// Получает цены.
        /// </summary>
        /// <param name="loadingHelper"></param>
        /// <param name="index"></param>
        /// <param name="startCountLoad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task GetPrices(JsonLoadingHelper loadingHelper, int index, int startCountLoad, int time)
        {
            loadingHelper.LoadingType = JsonLoadingType.GetPricesData;
            var prices = await Storage.GetModel(loadingHelper.Path) as T;
            await Task.Delay(time);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                for (int i = index, k = 0; k < startCountLoad && i < Count; i++, k++)
                {
                    if (prices?.Items != null && prices.Items.Any())
                    {
                        ProductModel.Items[i].Price = prices.Items[i].Price;

                        if (ProductModel.Items[i].Price != null)
                        {
                            ProductModel.Items[i].DiscountPrice =
                                prices.Items[i].DiscountPrice != null &&
                                prices.Items[i].DiscountPrice < ProductModel.Items[i].Price
                                ? prices.Items[i].DiscountPrice : 0;

                            ProductModel.Items[i].PriceDifference =
                                ProductModel.Items[i].DiscountPrice != null &&
                                ProductModel.Items[i].DiscountPrice > 0
                                ? ProductModel.Items[i].Price - ProductModel.Items[i].DiscountPrice : 0;
                        }
                    }
                    ProductModel.Items[i].IsLoadingPrice = false;
                }
            });
        }

        /// <summary>
        /// Получает остатки.
        /// </summary>
        /// <param name="loadingHelper"></param>
        /// <param name="index"></param>
        /// <param name="startCountLoad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task GetRemains(JsonLoadingHelper loadingHelper, int index, int startCountLoad, int time)
        {
            loadingHelper.LoadingType = JsonLoadingType.GetRemainsData;
            var remains = await Storage.GetModel(loadingHelper.Path) as T;
            await Task.Delay(time);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                for (int i = index, k = 0; k < startCountLoad && i < Count; i++, k++)
                {
                    if (remains != null && remains.Items != null && remains.Items.Count >= startCountLoad)
                    {
                        ProductModel.Items[i].Remains = remains.Items[i].Remains;
                        ProductModel.Items[i].IsEnabledBuy = true;
                    }
                    ProductModel.Items[i].IsLoadingRemains = false;
                }
            });
        }

        /// <summary>
        /// Запись данных в json.
        /// </summary>
        /// <param name="loadingHelper"></param>
        /// <param name="demoPostModel"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async void AddDemoData(JsonLoadingHelper loadingHelper, T demoPostModel = default(T), ObservableCollection<CurrentProductModel> items = null)
        {
            if (demoPostModel == null)
                demoPostModel = new T();

            if (items == null || items.Count == 0)
            {
                demoPostModel.Items = new ObservableCollection<CurrentProductModel>();
                for (int i = 0, k = 0; i < 175; i++, k++)
                {
                    demoPostModel.Items.Add(new CurrentProductModel()
                    {
                        //Sku = (st)(3002550 + (i + 1)),
                        Price = 108990,
                        DiscountPrice = (i + 1) > 10 && i <= 35 ? 98990 : 0,
                        //BuyIcon = "\xE7BF",
                        IsLoading = false,
                        Name = "Ноутбук Apple MacBook Pro 2017 Core i7/16/256 SSD Gold (MNYK2RU/A)",
                        ImageUrl = "https://rebabaskett.com/wp-content/uploads/2017/01/u_10150899.jpg",
                        //Remains = (uint)(k + 1)
                    });
                    if ((k + 1) == 10)
                        k = 0;
                }
            }

            await Storage.SetModel(loadingHelper.Path, demoPostModel);
        }

        public override T GetModel()
        {
            return ProductModel;
        }

        /// <summary>
        /// Модель товаров.
        /// </summary>
        public T ProductModel
        {
            get => _ProductModel;
            set => SetValue(ref _ProductModel, value);
        }
        private T _ProductModel;
    }
}
