using ArtTherapyCore.BaseModels;

namespace ArtTherapyCore.BaseStorage
{
    public abstract class BaseFactoryStorage
    {
        public BaseFactoryStorage()
        {
        }

        public abstract BaseStorage Create(string fileName);
    }

    public abstract class BaseStorage
    {
        protected string FileName { get; }

        public BaseStorage(string fileName)
        {
            FileName = fileName;
        }

        public abstract BaseModel GetModel();

        public abstract bool SetModel(BaseModel model);

        public abstract bool DeleteModel();
    }
}
