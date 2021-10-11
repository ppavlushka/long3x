using AutoMapper;
using long3x.Data.Entities;
using long3x.ViewModels;

namespace long3x.Mappings
{
    public class SignalMappingProfile: Profile
    {
        public SignalMappingProfile()
        {
            CreateMap<SignalEntity, SignalViewModel>()
                .ForMember(vm => vm.ChannelId, x => x.MapFrom(entity => entity.ChannelId))
                .ForMember(vm => vm.Coin1, x => x.MapFrom(entity => entity.Coin1))
                .ForMember(vm => vm.Coin2, x => x.MapFrom(entity => entity.Coin2))
                .ForMember(vm => vm.Leverage, x => x.MapFrom(entity => entity.Leverage))
                .ForMember(vm => vm.LongShort, x => x.MapFrom(entity => entity.LongShort))
                .ForMember(vm => vm.Risk, x => x.MapFrom(entity => entity.Risk))
                .ForMember(vm => vm.EntryZoneMin, x => x.MapFrom(entity => entity.EntryZoneMin))
                .ForMember(vm => vm.EntryZoneMax, x => x.MapFrom(entity => entity.EntryZoneMax))
                .ForMember(vm => vm.Targets, x => x.MapFrom(entity => entity.Targets))
                .ForMember(vm => vm.StopLoss, x => x.MapFrom(entity => entity.StopLoss))
                .ForMember(vm => vm.TradingType, x => x.MapFrom(entity => entity.TradingType))
                .ForMember(vm => vm.AdditionalInfo, x => x.MapFrom(entity => entity.AdditionalInfo))
                .ForMember(vm => vm.Ts, x => x.MapFrom(entity => entity.Ts))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
