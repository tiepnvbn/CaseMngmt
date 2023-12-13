using AutoMapper;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.Customers;

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
        }
    }
}
