using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class NewsService
    {

        public NewsService(TableWork tableWork)
        {
            tow = tableWork;
            
           
        }
      
        private readonly TableWork tow;


        public IEnumerable<News> GetAllNewsList()
        {
         
                return tow.NewsRepository.GetAllVal(x => x.CreatedByNavigation).OrderByDescending(x => x.CreatedUtc).ToList();
          
        }
        public int CreateNewsList(News CreateNews)
        {
            bool status = false;
            int Id = 0;
            if (CreateNews != null)
            {
                try
                {
                    CreateNews.CreatedUtc = DateTime.UtcNow;
                    CreateNews.IsActive = false;
                    tow.NewsRepository.Insert(CreateNews);
                    long dbstatus = tow.SaveChanges();
                    if (status = dbstatus > 0)
                    {
                        Id = CreateNews.Id;
                    }
                }
                catch (Exception e)
                {

                }
            }
            return Id;
        }
        public News GetNewsDetails(int Id)
        {
            return tow.NewsRepository.GetSingle(x => x.Id == Id);
        }
        public bool UpdateNews(News news)
        {
            bool status;
            try
            {
               
                    var updateNews = tow.NewsRepository.GetSingle(x => x.Id == news.Id);
                    updateNews.Category = news.Category;
                    updateNews.Description = news.Description;
                    tow.NewsRepository.Update(updateNews);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool NewsApproval(int Id)
        {
            bool status = false;
            try
            {
               
                    var existingNews = tow.NewsRepository.GetSingle(x => x.Id == Id);
                    if (existingNews.IsActive == true)
                    {
                        existingNews.IsActive = false;
                    }
                    else if (existingNews.IsActive == false)
                    {
                        existingNews.IsActive = true;
                    }
                    tow.NewsRepository.Update(existingNews);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
             
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool CreateNewsImage(int Id, string associateFile, string uploadPath)
        {
            bool status = false;
            try
            {
             
                    var NewsImage = tow.NewsRepository.GetSingle(x => x.Id == Id);
                    NewsImage.NewImagePath = associateFile;
                    tow.NewsRepository.Update(NewsImage);
                    tow.SaveChanges();
         
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
    }
}
