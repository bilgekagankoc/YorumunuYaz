using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
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
    public interface IYorumCevapService : IService<YorumCevapModel,Yorum,YorumunuYazContext>
    {

    }
    public class YorumCevapService : IYorumCevapService
    {
        public RepoBase<Yorum, YorumunuYazContext> Repo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Result Add(YorumCevapModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<YorumCevapModel> Query()
        {
            throw new NotImplementedException();
        }

        public Result SoftDelete(int id)
        {
            throw new NotImplementedException();
        }

        public Result Update(YorumCevapModel model)
        {
            throw new NotImplementedException();
        }
    }
}
