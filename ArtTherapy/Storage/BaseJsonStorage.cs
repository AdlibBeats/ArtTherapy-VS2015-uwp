﻿using ArtTherapyCore.BaseStorage;
using ArtTherapyCore.BaseModels;

namespace ArtTherapy.Storage
{
    public sealed class JsonFactoryStorage : BaseFactoryStorage
    {
        public override BaseStorage Create(string fileName) =>
            new BaseJsonStorage(fileName); 
    }

    public class BaseJsonStorage : BaseStorage
    {
        public BaseJsonStorage(string fileName) : base(fileName)
        {

        }

        public override bool DeleteModel()
        {
            return false;
        }

        public override BaseModel GetModel()
        {
            return null;
        }

        public override bool SetModel(BaseModel model)
        {
            return false;
        }
    }
}
