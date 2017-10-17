using ArtTherapyCore.BaseModels;
using System.Threading.Tasks;

namespace ArtTherapyCore.BaseStorage
{
    public abstract class BaseFactoryStorage<T> where T : BaseModel
    {
        public BaseFactoryStorage()
        {
        }

        public abstract BaseStorage<T> Create();
    }

    public abstract class BaseStorage<T> where T : BaseModel
    {
        public abstract Task<T> GetModel(string fileName);

        public abstract Task<bool> SetModel(string fileName, T model);

        public abstract Task<bool> DeleteModel(string fileName);
    }
}
