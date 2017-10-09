using ArtTherapy.AppStorage;
using ArtTherapy.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ArtTherapy.ViewModels
{
    public class PostEventArgs : EventArgs
    {
        public bool IsFullInitialized { get; private set; }

        public PostEventArgs(bool isFullInitialized)
        {
            IsFullInitialized = isFullInitialized;
        }
    }
    public class PostViewModel<T> : BaseViewModel<T> where T : PostModel, new()
    {
        public event EventHandler<PostEventArgs> Loaded;

        private List<TaskCompletionSource<bool>> IsLoadedList = new List<TaskCompletionSource<bool>>();

        public PostViewModel()
        {
            PostModel = new T()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };
        }

        public int Count { get; private set; }

        public int CurrentCount { get => PostModel.Items.Count; }

        public bool IsFullInitialized { get => Count.Equals(CurrentCount); }

        public void LoadData(double scrollViewerProgress)
        {
            Task.Factory.StartNew(async () =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    if (scrollViewerProgress > 0.999)
                    {
                        var postModel = await AppStorage<PostModel>.Get("PoetryRepositoryCount.json");
                        if (postModel != null)
                        {
                            Count = postModel.Count;
                            if (Count > 0 && CurrentCount < postModel.Count)
                            {
                                for (int i = 0; i < Count; i++)
                                {
                                    if (i == 20 || CurrentCount == Count) break;
                                    PostModel.Items.Add(new CurrentPostModel());
                                    Debug.WriteLine($"Добавлено {CurrentCount} из {Count}");
                                }
                                int startIndex = PostModel.Items.IndexOf(PostModel.Items.LastOrDefault());
                                if (startIndex >= 19)
                                {
                                    IsLoadedList.Add(new TaskCompletionSource<bool>());
                                    startIndex -= 19;
                                    for (int i = startIndex, k = 0; k < 20 && i < Count; i++, k++)
                                    {
                                        PostModel.Items[i].IsLoading = true;
                                    }
                                    await Task.Delay(1000);

                                    var fullPostModel = await AppStorage<PostModel>.Get("PoetryRepository.json");
                                    if (fullPostModel != null && fullPostModel.Items != null && fullPostModel.Items.Count > 0)
                                    {
                                        for (int i = startIndex, k = 0; k < 20 && i < Count; i++, k++)
                                        {
                                            PostModel.Items[i] = fullPostModel.Items[i];
                                            PostModel.Items[i].IsLoading = false;
                                            await Task.Delay(50);
                                        }
                                        IsLoadedList.LastOrDefault().TrySetResult(true);
                                    }
                                }
                            }
                        }
                        Loaded?.Invoke(this, new PostEventArgs(Count.Equals(CurrentCount)));
                    }
                 });
            });
        }

        public async void AddDemoData()
        {
            var demoPostModel = new PostModel()
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

            await AppStorage<PostModel>.Set(demoPostModel, "PoetryRepository.json");
        }

        public override T GetViewModel()
        {
            return PostModel;
        }

        public override async void Dispose()
        {
            foreach (var x in IsLoadedList)
                await x.Task;

            PostModel.Items.Clear();
            PostModel = null;

            IsLoadedList.Clear();
            IsLoadedList = null;
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
