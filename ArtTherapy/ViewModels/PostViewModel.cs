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
using Windows.UI.Xaml;

namespace ArtTherapy.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> Initialized;

        public string Title { get; private set; }

        public PostViewModel(string title)
        {
            Title = title;
            PostModel = new PostModel()
            {
                Items = new ObservableCollection<CurrentPostModel>()
            };
        }

        public int Count { get; set; }

        //public bool IsLoadedComplete { get; set; } = false;

        public async void Initialize()
        {
            var postModelCount = await AppStorage<PostModel>.Get(Title + "Count" + ".json");
            if (postModelCount != null)
            {
                Count = postModelCount.Count;

                if (Count > 0 && PostModel.Items.Count < Count)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (i == 20 || PostModel.Items.Count == Count) break;
                        PostModel.Items.Add(new CurrentPostModel());
                        Debug.WriteLine($"{PostModel.Items.Count} --- {Count}");
                    }
                    int startIndex = PostModel.Items.IndexOf(PostModel.Items.LastOrDefault());
                    Debug.WriteLine($"-------------- {startIndex}");
                    if (startIndex >= 19)
                    {
                        startIndex -= 19;
                        for (int i = startIndex, k = 0; k < 20 && i < Count; i++, k++)
                        {
                            PostModel.Items[i].IsLoading = true;
                        }
                        await Task.Delay(2000);

                        var postModel = await AppStorage<PostModel>.Get(Title + ".json");
                        if (postModel != null && postModel.Items != null && postModel.Items.Count > 0)
                        {

                            for (int i = startIndex, k = 0; k < 20 && i < Count; i++, k++)
                            {
                                PostModel.Items[i] = postModel.Items[i];
                                PostModel.Items[i].IsLoading = false;
                            }
                        }
                    }
                    await Task.Delay(2000);
                }
            }
            Initialized?.Invoke(this, new EventArgs());
        }
        public PostModel PostModel
        {
            get { return (PostModel)GetValue(PostModelProperty); }
            set
            {
                SetValue(PostModelProperty, value);
                OnPropertyChanged(nameof(PostModel));
            }
        }
        
        public static readonly DependencyProperty PostModelProperty =
            DependencyProperty.Register("PostModel", typeof(PostModel), typeof(PostViewModel), new PropertyMetadata(null));
    }
}
