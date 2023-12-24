using AutoMapper;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.Customers;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Types;
using Type = CaseMngmt.Models.Types.Type;

namespace CaseMngmt.Models.AutoMapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Customer, CustomerRequest>();
            CreateMap<CustomerRequest, Customer>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CompanyViewModel, Company>();
            CreateMap<Company, CompanyViewModel>();
            CreateMap<Company, CompanyRequest>();
            CreateMap<CompanyRequest, Company>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<TypeViewModel, Type>();
            CreateMap<Type, TypeViewModel>();
            CreateMap<Type, TypeRequest>();
            CreateMap<TypeRequest, Type>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Keyword, KeywordRequest>();
            CreateMap<KeywordRequest, Keyword>().ForMember(x => x.Id, opt => opt.Ignore());

        }
    }
}
