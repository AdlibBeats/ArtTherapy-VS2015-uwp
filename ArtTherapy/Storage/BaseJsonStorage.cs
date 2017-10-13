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

        public override bool DeleteModel(string fileName)
        {
            StorageFile file = null;

            return Task.Run(async () =>
            {
                try
                {
                    file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path + @"\" + fileName);
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
            })
            .GetAwaiter()
            .GetResult();
        }

        public override T GetModel(string fileName)
        {
            StorageFile file = null;

            return Task.Run(async () =>
            {
                string reader = String.Empty;
                Debug.WriteLine($"{fileName}: {ApplicationData.Current.LocalFolder.Path}");
                try
                {
                    file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path + @"\" + fileName);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);

                    try
                    {
                        file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                reader = await FileIO.ReadTextAsync(file);

                return String.IsNullOrEmpty(reader) ? null : JsonConvert.DeserializeObject<T>(reader);
            })
            .GetAwaiter()
            .GetResult();
        }

        public override bool SetModel(string fileName, T model)
        {
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);

            StorageFile file = null;

            return Task.Run(async () =>
            {
                try
                {
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }

                if (model == null) return false;
                else
                {
                    string writer = JsonConvert.SerializeObject(model);
                    await FileIO.WriteTextAsync(file, writer);
                    return true;
                }
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
