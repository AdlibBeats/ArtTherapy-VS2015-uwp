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
    public class PostViewModel<T> : BaseViewModel<T> where T : PostModel, new()
    {
        public BaseJsonStorage<T> Storage { get; set; }

        private JsonFactoryStorage<T> _JsonFactoryStorage = new JsonFactoryStorage<T>();

        public event EventHandler<PostEventArgs> Loaded;

        private List<TaskCompletionSource<bool>> _IsLoadedList = new List<TaskCompletionSource<bool>>();

        public PostViewModel()
        {
            PostModel = new T()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };

            Storage = (BaseJsonStorage<T>)_JsonFactoryStorage.Create();

            PageSize = 2;
        }

        public int PageSize { get; set; }

        public int Count { get; private set; }

        public int CurrentCount { get => PostModel.Items.Count; }

        public bool IsFullInitialized { get => Count.Equals(CurrentCount); }

        public void LoadData(ScrollViewer scrollViewer, int page = 1, int startCountLoad = 5)
        {
            if (scrollViewer.GetScrollViewProgress() > 0.999)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        var postModel = Storage.GetModel($"{PostModel.GetType().Name}Count.json") as PostModel;
                        if (postModel != null)
                        {
                            Count = postModel.Count;
                            if (Count > 0 && CurrentCount < Count)
                            {
                                for (int i = 0; i < Count; i++)
                                {
                                    if (i == startCountLoad || CurrentCount == Count) break;
                                    PostModel.Items.Add(new CurrentPostModel());
                                    Debug.WriteLine($"Добавлено {CurrentCount} из {Count}");
                                }
                                int startIndex = PostModel.Items.IndexOf(PostModel.Items.LastOrDefault());
                                if (startIndex >= startCountLoad - 1)
                                {
                                    _IsLoadedList.Add(new TaskCompletionSource<bool>());
                                    startIndex -= startCountLoad - 1;
                                    for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                    {
                                        PostModel.Items[i].IsLoading = true;
                                    }
                                    await Task.Delay(500);

                                    var fullPostModel = Storage.GetModel($"{PostModel.GetType().Name}.json") as PostModel;
                                    if (fullPostModel != null && fullPostModel.Items != null && fullPostModel.Items.Count > 0)
                                    {
                                        for (int i = startIndex, k = 0; k < startCountLoad && i < Count; i++, k++)
                                        {
                                            PostModel.Items[i] = fullPostModel.Items[i];
                                            PostModel.Items[i].IsLoading = false;
                                            await Task.Delay(50);
                                        }
                                        _IsLoadedList.LastOrDefault().TrySetResult(true);
                                    }
                                }
                            }
                            Loaded?.Invoke(this, new PostEventArgs(Count.Equals(CurrentCount)));
                        }
                    });
                });
            }
        }

        public void AddDemoData()
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
                    Name = "Apple MacBook Pro 2017",
                    Text = "https://rebabaskett.com/wp-content/uploads/2017/01/u_10150899.jpg",
                    Type = "4 шт."
                });
            }

            Storage.SetModel($"{PostModel.GetType().Name}.json", demoPostModel);
        }

        public override T GetModel()
        {
            return PostModel;
        }

        public override async void Dispose()
        {
            foreach (var x in _IsLoadedList)
                await x.Task;

            PostModel.Items.Clear();
            PostModel = null;

            _IsLoadedList.Clear();
            _IsLoadedList = null;
        }
        public T PostModel
        {
            get { return (T)GetValue(PostModelProperty); }
            set { SetValue(PostModelProperty, value); }
        }
        
        public static readonly DependencyProperty PostModelProperty =
            DependencyProperty.Register("PostModel", typeof(T), typeof(PostViewModel<T>), new PropertyMetadata(null));
    }
}
