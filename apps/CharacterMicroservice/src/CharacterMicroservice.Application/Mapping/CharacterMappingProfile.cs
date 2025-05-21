using System;
using AutoMapper;
using CharacterMicroservice.Application.ReadModels;
using CharacterMicroservice.Domain.Models.Entity.Write;

namespace CharacterMicroservice.Application.Mapping;

public class CharacterMappingProfile : Profile
{
    public CharacterMappingProfile()
    {
        CreateMap<CharacterSheet, CharacterRead>()
            .ForMember(dest => dest.Items,
                       opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId)))
            .ForMember(dest => dest.Skills,
                       opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillId)))
            .ForMember(dest => dest.Notes,
                       opt => opt.MapFrom(src => src.Notes.Select(n => n.Content)));
    }
}
