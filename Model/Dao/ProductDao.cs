using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
  public class ProductDao
  {
    DaoTaoTrucTuyen6Entities db = null;
    public ProductDao()
    {
      db = new DaoTaoTrucTuyen6Entities();
    }
    public IEnumerable<Product> ListAllPaging(long cateID, string searchString, int page, int pagesize)
    {
      IQueryable<Product> model = db.Products;
      if (cateID != -1)
      {
        model = model.Where(x => x.CategoryID == cateID);
      }
      if (!string.IsNullOrEmpty(searchString))
      {
        model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString));
      }
      return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pagesize);
    }
    public bool Delete(int id)
    {
      try
      {
        var product = db.Products.Find(id);
        db.Products.Remove(product);
        db.SaveChanges();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
    public long Insert(Product entity)
    {
      db.Products.Add(entity);
      db.SaveChanges();
      return entity.ID;
    }
    public Product ViewDetail(long id)
    {

      return db.Products.Find(id);
    }
    public bool Update(Product entity)
    {
      try
      {
        var product = db.Products.Find(entity.ID);
        product.Name = entity.Name;
        product.Code = entity.Code;
        product.MetaTitle = entity.MetaTitle;
        product.Description = entity.Description;
        product.Detail = entity.Detail;
        product.Image = entity.Image;
        product.ListType = entity.ListType;
        product.ListFile = entity.ListFile;
        product.CategoryID = entity.CategoryID;

        db.SaveChanges();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
    public List<Product> ListAllProduct()
    {
      return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
    }
    public List<Product> ListByCategoryID(string searchString, long CategoryID)
    {
      IOrderedQueryable<Product> model = db.Products;
      if (CategoryID == 0)
      {
        if (!string.IsNullOrEmpty(searchString))
        {
          return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => x.Status).OrderByDescending(x => x.CreateDate).ToList();
        }
        else
        {
          return model.Where(x => x.Status).OrderByDescending(x => x.CreateDate).ToList();
        }
      }
      else
      {
        if (!string.IsNullOrEmpty(searchString))
        {
          return model.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).Where(x => x.Status && x.CategoryID == CategoryID).OrderByDescending(x => x.CreateDate).ToList();
        }
        else
        {
          return model.Where(x => x.Status && x.CategoryID == CategoryID).OrderByDescending(x => x.CreateDate).ToList();
        }
      }
    }
  }
}