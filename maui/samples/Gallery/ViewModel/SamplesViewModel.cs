﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Xml;

namespace Syncfusion.Maui.ControlsGallery
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SamplesViewModel : Element , INotifyPropertyChanged
    {

        IList<ControlModel> allControlList = [];
        List<SearchModel>? list = [];
        List<SearchModel>? newSampleList = [];
        List<SearchModel>? updatedSampleList = [];
        internal bool exit = false;
        bool exitLoop = true;
        Assembly? currentAssembly;
        String? currentControlOfAssembly;
        string? ControlShortName;

        /// <summary>
        /// 
        /// </summary>
        public int FilterNewSampleCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public int FilterUpdatedSampleCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public new event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
		}
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ControlCategoryModel> AllControlCategories
        {
            get { return (ObservableCollection<ControlCategoryModel>)GetValue(AllControlCategoriesProperty); }
            set { SetValue(AllControlCategoriesProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        // Using a DependencyProperty as the backing store for AllControlCategories.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty AllControlCategoriesProperty =
            BindableProperty.Create("AllControlCategories", typeof(ObservableCollection<ControlCategoryModel>), typeof(SamplesViewModel), null);

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> MainPageList
        {
            get { return (ObservableCollection<object>)GetValue(MainPageListProperty); }
            set { SetValue(MainPageListProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SortedListProperty =
            BindableProperty.Create("SortedList", typeof(ObservableCollection<SearchModel>), typeof(SamplesViewModel), null, BindingMode.TwoWay);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SortedListNewProperty =
            BindableProperty.Create("SortedListNew", typeof(ObservableCollection<SearchModel>), typeof(SamplesViewModel), null, BindingMode.TwoWay);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SortedListUpdatedProperty =
            BindableProperty.Create("SortedListUpdated", typeof(ObservableCollection<SearchModel>), typeof(SamplesViewModel), null, BindingMode.TwoWay);

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SearchModel> SortedList
        {
            get { return (ObservableCollection<SearchModel>)GetValue(SortedListProperty); }
            set { SetValue(SortedListProperty, value); OnPropertyRaised(nameof(SortedList)); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SearchModel> SortedListNew
        {
            get { return (ObservableCollection<SearchModel>)GetValue(SortedListNewProperty); }
            set { SetValue(SortedListNewProperty, value); OnPropertyRaised(nameof(SortedListNew)); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SearchModel> SortedListUpdated
        {
            get { return (ObservableCollection<SearchModel>)GetValue(SortedListUpdatedProperty); }
            set { SetValue(SortedListUpdatedProperty, value); OnPropertyRaised(nameof(SortedListUpdated)); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty UpdatedSortedListProperty =
            BindableProperty.Create("UpdatedSortedList", typeof(ObservableCollection<ControlModel>), typeof(SamplesViewModel), null, BindingMode.TwoWay);


        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ControlModel> UpdatedSortedList
        {
            get { return (ObservableCollection<ControlModel>)GetValue(UpdatedSortedListProperty); }
            set { SetValue(UpdatedSortedListProperty, value); OnPropertyRaised(nameof(UpdatedSortedList)); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SortOptionProperty =
            BindableProperty.Create("SortOption", typeof(SortOption), typeof(SamplesViewModel), null, BindingMode.TwoWay);


        /// <summary>
        /// 
        /// </summary>
        public SortOption SortOption
        {
            get { return (SortOption)GetValue(SortOptionProperty); }
            set { SetValue(SortOptionProperty, value); OnPropertyRaised(nameof(SortOption)); }
        }

        /// <summary>
        /// 
        /// </summary>
        // Using a DependencyProperty as the backing store for MainPageList.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty MainPageListProperty =
            BindableProperty.Create("MainPageList", typeof(ObservableCollection<object>), typeof(SamplesViewModel), null);

        /// <summary>
        /// 
        /// </summary>
        public SamplesViewModel()
        {
            AllControlCategories = [];
            MainPageList = [];
            GetControlLists();
            GetAllControls();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetControlLists()
        {
            currentAssembly = BaseConfig.AssemblyName;
            foreach (var item in BaseConfig.AvailableControlStreamList)
            {
                ControlShortName = item.Key;
                ReadStream(item.Value);
            }

            foreach (var item in AllControlCategories)
            {
                MainPageList.Add(item);
                if (item is ControlCategoryModel controlCategoryModel)
                {
                    foreach (var control in controlCategoryModel.AllControls!)
                    {
                        MainPageList.Add(control);

                        foreach (var category in control.SampleCategories!)
                        {
                            if (category.SampleSubCategories?.Count > 1)
							{
								category.HasCategory = true;
							}
							else if(category.SampleSubCategories?.Count == 1)
							{
								category.SampleSubCategories![0].IsSubCategory = false;
							}
						}

                    }
                }
            }
        }

        /// <summary>
        /// To add all control to the list
        /// </summary>
        private void GetAllControls()
        {
            foreach (var item in AllControlCategories)
            {
                if (item is ControlCategoryModel controlCategoryModel)
                {
                    foreach (var control in controlCategoryModel.AllControls!)
                    {
                        if (control is ControlModel controlModel)
                        {
                            allControlList.Add(controlModel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To get the sorted list
        /// </summary>
        /// <param name="filterList">User selection filtered list</param>
        internal void GetSortedList(List<string> filterList)
        {
            if (UpdatedSortedList != null)
            {
                UpdatedSortedList.Clear();
            }
            else
            {
                UpdatedSortedList = [];
                OnPropertyRaised("UpdatedSortedList");
            }
            IList<ControlModel> sortedAndFilteredList = [];
            if (filterList.Contains("AllSamples") || filterList.Count == 0)
            {
                sortedAndFilteredList = allControlList;
            }
            else
            {
                foreach (var item in allControlList)
                {
                    if (filterList.Contains("NewSamples") && item.StatusTag == "New")
                    {
                        sortedAndFilteredList.Add(item);
                    }
                    if (filterList.Contains("UpdatedSamples") && item.StatusTag == "Updated")
                    {
                        sortedAndFilteredList.Add(item);
                    }
                }
            }

            if (SortOption == SortOption.Ascending)
            {
                sortedAndFilteredList = [.. sortedAndFilteredList.OrderBy(x => x.Title != null ? x.Title.ToString() : string.Empty)];
            }
            else if (SortOption == SortOption.Descending)
            {
                sortedAndFilteredList = [.. sortedAndFilteredList.OrderByDescending(x => x.Title != null ? x.Title.ToString() : string.Empty)];
            }
            UpdatedSortedList = new ObservableCollection<ControlModel>(sortedAndFilteredList);

        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SearchModel> SearchList
        {
            get { return (ObservableCollection<SearchModel>)GetValue(SearchListProperty); }
            set { SetValue(SearchListProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        // Using a DependencyProperty as the backing store for MainPageList.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty SearchListProperty =
            BindableProperty.Create("SearchList", typeof(ObservableCollection<SearchModel>), typeof(SamplesViewModel), null);


        /// <summary>
        /// 
        /// </summary>
        public String SearchText
        {
            get { return (String)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        // Using a DependencyProperty as the backing store for MainPageList.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create("SearchText", typeof(String), typeof(SamplesViewModel), null, BindingMode.TwoWay,null, OnSearchTextPropertyChanged);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnSearchTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var searchText = newValue.ToString();

            if (bindable is SamplesViewModel sender && searchText != null)
            {
                if (searchText.Length < 2)
                {
                    sender.SearchList?.Clear();
                    return;
                }

                sender.exitLoop = true;
                sender.PopulateSearchItem(searchText);


            }
        }

        internal void PopulateSearchItem(string searchText)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                exitLoop = true;
                PopulateSearchText(searchText);
            });
        }

        private bool ExitLoop()
        {
            if (exitLoop)
            {
                SearchList?.Clear();
            }
            return exitLoop;
        }

        private static int GetDelay()
        {
            if (Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
			{
				return  40;
			}
			else
			{
				return 50;
			}
		}

        private async void PopulateSearchText(string searchText)
        {

               await Task.Delay(SamplesViewModel.GetDelay());
            if (SearchList != null)
            {
                SearchList.Clear();
            }
            else
            {
                SearchList = [];
                OnPropertyRaised("SearchList");
            }
            exitLoop = false;


            foreach (var item in AllControlCategories)
            {
                if (ExitLoop())
				{
					return;
				}

				foreach (var controlModel in item.AllControls!)
                {
                    if (ExitLoop())
					{
						return;
					}

					foreach (var sampleCategoryModel in controlModel.SampleCategories!)
                    {
                        if (ExitLoop())
						{
							return;
						}

						foreach (var sampleSubCategoryModel in sampleCategoryModel.SampleSubCategories!)
                        {
                            if (ExitLoop())
							{
								return;
							}

							foreach (var cardLayoutModel in sampleSubCategoryModel.CardLayouts!)
                            {
                                if (ExitLoop())
								{
									return;
								}

								var show = cardLayoutModel.Samples?.Where(a => (a.ControlName != null && a.ControlName.ToLowerInvariant().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)) || (a.SampleName != null && a.SampleName.ToLowerInvariant().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)) || (a.Title != null && a.Title.ToLowerInvariant().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)) || (a.SearchTags != null && a.SearchTags.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)));
                                if (show != null)
                                {
                                    if (ExitLoop())
									{
										return;
									}

									foreach (var samples in show)
                                    {
                                        if (ExitLoop())
										{
											return;
										}

										SearchList.Add(
                                            new SearchModel()
                                            {
                                                Sample = samples,
                                                Control = controlModel
                                            });
                                        if(sampleCategoryModel.CategoryName != samples.Title)
										{
											samples.CategoryName = "(" + sampleCategoryModel.CategoryName +")";
										}

										if (ExitLoop())
										{
											return;
										}

										if (SearchList.Count % 5 == 0 || Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout == SBLayout.Desktop)
										{
											await Task.Delay(SamplesViewModel.GetDelay());
										}

										if (ExitLoop())
										{
											return;
										}
									}
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void PopulateSortAndFilterSamples(List<string> filteredList)
        {
#if WINDOWS || MACCATALYST
            FilterNewSampleCount = 0;
            FilterUpdatedSampleCount = 0;
            if (SortedList != null)
            {
                SortedList.Clear();                
            }
            else
            {
                SortedList = [];
                OnPropertyRaised("SortedList");
            }
             list?.Clear();
#else

            if (SortedListNew != null)
            {
                SortedListNew.Clear();
            }
            else
            {
                SortedListNew = [];
                OnPropertyRaised("SortedListNew");
            }

            if (SortedListUpdated != null)
            {
                SortedListUpdated.Clear();
            }
            else
            {
                SortedListUpdated = [];
                OnPropertyRaised("SortedListUpdated");
            }
            newSampleList?.Clear();
            updatedSampleList?.Clear();
#endif


            foreach (var item in AllControlCategories)
            {
                foreach (var controlModel in item.AllControls!)
                {
                    foreach (var sampleCategoryModel in controlModel.SampleCategories!)
                    {
                        foreach (var sampleSubCategoryModel in sampleCategoryModel.SampleSubCategories!)
                        {
                            foreach (var cardLayoutModel in sampleSubCategoryModel.CardLayouts!)
                            {
                                foreach (var samples in cardLayoutModel.Samples!)
                                {
                                    if (controlModel.StatusTag == "New" && filteredList.Contains("NewSamples"))
                                    {
#if WINDOWS || MACCATALYST
                                       AddSortAndFilterList(controlModel, samples);
#else
                                        AddNewSamples(controlModel, samples);
#endif
                                        FilterNewSampleCount++;
                                    }

                                    else if (filteredList.Contains("UpdatedSamples") && (controlModel.StatusTag == "Updated" && (sampleSubCategoryModel.StatusTag == "New" || samples.StatusTag == "New" || sampleCategoryModel.StatusTag == "New")))
                                    {
#if WINDOWS || MACCATALYST
                                       AddSortAndFilterList(controlModel, samples);
#else
                                        AddUpdatedSamples(controlModel, samples);
#endif
                                        FilterUpdatedSampleCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            SortMethod();
        }

        void SortMethod()
        {
            if (SortOption == SortOption.Ascending)
            {
#if WINDOWS || MACCATALYST
                list = list?.OrderBy(x => x.Control?.Title!).ToList();
#else
                newSampleList = newSampleList?.OrderBy(x => x.Control?.Title!).ToList();
                updatedSampleList = updatedSampleList?.OrderBy(x => x.Control?.Title!).ToList();
#endif
            }
            else if (SortOption == SortOption.Descending)
            {
#if WINDOWS || MACCATALYST
                list = list?.OrderByDescending(x => x.Control?.Title!).ToList();
#else
                newSampleList = newSampleList?.OrderByDescending(x => x.Control?.Title!).ToList();
                updatedSampleList = updatedSampleList?.OrderByDescending(x => x.Control?.Title!).ToList();
#endif
            }
            exit = false;
#if WINDOWS || MACCATALYST
            foreach (var temp in list!)
            {
                if (exit)
				{
					return;
				}

				SortedList?.Add(temp);
            }
#else
            foreach (var temp in newSampleList!)
            {
                if (exit)
				{
					return;
				}

				SortedListNew?.Add(temp);
            }
            foreach (var temp in updatedSampleList!)
            {
                if (exit)
				{
					return;
				}

				SortedListUpdated?.Add(temp);
            }
#endif
        }

#if WINDOWS || MACCATALYST
        void AddSortAndFilterList(ControlModel controlModel, SampleModel samples)
        {
            if(this.list != null)
            {
                list.Add(new SearchModel() { Control = controlModel, Sample = samples });
            }
        }
#else
		void AddNewSamples(ControlModel controlModel, SampleModel samples)
		{
			if (this.newSampleList != null)
			{
				newSampleList.Add(new SearchModel() { Control = controlModel, Sample = samples });
			}
		}
		void AddUpdatedSamples(ControlModel controlModel, SampleModel samples)
		{
			if (this.updatedSampleList != null)
			{
				updatedSampleList.Add(new SearchModel() { Control = controlModel, Sample = samples });
			}
		}
#endif
		/// <summary>
		/// 
		/// </summary>
		/// <param name="controlListStream"></param>
		public void ReadStream(Stream controlListStream)
        {
            if (controlListStream != null)
            {
                using var reader = new StreamReader(controlListStream);

                var controlXMLDocument = new XmlDocument();

                controlXMLDocument.Load(controlListStream);
                XmlNodeList? sampleNodes = controlXMLDocument.SelectNodes("SyncfusionControls");
                if (sampleNodes != null)
                {
                    foreach (XmlNode xmlNode in sampleNodes)
                    {
                        XmlElement element = (XmlElement)xmlNode;

                        foreach (var item in element.ChildNodes)
                        {
                            if (item is XmlElement xmlItem)
                            {
                                if (xmlItem.Name == "ControlCategory")
                                {
                                    var xmlControlCategoryItemName = xmlItem.GetAttribute("Name");
                                    var controlCatagoryObj = GetControlCategoryModel(xmlControlCategoryItemName);
                                    foreach (var controlItem in xmlItem.ChildNodes)
                                    {
                                        if (controlItem is XmlElement xmlControlItem)
                                        {
                                            if (xmlControlItem.Name == "Control")
                                            {
                                                var dummyControlModel = CreateControlModelObject(xmlControlItem);
                                                var controlModel = SamplesViewModel.GetControlModel(dummyControlModel, controlCatagoryObj);
                                                CheckForModel(controlModel, xmlControlItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemModel"></param>
        /// <param name="xmlElement"></param>
        public void CheckForModel(object ItemModel, XmlElement xmlElement)
        {
            foreach (var item in xmlElement.ChildNodes)
            {
                if (item is XmlElement catagoryItem)
                {

                    if (catagoryItem.Name == "Category")
                    {
                        DirectSampleCategoryModel(ItemModel, catagoryItem);
                    }
                    else if (catagoryItem.Name == "SubCategory")
                    {
                        DirectSampleSubCategoryModel(ItemModel, catagoryItem);
                    }
                    else if (catagoryItem.Name == "CardLayout")
                    {
                        DirectCardLayoutModel(ItemModel, catagoryItem);
                    }
                    else if (catagoryItem.Name == "Sample")
                    {
                        DirectSampleModel(ItemModel, catagoryItem);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public ControlModel CreateControlModelObject(XmlElement xmlElement)
        {
            var newControlModel = new ControlModel
            {
                Name = xmlElement.GetAttribute("ControlName"),
                Title = xmlElement.GetAttribute("Title"),
                Description = xmlElement.GetAttribute("Description"),
                Image = xmlElement.GetAttribute("Image").ToLowerInvariant(),
                StatusTag = xmlElement.GetAttribute("StatusTag"),
                DisplayName = xmlElement.GetAttribute("DisplayName")
            };
            var val = xmlElement.GetAttribute("IsPreview");
            if(val.ToLowerInvariant()=="true")
            {
                newControlModel.IsPreview = true;
            }

            currentControlOfAssembly = newControlModel.Name;

            return newControlModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catagorySubModel"></param>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        public static SampleSubCategoryModel GetSubCategoryModel(SampleSubCategoryModel catagorySubModel, SampleCategoryModel categoryModel)
        {
            categoryModel.SampleSubCategories ??= [];

			foreach (var item in categoryModel.SampleSubCategories)
            {
                if (item.SubCategoryName == catagorySubModel.SubCategoryName)
                {
                    return item;
                }
            }

            categoryModel.SampleSubCategories.Add(catagorySubModel);
            return catagorySubModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catagoryModel"></param>
        /// <param name="controlModel"></param>
        /// <returns></returns>
        public static SampleCategoryModel GetCategoryModel(SampleCategoryModel catagoryModel, ControlModel controlModel)
        {
            controlModel.SampleCategories ??= [];

			foreach (var item in controlModel.SampleCategories)
            {
                if (item.CategoryName == catagoryModel.CategoryName)
                {
                    return item;
                }
            }

            controlModel.SampleCategories.Add(catagoryModel);
            return catagoryModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlModel"></param>
        /// <param name="controlCategoryModel"></param>
        /// <returns></returns>
        public static ControlModel GetControlModel(ControlModel controlModel, ControlCategoryModel controlCategoryModel)
        {
            controlCategoryModel.AllControls ??= [];

			foreach (var item in controlCategoryModel.AllControls)
            {
                if (item.Name == controlModel.Name)
                {
                    return item;
                }
            }

            controlCategoryModel.AllControls.Add(controlModel);

            return controlModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlCategoryModelName"></param>
        /// <returns></returns>
        public ControlCategoryModel GetControlCategoryModel(string controlCategoryModelName)
        {
            foreach (var item in AllControlCategories)
            {
                if (controlCategoryModelName == item.Name)
                {
                    return item;
                }
            }

            var newControlCategoryModel = new ControlCategoryModel() { Name = controlCategoryModelName };
            AllControlCategories.Add(newControlCategoryModel);

            return newControlCategoryModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public string currentCategoryName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string currentSubCategoryName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlSampleElement"></param>
        /// <returns></returns>
        public SampleModel CreateSampleModel(XmlElement xmlSampleElement)
        {
            var sampleModel = new SampleModel
            {
                Title = xmlSampleElement.GetAttribute("Title"),
                SampleName = xmlSampleElement.GetAttribute("SampleName"),
                Description = xmlSampleElement.GetAttribute("Description"),
                StatusTag = xmlSampleElement.GetAttribute("StatusTag"),
                SearchTags = xmlSampleElement.GetAttribute("SearchTags"),
                ShowExpandIcon = xmlSampleElement.GetAttribute("ShowExpandIcon").Equals("False", StringComparison.Ordinal) ? false : true,
                IsGettingStarted = xmlSampleElement.GetAttribute("IsGettingStarted").Equals("True", StringComparison.Ordinal) ? true : false,
                CodeViewerPath = xmlSampleElement.GetAttribute("CodeViewerPath"),
                Platforms = xmlSampleElement.GetAttribute("Platforms"),
                VideoLink = xmlSampleElement.GetAttribute("VideoLink"),
                SourceLink = xmlSampleElement.GetAttribute("SourceLink"),
                WhatsNew = xmlSampleElement.GetAttribute("WhatsNew"),
                AssemblyName = currentAssembly,
                ControlName = currentControlOfAssembly,
                ControlShortName = ControlShortName
            };
            if (currentCategoryName != String.Empty)
            {
                sampleModel.SamplePath = currentCategoryName;
                currentCategoryName = string.Empty;
            }
            if (currentSubCategoryName != String.Empty)
            {
                sampleModel.SamplePath = "/ " + currentSubCategoryName;
                currentSubCategoryName = string.Empty;
            }
            sampleModel.SamplePath = "/ " + sampleModel.Title;
            return sampleModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        /// <param name="xmlElement"></param>
        public void DirectSampleModel(object itemModel, XmlElement xmlElement)
        {
            if (itemModel is ControlModel controlModel)
            {
                controlModel.SampleCategories ??= [];

				var sampleModel = CreateSampleModel(xmlElement);
                if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                {
                    var dummyCardLayoutModel = SamplesViewModel.GetDummayCardLayoutModel();
                    var dummySampleSubCategoryModel = SamplesViewModel.GetDummySampleSubCategoryModel();
                    var dummySampleCategoryModel = SamplesViewModel.GetDummySampleCategoryModel();
                    dummySampleCategoryModel.HasCategory = false;

                    dummySampleCategoryModel.CategoryName = sampleModel.Title;
                    dummyCardLayoutModel.Samples?.Add(sampleModel);
                    dummySampleSubCategoryModel.CardLayouts?.Add(dummyCardLayoutModel);
                    dummySampleCategoryModel.SampleSubCategories?.Add(dummySampleSubCategoryModel);
                    controlModel.SampleCategories.Add(dummySampleCategoryModel);
                    dummySampleCategoryModel.StatusTag = sampleModel.StatusTag;

                    sampleModel.SamplePath = sampleModel.Title;
                }

            }
            else if (itemModel is SampleCategoryModel categoryModel)
            {
                categoryModel.SampleSubCategories ??= [];

				var sampleModel = CreateSampleModel(xmlElement);
                if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                {
                    var dummyCardLayoutModel = SamplesViewModel.GetDummayCardLayoutModel();
                    var dummySampleSubCategoryModel = SamplesViewModel.GetDummySampleSubCategoryModel();
                    categoryModel.HasCategory = false;

                    dummySampleSubCategoryModel.SubCategoryName = sampleModel.Title;
                    dummyCardLayoutModel.Samples?.Add(sampleModel);
                    dummySampleSubCategoryModel.CardLayouts?.Add(dummyCardLayoutModel);
                    categoryModel.SampleSubCategories.Add(dummySampleSubCategoryModel);
                    dummySampleSubCategoryModel.StatusTag = sampleModel.StatusTag;
                    sampleModel.SamplePath = categoryModel.CategoryName + " / " + sampleModel.Title;
                }
            }
            else if (itemModel is SampleSubCategoryModel subCategoryModel)
            {
                subCategoryModel.CardLayouts ??= [];

				var sampleModel = CreateSampleModel(xmlElement);
                if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                {
                    var dummyCardLayoutModel = SamplesViewModel.GetDummayCardLayoutModel();

                    subCategoryModel.SubCategoryName = sampleModel.Title;
                    dummyCardLayoutModel.Samples?.Add(sampleModel);
                    subCategoryModel.CardLayouts.Add(dummyCardLayoutModel);

                    sampleModel.SamplePath += " / " + sampleModel.Title;
                }
            }
            else if (itemModel is CardLayoutModel cardLayoutModel)
            {
                cardLayoutModel.Samples ??= [];

				var sampleModel = CreateSampleModel(xmlElement);
                if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                {
                    cardLayoutModel.Title = sampleModel.Title;
                    cardLayoutModel.Samples.Add(sampleModel);
                    sampleModel.SamplePath += " / " + sampleModel.Title;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SampleCategoryModel GetDummySampleCategoryModel()
        {
            return new SampleCategoryModel() { SampleSubCategories = [], HasCategory = false };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SampleSubCategoryModel GetDummySampleSubCategoryModel()
        {
            return new SampleSubCategoryModel() { IsApplicable = false, CardLayouts = [] };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CardLayoutModel GetDummayCardLayoutModel()
        {
            return new CardLayoutModel() { IsApplicable = false, Samples = [] };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        /// <param name="xmlCardElement"></param>
        public void DirectCardLayoutModel(object itemModel, XmlElement xmlCardElement)
        {
            if (itemModel is SampleCategoryModel categoryModel)
            {
                categoryModel.SampleSubCategories ??= [];

				categoryModel.HasCategory = false;
                var dummySampleSubCategoryModel = SamplesViewModel.GetDummySampleSubCategoryModel();
                foreach (var sampleModelItem in xmlCardElement.ChildNodes)
                {
                    if (sampleModelItem is XmlElement xmlElement)
                    {
                        var sampleModel = CreateSampleModel(xmlElement);
                        if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                        {
                            var dummyCardLayoutModel = new CardLayoutModel() { IsApplicable = true, Title = sampleModel.Title, StatusTag = sampleModel.StatusTag };
                            dummyCardLayoutModel.Samples ??= [];

							dummyCardLayoutModel.Samples.Add(sampleModel);
                            dummySampleSubCategoryModel.CardLayouts?.Add(dummyCardLayoutModel);
                            dummySampleSubCategoryModel.StatusTag = sampleModel.StatusTag;
                        }
                    }
                }
                categoryModel.SampleSubCategories?.Add(dummySampleSubCategoryModel);
            }
            else if (itemModel is SampleSubCategoryModel subCategoryModel)
            {
                subCategoryModel.CardLayouts ??= [];

				foreach (var sampleModelItem in xmlCardElement.ChildNodes)
                {
                    if (sampleModelItem is XmlElement xmlElement)
                    {
                        var sampleModel = CreateSampleModel(xmlElement);
                        if (string.IsNullOrEmpty(sampleModel.Platforms) || sampleModel.Platforms.Contains(BaseConfig.RunTimeDevicePlatform.ToString(), StringComparison.Ordinal))
                        {
                            var dummyCardLayoutModel = new CardLayoutModel() { IsApplicable = true, Title = sampleModel.Title, StatusTag = sampleModel.StatusTag };
                            dummyCardLayoutModel.Samples ??= [];

							dummyCardLayoutModel.Samples?.Add(sampleModel);
                            subCategoryModel.CardLayouts?.Add(dummyCardLayoutModel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        /// <param name="xmlSubCateGoryElement"></param>
        public void DirectSampleSubCategoryModel(object itemModel, XmlElement xmlSubCateGoryElement)
        {
            if (itemModel is SampleCategoryModel categoryModel)
            {
                categoryModel.SampleSubCategories ??= [];

				SampleSubCategoryModel samplesubCategoryModel = new()
                {
                    SubCategoryName = xmlSubCateGoryElement.GetAttribute("Title"),
                    StatusTag = xmlSubCateGoryElement.GetAttribute("StatusTag")
                };
                currentSubCategoryName = samplesubCategoryModel.SubCategoryName;
                categoryModel.SampleSubCategories.Add(samplesubCategoryModel);

                CheckForModel(samplesubCategoryModel, xmlSubCateGoryElement);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        /// <param name="xmlCateGoryElement"></param>
        public void DirectSampleCategoryModel(object itemModel, XmlElement xmlCateGoryElement)
        {
            if (itemModel is ControlModel controlModel)
            {
                controlModel.SampleCategories ??= [];

				SampleCategoryModel sampleCategoryModel = new()
                {
                    CategoryName = xmlCateGoryElement.GetAttribute("Title"),
                    StatusTag = xmlCateGoryElement.GetAttribute("StatusTag")
                };
                controlModel.SampleCategories.Add(sampleCategoryModel);
                currentCategoryName = sampleCategoryModel.CategoryName;
                CheckForModel(sampleCategoryModel, xmlCateGoryElement);
            }

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ControlModel? Control { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SampleModel? Sample { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SortOption
    {
        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        Ascending,

        /// <summary>
        /// 
        /// </summary>
        Descending,
    }
}