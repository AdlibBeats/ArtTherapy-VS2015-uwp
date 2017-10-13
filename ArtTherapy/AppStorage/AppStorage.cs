using ArtTherapy.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using ArtTherapy.Models;
using ArtTherapyCore.BaseModels;

namespace ArtTherapy.AppStorage
{
    //public static class AppStorage<T> where T : BaseModel
    //{
    //    #region Get Set Post Model
    //    public static async Task<T> Get(string fileName)
    //    {
    //        StorageFile file = null;

    //        Debug.WriteLine($"{fileName}: {ApplicationData.Current.LocalFolder.Path}");

    //        try
    //        {
    //            file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path + @"\" + fileName);
    //        }
    //        catch
    //        {
    //            file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
    //        }

    //        string reader = FileIO.ReadTextAsync(file)
    //            .AsTask()
    //            .ConfigureAwait(false)
    //            .GetAwaiter()
    //            .GetResult();

    //        return String.IsNullOrEmpty(reader) ? null : JsonConvert.DeserializeObject<T>(reader);
    //    }

    //    public static async Task Set(T value, string fileName)
    //    {
    //        Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);

    //        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"\" + fileName, CreationCollisionOption.ReplaceExisting);
    //        string writer = JsonConvert.SerializeObject(value);
    //        if (value != null)
    //            FileIO.WriteTextAsync(file, writer)
    //                .AsTask()
    //                .ConfigureAwait(false)
    //                .GetAwaiter()
    //                .GetResult();
    //    }

    //    public static async Task Remove(string fileName)
    //    {
    //        StorageFile file = null;

    //        try
    //        {
    //            file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path + @"\" + fileName);
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.Message);
    //        }
    //        finally
    //        {
    //            if (file != null)
    //                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
    //        }
    //    }
    //    #endregion
    //}
}
