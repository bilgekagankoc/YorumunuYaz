using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IYorumService : IService<YorumModel,Yorum,YorumunuYazContext>
    {

    }

    public class YorumService : IYorumService
    {
        public RepoBase<Yorum, YorumunuYazContext> Repo { get; set; } = new Repo<Yorum, YorumunuYazContext>();

        public IQueryable<YorumModel> Query()
        {
            throw new NotImplementedException();
        }

        public Result Add(YorumModel model)
        {
            throw new NotImplementedException();
        }

        public Result Update(YorumModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Result SoftDelete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }
    }
}
