﻿using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
  public interface IMenuRepository:IRepository<category>    
    {

        void UpdateMenu(category menue);   
    }
}
