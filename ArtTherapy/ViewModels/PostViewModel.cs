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
    public class PostViewModel<T> : BaseViewModel<T> where T : ProductModel, new()
    {
        public BaseJsonStorage<T> Storage { get; private set; }

        private JsonFactoryStorage<T> _JsonFactoryStorage = new JsonFactoryStorage<T>();

        public event EventHandler<PostEventArgs> Loaded;

        private List<TaskCompletionSource<bool>> _IsLoadedList = new List<TaskCompletionSource<bool>>();

        public PostViewModel()
        {
            ProductModel = new T()
            {
                Items = new ObservableCollection<CurrentProductModel>()
            };

            Storage = (BaseJsonStorage<T>)_JsonFactoryStorage.Create();

            PageSize = 2;
        }

        public int PageSize { get; set; }

        public int Count { get; private set; }

        public int CurrentCount { get => ProductModel.Items.Count; }

        public bool IsFullInitialized { get => Count.Equals(CurrentCount); }

        public void LoadData(double scrollViewerProgress, bool canLoadingImages = true, int page = 1, int startCountLoad = 10, int fullDataTimeLoading = 1000, int smallDataTimeLoading = 50)
        {
            //await AddDemoData(new LoadingHelper(LoadingType.GetRemainsData));
            if (scrollViewerProgress > 0.9999)
            {
                Task tt = Task.Run(async () =>
                {
                    LoadingHelper loadingHelper = new LoadingHelper(LoadingType.GetCount);
                    var postModel = await Storage.GetModel(loadingHelper.Path) as T;
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        if (postModel != null)
                        {
                            Count = postModel.Count;
                            if (Count > 0 && CurrentCount < Count)
                            {
                                for (int i = 0; i < Count; i++)
                                {
                                    if (i == startCountLoad || CurrentCount == Count) break;
                                    ProductModel.Items.Add(new CurrentProductModel()
                                    {
                                        IsEnabledBuy = false,
                                        IsLoading = true
                                    });
                                    Debug.WriteLine($"Добавлено {CurrentCount} из {Count}");
                                }

                                _IsLoadedList.Add(new TaskCompletionSource<bool>());

                                int startIndex = ProductModel.Items.IndexOf(ProductModel.Items.LastOrDefault());
                                if (startIndex >= startCountLoad - 1)
                                {
                                    int startCountLoadParam = startCountLoad - 1;
                                    startIndex -= startCountLoadParam;

                                    //Назвние и Sku
                                    loadingHelper.LoadingType = LoadingType.GetFullData;
                                    var fullPostModel = await Storage.GetModel(loadingHelper.Path) as T;
                                    await Task.Delay(fullDataTimeLoading);
                                    await Task.Run(async () =>
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                        {
                                            if (fullPostModel != null && fullPostModel.Items != null && fullPostModel.Items.Count > 0)
                                            {
                                                for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                                {

                                                    ProductModel.Items[i] = fullPostModel.Items[i];
                                                    ProductModel.Items[i].IsLoading = false;

                                                    ProductModel.Items[i].IsLoadingImage = true;
                                                    ProductModel.Items[i].IsLoadingPrice = true;
                                                    ProductModel.Items[i].IsLoadingRemains = true;
                                                }
                                            }
                                        });
                                    });

                                    Loaded?.Invoke(this, new PostEventArgs(Count.Equals(CurrentCount)));

                                    //Картинки
                                    Task t1 = null;
                                    //if (canLoadingImages)
                                    //{
                                        loadingHelper.LoadingType = LoadingType.GetImagesData;
                                        var images = await Storage.GetModel(loadingHelper.Path) as T;
                                        await Task.Delay(smallDataTimeLoading);
                                        t1 = Task.Run(async () =>
                                        {
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                            {
                                                if (images != null && images.Items != null && images.Items.Count > 0)
                                                {
                                                    for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                                    {
                                                        ProductModel.Items[i].ImageUrl = images.Items[i].ImageUrl;
                                                        ProductModel.Items[i].IsLoadingImage = false;
                                                    }
                                                }
                                            });
                                        });
                                    //}

                                    //Цена
                                    loadingHelper.LoadingType = LoadingType.GetPricesData;
                                    var prices = await Storage.GetModel(loadingHelper.Path) as T;
                                    await Task.Delay(smallDataTimeLoading);
                                    Task t2 = Task.Run(async () =>
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                        {
                                            if (prices != null && prices.Items != null && prices.Items.Count > 0)
                                            {
                                                for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                                {
                                                    ProductModel.Items[i].DiscountPrice =
                                                        prices.Items[i].DiscountPrice < prices.Items[i].Price ? prices.Items[i].DiscountPrice : 0;
                                                    ProductModel.Items[i].Price = prices.Items[i].Price;
                                                    if (ProductModel.Items[i].DiscountPrice > 0)
                                                        ProductModel.Items[i].PriceDifference =
                                                            ProductModel.Items[i].Price - ProductModel.Items[i].DiscountPrice;
                                                    ProductModel.Items[i].IsLoadingPrice = false;
                                                }
                                            }
                                        });
                                    });

                                    //Остатки
                                    loadingHelper.LoadingType = LoadingType.GetRemainsData;
                                    var remains = await Storage.GetModel(loadingHelper.Path) as T;
                                    await Task.Delay(smallDataTimeLoading);
                                    Task t3 = Task.Run(async () =>
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                        {
                                            if (remains != null && remains.Items != null && remains.Items.Count > 0)
                                            {
                                                for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                                {
                                                    ProductModel.Items[i].Remains = remains.Items[i].Remains;
                                                    ProductModel.Items[i].IsLoadingRemains = false;
                                                    ProductModel.Items[i].IsEnabledBuy = true;
                                                }
                                            }
                                        });
                                    });

                                    if (canLoadingImages && t1 != null)
                                        await Task.WhenAll(new[] {t1, t2, t3 });
                                    else
                                        await Task.WhenAll(new[] { t2, t3 });

                                    _IsLoadedList.LastOrDefault().TrySetResult(true);
                                }
                            }
                        }
                    });
                });
            }
        }

        public async Task AddDemoData(LoadingHelper loadingHelper)
        {
            var demoPostModel = new T()
            {
                Items = new ObservableCollection<CurrentProductModel>()
            };

            for (int i = 0, k = 0; i < 175; i++, k++)
            {
                demoPostModel.Items.Add(new CurrentProductModel()
                {
                    //Sku = (uint)(3002550 + (i + 1)),
                    //Price = 108990,
                    //DiscountPrice = (i + 1) > 10 && i <= 35 ? 98990 : 0,
                    //Image = null,
                    //BuyIcon = "\xE7BF",
                    //IsLoading = false,
                    //Name = "Ноутбук Apple MacBook Pro 2017 Core i7/16/256 SSD Gold (MNYK2RU/A)",
                    //ImageUrl = "https://rebabaskett.com/wp-content/uploads/2017/01/u_10150899.jpg",
                    //Remains = (uint)(k + 1)
                });
                if ((k + 1) == 10)
                    k = 0;
            }

            await Storage.SetModel(loadingHelper.Path, demoPostModel);
        }

        public override T GetModel()
        {
            return ProductModel;
        }

        public override async void Dispose()
        {
            foreach (var x in _IsLoadedList)
                await x.Task;

            ProductModel.Items.Clear();
            ProductModel = null;

            _IsLoadedList.Clear();
            _IsLoadedList = null;
        }
        public T ProductModel
        {
            get { return (T)GetValue(PostModelProperty); }
            set { SetValue(PostModelProperty, value); }
        }
        
        public static readonly DependencyProperty PostModelProperty =
            DependencyProperty.Register("ProductModel", typeof(T), typeof(PostViewModel<T>), new PropertyMetadata(null));
    }
}
