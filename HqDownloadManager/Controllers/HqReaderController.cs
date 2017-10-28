using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DependencyInjectionResolver;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.ViewModels;
using Newtonsoft.Json;

namespace HqDownloadManager.Controllers {
    public class HqReaderController : Controller {
        private HqReaderViewModel _hqReader;
        private ICollectionView _collectionView;
        private Hq _hq;
        private int hqSize;
        private int actualChapter = 0;

        public HqReaderController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper) {
        }

        public override void Init(params object[] values) {
            if (values[0] is Hq hq) {
                _hq = hq;
                hqSize = _hq.Chapters.Count;
                _hqReader = controlsHelper.FindResource<HqReaderViewModel>("ReaderControl");
                if (userLibrary.UserReadings.FindOne(_hq.Link) is UserReading userReading) {
                    var reader = JsonConvert.DeserializeObject<HqReaderViewModel>(
                        Encoding.ASCII.GetString(userReading.HqReaderViewModel));
                    actualChapter = _hq.Chapters.IndexOf(reader.ActualChapter);
                    _hqReader.ActualChapter = reader.ActualChapter;
                    _hqReader.NextChapter = reader.NextChapter;
                    _hqReader.PreviousChapter = reader.PreviousChapter;
                    _hqReader.ActualPage = reader.ActualPage;
                    _collectionView = controlsHelper.FindResource<CollectionViewSource>("Reader").View;
                    _collectionView.CurrentChanged += CollectionViewOnCurrentChanged;
                    _collectionView.MoveCurrentToPosition(reader.ActualPage);
                } else {
                    LoadActualChapter();
                }
            }


        }

        private void CollectionViewOnCurrentChanged(object sender, EventArgs eventArgs) {

        }

        public void LoadActualChapter() {
            var index = actualChapter;
            if (_hq.Chapters[index].Pages == null || _hq.Chapters[index].Pages.Count == 0) {
                Task<Chapter>.Factory.StartNew(() => {
                    dispatcher.Invoke(() => {
                        notification.Visibility = Visibility.Visible;
                    });
                    var chap = sourceManager.GetInfo(_hq.Chapters[index].Link) as Chapter;
                    return chap;
                }).
                    ContinueWith((chapResult) => {
                        dispatcher.Invoke(() => {
                            _hqReader.ActualChapter = chapResult.Result;
                            _hq.Chapters[index] = chapResult.Result;
                            notification.Visibility = Visibility.Hidden;
                            _collectionView = controlsHelper.FindResource<CollectionViewSource>("Reader").View;
                            _collectionView.CurrentChanged += CollectionViewOnCurrentChanged;
                            var userReading = new UserReading {
                                Link = _hq.Link, Date = DateTime.Now,
                                HqReaderViewModel = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_hqReader))
                            };
                            userLibrary.UserReadings.Save(userReading);
                        });
                    });

                LoadNextChapter();
            }
        }

        public void LoadNextChapter() {
            var index = actualChapter++;
            if (index < hqSize) {
                if (_hq.Chapters[index].Pages == null || _hq.Chapters[index].Pages.Count == 0) {
                    Task<Chapter>.Factory
                        .StartNew(() => sourceManager.GetInfo(_hq.Chapters[index].Link) as Chapter).ContinueWith(
                            (chapResult) => {
                                dispatcher.Invoke(() => {
                                    _hqReader.NextChapter = chapResult.Result;
                                    _hq.Chapters[index] = chapResult.Result;
                                });
                            });
                } else {
                    _hqReader.NextChapter = _hq.Chapters[index];
                }
            } else {
                _hqReader.NextChapter = null;
            }
        }


        public void NextPage() {
            if (_collectionView.MoveCurrentToNext()) {
                _hqReader.ActualPage = _collectionView.CurrentPosition;
                SaveState();
            } else {
                NextChapter();
            }

        }

        public void PreviousPage() {
            if (_collectionView.MoveCurrentToPrevious()) {
                _hqReader.ActualPage = _collectionView.CurrentPosition;
                SaveState();
            } else {
                PreviousChapter();
            }
        }

        public void NextChapter() {
            if (actualChapter < hqSize) {
                _hqReader.ActualPage = 0;
                _hqReader.PreviousChapter = _hqReader.ActualChapter;
                _hqReader.ActualChapter = _hqReader.NextChapter;
                actualChapter++;
                _collectionView = controlsHelper.FindResource<CollectionViewSource>("Reader").View;
                _collectionView.CurrentChanged += CollectionViewOnCurrentChanged;
                SaveState();
                LoadNextChapter();
            }
        }

        public void PreviousChapter() {
            if (actualChapter > 0) {
                _hqReader.ActualPage = 0;
                _hqReader.NextChapter = _hqReader.ActualChapter;
                _hqReader.ActualChapter = _hqReader.PreviousChapter;
                actualChapter--;
                _collectionView = controlsHelper.FindResource<CollectionViewSource>("Reader").View;
                _collectionView.CurrentChanged += CollectionViewOnCurrentChanged;
                _hqReader.PreviousChapter = _hq.Chapters[actualChapter];
                SaveState();
            } else if (actualChapter == 0) {
                _hqReader.PreviousChapter = null;
            }
        }

        private void SaveState() {
            var userReading = new UserReading {
                Link = _hq.Link, Date = DateTime.Now,
                HqReaderViewModel = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_hqReader))
            };
            userLibrary.UserReadings.Update(userReading);
        }
    }
}
