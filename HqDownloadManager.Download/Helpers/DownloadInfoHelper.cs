using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.CustomEventArgs;
using System.Text.RegularExpressions;

namespace HqDownloadManager.Download.Helpers {
    internal class DownloadInfoHelper {
        private readonly DownloadContext _downloadContext;
        private readonly object _lock = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();


        public DownloadInfoHelper(DownloadContext downloadContext) {
            _downloadContext = downloadContext;
        }

        public void SaveDownloadInfo(HqDownloadInfo downloadInfo) {
            lock (_lock2) {
                if (!string.IsNullOrEmpty(downloadInfo.Hq.Link)) {
                    if (downloadInfo.Hq.Chapters != null && downloadInfo.Hq.Chapters.Count() > 0) {
                        foreach (var chapter in downloadInfo.Hq.Chapters) {
                            chapter.Hq = null;
                            if (chapter.Pages != null && chapter.Pages.Count() > 0) {
                                foreach (var page in chapter.Pages) {
                                    page.Chapter = null;
                                }
                            }
                        }
                    }

                    using (StreamWriter file = File.CreateText($"{downloadInfo.Path}\\DownloadInfo.Json")) {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, downloadInfo);
                    }

                    _downloadContext.HqDownloadInfo.SaveOrReplace(downloadInfo);
                }           
            }
        }

        public List<HqDownloadInfo> GetHqsDownloadInfo() {
            lock (_lock3) {
                CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Processando arquivos..."));
                var list = new List<HqDownloadInfo>();
                var drInfo = new DirectoryInfo($"{CoreConfiguration.DownloadLocation}\\Hqs");
                if (drInfo != null) {
                    var dirs = drInfo.GetDirectories();
                    if (dirs != null && dirs.Count() > 0) {
                        CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"{dirs.Count()} diretoórios encontrados"));
                        foreach (var dir in dirs) {
                            var downloadInfo = new HqDownloadInfo();
                            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {dir.Name}"));
                            if (_downloadContext.HqDownloadInfo.Find().Where(x => x.Path == dir.FullName).Execute().FirstOrDefault() is HqDownloadInfo dwIfo) {
                                downloadInfo = dwIfo;
                            } else if (File.Exists($"{dir.FullName}\\DownloadInfo.Json")) {
                                using (StreamReader file = File.OpenText($"{dir.FullName}\\DownloadInfo.Json")) {
                                    JsonSerializer serializer = new JsonSerializer();
                                    downloadInfo = (HqDownloadInfo)serializer.Deserialize(file, typeof(HqDownloadInfo));
                                }
                                if (!string.IsNullOrEmpty(downloadInfo.Hq.Link)) {
                                    if (!(_downloadContext.Hq.FindOne(downloadInfo.Hq.Link) is Hq)) {
                                        downloadInfo.Hq.IsDetailedInformation = false;
                                        _downloadContext.Hq.Save(downloadInfo.Hq);
                                    }
                                    _downloadContext.HqDownloadInfo.Save(downloadInfo);
                                }
                            } else {
                                var hq = new Hq {
                                    Title = dir.Name
                                };
                                downloadInfo = new HqDownloadInfo {
                                    Hq = hq, Path = dir.FullName
                                };
                            }

                            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"{downloadInfo.Hq.Title} adicionado!"));
                            list.Add(downloadInfo);
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                return list;
            }
        }

        public HqDownloadInfo GetChaptersFromDownloadInfo(HqDownloadInfo downloadInfo) {
            lock (_lock4) {
                downloadInfo.Hq.Chapters = new List<Chapter>();
                var subDirInfo = new DirectoryInfo(downloadInfo.Path);
                if (subDirInfo != null) {
                    var subDirs = subDirInfo.GetDirectories();
                    if (subDirs != null && subDirs.Count() > 0) {
                        foreach (var chapDir in subDirs) {
                            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Adiconando capitulo {chapDir.Name}..."));
                            var count = 0;
                            var chapName = Regex.Replace(chapDir.Name, @"\d+", n => {
                                var value = n.Value;
                                var replacement = value.PadLeft(4, '0');
                                count++;
                                if (count <= 1) {
                                    return replacement;
                                }
                                return value;
                            });

                            var chap = new Chapter { Title = chapName, Pages = new List<Page>() };
                            var pageFiles = chapDir.GetFiles();
                            if (pageFiles != null && pageFiles.Count() > 0) {
                                foreach (var file in pageFiles) {
                                    var num = int.Parse(Path.GetFileNameWithoutExtension(file.Name));
                                    CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Adiconando página {num}..."));
                                    chap.Pages.Add(new Page {
                                        Number = num,
                                        Source = file.FullName
                                    });
                                }
                            }
                            downloadInfo.Hq.Chapters.Add(chap);
                        }

                        downloadInfo.Hq.Chapters = downloadInfo.Hq.Chapters.OrderBy(x => x.Title).ToList();
                        downloadInfo.LastDownloadedChapter = downloadInfo.Hq.Chapters.Last();
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                return downloadInfo;
            }
        }



        public void DeleteDownloadInfo(HqDownloadInfo downloadInfo, bool deleteFiles = false) {
            lock (_lock) {
                if (downloadInfo.Hq != null) {
                    _downloadContext.HqDownloadInfo.Delete(downloadInfo);
                    if (deleteFiles && !string.IsNullOrEmpty(downloadInfo.Path)) {
                        Directory.Delete(downloadInfo.Path, true);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
            }
        }
    }
}
