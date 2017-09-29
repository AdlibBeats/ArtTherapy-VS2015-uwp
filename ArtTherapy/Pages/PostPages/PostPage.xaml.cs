using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ArtTherapy.AppStorage;
using ArtTherapy.Models.PostModels;
using Windows.Storage;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ArtTherapy.Pages.PostPages
{
    public sealed partial class PostPage : Page, IPage
    {
        public uint Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private uint _Id;

        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _Title;

        public NavigateEventTypes NavigateEventType
        {
            get { return _NavigateEventType; }
            set
            {
                _NavigateEventType = value;
                OnPropertyChanged(nameof(NavigateEventType));
            }
        }
        private NavigateEventTypes _NavigateEventType;

        public Frame RootFrame
        {
            get { return _RootFrame; }
            set
            {
                _RootFrame = value;
                OnPropertyChanged(nameof(RootFrame));
            }
        }
        private Frame _RootFrame;

        public event EventHandler<EventArgs> Initialized;

        public PostPage()
        {
            this.InitializeComponent();

            Id = 2;
            Title = "Стихи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
            Initialized?.Invoke(this, new EventArgs());
        }

        public PostPage(uint id, string title)
        {
            this.InitializeComponent();

            Id = id;
            Title = title;
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
            Initialized?.Invoke(this, new EventArgs());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //_CurrentPostModel = value;
            //var file = await SetAsync("path", _CurrentPostModel);
//            var postModel = new CurrentPostModel()
//            {
//                Id = 1,
//                Name= "ШКАТУЛКА С ЖЕЛАНИЯМИ",
//                Text =
//                @"ШКАТУЛКА С ЖЕЛАНИЯМИ

//У каждого из нас есть маленькие и большие заветные желания. И мы аккуратно их собираем в волшебную шкатулку и оберегаем от посторонних глаз! Еще бы, ведь наши мечты – как дети, и очень хочется, чтобы они сбылись! Словно радужные самоцветы, они переливаются в наших руках и жаждут поскорее родиться на свет и воплотиться в жизнь!

//Но почему же тогда у одних людей мечты сбываются легко и просто, а другие годами бьются как рыба об лед? Или почему второстепенные желания сбываются с наименьшими усилиями, а те самые, заветные, не исполняются? Ответов может быть очень много - это и неумение ждать, и отсутствие необходимой энергии, и бездействие. Но самые главные причины лежат на поверхности – это, как правило, завышенная важность и слишком глобальные мечты.

//Подумайте, что вы хотите на самом деле, когда мечтаете о муже, деньгах, красивой машине? Ведь очень даже возможно, вы просто мечтаете о тех эмоциях, которые подарит вам ваша мечта. Потратив деньги, вы приобретете чувство радости от покупок или интересных мест, куда вы сможете сходить. Машина поможет усилить ваше чувство значимости. Муж станет надежным плечом, с которым будет легче преодолевать жизненные трудности. Конечно, мечтать на полную катушку никто не запрещал. Но что вы можете сделать прямо сейчас, чтобы стать счастливым? Пока вы не заработали кучу денег, почувствуйте радость от бесплатных вещей - красивого заката, интересной книги, общения с другом или ароматной ванной. Пока вы не накопили на машину, сделайте что-то, что повысит ваше чувство значимости и заставит вас уважать. Предложите интересный проект на работе, научитесь готовить яблочный пирог, похудейте, в конце концов. Чувство надежности вам может дать ваша семья, ну если это невозможно, станьте самому себе надежным помощником – тем более, этот навык наверняка пригодиться вам в жизни. А когда маленькие мечты исполнятся, тогда легче будет двигаться к глобальным целям.

//Кроме того, на осуществление нашего желания влияет степень важности, которую мы придаем своей цели. Ведь согласитесь, можно топать ножкой и недовольно ждать, когда же я получу желаемое, а можно спокойно заниматься своими делами, не придавая мечтам излишнего значения. Предположим, это выглядит так. Моя цель - встретить мужчину своей мечты. Что я могу сделать в такой ситуации? Я могу записать свое желание на бумаге, произносить аффирмации, осуществлять реальные физические действия (поддерживать свое тело в форме, читать книги, повышать свой интеллектуальный уровень, вести наполненную интересную жизнь, пройти курсы женственности и сексуальности, зарегистрироваться на сайте знакомств). А самое главное, посыл должен быть такой: если я встречу своего идеального мужчину – отлично! Спасибо, Вселенная, за ценный подарок! Если не получается – ничего страшного, у меня и так яркая жизнь, работа, хобби, подруги и книги.

//Энергией для шкатулки желаний является также живительная благодарность, без которой не сбудется ни одна мечта и позитивный настрой, с которым легче идти к своей цели. А самое главное, важно не забывать - что наша задача быть счастливым здесь и сейчас, а не где-то в далеком необозримом будущем.

//Автор: Юлия Свиридова"
//            };

//            string url = "lol.json";
//            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
//            await AppStorage.AppStorage.SetAsync(url, postModel);
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
        //    pr.IsActive = true;
        //    pr.Visibility = Visibility.Visible;
        //    richTextBox.Visibility = Visibility.Collapsed;
        //    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
        //    {
        //        var reader = await AppStorage.AppStorage.GetAsync("lol.json");
        //        if (reader != null)
        //        {
        //            var post = await Task.Factory.StartNew(() =>
        //            {
        //                return JsonConvert.DeserializeObject<CurrentPostModel>(reader);
        //            });
        //            if (post != null)
        //                richTextBox.Text = post.Text;
        //        }
        //        pr.IsActive = false;
        //        pr.Visibility = Visibility.Collapsed;
        //        richTextBox.Visibility = Visibility.Visible;
        //    });
        }
    }
}
