﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using HelloXFPrism.Models;
using HelloXFPrism.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace HelloXFPrism.ViewModels
{
    public class ItemsViewModel : BaseViewModel, INavigatedAware
    {
        private readonly INavigationService navigationService;
        private readonly IDataStore<Item> itemDataStore;
        private Command loadItemsCommand;
        private Command addItemCommand;
        private Item selectedItem;

        public ItemsViewModel(INavigationService navigationService, IDataStore<Item> itemDataStore)
        {
            this.navigationService = navigationService;
            this.itemDataStore = itemDataStore;

            this.Title = "Browse";
            this.Items = new ObservableCollection<Item>();
        }

        public ObservableCollection<Item> Items { get; }

        public Item SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (this.SetProperty(ref this.selectedItem, value, nameof(this.SelectedItem)))
                {
                    if (value != null)
                    {
                        var navigationParams = new NavigationParameters { { "model", value } };
                        this.navigationService.NavigateAsync("ItemDetailPage", navigationParams);
                        this.selectedItem = null;
                    }
                }
            }
        }

        public ICommand LoadItemsCommand
        {
            get { return this.loadItemsCommand ?? (this.loadItemsCommand = new Command(async () => await this.ExecuteLoadItemsCommand())); }
        }

        public ICommand AddItemCommand
        {
            get { return this.addItemCommand ?? (this.addItemCommand = new Command(async () => { await this.navigationService.NavigateAsync("NewItemPage"); })); }
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            try
            {
                this.Items.Clear();
                var items = await this.itemDataStore.GetItemsAsync(forceRefresh: true);
                foreach (var item in items)
                {
                    this.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.New)
            {
                this.LoadItemsCommand.Execute(null);
            }
            if (navigationMode == NavigationMode.Back)
            {
                var isNewItem = parameters["isNewItem"] as bool?;
                if (isNewItem.HasValue && isNewItem.Value)
                {
                    this.LoadItemsCommand.Execute(null);
                }
            }
        }
    }
}