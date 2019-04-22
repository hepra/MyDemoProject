using MyDemoList.Models;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Wemew.Program.ZipInfo
{
    /// <summary>
    /// Zip读取 和压缩类
    /// </summary>
    public class ZipRepertory
    {
      
        public Stream GetVideoStream(string path, string password,int index)
        {
            using (var stream = File.OpenRead(path))
            {
                using (var archive = ZipArchive.Open(stream,
                    new ReaderOptions
                    {
                        Password = password,
                        LeaveStreamOpen = true,
                        ArchiveEncoding = new ArchiveEncoding()
                        {
                            Default = Encoding.UTF8
                        }
                    }
                ))
                {
                    foreach(var e in archive.Entries)
                    {
                        if(e.Key.ToLower().Contains(".dat"))
                        {
                            var videoArchive = e;
                            var videoStream = videoArchive.OpenEntryStream();
                            var copyLength = videoArchive.Size;
                            var copyBytes = new byte[copyLength];
                            var memoryStream = new MemoryStream(copyBytes.Length);
                            videoStream.Read(copyBytes, 0, copyBytes.Length);
                            memoryStream.Write(copyBytes, 0, copyBytes.Length);
                            return videoStream;
                        }
                    }
                }
            }
            return null;
        }
        public VlcStream GetVideoStream(string path, string password)
        {
            var stream = File.OpenRead(path);
            var option = new ReaderOptions
            {
                Password = password,
                LeaveStreamOpen = true,
                ArchiveEncoding = new ArchiveEncoding()
                {
                    Default = Encoding.UTF8
                }
            };
            var archive = ZipArchive.Open(stream, option );
            VlcStream temp = new VlcStream();
            temp.Stream = archive;
            temp.ZipPath = path;
            temp.Option = option;
            temp.check = entity => IsVideo(Path.GetExtension(entity.Key));
            temp.fileStream = stream;
            return temp;
        }
        public VlcStream GetVideoStream(string path)
        {
            var stream = File.OpenRead(path);
            var option = new ReaderOptions
            {
                LeaveStreamOpen = true,
                ArchiveEncoding = new ArchiveEncoding()
                {
                    Default = Encoding.UTF8
                }
            };
            var archive = ZipArchive.Open(stream, option);
            VlcStream temp = new VlcStream();
            temp.Stream = archive;
            temp.ZipPath = path;
            temp.Option = option;
            temp.check = entity => IsImage(Path.GetExtension(entity.Key));
            temp.fileStream = stream;
            return temp;
        }
        public void CreateZip(string imageFile,string videoFile,string savePath,string password, string comment)
        {
            var stream = File.Create(savePath);
            using (var archive = new ZipOutputStream(stream))
            {
                archive.Password = password;
                archive.SetComment(comment);
                archive.SetLevel(6);

                archive.PutNextEntry(imageFile, comment);
                archive.PutNextEntry(videoFile, comment);
            }
        }


        public bool CheckZip(string path, string password)
        {
            var checkEntity = new ZipEntity();

            using (var stream = File.OpenRead(path))
            {
                using (var archive = ZipArchive.Open(stream,
                    new ReaderOptions
                    {
                        Password = password,
                        LeaveStreamOpen = true,
                        LookForHeader = true,
                        ArchiveEncoding = new ArchiveEncoding()
                        {
                            Default = Encoding.UTF8
                        }
                    }
                ))
                {
                    var imageArchive = archive.Entries.FirstOrDefault(entity => IsImage(Path.GetExtension(entity.Key)));

                    if (imageArchive == null)
                        throw new Exception($"Rar {path} not found image file");

                    var root = $"";
                    var imageFile = $"{root}\\{imageArchive.Key}";
                    var comment = imageArchive.Comment;

                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    //imageArchive.Save(imageFile);

                    checkEntity.ImagePath = imageFile;
                    checkEntity.Descript = comment;

                    var videoArchive = archive.Entries.FirstOrDefault(entity => IsVideo(Path.GetExtension(entity.Key)));

                    checkEntity.FindVideo = videoArchive != null;
                }
            }

            return checkEntity.FindVideo && !string.IsNullOrWhiteSpace(checkEntity.Descript) && !string.IsNullOrWhiteSpace(checkEntity.ImagePath);
        }

        private bool IsImage(string key)
        {
            var images = new[]
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".bmp"
            };

            return images.Contains(key.ToLower());
        }

        private bool IsJson(string key)
        {
            var json = new[]
            {
                ".Json"
            };

            return json.Contains(key.ToLower());
        }

        private bool IsVideo(string key)
        {
            return ".dat".Equals(key.ToLower());
        }
    }
    internal static class ZipOutputStreamExtension
    {
        public static void PutNextEntry(this ZipOutputStream archive, string filename, string comment)
        {
            var entity = new ZipEntry(Path.GetFileName(filename));

            entity.Comment = comment;
            entity.IsUnicodeText = true;
            archive.PutNextEntry(entity);

            var blockLength = 1024 * 4;
            using (var stream = File.OpenRead(filename))
            {
                while (stream.Position < stream.Length)
                {
                    var copyLength = stream.Length - stream.Position > blockLength ? blockLength : stream.Length - stream.Position;
                    var copyBytes = new byte[copyLength];

                    stream.Read(copyBytes, 0, copyBytes.Length);
                    archive.Write(copyBytes, 0, copyBytes.Length);
                }
            }
        }
    }
}
