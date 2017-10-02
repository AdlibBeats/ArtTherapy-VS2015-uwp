using ArtTherapy.AppStorage;
using ArtTherapy.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArtTherapy.Extensions;

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
    public class PostViewModel : BaseViewModel
    {
        public event EventHandler<PostEventArgs> Loaded;

        public TaskCompletionSource<bool> IsLoaded = new TaskCompletionSource<bool>();

        public PostViewModel()
        {
            PostModel = new PostModel()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };
        }

        public async Task LoadData(double scrollViewerProgress)
        {
            await Task.Factory.StartNew(async () =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        if (scrollViewerProgress > 0.999)
                        {
                            var postModel = await AppStorage<PostModel>.Get("PoetryRepositoryCount.json");
                            if (postModel != null)
                            {
                                if (postModel.Count > 0 && PostModel.Items.Count < postModel.Count)
                                {
                                    for (int i = 0; i < postModel.Count; i++)
                                    {
                                        if (i == 20 || PostModel.Items.Count == postModel.Count) break;
                                        PostModel.Items.Add(new CurrentPostModel());
                                        Debug.WriteLine($"Добавлено {PostModel.Items.Count} из {postModel.Count}");
                                    }
                                    int startIndex = PostModel.Items.IndexOf(PostModel.Items.LastOrDefault());
                                    if (startIndex >= 19)
                                    {
                                        startIndex -= 19;
                                        for (int i = startIndex, k = 0; k < 20 && i < postModel.Count; i++, k++)
                                        {
                                            PostModel.Items[i].IsLoading = true;
                                        }
                                        await Task.Delay(1000);

                                        var fullPostModel = await AppStorage<PostModel>.Get("PoetryRepository.json");
                                        if (fullPostModel != null && fullPostModel.Items != null && fullPostModel.Items.Count > 0)
                                        {
                                            for (int i = startIndex, k = 0; k < 20 && i < postModel.Count; i++, k++)
                                            {
                                                PostModel.Items[i] = fullPostModel.Items[i];
                                                PostModel.Items[i].IsLoading = false;
                                                await Task.Delay(50);
                                            }
                                        }
                                    }
                                }
                            }
                            Loaded?.Invoke(this, new PostEventArgs(postModel.Count == PostModel.Items.Count));
                        }
                    });
            });
        }

        public PostModel PostModel
        {
            get { return (PostModel)GetValue(PostModelProperty); }
            set { SetValue(PostModelProperty, value); }
        }
        
        public static readonly DependencyProperty PostModelProperty =
            DependencyProperty.Register("PostModel", typeof(PostModel), typeof(PostViewModel), new PropertyMetadata(null));
    }
}
