using AutoMapper;
using Notely.Dto;
using Notely.Dto.Auth;
using Notely.Entities;

namespace Notely.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        //---------------------------------Compte---------------------------------

        CreateMap<Compte, CompteDTO>();
        CreateMap<Compte, LoginResponseDTO>();

        //---------------------------------Cours---------------------------------

        CreateMap<Cours, CoursDTO>()
            .ForMember(d => d.NombreChapitres, o => o.MapFrom(s => s.Chapitres.Count))
            .ForMember(d => d.NombreChapitresAppris, o => o.MapFrom(s => s.Chapitres.Count(c => c.Etat == "appris")))
            .ForMember(d => d.NombreTachesOuvertes, o => o.MapFrom(s => s.Todos.Count(t => !t.Fait)));

        CreateMap<Cours, CoursDetailDTO>();

        CreateMap<CoursCreateDTO, Cours>();
        CreateMap<CoursUpdateDTO, Cours>();

        //---------------------------------Chapitre---------------------------------

        CreateMap<Chapitre, ChapitreDTO>();
        CreateMap<ChapitreCreateDTO, Chapitre>();
        CreateMap<ChapitreUpdateDTO, Chapitre>();

        //---------------------------------Todo---------------------------------

        CreateMap<Todo, TodoDTO>()
            .ForMember(d => d.NomCours, o => o.MapFrom(s => s.CoursNav.Nom));
        CreateMap<TodoCreateDTO, Todo>();
        CreateMap<TodoUpdateDTO, Todo>();

        //---------------------------------Note---------------------------------

        CreateMap<Note, NoteDTO>();
        CreateMap<NoteCreateDTO, Note>();
        CreateMap<NoteUpdateDTO, Note>();

        //---------------------------------Evenement---------------------------------

        CreateMap<Evenement, EvenementDTO>();
        CreateMap<EvenementCreateDTO, Evenement>();
        CreateMap<EvenementUpdateDTO, Evenement>();

        //---------------------------------Seance / suivi salle---------------------------------

        CreateMap<Seance, SeanceDTO>()
            .ForMember(d => d.Exercices, o => o.MapFrom(s => s.ExercicesSeance));
        CreateMap<SeanceCreateDTO, Seance>();
        CreateMap<SeanceUpdateDTO, Seance>();

        CreateMap<ExerciceSeance, ExerciceSeanceDTO>();
        CreateMap<ExerciceSeanceCreateDTO, ExerciceSeance>();

        CreateMap<Serie, SerieDTO>();
        CreateMap<SerieCreateDTO, Serie>();
        CreateMap<SerieUpdateDTO, Serie>();
    }
}
