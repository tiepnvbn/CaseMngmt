using CaseMngmt.Models.Account;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.Database;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Companies;
using CaseMngmt.Repository.CompanyTemplates;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.Templates;
using CaseMngmt.Repository.Types;
using Microsoft.AspNetCore.Identity;

namespace CaseMngmt.Server
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder UseItToSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                var companyManager = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
                var typeManager = scope.ServiceProvider.GetRequiredService<ITypeRepository>();
                var templateManager = scope.ServiceProvider.GetRequiredService<ITemplateRepository>();
                var keywordManager = scope.ServiceProvider.GetRequiredService<IKeywordRepository>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var companyTemplateManager = scope.ServiceProvider.GetRequiredService<ICompanyTemplateRepository>();

                var defaultCompanyGuid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DE");
                var defaultCompany2Guid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DD");
                var checkData = companyManager.GetByIdAsync(defaultCompanyGuid);
                if (checkData.Result != null)
                {
                    return app;
                }
                companyManager.AddAsync(new Models.Companies.Company
                {
                    Id = defaultCompanyGuid,
                    Name = "CASE Company DEFAULT",
                    BuildingName = "CASE Company DEFAULT",
                    City = "Ha Noi",
                    StateProvince = "Cau Giay",
                    Street = "Pham Van Bach",
                    RoomNumber = "10",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    PhoneNumber = "0904752252",
                    PostCode1 = "1",
                    PostCode2 = "2",
                    Deleted = false,
                });
                companyManager.AddAsync(new Models.Companies.Company
                {
                    Id = defaultCompany2Guid,
                    Name = "CASE Company",
                    BuildingName = "CASE Company",
                    City = "Ha Noi",
                    StateProvince = "Cau Giay",
                    Street = "Pham Van Bach",
                    RoomNumber = "10",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    PhoneNumber = "09099999999",
                    PostCode1 = "1",
                    PostCode2 = "2",
                    Deleted = false,
                });

                #region Type

                var defaultNumbericTypeGuid = Guid.NewGuid();
                var defaultNumbericType2Guid = Guid.NewGuid();
                var defaultDateTypeGuid = Guid.NewGuid();
                var defaultDatetimeTypeGuid = Guid.NewGuid();
                var defaultCurrencyTypeGuid = Guid.NewGuid();
                var defaultAlphanumericTypeGuid = Guid.NewGuid();
                var defaultListTypeGuid = Guid.NewGuid();
                var listType = new List<Models.Types.Type>() {
                    new Models.Types.Type
                    {
                        Id = defaultNumbericTypeGuid,
                        Name = "Numberic (Up to 6 digits)",
                        Value = "float"
                    },
                    new Models.Types.Type
                    {
                        Id = defaultNumbericType2Guid,
                        Name = "Numberic (Up to 15 digits)",
                        Value = "double"
                    },
                    new Models.Types.Type
                    {
                        Id=defaultDateTypeGuid,
                        Name = "Date",
                        Value = "datetime"
                    },
                    new Models.Types.Type
                    {
                        Id= defaultDatetimeTypeGuid,
                        Name = "Datetime",
                        Value = "datetime"
                    },
                    new Models.Types.Type
                    {
                        Id= defaultCurrencyTypeGuid,
                        Name = "Currency",
                        Value = "decimal"
                    },
                    new Models.Types.Type
                    {
                        Id = defaultAlphanumericTypeGuid,
                        Name = "Alphanumeric",
                        Value = "string"
                    },
                    new Models.Types.Type
                    {
                        Id = defaultListTypeGuid,
                        Name = "List (Alphanumeric)",
                        Value = "list"
                    }
                };
                typeManager.AddMultiAsync(listType).ConfigureAwait(false);

                #endregion

                #region Template

                var defaultTemplateId = Guid.NewGuid();
                templateManager.AddAsync(new Template { Id = defaultTemplateId, Name = "Default Template" });

                #endregion

                #region CompanyTemplate

                companyTemplateManager.AddAsync(new Models.CompanyTemplates.CompanyTemplate
                {
                    CompanyId = defaultCompany2Guid,
                    TemplateId = defaultTemplateId
                });

                #endregion

                #region Keyword

                var listKeywordDefault = new List<Keyword> {
                    new Keyword
                    {
                        Name = "Customer Name",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 20,
                        IsRequired =true,
                        Searchable =true,
                        Order = 1,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 50,
                        IsRequired =true,
                        Searchable =true,
                        Order = 2,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Phone Number",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 12,
                        IsRequired =true,
                        Searchable =true,
                        Order = 3,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Customer Contact Person",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 4,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Reception Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 5,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Request Type",
                        TypeId = defaultListTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 6,
                        //Metadata = "Jet,Bass Boat,Marine Boat,OEM,Other",
                        Metadata = "Land Parcel Survey,Topographic Survey,Geological Survey,Seismic Survey,Groundwater Survey,Architectural Design,New Construction,Remodeling/Renovation Construction,Demolition Construction",
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Site Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 7,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Scheduled Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 8,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Internal PIC",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 9,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Submission Status",
                        TypeId = defaultListTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 10,
                        Metadata = "Not yet,Done",
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Submission Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 11,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Payment Status",
                        TypeId = defaultListTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 12,
                        Metadata = "Billed,Check Payment,Complete",
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Invoice Amount",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 13,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Invoice Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 14,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Arrival Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 15,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Payment Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired = true,
                        Searchable = true,
                        Order = 16,
                        Source = string.Empty
                    },
                    new Keyword
                    {
                        Name = "Note",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 1000,
                        IsRequired =true,
                        Searchable =true,
                        Order = 17,
                        Source = string.Empty
                    }
                };
                keywordManager.AddMultiAsync(listKeywordDefault).ConfigureAwait(false);

                #endregion
                var userExists = userManager.FindByNameAsync("SuperAdmin");
                if (userExists.Result == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = "hoangthanhduong@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "SuperAdmin",
                        CompanyId = defaultCompanyGuid
                    };

                    userManager.CreateAsync(user, "Admin@123");

                    var user2 = new ApplicationUser
                    {
                        Email = "tanbc0901@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "Admin",
                        CompanyId = defaultCompany2Guid
                    };

                    userManager.CreateAsync(user2, "Admin@123");

                    System.Threading.Thread.Sleep(2000);

                    var roleSuperAdminCheck = roleManager.RoleExistsAsync("SuperAdmin");
                    if (!roleSuperAdminCheck.Result)
                    {
                        roleManager.CreateAsync(new ApplicationRole(UserRoles.SuperAdmin));
                    }
                    var roleAdminCheck = roleManager.RoleExistsAsync("Admin");
                    if (!roleAdminCheck.Result)
                    {
                        roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
                    }

                    System.Threading.Thread.Sleep(2000);
                    userManager.AddToRoleAsync(user, "SuperAdmin");
                    
                    System.Threading.Thread.Sleep(2000);
                    userManager.AddToRoleAsync(user2, "Admin");
                     System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {

            }
            return app;
        }
    }
}
