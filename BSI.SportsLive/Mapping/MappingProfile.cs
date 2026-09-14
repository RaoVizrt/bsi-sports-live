using AutoMapper;
using BSI.SportsLive.DTOs;
using BSI.SportsLive.Models;

namespace BSI.SportsLive.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));
            CreateMap<TeamCreateDto, Team>();
            CreateMap<TeamUpdateDto, Team>();

            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty));
            CreateMap<PlayerCreateDto, Player>();
            CreateMap<PlayerUpdateDto, Player>();

            CreateMap<Player, PlayerSummaryDto>()
                .ForMember(dest => dest.BroadcastName, opt => opt.MapFrom(src => src.BroadcastName))
                .ForMember(dest => dest.ShirtNumber, opt => opt.MapFrom(src => src.ShirtNumber));

            // Tournament mappings
            CreateMap<Tournament, TournamentDto>();
            CreateMap<TournamentCreateDto, Tournament>();
            CreateMap<TournamentUpdateDto, Tournament>();
            CreateMap<TournamentTeam, TournamentTeamDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty));

            // Schedule mappings
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament != null ? src.Tournament.Name : string.Empty))
                .ForMember(dest => dest.TeamAName, opt => opt.MapFrom(src => src.TeamA != null ? src.TeamA.Name : string.Empty))
                .ForMember(dest => dest.TeamBName, opt => opt.MapFrom(src => src.TeamB != null ? src.TeamB.Name : string.Empty));
            CreateMap<ScheduleCreateDto, Schedule>();
            CreateMap<ScheduleUpdateDto, Schedule>();
        }
    }
}
