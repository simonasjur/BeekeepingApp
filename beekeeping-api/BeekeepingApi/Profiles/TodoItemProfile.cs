using AutoMapper;
using BeekeepingApi.DTOs.TodoItemDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemReadDTO>();
            CreateMap<TodoItemCreateDTO, TodoItem>();
            CreateMap<TodoItemEditDTO, TodoItem>();
        }
    }
}
