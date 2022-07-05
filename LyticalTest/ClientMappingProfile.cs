using AutoMapper;
using LyticalTest.Models;
using LyticalTest.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest
{
    public class ClientMappingProfile : Profile
    {

        public ClientMappingProfile()
        {
            CreateSimpleMapping();
            CreateComplexMapping();

        }


        private void CreateSimpleMapping()
        {
            //handles it by default
        }

        private void CreateComplexMapping()
        {
              
            CreateMap<UserModel, LoginResponse>()
                .ReverseMap();

            CreateMap<UserModel, AddUserResponse>()
                .ReverseMap();
            CreateMap<ApplicationUser, UserModel>()
              .ReverseMap();

     
        }
    }


  
}
