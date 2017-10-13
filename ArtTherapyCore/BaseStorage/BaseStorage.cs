using ArtTherapyCore.BaseModels;

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
        public abstract T GetModel(string fileName);

        public abstract bool SetModel(string fileName, T model);

        public abstract bool DeleteModel(string fileName);
    }
}
