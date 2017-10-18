using ArtTherapyCore.BaseStorage;
using ArtTherapyCore.BaseModels;
using Windows.Storage;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace ArtTherapy.Storage
{
    public sealed class JsonFactoryStorage<T> : BaseFactoryStorage<T> where T : BaseModel
    {
        public override BaseStorage<T> Create() =>
            new BaseJsonStorage<T>(); 
    }

    public class BaseJsonStorage<T> : BaseStorage<T> where T : BaseModel
    {
        public BaseJsonStorage() : base()
        {
        }

        public override Task<bool> DeleteModel(string fileName)
        {
            //var p = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            StorageFile file = null;

            return Task.Run(async () =>
            {
                try
                {
                    file = await StorageFile.GetFileFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\ArtTherapyCore\Repository" + @"\" + fileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                if (file != null)
                {
                    try
                    {
                        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        return false;
                    }
                }
                else
                    return false;
            });
        }

        public override Task<T> GetModel(string fileName)
        {
            StorageFile file = null;
            StorageFolder folder1 = null;
            StorageFolder folder2 = null;

            return Task.Run(async () =>
            {
                string reader = String.Empty;
                Debug.WriteLine($"{fileName}: {Windows.ApplicationModel.Package.Current.InstalledLocation.Path}");
                try
                {
                    try
                    {
                        folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("ArtTherapyCore");
                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                        folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFolderAsync("ArtTherapyCore", CreationCollisionOption.OpenIfExists);
                    }

                    try
                    {
                        folder2 = await folder1.GetFolderAsync("Repository");
                    }
                    catch (Exception ex2)
                    {
                        Debug.WriteLine(ex2.Message);
                        folder2 = await folder1.CreateFolderAsync("Repository", CreationCollisionOption.OpenIfExists);
                    }

                    file = await StorageFile.GetFileFromPathAsync(folder2.Path + @"\" + fileName);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);

                    try
                    {
                        file = await folder2.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                reader = await FileIO.ReadTextAsync(file);

                return String.IsNullOrEmpty(reader) ? null : JsonConvert.DeserializeObject<T>(reader);
            });
        }

        public override Task<bool> SetModel(string fileName, T model)
        {
            Debug.WriteLine($"{fileName}: {Windows.ApplicationModel.Package.Current.InstalledLocation.Path}");

            StorageFile file = null;
            StorageFolder folder1 = ApplicationData.Current.LocalFolder;
            //StorageFolder folder2 = null;

            return Task.Run(async () =>
            {
                try
                {
                    //try
                    //{
                    //    folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("ArtTherapyCore");
                    //}
                    //catch (Exception ex1)
                    //{
                    //    Debug.WriteLine(ex1.Message);
                    //    folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFolderAsync("ArtTherapyCore", CreationCollisionOption.OpenIfExists);
                    //}

                    //try
                    //{
                    //    folder2 = await folder1.GetFolderAsync(@"Repository");
                    //}
                    //catch (Exception ex2)
                    //{
                    //    Debug.WriteLine(ex2.Message);
                    //    folder2 = await folder1.CreateFolderAsync(@"Repository", CreationCollisionOption.ReplaceExisting);
                    //}

                    file = await folder1.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return false;
                }

                if (model == null) return false;
                else
                {
                    string writer = JsonConvert.SerializeObject(model);
                    await FileIO.WriteTextAsync(file, writer);
                    return true;
                }
            });
        }
    }
}
