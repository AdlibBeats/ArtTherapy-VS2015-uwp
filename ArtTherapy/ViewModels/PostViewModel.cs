using ArtTherapy.Models.PostModels;
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
using ArtTherapyCore.Extensions;
using System.ComponentModel;

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

    public enum LoadingType : byte
    {
        GetCount,
        GetFullData,
        GetImagesData,
        GetPricesData,
        GetRemainsData
    }

    public class LoadingHelper
    {
        public LoadingHelper(LoadingType loadingType)
        {
            LoadingType = loadingType;
        }

        public int StartIndex { get; set; }

        public int OnStartLoadingCount { get; set; }

        public string Path
        {
            get
            {
                switch (LoadingType)
                {
                    case LoadingType.GetCount: return "PostModelCount.json";
                    case LoadingType.GetFullData: return "PostModel.json";
                    case LoadingType.GetImagesData: return "PostModelImages.json";
                    case LoadingType.GetPricesData: return "PostModelPrices.json";
                    case LoadingType.GetRemainsData: return "PostModelRemains.json";
                    default: return String.Empty;
                }
            }
        }

        public PostModel PostModel { get; set; }

        public bool IsLoaded { get; set; }

        public LoadingType LoadingType { get; set; }
    }

    public class PostViewModel<T> : BaseViewModel<T> where T : PostModel, new()
    {
        public BaseJsonStorage<T> Storage { get; private set; }

        private JsonFactoryStorage<T> _JsonFactoryStorage = new JsonFactoryStorage<T>();

        public event EventHandler<PostEventArgs> Loaded;

        private BackgroundWorker getCountWorker = new BackgroundWorker();
        private BackgroundWorker fullDataWorker = new BackgroundWorker();
        private BackgroundWorker imagesDataWorker = new BackgroundWorker();
        private BackgroundWorker pricesDataWorker = new BackgroundWorker();
        private BackgroundWorker remainsDataWorker = new BackgroundWorker();

        public PostViewModel()
        {
            PostModel = new T()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };

            Storage = (BaseJsonStorage<T>)_JsonFactoryStorage.Create();

            PageSize = 2;

            getCountWorker.WorkerReportsProgress = true;
            getCountWorker.DoWork += OnGetCountBackgroundWorker;
            getCountWorker.RunWorkerCompleted += GetCountBackgroundWorker_Completed;
            fullDataWorker.WorkerReportsProgress = true;
            fullDataWorker.DoWork += OnFullDataBackgroundWorker;
            fullDataWorker.RunWorkerCompleted += FullDataBackgroundWorker_Completed;
            imagesDataWorker.WorkerReportsProgress = true;
            imagesDataWorker.DoWork += OnDataBackgroundWorkerOne;
            imagesDataWorker.RunWorkerCompleted += DataBackgroundWorkerOne_Completed;
            pricesDataWorker.WorkerReportsProgress = true;
            pricesDataWorker.DoWork += OnDataBackgroundWorkerTwo;
            pricesDataWorker.RunWorkerCompleted += DataBackgroundWorkerTwo_Completed;
            remainsDataWorker.WorkerReportsProgress = true;
            remainsDataWorker.DoWork += OnDataBackgroundWorkerThree;
            remainsDataWorker.RunWorkerCompleted += DataBackgroundWorkerThree_Completed;
        }

        public int PageSize { get; set; }

        public int Count { get; private set; }

        public int CurrentCount { get => PostModel.Items.Count; }

        public bool IsFullInitialized { get => Count.Equals(CurrentCount); }

        private void OnGetCountBackgroundWorker(object sender, DoWorkEventArgs e)
        {
            var loadingHelper = e.Argument as LoadingHelper;
            if (loadingHelper != null)
            {
                var postModel = Storage.GetModel(loadingHelper.Path).ConfigureAwait(false).GetAwaiter().GetResult();
                if (postModel != null)
                {
                    Count = postModel.Count;
                    if (Count > 0 && CurrentCount < Count)
                        e.Result = loadingHelper;
                }
            }
        }

        private void GetCountBackgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadingHelper = e.Result as LoadingHelper;
            if (loadingHelper != null)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (i == loadingHelper.OnStartLoadingCount || CurrentCount == Count) break;

                    PostModel.Items.Add(new CurrentPostModel()
                    {
                        IsEnabledBuy = false,
                        IsLoading = true
                    });
                    Debug.WriteLine($"Добавлено {CurrentCount} из {Count}");
                }

                loadingHelper.StartIndex = PostModel.Items.IndexOf(PostModel.Items.LastOrDefault());
                if (loadingHelper.StartIndex != -1 && loadingHelper.StartIndex >= loadingHelper.OnStartLoadingCount - 1)
                {
                    loadingHelper.StartIndex -= loadingHelper.OnStartLoadingCount - 1;
                    loadingHelper.LoadingType = LoadingType.GetFullData;
                    if (fullDataWorker.IsBusy != true)
                        fullDataWorker.RunWorkerAsync(loadingHelper);
                }
            }
        }

        private void OnFullDataBackgroundWorker(object sender, DoWorkEventArgs e)
        {
            var loadingHelper = e.Argument as LoadingHelper;
            if (loadingHelper != null)
            {
                loadingHelper.PostModel = Storage.GetModel(loadingHelper.Path).ConfigureAwait(false).GetAwaiter().GetResult();
                //Task.Delay(500).Wait();
                e.Result = loadingHelper;
            }
        }

        private void FullDataBackgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadingHelper = e.Result as LoadingHelper;
            if (loadingHelper != null)
            {
                if (loadingHelper.PostModel != null && loadingHelper.PostModel.Items != null && loadingHelper.PostModel.Items.Count > 0)
                {
                    Loaded?.Invoke(this, new PostEventArgs(IsFullInitialized));
                    for (int i = loadingHelper.StartIndex, k = 0; k < loadingHelper.OnStartLoadingCount && i < Count; i++, k++)
                    {
                        PostModel.Items[i] = loadingHelper.PostModel.Items[i];
                        PostModel.Items[i].IsLoading = false;

                        PostModel.Items[i].IsLoadingImage = true;
                        PostModel.Items[i].IsLoadingPrices = true;
                        PostModel.Items[i].IsLoadingRemains = true;
                    }
                    //Loaded?.Invoke(this, new PostEventArgs(IsFullInitialized));

                    Debug.WriteLine(loadingHelper.LoadingType);
                    loadingHelper.LoadingType = LoadingType.GetImagesData;
                    if (imagesDataWorker.IsBusy != true)
                        imagesDataWorker.RunWorkerAsync(loadingHelper);
                }
            }
        }

        private void OnDataBackgroundWorkerOne(object sender, DoWorkEventArgs e)
        {
            var loadingHelper = e.Argument as LoadingHelper;
            if (loadingHelper != null)
            {
                loadingHelper.PostModel = Storage.GetModel(loadingHelper.Path).ConfigureAwait(false).GetAwaiter().GetResult();
                //Task.Delay(500).Wait();
                e.Result = loadingHelper;
            }
        }

        private void DataBackgroundWorkerOne_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadingHelper = e.Result as LoadingHelper;
            if (loadingHelper != null)
            {
                if (loadingHelper.PostModel != null && loadingHelper.PostModel.Items != null && loadingHelper.PostModel.Items.Count > 0)
                {
                    for (int i = loadingHelper.StartIndex, k = 0; k < loadingHelper.OnStartLoadingCount && i < Count; i++, k++)
                    {
                        PostModel.Items[i].Text = loadingHelper.PostModel.Items[i].Text;
                        PostModel.Items[i].IsLoadingImage = false;
                    }

                    Debug.WriteLine(loadingHelper.LoadingType);
                    loadingHelper.LoadingType = LoadingType.GetPricesData;
                    if (pricesDataWorker.IsBusy != true)
                        pricesDataWorker.RunWorkerAsync(loadingHelper);
                }
            }
        }

        private void OnDataBackgroundWorkerTwo(object sender, DoWorkEventArgs e)
        {
            var loadingHelper = e.Argument as LoadingHelper;
            if (loadingHelper != null)
            {
                loadingHelper.PostModel = Storage.GetModel(loadingHelper.Path).ConfigureAwait(false).GetAwaiter().GetResult();
                //Task.Delay(500).Wait();
                e.Result = loadingHelper;
            }
        }

        private void DataBackgroundWorkerTwo_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadingHelper = e.Result as LoadingHelper;
            if (loadingHelper != null)
            {
                if (loadingHelper.PostModel != null && loadingHelper.PostModel.Items != null && loadingHelper.PostModel.Items.Count > 0)
                {
                    for (int i = loadingHelper.StartIndex, k = 0; k < loadingHelper.OnStartLoadingCount && i < Count; i++, k++)
                    {
                        PostModel.Items[i].Description = loadingHelper.PostModel.Items[i].Description;
                        PostModel.Items[i].IsLoadingPrices = false;
                    }

                    Debug.WriteLine(loadingHelper.LoadingType);
                    loadingHelper.LoadingType = LoadingType.GetRemainsData;
                    if (remainsDataWorker.IsBusy != true)
                        remainsDataWorker.RunWorkerAsync(loadingHelper);
                }
            }
        }

        private void OnDataBackgroundWorkerThree(object sender, DoWorkEventArgs e)
        {
            var loadingHelper = e.Argument as LoadingHelper;
            if (loadingHelper != null)
            {
                loadingHelper.PostModel = Storage.GetModel(loadingHelper.Path).ConfigureAwait(false).GetAwaiter().GetResult();
                //Task.Delay(500).Wait();
                e.Result = loadingHelper;
            }
        }

        private void DataBackgroundWorkerThree_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadingHelper = e.Result as LoadingHelper;
            if (loadingHelper != null)
            {
                if (loadingHelper.PostModel != null && loadingHelper.PostModel.Items != null && loadingHelper.PostModel.Items.Count > 0)
                {
                    for (int i = loadingHelper.StartIndex, k = 0; k < loadingHelper.OnStartLoadingCount && i < Count; i++, k++)
                    {
                        PostModel.Items[i].Type = loadingHelper.PostModel.Items[i].Type;
                        PostModel.Items[i].IsLoadingRemains = false;
                        PostModel.Items[i].IsEnabledBuy = true;
                    }
                    Debug.WriteLine(loadingHelper.LoadingType);
                }
            }
        }

        public void LoadData(double scrollViewerProgress, int page = 1, int startCountLoad = 10)
        {
            if (scrollViewerProgress > 0.999)
            {
                LoadingHelper loadingHelper = new LoadingHelper(LoadingType.GetCount);
                loadingHelper.OnStartLoadingCount = startCountLoad;
                if (getCountWorker.IsBusy != true)
                    getCountWorker.RunWorkerAsync(loadingHelper);
            }
        }

        public void AddDemoData(LoadingHelper loadingHelper)
        {
            var demoPostModel = new T()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };

            for (int i = 0; i < 175; i++)
            {
                demoPostModel.Items.Add(new CurrentPostModel()
                {
                    Id = (uint)(i + 1),
                    Description = "108990р.",
                    Image = null,
                    BuyIcon = "\xE7BF",
                    IsLoading = false,
                    Name = "Ноутбук Apple MacBook Pro 2017 Core i7/16/256 SSD Gold (MNYK2RU/A)",
                    Text = "https://rebabaskett.com/wp-content/uploads/2017/01/u_10150899.jpg",
                    Type = "4 шт."
                });
            }

            Storage.SetModel(loadingHelper.Path, demoPostModel);
        }

        public override T GetModel()
        {
            return PostModel;
        }

        public T PostModel
        {
            get => _PostModel;
            set => SetValue(ref _PostModel, value);
        }
        private T _PostModel;
    }
}
