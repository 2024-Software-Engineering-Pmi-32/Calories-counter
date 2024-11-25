// <copyright file="UsefulArticlesWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public partial class UsefulArticlesWindow : Window
    {
        public UsefulArticlesWindow()
        {
            InitializeComponent();
            this.LoadArticles();
        }

        private void LoadArticles()
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Title = "Як покращити здоров'я: 10 дієвих порад",
                    Url = "https://www.bing.com/search?pglt=171&q=поради+для+здоров%27я&cvid=da583e07f5d442d1944a8892acc6928f&gs_lcrp=EgRlZGdlKgYIABBFGDkyBggAEEUYOdIBCDg3NDJqMGoxqAIIsAIB&FORM=ANNTA1&PC=ASTS",
                },
                new Article
                {
                    Title = "12 порад, як розпочати здоровий спосіб життя",
                    Url = "https://gosta.media/medytsyna/12-porad-iak-rozpochaty-zdorovyj-sposib-zhyttia/",
                },
                new Article
                {
                    Title = "8 тверджень, чому варто пити достатньо води",
                    Url = "https://uamodna.com/articles/8-tverdzhenj-chomu-varto-pyty-dostatnjo-vody/",
                },
                new Article
                {
                    Title = "Як легко перейти на правильне харчування та швидко не зірватися",
                    Url = "https://www.online.ua/guide/yak-legko-pereyti-na-zdorove-harchuvannya-ta-shvidko-ne-zirvatisya-841690/#:~:text=%D0%BF%D1%80%D0%B8%D0%B9%D0%BE%D0%BC%20%D1%97%D0%B6%D1%96%20%D0%BC%D0%B0%D1%94%20%D0%B1%D1%83%D1%82%D0%B8%20%C2%AB%D0%B7%D0%B0%20%D1%80%D0%BE%D0%B7%D0%BA%D0%BB%D0%B0%D0%B4%D0%BE%D0%BC%C2%BB%20%E2%80%93%20%D0%B2,%D1%81%D0%BD%D1%96%D0%B4%D0%B0%D0%BD%D0%BE%D0%BA%2C%20%D1%82%D0%BE%D0%B4%D1%96%20%D1%8F%D0%BA%20%D0%B2%D0%B2%D0%B5%D1%87%D0%B5%D1%80%D1%96%20%D0%BA%D1%80%D0%B0%D1%89%D0%B5%20%D0%BE%D0%B1%D1%96%D0%B9%D1%82%D0%B8%D1%81%D1%8F%20%D0%BB%D0%B5%D0%B3%D0%BA%D0%B8%D0%BC%D0%B8%20%D1%81%D1%82%D1%80%D0%B0%D0%B2%D0%B0%D0%BC%D0%B8%3B",
                },
            };

            ArticlesListBox.ItemsSource = articles;
        }

        private void ArticlesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ArticlesListBox.SelectedItem is Article selectedArticle && !string.IsNullOrEmpty(selectedArticle.Url))
            {
                _ = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = selectedArticle.Url,
                    UseShellExecute = true,
                });
            }
        }
    }

    public class Article
    {
        public string Title { get; set; }

        public string Url { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Title;
        }
    }
}
