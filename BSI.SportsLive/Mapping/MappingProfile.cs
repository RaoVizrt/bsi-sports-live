using AutoMapper;
using BSI.SportsLive.DTOs;
using BSI.SportsLive.Models;

namespace BSI.SportsLive.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Team mapping — sirf CURRENT players (LeftDate == null) dikhayein
            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src =>
                    src.TeamPlayers.Where(tp => tp.LeftDate == null).Select(tp => tp.Player)));
            CreateMap<TeamCreateDto, Team>();
            CreateMap<TeamUpdateDto, Team>();

            // Player mapping — Global player, current + past teams dono
            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.CurrentTeams, opt => opt.MapFrom(src =>
                    src.TeamPlayers.Where(tp => tp.LeftDate == null).Select(tp => new TeamMembershipDto
                    {
                        TeamId = tp.TeamId,
                        TeamName = tp.Team.Name,
                        JoinedDate = tp.JoinedDate,
                        LeftDate = tp.LeftDate
                    })))
                .ForMember(dest => dest.PastTeams, opt => opt.MapFrom(src =>
                    src.TeamPlayers.Where(tp => tp.LeftDate != null).Select(tp => new TeamMembershipDto
                    {
                        TeamId = tp.TeamId,
                        TeamName = tp.Team.Name,
                        JoinedDate = tp.JoinedDate,
                        LeftDate = tp.LeftDate
                    })));
            CreateMap<PlayerCreateDto, Player>();
            CreateMap<PlayerUpdateDto, Player>();

            CreateMap<Player, PlayerSummaryDto>();

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