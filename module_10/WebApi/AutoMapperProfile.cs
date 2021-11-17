using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //View <--> Domain
            CreateMap<Lector, Models.Domain.Lector>().
                ForMember(lect => lect.Email, opt => opt.MapFrom(lectV => Models.Domain.Email.TryCreate(lectV.Email))).
                ReverseMap().
                ForMember(lectV => lectV.Email, opt => opt.MapFrom(lect => lect.Email.CorrectEmail));

            CreateMap<Student, Models.Domain.Student>().
                ForMember(stud => stud.Email, opt => opt.MapFrom(studV => Models.Domain.Email.TryCreate(studV.Email))).
                ForMember(stud => stud.PhoneNumber, opt => opt.MapFrom(studV => Models.Domain.PhoneNumber.TryCreate(studV.PhoneNumber))).
                ReverseMap().
                ForMember(studV => studV.Email, opt => opt.MapFrom(stud => stud.Email.CorrectEmail)).
                ForMember(studV => studV.PhoneNumber, opt => opt.MapFrom(stud => stud.PhoneNumber.CorrectPhoneNumber));

            CreateMap<Lection, Models.Domain.Lection>().
                ForMember(lect => lect.Date, opt => opt.MapFrom(lectV => Models.Domain.Date.TryCreate(lectV.Date))).
                ReverseMap().
                ForMember(lectV => lectV.Date, opt => opt.MapFrom(lect => lect.Date.CorrectDate.ToString("dd.MM.yyyy")));

            CreateMap<Homework, Models.Domain.Homework>().
                ForMember(work => work.Mark, opt => opt.MapFrom(workV => Models.Domain.Mark.TryCreate(workV.Mark))).
                ReverseMap().
                ForMember(workV => workV.Mark, opt => opt.MapFrom(work => work.Mark.CorrectMark));

            CreateMap<Attendance, Models.Domain.Attendance>().ReverseMap();

            //Domain <--> Database
            CreateMap<Models.Database.Lector, Models.Domain.Lector>().
                ForMember(lect => lect.Fio, opt => opt.MapFrom(lectDB => lectDB.Fio.TrimEnd())).
                ForMember(lect => lect.Email, opt => opt.MapFrom(lectDB => Models.Domain.Email.TryCreate(lectDB.Email.TrimEnd()))).
                ReverseMap().
                ForMember(lectDB => lectDB.Email, opt => opt.MapFrom(lect => lect.Email.CorrectEmail));

            CreateMap<Models.Database.Student, Models.Domain.Student>().
                ForMember(stud=> stud.Fio, opt => opt.MapFrom(studDB => studDB.Fio.TrimEnd())).
                ForMember(stud => stud.Email, opt => opt.MapFrom(studDB => Models.Domain.Email.TryCreate(studDB.Email.TrimEnd()))).
                ForMember(stud => stud.PhoneNumber, opt => opt.MapFrom(studDB => Models.Domain.PhoneNumber.TryCreate(studDB.PhoneNumber.TrimEnd()))).
                ReverseMap().
                ForMember(studDB => studDB.Email, opt => opt.MapFrom(stud => stud.Email.CorrectEmail)).
                ForMember(studDB => studDB.PhoneNumber, opt => opt.MapFrom(stud => stud.PhoneNumber.CorrectPhoneNumber));

            CreateMap<Models.Database.Lection, Models.Domain.Lection>().
                ForMember(lect => lect.Topic, opt => opt.MapFrom(lectDB => lectDB.Topic.TrimEnd())).
                ForMember(lect => lect.Date, opt => opt.MapFrom(lectDB => Models.Domain.Date.TryCreate(lectDB.Date))).
                ReverseMap().
                ForMember(lectDB => lectDB.Date, opt => opt.MapFrom(lect => lect.Date.CorrectDate.ToString("dd.MM.yyyy")));

            CreateMap<Models.Database.Homework, Models.Domain.Homework>().
                ForMember(work => work.Mark, opt => opt.MapFrom(workDB => Models.Domain.Mark.TryCreate(workDB.Mark))).
                ReverseMap().
                ForMember(workDB => workDB.Mark, opt => opt.MapFrom(work => work.Mark.CorrectMark));

            CreateMap<Models.Database.Attendance, Models.Domain.Attendance>().ReverseMap();
        }
    }
}
